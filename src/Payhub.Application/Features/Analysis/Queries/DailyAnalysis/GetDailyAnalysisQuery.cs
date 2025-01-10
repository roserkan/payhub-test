using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Abstractions.Services;
using Payhub.Application.Common.DTOs.Analysis;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Analysis.Queries.DailyAnalysis;

public sealed record GetDailyAnalysisQuery : IQuery<DailyAnalysDto>
{
    public List<int>? SiteIds { get; set; }
}


public sealed class GetDailyAnalysisQueryHandler : IQueryHandler<GetDailyAnalysisQuery, DailyAnalysDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionService _permissionService;
    
    public GetDailyAnalysisQueryHandler(IUnitOfWork unitOfWork, IPermissionService permissionService)
    {
        _unitOfWork = unitOfWork;
        _permissionService = permissionService;
    }

    public async Task<DailyAnalysDto> Handle(GetDailyAnalysisQuery request, CancellationToken cancellationToken)
    {
        DateTime startOfToday = DateTime.Today; // Bugünün başlangıcı 00:00:00
        DateTime currentTime = DateTime.Now; // Şu anki zaman
        DateTime startOfYesterday = startOfToday.AddDays(-1); // Dünün başlangıcı 00:00:00
        DateTime endOfYesterday = startOfToday.AddMilliseconds(-1); // Dünün bitişi 23:59:59.999
        DateTime last24Hours = currentTime.AddHours(-24); // 24 saat öncesi

        var siteIdList = request.SiteIds;
        var permitted = await _permissionService.GetSitePermissionsAsync();
        if (siteIdList?.Count == 0)
            siteIdList = permitted.ToList();
        if((siteIdList == null || siteIdList.Count == 0) && permitted.ToList().Count == 0)
            siteIdList = new List<int>();
        
        
        // Bugünkü yatırımların toplam tutarı ve sayısı
        var todayDepositData = await _unitOfWork.DepositRepository.Query()
            .Where(d => d.CreatedDate >= startOfToday && 
                        d.CreatedDate <= currentTime && 
                        d.Status == DepositStatus.Confirmed && 
                        (siteIdList.Contains(d.SiteId)))
            .GroupBy(d => 1)
            .Select(g => new
            {
                TotalAmount = g.Sum(d => d.PayedAmount),
                Count = g.Count()
            })
            .FirstOrDefaultAsync(cancellationToken) ?? new { TotalAmount = 0M, Count = 0 };

        // Bugünkü çekimlerin toplam tutarı ve sayısı
        var todayWithdrawData = await _unitOfWork.WithdrawRepository.Query()
            .Where(w => w.CreatedDate >= startOfToday && 
                        w.CreatedDate <= currentTime && 
                        w.Status == WithdrawStatus.Confirmed &&
                        (siteIdList.Contains(w.SiteId)))
            .GroupBy(w => 1)
            .Select(g => new
            {
                TotalAmount = g.Sum(w => w.PayedAmount),
                Count = g.Count()
            })
            .FirstOrDefaultAsync(cancellationToken) ?? new { TotalAmount = 0M, Count = 0 };

        var hourlyTemplate = Enumerable.Range(0, 24).Select(hour => new HourlyDataDto
        {
            HourFormatted = $"{hour:D2}:00", // Saat formatı HH:mm (örn: 00:00, 01:00, ...)
            TotalAmount = 0, // Varsayılan olarak 0 yatırıyoruz.
            Count = 0 // Varsayılan olarak adet 0
        }).ToList();

        // Son 24 saatlik yatırımlar
        var depositsInLast24Hours = await _unitOfWork.DepositRepository.Query()
            .Where(d => d.CreatedDate >= last24Hours && d.CreatedDate <= currentTime && d.Status == DepositStatus.Confirmed && 
                        (siteIdList.Contains(d.SiteId)))
            .GroupBy(d => d.CreatedDate.Hour)
            .Select(g => new HourlyDataDto
            {
                HourFormatted = $"{g.Key:D2}:00", // Saat formatı HH:mm
                TotalAmount = g.Sum(d => d.PayedAmount),
                Count = g.Count()
            })
            .ToListAsync(cancellationToken);

        // Son 24 saatlik çekimler
        var withdrawsInLast24Hours = await _unitOfWork.WithdrawRepository.Query()
            .Where(w => w.CreatedDate >= last24Hours && w.CreatedDate <= currentTime && w.Status == WithdrawStatus.Confirmed &&
                        (siteIdList.Contains(w.SiteId)))
            .GroupBy(w => w.CreatedDate.Hour)
            .Select(g => new HourlyDataDto
            {
                HourFormatted = $"{g.Key:D2}:00", // Saat formatı HH:mm
                TotalAmount = g.Sum(w => w.PayedAmount),
                Count = g.Count()
            })
            .ToListAsync(cancellationToken);

        // Yatırımlar için saatlik listeyi tamamlama (boş saatler 0 olarak kalacak)
        var completeDeposits = hourlyTemplate
            .Select(ht => depositsInLast24Hours.FirstOrDefault(d => d.HourFormatted == ht.HourFormatted) ?? ht)
            .ToList();

        // Çekimler için saatlik listeyi tamamlama (boş saatler 0 olarak kalacak)
        var completeWithdraws = hourlyTemplate
            .Select(ht => withdrawsInLast24Hours.FirstOrDefault(w => w.HourFormatted == ht.HourFormatted) ?? ht)
            .ToList();

        // Dünkü yatırımların toplam tutarı ve sayısı
        var yesterdayDepositData = await _unitOfWork.DepositRepository.Query()
            .Where(d => d.CreatedDate >= startOfYesterday && d.CreatedDate <= endOfYesterday && d.Status == DepositStatus.Confirmed)
            .GroupBy(d => 1)
            .Select(g => new
            {
                TotalAmount = g.Sum(d => d.PayedAmount),
                Count = g.Count()
            })
            .FirstOrDefaultAsync(cancellationToken) ?? new { TotalAmount = 0M, Count = 0 };

        // Dünkü çekimlerin toplam tutarı ve sayısı
        var yesterdayWithdrawData = await _unitOfWork.WithdrawRepository.Query()
            .Where(w => w.CreatedDate >= startOfYesterday && w.CreatedDate <= endOfYesterday && w.Status == WithdrawStatus.Confirmed)
            .GroupBy(w => 1)
            .Select(g => new
            {
                TotalAmount = g.Sum(w => w.PayedAmount),
                Count = g.Count()
            })
            .FirstOrDefaultAsync(cancellationToken) ?? new { TotalAmount = 0M, Count = 0 };

        // Yüzdelik değerler
        var depositPercent = yesterdayDepositData.TotalAmount > 0 ? ((todayDepositData.TotalAmount - yesterdayDepositData.TotalAmount) / yesterdayDepositData.TotalAmount) * 100 : 0;
        var withdrawPercent = yesterdayWithdrawData.TotalAmount > 0 ? ((todayWithdrawData.TotalAmount - yesterdayWithdrawData.TotalAmount) / yesterdayWithdrawData.TotalAmount) * 100 : 0;
        var result = new DailyAnalysDto()
        {
            TotalDepositAmount = todayDepositData.TotalAmount,
            TotalDepositCount = todayDepositData.Count,
            TotalWithdrawAmount = todayWithdrawData.TotalAmount,
            TotalWithdrawCount = todayWithdrawData.Count,
            DepositPercent = depositPercent,
            WithdrawPercent = withdrawPercent,
            HourlyDeposits = completeDeposits,
            HourlyWithdraws = completeWithdraws,
        };

        return result;
    }
}
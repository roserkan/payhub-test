namespace Payhub.Application.Common.DTOs.Analysis;

public sealed record DailyAnalysDto
{
    // Bugün gelen yatırımların toplam miktarı
    public decimal TotalDepositAmount { get; set; }

    // Bugün gelen çekimlerin toplam miktarı
    public decimal TotalWithdrawAmount { get; set; }

    // Yatırımlar için 24 saat önceye göre % artış veya azalış
    public decimal DepositPercent { get; set; }

    // Çekimler için 24 saat önceye göre % artış veya azalış
    public decimal WithdrawPercent { get; set; }

    // Son 24 saat içindeki saatlik bazda yatırımların toplamları
    public List<HourlyDataDto> HourlyDeposits { get; set; } = new();

    // Son 24 saat içindeki saatlik bazda çekimlerin toplamları
    public List<HourlyDataDto> HourlyWithdraws { get; set; } = new();

    public int TotalDepositCount { get; set; }
    public int TotalWithdrawCount { get; set; }
}
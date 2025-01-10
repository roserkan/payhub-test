using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.Accounts;
using Payhub.Application.Common.DTOs.Devices;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Devices.Queries.GetAll;

public sealed class GetAllDevicesQueryHandler : IQueryHandler<GetAllDevicesQuery, IEnumerable<DeviceDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAllDevicesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<DeviceDto>> Handle(GetAllDevicesQuery request, CancellationToken cancellationToken)
    {
        var devices = await _unitOfWork.Devices.GetAllWithSelectorAsync(
            selector: r => new DeviceDto
            {
                Id = r.Id,
                Name = r.Name,
                SerialNumber = r.SerialNumber,
                Description = r.Description,
                CreatedDate = r.CreatedDate,
                UpdatedDate = r.UpdatedDate,
                Account = new AccountDto()
                {
                    Id = r.Account.Id,
                    Name = r.Account.Name,
                    AccountNumber = r.Account.AccountNumber,
                    IsActive = r.Account.IsActive,
                    Bank = new BankDto()
                    {
                        Id = r.Account.Bank.Id,
                        Name = r.Account.Bank.Name,
                        IconUrl = r.Account.Bank.IconUrl
                    }
                }
            },
            cancellationToken: cancellationToken);
        
        return devices;
    }
}
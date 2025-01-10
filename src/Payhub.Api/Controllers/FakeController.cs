using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Common.DTOs.Infra;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Models.RequestModels;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Models.ResponseModels;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Results;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FakeController : ControllerBase
{
    [HttpPost("deposit-callback")]
    public IActionResult DepositCallback(InfraDepositCallbackDto dto)
    {
        var amount = (int)dto.Amount;
        
        if (amount % 2 == 0)
        {
            return Ok(new InfraDepositCallbackResponseDto
            {
                Message = "Deposit callback successful.",
                Success = true
            });
        }
        else
        {
            return BadRequest(new InfraDepositCallbackResponseDto
            {
                Message = "Deposit callback failed.",
                Success = false,
            });
        }
    }
    
    [HttpPost("withdraw-callback")]
    public IActionResult WithdrawCallback(InfraWithdrawCallbackDto dto)
    {
        var amount = (int)dto.Amount;
        
        if (amount % 2 == 0)
        {
            return Ok(new InfraWithdrawCallbackResponseDto()
            {
                Message = "Deposit callback successful.",
                Success = true
            });
        }
        else
        {
            return BadRequest(new InfraWithdrawCallbackResponseDto
            {
                Message = "Deposit callback failed.",
                Success = false,
            });
        }
    }
    
    [HttpGet("payox-get-available-banks")]
    public IActionResult PayoxAvailableBanks()
    {
        var result = new ApiResponse<List<PayoxAvailableBankResponse>>(); 
        
        result.Status = 200;
        result.Message = "Success";
        result.Data = new List<PayoxAvailableBankResponse>
        {
            new PayoxAvailableBankResponse
            {
                Id = "5fe0f093bb4f0b001b5ea63d",
            },
            new PayoxAvailableBankResponse
            {
                Id = "5fe0f093bb4f0b001b5ea63d"
            }
        };
        
        return Ok(result);
    }
    
    [HttpPost("payox-create-deposit")]
    [Consumes("application/x-www-form-urlencoded")]
    public IActionResult PayoxCreateDeposit([FromForm]PayoxCreateDepositRequest request)
    {
        var result = new ApiResponse<PayoxCreateDepositResponse>();
        
        result.Status = 200;
        result.Message = "Success";
        result.Data = new PayoxCreateDepositResponse
        {
            TransactionId = Guid.NewGuid().ToString("N"),
            ProcessId = request.ProcessId,
            BankId = request.BankId,
            Amount = request.Amount,
            UserId = request.UserId,
            Name = request.Name,
            UserName = request.UserName,
            Type = "deposit",
            ConvertedName = request.UserName,
            Status = "pending",
            Bank = "Fake Bank",
            BankAccountName = "Fake Bank Account Name",
            BankAccountIban = "TR280006276256222621885935",
            Hash = Guid.NewGuid().ToString("N")
        };
      
        
        return Ok(result);
    }
    
    [HttpPost("payox-create-withdraw")]
    [Consumes("application/x-www-form-urlencoded")]
    public IActionResult PayoxCreateWithdraw([FromForm]PayoxCreateWithdrawRequest request)
    {
        var result = new ApiResponse<PayoxCreateWithdrawResponse>();
        
        result.Status = 200;
        result.Message = "Success";
        result.Data = new PayoxCreateWithdrawResponse
        {
            TransactionId = Guid.NewGuid().ToString("N"),
            ProcessId = request.ProcessId,
            BankId = request.BankId,
            Amount = request.Amount,
            UserId = request.UserId,
            Name = request.Name,
            UserName = request.UserName,
            Type = "withdrawal",
            ConvertedName = request.UserName,
            Status = "pending",
            Bank = "Fake Bank",
            Hash = Guid.NewGuid().ToString("N"),
            AccountName = "Fake Account Name",
            Iban = "TR280006276256222621885123"
        };
        
        return Ok(result);
    }
}
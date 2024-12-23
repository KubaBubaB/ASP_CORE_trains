﻿using DotNET_ASP_App.DTOs.Responses;
using DotNET_ASP_App.Service;
using Microsoft.AspNetCore.Mvc;

namespace DotNET_ASP_App.Controller;

[ApiController]
[Route("api")]
public class BlockchainController : ControllerBase
{
    private readonly BlockchainService blockchainService;

    public BlockchainController(BlockchainService blockchainService_)
    {
        blockchainService = blockchainService_;
    }

    [HttpGet("wallet/{id}")]
    public async Task<IActionResult> GetBalance(int id)
    {
        try
        {
            if (id < 0 || id > 15) return BadRequest("Invalid sensor id.");

            var sensorBalance = await blockchainService.GetBalance(id);
            return Ok(sensorBalance);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("wallets")]
    public async Task<IActionResult> GetWalletsBalance()
    {
        try
        {
            var sensorBalance = new GetAllWalletsBalance();
            sensorBalance.Balances = new List<GetAllWalletsBalance.WalletBalance>();
            
            for (int i = 0; i < 16; i++)
            {
                sensorBalance.Balances.Add(new GetAllWalletsBalance.WalletBalance
                {
                    Id = i,
                    Balance = await blockchainService.GetBalance(i),
                });
            }

            return Ok(sensorBalance);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
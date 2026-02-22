using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MudBlazorCrmApp.Data;
using MudBlazorCrmApp.Shared.Models;

namespace MudBlazorCrmApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableRateLimiting("Fixed")]
public class ImportController(ApplicationDbContext _ctx, ILogger<ImportController> _logger) : ControllerBase
{
    private readonly ApplicationDbContext ctx = _ctx;
    private readonly ILogger<ImportController> logger = _logger;

    [HttpPost("contacts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ImportResultDto>> ImportContacts(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
            };
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<Contact>().ToList();

            var imported = 0;
            foreach (var record in records)
            {
                record.Id = null;
                record.CreatedDate = DateTime.UtcNow;
                record.ModifiedDate = DateTime.UtcNow;
                await ctx.Contact.AddAsync(record);
                imported++;
            }

            await ctx.SaveChangesAsync();

            return Ok(new ImportResultDto { Imported = imported, Total = records.Count });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error importing contacts");
            return BadRequest($"Error importing CSV: {ex.Message}");
        }
    }

    [HttpPost("customers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ImportResultDto>> ImportCustomers(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
            };
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<Customer>().ToList();

            var imported = 0;
            foreach (var record in records)
            {
                record.Id = null;
                record.CreatedDate = DateTime.UtcNow;
                record.ModifiedDate = DateTime.UtcNow;
                await ctx.Customer.AddAsync(record);
                imported++;
            }

            await ctx.SaveChangesAsync();

            return Ok(new ImportResultDto { Imported = imported, Total = records.Count });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error importing customers");
            return BadRequest($"Error importing CSV: {ex.Message}");
        }
    }

    [HttpPost("leads")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ImportResultDto>> ImportLeads(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
            };
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<Lead>().ToList();

            var imported = 0;
            foreach (var record in records)
            {
                record.Id = null;
                record.CreatedDate = DateTime.UtcNow;
                record.ModifiedDate = DateTime.UtcNow;
                await ctx.Lead.AddAsync(record);
                imported++;
            }

            await ctx.SaveChangesAsync();

            return Ok(new ImportResultDto { Imported = imported, Total = records.Count });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error importing leads");
            return BadRequest($"Error importing CSV: {ex.Message}");
        }
    }

    [HttpPost("opportunities")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ImportResultDto>> ImportOpportunities(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
            };
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<Opportunity>().ToList();

            var imported = 0;
            foreach (var record in records)
            {
                record.Id = null;
                record.CreatedDate = DateTime.UtcNow;
                record.ModifiedDate = DateTime.UtcNow;
                await ctx.Opportunity.AddAsync(record);
                imported++;
            }

            await ctx.SaveChangesAsync();

            return Ok(new ImportResultDto { Imported = imported, Total = records.Count });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error importing opportunities");
            return BadRequest($"Error importing CSV: {ex.Message}");
        }
    }

    [HttpPost("sales")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ImportResultDto>> ImportSales(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
            };
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<Sale>().ToList();

            var imported = 0;
            foreach (var record in records)
            {
                record.Id = null;
                record.CreatedDate = DateTime.UtcNow;
                record.ModifiedDate = DateTime.UtcNow;
                await ctx.Sale.AddAsync(record);
                imported++;
            }

            await ctx.SaveChangesAsync();

            return Ok(new ImportResultDto { Imported = imported, Total = records.Count });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error importing sales");
            return BadRequest($"Error importing CSV: {ex.Message}");
        }
    }
}

public class ImportResultDto
{
    public int Imported { get; set; }
    public int Total { get; set; }
    public List<string>? Errors { get; set; }
}

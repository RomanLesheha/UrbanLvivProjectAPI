using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrbanLvivProjectAPI.Interfaces;
using UrbanLvivProjectAPI.Models;
using UrbanLvivProjectAPI.Models.RequestModels;

namespace UrbanLvivProjectAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : Controller
{
    private readonly IReportService _reportService;
    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReports()
    {
        List<ReportDetails> result = await _reportService.GetAllReports();
        return Ok(JsonConvert.SerializeObject(result));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateReport(ReportCreate report)
    {
        ServerResponse result = await _reportService.CreateReport(report);
        return Ok(JsonConvert.SerializeObject(result));
    }
}
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

    [HttpGet("GetAllReports")]
    public async Task<IActionResult> GetAllReports()
    {
        List<ReportDetails> result = await _reportService.GetAllReports();
        return Ok(JsonConvert.SerializeObject(result));
    }
    
    [HttpGet("GetAllActiveReports")]
    public async Task<IActionResult> GetAllActiveReports()
    {
        List<ReportDetails> result = await _reportService.GetAllActiveReports();
        return Ok(JsonConvert.SerializeObject(result));
    }
    
    [HttpGet("GetUserReports/{userId}")]
    public async Task<IActionResult> GetUserReports(int userId)
    {
        List<ReportDetails> result = await _reportService.GetUserReports(userId);
        return Ok(JsonConvert.SerializeObject(result));
    }
    
    [HttpPost("CreateReport")]
    public async Task<IActionResult> CreateReport(ReportCreate report)
    {
        ServerResponse result = await _reportService.CreateReport(report);
        return Ok(JsonConvert.SerializeObject(result));
    }
    
    [HttpPost("UpdateReportByUser/{reportId}")]
    public async Task<IActionResult> UpdateReportByUser(int reportId , ReportUpdate report)
    {
        ServerResponse result = await _reportService.UpdateReportByUser(reportId , report);
        return Ok(JsonConvert.SerializeObject(result));
    }
    
    [HttpPost("CancelReport/{reportId}")]
    public async Task<IActionResult> CancelReportByUser(int reportId)
    {
        ServerResponse result = await _reportService.CancelReportByUser(reportId);
        return Ok(JsonConvert.SerializeObject(result));
    }
}
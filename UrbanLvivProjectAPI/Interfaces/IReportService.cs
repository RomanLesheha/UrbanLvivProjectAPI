using UrbanLvivProjectAPI.Models;
using UrbanLvivProjectAPI.Models.RequestModels;

namespace UrbanLvivProjectAPI.Interfaces;

public interface IReportService
{
    Task<ServerResponse> CreateReport(ReportCreate reportModel);
    Task<List<ReportDetails>> GetAllReports();
}
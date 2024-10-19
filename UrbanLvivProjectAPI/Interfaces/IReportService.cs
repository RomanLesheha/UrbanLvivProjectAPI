using UrbanLvivProjectAPI.Models;
using UrbanLvivProjectAPI.Models.RequestModels;

namespace UrbanLvivProjectAPI.Interfaces;

public interface IReportService
{
    Task<ServerResponse> CreateReport(ReportCreate reportModel);
    Task<List<ReportDetails>> GetAllReports();
    Task<List<ReportDetails>> GetAllActiveReports();
    Task<List<ReportDetails>> GetUserReports(int userId);
    Task<ServerResponse> UpdateReportByUser(int reportId, ReportUpdate updatedReportModel);
    Task<ServerResponse> CancelReportByUser(int reportId);

}
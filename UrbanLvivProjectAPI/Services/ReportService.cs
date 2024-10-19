using UrbanLvivProjectAPI.Interfaces;
using UrbanLvivProjectAPI.Models;
using UrbanLvivProjectAPI.Models.GeneralModels;
using UrbanLvivProjectAPI.Models.RequestModels;

namespace UrbanLvivProjectAPI.Services;

public class ReportService : IReportService
{
    private readonly IFirebaseConnectingService _firebaseConnectingService;

    public ReportService(IFirebaseConnectingService firebaseConnectingService)
    {
        _firebaseConnectingService = firebaseConnectingService;
    }


    public async Task<ServerResponse> CreateReport(ReportCreate reportModel)
    {
        using (var client = _firebaseConnectingService.GetFirebaseClient())
        {
            if (client != null)
            {
                Random rand = new Random();
                int reportId = rand.Next(10000000, 99999999);

                var report = new Report
                {
                    Id = reportId,
                    Title = reportModel.title,
                    Description = reportModel.description,
                    ImageUrl = reportModel.image,
                    TypeOfProblem = Enum.Parse<ProblemType>(reportModel.typeOfProblem.ToString()),
                    CreatorId = reportModel.creatorId,
                    Location = reportModel.location,
                    TimeOfCreation = DateTime.Now
                };

                var setResponse = await client.SetAsync($"Reports/{reportId}", report);

                if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new ServerResponse(message: "Report uploaded successfully", statusCode: 200);
                }
                else
                {
                    return new ServerResponse(message: "Failed to upload report", statusCode: 400);
                }
            }
            else
            {
                return new ServerResponse(message: "Failed to connect to Firebase", statusCode: 400);
            }
        }
    }

   
    public async Task<List<ReportDetails>> GetAllReports()
    {
        using (var client = _firebaseConnectingService.GetFirebaseClient())
        {
            if (client != null)
            {
                var response = await client.GetAsync("Reports");

                if (response.Body != "null")
                {
                    var reports = response.ResultAs<Dictionary<string, ReportDetails>>();
                    return reports.Values.ToList();
                }
                else
                {
                    return new List<ReportDetails>();
                }
            }
            else
            {
                return new List<ReportDetails>();
            }
        }
    }

    public async Task<List<ReportDetails>> GetAllActiveReports()
    {
        using (var client = _firebaseConnectingService.GetFirebaseClient())
        {
            if (client != null)
            {
                var response = await client.GetAsync("Reports");

                if (response.Body != "null")
                {
                    var reports = response.ResultAs<Dictionary<string, Report>>();
                    
                    var activeReports = reports.Values
                        .Where(report => !report.CanceledByUser)
                        .Select(report => new ReportDetails
                        {
                            id = report.Id,
                            title = report.Title,
                            description = report.Description,
                            imageUrl = report.ImageUrl,
                            typeOfProblem = (int)report.TypeOfProblem,
                            isDone = report.IsCompleted,
                            location = report.Location,
                            timeOfCreation = report.TimeOfCreation.ToString("o"),
                            priority = report.Priority,
                            creatorId = report.CreatorId
                        })
                        .ToList();

                    return activeReports;
                }
                else
                {
                    return new List<ReportDetails>();
                }
            }
            else
            {
                return new List<ReportDetails>();
            }
        }
    }
    public async Task<List<ReportDetails>> GetUserReports(int userId)
    {
        using (var client = _firebaseConnectingService.GetFirebaseClient())
        {
            if (client != null)
            {
                var response = await client.GetAsync("Reports");

                if (response.Body != "null")
                {
                    var reports = response.ResultAs<Dictionary<string, ReportDetails>>();
                    
                    var userReports = reports.Values
                        .Where(report => report.creatorId == userId)
                        .ToList();

                    return userReports;
                }
                else
                {
                    return new List<ReportDetails>(); 
                }
            }
            else
            {
                return new List<ReportDetails>();
            }
        }
    }
    
    public async Task<ServerResponse> UpdateReportByUser(int reportId, ReportUpdate updatedReportModel)
    {
        using (var client = _firebaseConnectingService.GetFirebaseClient())
        {
            if (client != null)
            {
                var response = await client.GetAsync($"Reports/{reportId}");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var report = response.ResultAs<Report>();
                    
                    if (report.ProcessingStatus != ProcessingStatus.InReview || report.ProcessingStatus != ProcessingStatus.New)
                    {
                        return new ServerResponse(message: "Report cannot be updated because it has already been approved", statusCode: 403);
                    }
                    
                    report.Title = updatedReportModel.title;
                    report.Description = updatedReportModel.description;
                    report.ImageUrl = updatedReportModel.image;
                    report.TypeOfProblem = Enum.Parse<ProblemType>(updatedReportModel.typeOfProblem.ToString());
                    report.Location = updatedReportModel.location;

                    var setResponse = await client.SetAsync($"Reports/{reportId}", report);

                    if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return new ServerResponse(message: "Report updated successfully", statusCode: 200);
                    }
                    else
                    {
                        return new ServerResponse(message: "Failed to update report", statusCode: 400);
                    }
                }
                else
                {
                    return new ServerResponse(message: "Report not found", statusCode: 404);
                }
            }
            else
            {
                return new ServerResponse(message: "Failed to connect to Firebase", statusCode: 400);
            }
        }
    }

    
    public async Task<ServerResponse> CancelReportByUser(int reportId)
    {
        using (var client = _firebaseConnectingService.GetFirebaseClient())
        {
            if (client != null)
            {
                var response = await client.GetAsync($"Reports/{reportId}");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var report = response.ResultAs<Report>();
                    report.CanceledByUser = true;

                    var setResponse = await client.SetAsync($"Reports/{reportId}", report);
                    if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return new ServerResponse(message: "Report canceled successfully", statusCode: 200);
                    }
                    else
                    {
                        return new ServerResponse(message: "Failed to cancel report", statusCode: 400);
                    }
                }
                else
                {
                    return new ServerResponse(message: "Report not found", statusCode: 404);
                }
            }
            else
            {
                return new ServerResponse(message: "Failed to connect to Firebase", statusCode: 400);
            }
        }
    }
}
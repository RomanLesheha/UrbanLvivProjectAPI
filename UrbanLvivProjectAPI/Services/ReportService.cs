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
                    Id = reportId, // Генеруємо Id з унікального рядка
                    Title = reportModel.title,
                    Description = reportModel.description,
                    ImageUrl = reportModel.image,
                    TypeOfProblem = Enum.Parse<ProblemType>(reportModel.typeOfProblem), // Парсинг типу проблеми з рядка
                    CreatorId = int.Parse(reportModel.creatorId), // Перетворення ID користувача з рядка в число
                    Location = reportModel.location,
                    TimeOfCreation = DateTime.Now, // Поточний час створення
                    Priority = 0, // Пріоритет за замовчуванням (буде призначений ШІ пізніше)
                    IsProcessedByAI = false, // Ще не оброблено ШІ
                    AIProcessingStatus = AIProcessingStatus.Pending, // Статус обробки ШІ
                    AIDescription = string.Empty, // Опис від ШІ (порожній до обробки)
                    ProcessingStatus = ProcessingStatus.New, // Статус репорту на початковій стадії
                    IsCompleted = false, // Звіт не виконаний
                    CompletionPercentage = 0, // Відсоток виконання 0 на початку
                    AIRecommendations = string.Empty, // Рекомендації від ШІ
                    OfficialSummary = string.Empty // Офіційний підсумок відсутній до завершення
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


}
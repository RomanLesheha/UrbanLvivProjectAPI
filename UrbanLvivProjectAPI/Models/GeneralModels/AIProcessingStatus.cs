namespace UrbanLvivProjectAPI.Models.GeneralModels;

public enum AIProcessingStatus
{
    Pending,        // The report is awaiting AI processing
    InProgress,     // The report is currently being processed by AI
    Completed,      // AI processing has been completed
    Error           // An error occurred during AI processing
}
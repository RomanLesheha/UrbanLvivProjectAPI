namespace UrbanLvivProjectAPI.Models.GeneralModels;

public enum ProcessingStatus
{
    New,            // The report has just been created and is awaiting further actions
    InReview,       // The report is being reviewed by the authorities or AI
    Approved,       // The report has been approved for action
    InProgress,     // Actions to resolve the issue are in progress
    Completed,      // The issue has been resolved
    Rejected        // The report was rejected (e.g., invalid or duplicate)
}
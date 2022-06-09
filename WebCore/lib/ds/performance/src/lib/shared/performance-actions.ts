
const PERFORMANCE   = "Performance";
const GOAL_TRACKING = "GoalTracking";
const EMPLOYEE_GOAL = "EmployeeGoal";
const RATING        = "PerformanceReviewRating";
const ATTACHMENT    = "Attachment";

export const PERFORMANCE_ACTIONS = {
    Performance: {
        WriteReview:                     `${PERFORMANCE}.WriteReview`,
        ReadReview:                      `${PERFORMANCE}.ReadReview`,
        WriteFeedbackSetup:              `${PERFORMANCE}.WriteFeedbackSetup`,
        ReadFeedbackSetup:               `${PERFORMANCE}.ReadFeedbackSetup`,
        ReadCompetencies:                `${PERFORMANCE}.ReadCompetencies`,
        WriteCompetencySetup:            `${PERFORMANCE}.WriteCompetencySetup`,
        AssignEmployeeCompetencyModel:   `${PERFORMANCE}.AssignEmployeeCompetencyModel`,
        AssignJobProfileCompetencyModel: `${PERFORMANCE}.AssignJobProfileCompetencyModel`,
        AdministrateOwnPerformanceSetup: `${PERFORMANCE}.AdministrateOwnPerformanceSetup`,
        ReleaseEvaluationToEmployee:     `${PERFORMANCE}.ReleaseEvaluationToEmployee`,
        WriteEvaluation:                 `${PERFORMANCE}.WriteEvaluation`,
    },
    GoalTracking: {
        ReadGoals:  `${GOAL_TRACKING}.ReadGoals`,
        WriteGoals: `${GOAL_TRACKING}.WriteGoals`,
    },
    EmployeeGoal: {
        Read:  `${EMPLOYEE_GOAL}.Read`,
        Write: `${EMPLOYEE_GOAL}.Write`,
    },
    Ratings: {
        ReadRatings:  `${RATING}.ReadRatings`,
        WriteRatings: `${RATING}.WriteRatings`,
    },
    Attachment: {
        EmployeeAccessOnly: `${ATTACHMENT}.EmployeeAccessOnly`,     // Permissions required to view when attachment's IsEmployeeView is true.
        AddEditAttachments: `${ATTACHMENT}.AddEditAttachments`,     // Permissions required to add and modify the attachments.
        AllEmployeesViewOnly: `${ATTACHMENT}.AllEmployeesViewOnly`  // Permissions required to view the attachments.
    }
};
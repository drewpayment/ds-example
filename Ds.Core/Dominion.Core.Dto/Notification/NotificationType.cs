using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Notification
{
    public enum NotificationType : int
    {
        LeaveManagementRequest              = 1,
        LeaveManagementRequestStatusChange  = 2,
        OpenEnrollmentEnding                = 3,
        TaxPacketAvailable                  = 4,
        W2ClientReady                       = 5,
        W2EmployeeReady                     = 6,
        ApplicantTrackingCorrespondence     = 7,
        ApplicantTrackingHiringWorkflowTask = 8,
        ApplicantTrackingPostingOwner       = 9,
        DisciplineLevelReached              = 10,
        NewLifeEvent                        = 11,
        NewSystemAdminCreated               = 12,
        EmployeeCompletedOnboarding         = 13,
        NewGoalAssigned                     = 14,
        CompleteEvaluation                  = 15,
        EvaluationCompleted                 = 16,
        ReviewMeetingInvite                 = 17,
        EvaluationSharedWithEmployee        = 18,
        CommentAddedToGoal                  = 19,
        NewCompanyContactCreated            = 20,
        NewCompanyAdminCreated              = 21,
        I9ReadyToBeCertified                = 22,
        OverdueReminders                    = 23,
        WelcomeToOnboarding                 = 24,
        NewEmployeeAddedToOnboarding        = 25,
        NewEmployeeAssignedToSupervisor     = 26,
		EmployeeBirthdayList                = 27,
        EmployeeWorkAnniversaryList           = 28,
        ApprovalProcess                       = 29,
		EmployeeChangedDirectDepositSetup     = 30,
        EmployeeNinetyDayWorkAnniversaryList  = 31,
		EmployeeChangedW4                     = 32,
        LeaveManagementRequestCancellation    = 33,
        NewNoteOnApplicant                    = 34,
        SubmittedApplication                  = 35,		
        NewPendingBillingCreditCreated        = 36,
        EmployeeNavigatorUpdatedEmployee = 37,
        EmployeeNavigatorAddedDeduction = 38,
        EmployeeNavigatorSyncError = 39,
    }
}

using System;

namespace Dominion.Core.Dto.Performance
{
    /// <summary>
    /// Roles for performance evaluations.
    /// </summary>
    public enum EvaluationRoleType : byte
    {
        Manager = 1,
        Self    = 2,
        Peer    = 3
    }

    public enum EvaluationSupervisorType
    {
        DirectSupervisor = -1
    }

    public static class EvaluationRoleTypeExtensions
    {
        public static string ToFriendlyText(this EvaluationRoleType role, bool appendEvaluationText = false)
        {
            var friendlyText = string.Empty;

            switch (role)
            {
                case EvaluationRoleType.Manager:
                    friendlyText = "Supervisor";
                    break;
                case EvaluationRoleType.Self:
                    friendlyText = "Employee Self-";
                    break;
                case EvaluationRoleType.Peer:
                    friendlyText = "Peer";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(role), role, null);
            }

            if (!appendEvaluationText) return friendlyText;

            const string evalText = "Evaluation";
            friendlyText += role != EvaluationRoleType.Self ? (" " + evalText) : evalText;

            return friendlyText;
        }
    }
}

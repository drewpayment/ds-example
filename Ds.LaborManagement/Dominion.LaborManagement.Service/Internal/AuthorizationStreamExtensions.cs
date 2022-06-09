using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Services.Security.Authorization;
using Dominion.LaborManagement.Service.Api;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.Utility.OpResult;
using Dominion.Utility.Security;

namespace Dominion.LaborManagement.Service.Internal
{
    public static class AuthorizationStreamExtensions
    {

        /// <summary>
        /// Checks that the user has either the <see cref="ClockActionType.User"/> or <see cref="ClockActionType.Administrator"/> roles
        /// and optionally that the employee ID or client ID matches.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="employeeId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public static IAuthorizationStream<T> CheckClockAdminOrNormalUser<T>(this IAuthorizationStream<T> stream,
            int? employeeId = null, int? clientId = null)
        {
            return stream.CheckClockAdminOrNormalUser(
                adminCheck: IdentityIfNull<T, int?>(clientId, a => a.CheckClientId(clientId.Value)), 
                normalCheck: IdentityIfNull<T, int?>(employeeId, a => a.CheckEmployeeId(employeeId.Value)));
        }

        private static Func<IAuthorizationStream<T>, IAuthorizationStream<T>> IdentityIfNull<T, TValue>(
            TValue employeeId,
            Func<IAuthorizationStream<T>, IAuthorizationStream<T>> check)
        {
            return employeeId != null ? check : a => a;
        }

        /// <summary>
        /// Checks that the user has either the <see cref="ClockActionType.User"/> or <see cref="ClockActionType.Administrator"/> roles
        /// and optionally that the admin check or normal check matches.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="adminCheck"></param>
        /// <param name="normalCheck"></param>
        /// <returns></returns>
        public static IAuthorizationStream<T> CheckClockAdminOrNormalUser<T>(
            this IAuthorizationStream<T> stream, 
            Func<IAuthorizationStream<T>, IAuthorizationStream<T>> adminCheck = null, 
            Func<IAuthorizationStream<T>, IAuthorizationStream<T>> normalCheck = null)
        {
            return stream.CheckActionTypePattern(new Dictionary<ActionType, Func<IAuthorizationStream<T>, IAuthorizationStream<T>>>()
            {
                {ClockActionType.Administrator, a => a.Pipe(IdentityIfNull(adminCheck, adminCheck)) },
                {ClockActionType.User, a => a.Pipe(IdentityIfNull(normalCheck, normalCheck)) }
            });
        }

        /// <summary>
        /// Asserts that the employee with the given ID can punch from the given IP address.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="punchProvider"></param>
        /// <param name="employeeId"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static IAuthorizationStream<T> CheckCanPunchFromIp<T>(this IAuthorizationStream<T> stream, IEmployeePunchProvider punchProvider, int employeeId, string ipAddress)
        {
            var data = default(T);
            return stream
                .Then(d => data = d)
                .Then(d => punchProvider.CanEmployeePunchFromIp(employeeId, ipAddress))
                .Assert(result => result.Data.CanPunch, $"You cannot punch from this IP Address ({ipAddress})")
                .Then(d => data);
        }

    }
}

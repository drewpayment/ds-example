using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using Dominion.Utility.Configs;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.Utility.Web
{
    public class SessionManagementConfig
    {
        public string Name { get; set; }

        public bool Debug { get; set; } = true;

        public string LogoutUrl { get; set; }

        public int SessionTimeoutMinutes { get; set; }

        public decimal KeepAliveMinutes { get; set; }

        public decimal IdleMinutes { get; set; }

        public decimal LogoutMinutes { get; set; }

        public string GenerateJs()
        {
            var js = $@"
    authMonitor.cfg.name = '{Name}';
    authMonitor.cfg.debug = {Debug.ToString().ToLower()};
    authMonitor.cfg.keepAliveInterval = {KeepAliveMinutes.ToMilliseconds()};
    authMonitor.cfg.inactiveInterval  = {IdleMinutes.ToMilliseconds()};
    authMonitor.cfg.logoutInterval    = {LogoutMinutes.ToMilliseconds()};
    authMonitor.cfg.logoutUrl    = {LogoutUrl.ToJsStringOrNull()};
    //authMonitor.restart();        
    authMonitor.start();        
";

            return js;
        }

        public string Execute(HttpSessionStateBase session)
        {
            //we always want this
            var js = GenerateJs();
            session.Timeout = SessionTimeoutMinutes;
            return js;
        }

        public string Execute(HttpSessionState session)
        {
            //we always want this
            var js = GenerateJs();
            session.Timeout = SessionTimeoutMinutes;
            return js;
        }

        public static SessionManagementConfig TestItBasic()
        {
            var obj = new SessionManagementConfig()
            {
                Name = "TB1",
                Debug = true,
                SessionTimeoutMinutes = 20,
                KeepAliveMinutes = .5m,
                IdleMinutes = 1m,
                LogoutMinutes = .5m,
                LogoutUrl = null,
            };

            return obj;
        }

        public static SessionManagementConfig LogoutToDsDotComSmokeTest()
        {
            var obj = new SessionManagementConfig()
            {
                Name = "DS SMOKE",
                Debug = true,
                SessionTimeoutMinutes = 20,
                KeepAliveMinutes = .5m,
                IdleMinutes = 1m,
                LogoutMinutes = .5m,
                LogoutUrl = ConfigValues.DominionDotCom,
            };

            return obj;
        }

        public static SessionManagementConfig LogoutToDsDotCom()
        {
            var obj = new SessionManagementConfig()
            {
                Name = "DS",
                SessionTimeoutMinutes = 20,
                KeepAliveMinutes = 5m,
                IdleMinutes = 7.5m,
                LogoutMinutes = 1m,
                LogoutUrl = ConfigValues.DominionDotCom,
            };

            return obj;
        }

        /// <summary>
        /// This config seems to be something used in many pages.
        /// </summary>
        /// <returns></returns>
        public static SessionManagementConfig LockDownMode()
        {
            var obj = new SessionManagementConfig()
            {
                Name = "LOCK N",
                SessionTimeoutMinutes = 20,
                KeepAliveMinutes = 1m,
                IdleMinutes = 2m,
                LogoutMinutes = 1m,
                LogoutUrl = null,
            };

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SessionManagementConfig SuperLockDownMode()
        {
            var obj = new SessionManagementConfig()
            {
                Name = "LOCK S",
                SessionTimeoutMinutes = 20,
                KeepAliveMinutes = 0.5m,
                IdleMinutes = 1m,
                LogoutMinutes = 0.5m,
                LogoutUrl = null,
            };

            return obj;
        }
    }
}

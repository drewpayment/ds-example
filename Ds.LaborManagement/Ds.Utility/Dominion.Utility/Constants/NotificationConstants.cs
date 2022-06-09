namespace Dominion.Utility.Constants
{
    public static class NotificationConstants
    {
        #region URL / PAGE PATHS

            #region PERFORMANCE

                #region EMPLOYEE PAYROLL

                public const string EMPLOYEE_PERFORMANCE_PAYROLL_BASE_URL =
                    "EmployeePerformance.aspx?submenu=employee#/performance/employees";

                public const string EMPLOYEE_COMPETENCIES_PAYROLL_URL = EMPLOYEE_PERFORMANCE_PAYROLL_BASE_URL + "/competencies";

                /// <summary>
                /// <see cref="EMPLOYEE_PERFORMANCE_PAYROLL_BASE_URL"/>
                /// </summary>
                public const string EMPLOYEE_REVIEWS_PAYROLL_URL = EMPLOYEE_PERFORMANCE_PAYROLL_BASE_URL + "/reviews";

                /// <summary>
                /// <see cref="EMPLOYEE_PERFORMANCE_PAYROLL_BASE_URL"/>
                /// </summary>
                public const string EMPLOYEE_GOALS_PAYROLL_URL = EMPLOYEE_PERFORMANCE_PAYROLL_BASE_URL + "/goals";

                public const string EMPLOYEE_PERFORMANCE_PERF_MANAGER_STATUS = "PerformanceReviews.aspx?submenu=performance&hash=/performance/manage/status";
                #endregion

                #region COMPANY PAYROLL

                public const string COMPANY_PERFORMANCE_PAYROLL_BASE_URL = "performancereviews.aspx?submenu=company#/performance";

                /// <summary>
                /// <see cref="COMPANY_PERFORMANCE_PAYROLL_BASE_URL"/>
                /// </summary>
                public const string COMPANY_GOALS_PAYROLL_URL = COMPANY_PERFORMANCE_PAYROLL_BASE_URL + "/goals";

                #endregion

                #region EMPLOYEE ESS

                public const string EMPLOYEE_PERFORMANCE_ESS_BASE_URL = "performance";

                /// <summary>
                /// <see cref="EMPLOYEE_PERFORMANCE_ESS_BASE_URL"/>
                /// </summary>
                public const string EMPLOYEE_GOALS_ESS_URL = EMPLOYEE_PERFORMANCE_ESS_BASE_URL + "/goals";

                /// <summary>
                /// <see cref="EMPLOYEE_PERFORMANCE_ESS_BASE_URL"/>
                /// </summary>
                public const string EMPLOYEE_REVIEWS_ESS_URL = EMPLOYEE_PERFORMANCE_ESS_BASE_URL + "/reviews";

                #endregion

            #endregion //PERFORMANCE

            #region ONBOARDING

            public const string ONBOARDING_BASE_URL = "Onboarding";

        #endregion

        #region APPLICANT TRACKING

        public const string APPLICANTTRACKING_QUALIFY_APPLICANTS_URL = "QualifyApplicants.aspx";

        #endregion

        #region BENEFITS

        /// <summary>
        /// Uses host/domain name: Dominion.Core.Dto.Core.DominionApp.Payroll
        /// </summary>
        public const string BENEFIT_LIFE_EVENT_ADMIN_PATH = "BenefitLifeEvent.aspx";

        #endregion

        #region CompanyRoutes

        public const string COMPANY_BILLING = "admin/billing";

        #endregion

        #region PAYROLL

        public const string CHANGE_EMPLOYEE_URL = "ChangeEmployee.aspx?Submenu=Employee&URL=Employee.aspx";
            public const string EMPLOYEE_DEDUCTION_URL =
                "ChangeEmployee.aspx?SubMenu=Employee&Force=True&URL=EmployeeDeduction.aspx&Clock=0";

            public const string EMPLOYEE_TAXES_URL =
                "ChangeEmployee.aspx?SubMenu=Employee&Force=True&URL=EmployeeTaxes.aspx&Clock=0";

        #region REPORTS

        public const string COMPANY_REPORT_VIEWER = "CompanyReportViewer.aspx";

                #endregion

        #endregion

        #endregion // URL / PAGE PATHS

        #region LINK TITLES

        public const string CLICK_HERE = "Click here";
        public const string LOG_IN = "Log in";
        public const string SIGN_IN = "Sign in";
        
        #endregion

        #region HTML TAGS

        public const string LINE_BREAK_TAG = "<br />";
        public const string EMPHASIS_TAG = "<em>";
        public const string EMPHASIS_END_TAG = "</em>";
        public const string BOLD_TAG = "<strong>";
        public const string BOLD_END_TAG = "</strong>";
        public const string SMALL_TAG = "<small>";
        public const string SMALL_END_TAG = "</small>";
        public const string SUBSCRIPT_TAG = "<sub>";
        public const string SUBSCRIPT_END_TAG = "</sub>";
        public const string UNORDERED_LIST_TAG = "<ul>";
        public const string UNORDERED_LIST_END_TAG = "</ul>";
        public const string LIST_ITEM_TAG = "<li>";
        public const string LIST_ITEM_END_TAG = "</li>";


        #endregion

        #region Special Chars

        public const string QUOTATION_MARK = "\"";

        #endregion

        #region SMS CHARS

        // https://support.twilio.com/hc/en-us/articles/223181468-How-do-I-Add-a-Line-Break-in-my-SMS-or-MMS-Message-
        public const string SMS_LINE_BREAK = "%0a";

        #endregion
    }
}

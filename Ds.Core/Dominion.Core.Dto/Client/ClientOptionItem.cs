using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientOptionItem
    {
        public enum ApprovalOptions : int
        {
            None = 10,
            Hours_And_Benefits = 11,
            Exceptions = 12,
            Everyday = 17,
            All_Activity = 18,
            Cost_Center = 26
        }

        public enum ShiftOptions : int
        {
            None = 66,
            Manual = 67,
            Automatic_Splits = 68,
            Apply_Premium_To_All_Schedules_Within_Start_Stop = 69,
            Automatic_Based_On_Hours_Worked = 75
        }

        public enum ClientOptions : int
        {
            Reporting_Hide_Routing_On_Master_Control = 1,
            Payroll_Employee_Picker = 2,
            Time_Employee_Picker = 3,
            Payroll_Include_Salary_Hours_Import = 4,
            Payroll_Days_For_Password_Expiration = 5,
            Time_IP_Security = 6,
            Time_Default_Punch_Calendar_View = 7,
            Payroll_Populate_Payroll_Totals_On_Import = 8,
            Time_Punches_To_Show_To_Employees = 9,
            Time_Show_Punch_Exceptions_Warning_On_Import = 10,
            Time_Approval_Options = 11,
            Time_Increase_Tips_To_Match_Wage = 12,
            Time_Block_Supervisor_From_Authorizing_Timecards = 13,
            Time_Number_Days_Show_On_Punch_Screen = 14,
            Time_Use_Shifts = 15,
            Require_Employee_To_Pick_CostCenter = 16,
            Days_To_Show_On_Punch = 17,
            Payroll_W2_ConsentType = 18,
            Time_Users_Allowed_To_Edit_Schedules = 19,
            Payroll_Who_Will_Print_W2s = 20,
            PreviewCostCenterWarning = 21,
            Payroll_Include_Adjustments_Employee_Check_History = 22,
            Time_Include_Weekends_Default = 23,
            Allow_Adjustment_Imports = 24,
            Password_Protect_Paystubs = 25,
            AvgHoursOnMasterControl = 26,
            Day_Employee_Has_ESS_Access = 27,
            Allow_CompanyAdmin_To_Print_ManualChecks = 28,
            Automatically_Terminate_On_SeparationDate = 29,
            EmployeeViewOnlyLeaveManagement = 30,
            SplitSalaryBasedOnGridHours = 31,
            DefaultEmployeeSequence = 32,
            Payroll_Enable_Temp_Agency_Billing = 33,
            Show_Temps = 35,
            Departments_Across_All_Divisions = 36,
            Allow_Income_Wage_Exempt = 37,
            Time_Show_Hours_In_Hundreths = 38,
            Hide_Inactive_Deductions = 39,
            Enable_Customized_Report_Owner_Option = 40,
            Lock_Timecards = 41,
            Lock_Closed_Payrolls = 42,
            Stop_Direct_Deposit_on_Last_Pay = 43,
            Allow_Employee_Tax_Status_Change_Requests = 44,
            Disable_Employee_List_Dropdown_List_In_Employee_Header = 45,
            Supervisors_Can_Enable_Employees = 46,
            Time_Show_Clock_Dropdown_List = 47,
            Catchup_Suta_and_StateFuta = 48,
            Time_Split_Holidays_Among_Shifts = 49,
            Time_Show_Subcheck_On_Benefit_Screen = 50,
            Company_Admin_LockOut_Minutes = 51,
            Supervisor_Lockout_Minutes = 52,
            Employee_Lockout_Minutes = 53,
            Type_Of_Hours = 54,
            Show_Split_Shifts = 55,
            Add_Deduction_By_Code = 56,
            Payroll_Import_Multiple_Files = 57,
            Time_Show_Cost_Center_Tooltip = 58, //rrice 11/4/2011 CR 6158
            Allow_PreOverTime_Allocation = 59,
            Default_OneTime_Deduction_Import_Option = 60, //ronk 5/22/12
            Run_Labor_Source_Reports_In_Hundredths = 61, //coleb 6/19/12 Case 570
            ZeroFillWorkCode = 62,
            ShowRateOverrideOnBenefits = 63, //Scott B
            AllowEditOfNotesOnLockedTimeCard = 64,
            AllowEditOfSchedulesOnLockedTimeCard = 65, //9/7/12 ronk case 907
            Allow_Time_Off_Requests_From_Punch_Calendar = 66, //ColeB 08/17/2016
            Make_Points_Report_Visible_To_Employees = 67, //ColeB 08/17/2016
            Who_Will_Print_ACA_Reports = 68, //ColeB 08/17/2016
            Company_Admin_Can_Edit_Locked_Time_Cards = 69, //ColeB 08/17/2016
            Client_Can_Use_Employee_Import = 72, //LarsonCo 02/02/2018
            IPad_Pin_Number_Length = 73, //LarsonCo 01/01/2019
        }
    }
}

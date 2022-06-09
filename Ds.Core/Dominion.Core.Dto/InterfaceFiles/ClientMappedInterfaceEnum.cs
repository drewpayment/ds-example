using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Dominion.Core.Dto.InterfaceFiles
{
    /// <sumarry>
    /// All of the currently existing <see cref="ClientMappedInterfaceEnum"/>s that are not ClientID specific.
    /// </sumarry>
    public enum MappedImportEnum
    {
        [Description("Generic Employee Import")]
        GenericEmployeeImport = ClientMappedInterfaceEnum.GenericEmployeeImport,        // = 16,
        [Description("Employee Deduction Import")]
        EmployeeDeductionImport = ClientMappedInterfaceEnum.EmployeeDeductionImport,    // = 22,
        [Description("Employee Dependent Import")]
        EmployeeDependentImport = ClientMappedInterfaceEnum.EmployeeDependentImport,    // = 23,
        [Description("Employee Tax Import")]
        EmployeeTaxImport = ClientMappedInterfaceEnum.EmployeeTaxImport,                // = 24,
    }

    /// <summary>
    /// Taken from <c>ClientMappedInterface</c> table.
    /// Appended associated ClientCode to enum names that would otherwise have the same name:
    /// 
    /// "EEOC",
    /// "HR",
    /// "Events",
    /// "HR Load",
    /// "Employee Load"
    /// 
    /// </summary>
    public enum ClientMappedInterfaceEnum
    {
        [Description("Kronos Import")]
        KronosImport            = 1,  // ClientCode=ASP1, ClientID=691
        [Description("EEOC")]
        EEOCPRI2                = 2,  // ClientCode=PRI2, ClientID=310
        [Description("HR")]
        HRPRI2                  = 3,  // ClientCode=PRI2, ClientID=310
        [Description("Events")]
        EventsPRI2              = 4,  // ClientCode=PRI2, ClientID=310
        [Description("EEOC")]
        EEOCAMA1                = 5,  // ClientCode=AMA1, ClientID=777
        [Description("Events")]
        EventsLMC2              = 6,  // ClientCode=LMC2, ClientID=344
        [Description("PAAG")]
        PAAG                    = 7,  // ClientCode=8082, ClientID=635
        [Description("ABRA")]
        ABRA                    = 8,  // ClientCode=VAN2, ClientID=874
        [Description("HR")]
        HRWDG2                  = 9,  // ClientCode=WDG2, ClientID=580
        [Description("Employee Load")]
        EmployeeLoadRQF1        = 10, // ClientCode=RQF1, ClientID=785
        [Description("HR Load")]
        HRLoadK9P2              = 11, // ClientCode=K9P2, ClientID=659
        [Description("TSI1 To CSS2")]
        TSI1ToCSS2              = 12, // ClientCode=CSS2, ClientID=406
        [Description("HR Import")]
        HRImport                = 13, // ClientCode=WTK1, ClientID=794
        [Description("EEOC")]
        EEOC                    = 14, // ClientCode=KM32, ClientID=562
        [Description("Employee Load")]
        EmployeeLoadICM1        = 15, // ClientCode=ICM1, ClientID=1017
        [Description("Generic Employee Import")]
        GenericEmployeeImport   = 16, // ClientCode=NULL, ClientID=-1
        [Description("HR Load")]
        HRLoadEHA2              = 17, // ClientCode=EHA2, ClientID=1040
        [Description("HR Load")]
        HRLoadMAC2              = 18, // ClientCode=MAC2, ClientID=1041
        [Description("HR Load")]
        HRLoad4582              = 19, // ClientCode=4582, ClientID=1039
        [Description("HR Load")]
        HRLoadMCS2              = 20, // ClientCode=MCS2, ClientID=1079
        [Description("EEOC Load")]
        EEOCLoad                = 21, // ClientCode=TUR2, ClientID=1074
        [Description("Employee Deduction Import")]
        EmployeeDeductionImport = 22, // ClientCode=NULL, ClientID=-1
        [Description("Employee Dependent Import")]
        EmployeeDependentImport = 23, // ClientCode=NULL, ClientID=-1
        [Description("Employee Tax Import")]
        EmployeeTaxImport       = 24, // ClientCode=NULL, ClientID=-1
    }
}

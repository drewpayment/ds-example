namespace Dominion.Core.Dto.Employee
{
    public enum EmployeeStatusType
    {
        Unknown = 0,
        FullTime = 1, 
        PartTime = 2, 
        CallIn = 3, 
        Special = 4, 
        Layoff = 5, 
        Terminated = 6, 
        Manager = 7, 
        LastPay = 8, 
        FullTimeTemp = 9, 
        MilitaryLeave = 10, 
        LeaveOfAbsence = 11,
        StudentIntern = 12,
        Retired = 13,
        Seasonal = 14,
        PartTimeTemp = 15,
        Severance = 16,
        Furlough = 17
    }

    public static class EmployeeStatusTypeExtensions
    {
        public static bool IsInactiveEmployeeStatusType(this EmployeeStatusType value)
        {
            // The "right" way to do this would probably be to query the DB...
            // Kinda seems overkill though? Hard-coding this mapping for now...
            return (
                value == EmployeeStatusType.Layoff              // 5
                || value == EmployeeStatusType.Terminated       // 6
                //|| value == EmployeeStatusType.LastPay        // 8
                || value == EmployeeStatusType.LeaveOfAbsence   // 11
                || value == EmployeeStatusType.Retired          // 13
                || value == EmployeeStatusType.Furlough         // 17
            );
        }
    }
}

/**
EmployeeStatusID	EmployeeStatus	Active	EmployeeStatusCode	ACA
1	Full Time	1	A	1
2	Part Time	1	B	1
3	Call In	1	C	1
4	Special	1	S	1
5	Layoff	0	K	0
6	Terminated	0	T	0
7	Manager	1	M	1
8	Last Pay	1	L	1
9	Full Time Temp	1	E	0
10	Military Leave	1	I	1
11	Leave Of Absence	0	O	0
12	Student Intern	1	SI	1
13	Retired	0	R	1
14	Seasonal	1	SE	1
15	Part Time Temp	1	PE	0
16	Severance	1	SV	1
17	Furlough	0	F	0
 */



// Constants (magic numbers) ripped from CompanyAccrual.aspx.vb
// FIXME: This entire class should ideally be replaced with proper enums...
export class ClientAccrualConstants {
  private static readonly COL_SERVICE: number = 0;
  private static readonly COL_SERVICETYPE: number = 1;
  private static readonly COL_LIMIT: number = 2;
  private static readonly COL_LIMITTYPE: number = 3;
  private static readonly COL_CARRYOVERAMOUNT: number = 4;
  private static readonly COL_RATECARRYOVERMAX: number = 13;
  private static readonly COL_CARRYOVERPERIODTYPE: number = 7;
  private static readonly COL_CARRYOVERPERIOD: number = 14;
  private static readonly COL_DELETE: number = 16;

  // FIXME...
  public static readonly NO_VALUE_SELECTED = null; //old school thinking -2147483648;
  public static readonly NEW_ENTITY_ID = 0; // CommonConstants.NEW_ENTITY_ID
  // Currently used to differentiate between new "from-scratch" and new "copied"
  public static readonly NEW_ACCRUAL_ID = -1;
  public static readonly ADD_ACCRUAL_POLICY = '-- Add Policy --';
  public static readonly PRORATEDWHENTOAWARD = 1;

  // Paid Time Off default values
  // JIRA LM-135
  public static readonly PTO_PLNTYPE: number = 3;                         // Plan Type:                     3=Custom
  // static readonly PTO_EARNING: number = 16615;                    // Earning Type:                  N/A
  public static readonly PTO_HOURS: number = 35;                          // Hours worked to earn award:    35
  public static readonly PTO_EETYPE: number = 1;                          // Employee Type:                 1=Hourly
  public static readonly PTO_EESTATUS: number = 8;                        // Employee Status:               8=Full Time/Part Time
  public static readonly PTO_REFPNT: number = 5;                          // Referendce Date Method:        5=Anniversary Hire Date
  public static readonly PTO_BALOPT: number = 3;                          // Balance Option:                3=Pay Up to Balance
  public static readonly PTO_CRYOVER: number = 2;                         // Carry Over Method:             2=Take All Out Of New Balance
  public static readonly PTO_UNITS: number = 1;                           // Units:                         1=Hours
  public static readonly PTO_ROLLHOURS: boolean = true;                   // Allow Hours to Roll-Over weeks
  public static readonly PTO_REFTYPE: number = 1;                         // Reference Type:                1=Hire Date
  public static readonly PTO_ACCRUEWHENPAID: boolean = true;              // Accrue Only When Paid
  public static readonly PTO_SHOWONSTUB: boolean = true;                  // Show on Stub

  // Constants for auto-created schedule
  // JIRA LM-135
  public static readonly COL_SVSTRT: number = 1;                                                 // Service Start:                 1 (day)
  public static readonly COL_STRTFRQ: number = 1;                                                // Service Start Frequency:       1=Day
  public static readonly COL_SVFRQ: number = 2;                                                  // Service Frequency:             2=Completed
  public static readonly COL_SVEND: number = 99;                                                 // Service End:                   99 (years)
  public static readonly COL_SVENDFRQ: number = 3;                                               // Service End Frequency:         3=Years
  public static readonly COL_BALLMT: number = ClientAccrualConstants.NO_VALUE_SELECTED;          // Max balance:                   N/A
  public static readonly COL_SVACCAMTLMT: number = 40;                                           // Max accrual balance:           40(hrs)
  public static readonly COL_REWARD: number = 1;                                                 // Reward unit per accrual:       1(hour) (i.e. 1 hour per 35 hours worked)
  public static readonly COL_SVREWFRQ: number = 2;                                               // Reward Type:                   2=Flat
  public static readonly COL_RENWEND: number = ClientAccrualConstants.NO_VALUE_SELECTED;         // Max Hours:                     N/A
  public static readonly COL_SVRENEWFRQ: number = 1;                                             // Service renew Type:            1=Payroll
  public static readonly COL_CARROVR: number = 40;                                               // Carryover:                     40(hrs)
  public static readonly COL_SVCRROVRFRQ: number = 2;                                            // Carryover Type:                2=Flat
  public static readonly COL_SVCRRWNFRQ: number = 3;                                             // Carryover When Frequency:      3=Calendar Year
  public static readonly COL_SVCRROVRTIL: number = ClientAccrualConstants.NO_VALUE_SELECTED;     // Carryover Til:                 N/A
  public static readonly COL_SVCRROVRTILFQ: number = ClientAccrualConstants.NO_VALUE_SELECTED;   // Carryover til Frequency:       N/A
  public static readonly COL_RTCRROVRMX: number = 40;                                            // Max carryover:                 40hrs

  public static readonly DO_NOT_EDIT_NOTE: string = 'Do not make ANY changes to the schedule without consulting a developer!';
};

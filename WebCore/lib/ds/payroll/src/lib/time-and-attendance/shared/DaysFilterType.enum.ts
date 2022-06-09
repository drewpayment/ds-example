export enum DaysFilterType {
    /*
    Defines that every single day in the date range should appear. ("Filler")
    **/
    AllDays = 1,
/*
    Defines that every week day in the date range should appear. ("Filler")
    **/
    AllWeekdays = 2,
/*
    Defines that all days in the date range that have a schedule should appear. ("Filler"/"Remover" - Kinda is default)
    **/
    AllScheduledDays = 3,
/*
    @description Defines that all days in the date range that have "activity" should appear. ("Remover")
        
        Activity is defined as days with:
        - One or more punches/benefits/pending benefits
        - or one or more exceptions
    **/
    DatesWithActivity = 4,
/*
    Defines that only dates that have one or more exceptions in the date range should appear. ("Remover")
    **/
    DatesWithExceptions = 5,
    /*
    Defines the ID for the days filter control.
    **/
    ControlID = 1
}

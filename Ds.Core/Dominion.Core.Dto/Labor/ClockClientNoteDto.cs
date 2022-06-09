namespace Dominion.Core.Dto.Labor
{
    public class ClockClientNoteDto : IHasClockClientNoteValidation
    {
        public int    ClockClientNoteId  { get; set; }
        public int    ClientId           { get; set; }
        public string Note               { get; set; }
        public bool   IsHideFromEmployee { get; set; }
        public bool   IsActive           { get; set; }
        public bool   InUse              { get; set; }
    }
}

namespace Dominion.Core.Dto.Push
{
    public class TemplateDto
    {
        public int    Id      { get; set; }
        public string DevSn   { get; set; }
        public string Pin     { get; set; }
        public string Fid     { get; set; }
        public short? Size    { get; set; }
        public string Valid   { get; set; }
        public string TmpStr  { get; set; }
        public string TmpType { get; set; }
    }
}

using System;

namespace Dominion.Utility.Validation
{
    [Serializable]
    public class SinglePropValidationDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string PropertyName { get; set; }
        public string ClientId { get; set; }
        public string Message { get; set; }
        public bool HasError { get; set; }
    }
}
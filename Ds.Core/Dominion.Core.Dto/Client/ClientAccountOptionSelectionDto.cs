using Dominion.Core.Dto.Misc;

namespace Dominion.Core.Dto.Client
{
    public class ClientAccountOptionSelectionDto
    {
        public int?                  ClientAccountOptionId { get; set; }
        public int                   ClientId              { get; set; }
        public AccountOption         AccountOption         { get; set; }
        public string                SelectedValue         { get; set; }

        /// <summary>
        /// Will only be set to true/false if option is a boolean data type.
        /// Otherwise, see <see cref="SelectedItem"/> for selected option details.
        /// </summary>
        public bool?                IsEnabled              { get; set; }

        /// <summary>
        /// Will only be set if option is a list data type.
        /// Otherwise, see <see cref="IsEnabled"/> for boolean option value.
        /// </summary>
        public Dominion.Core.Dto.Misc.AccountOptionItemDto SelectedItem  { get; set; }
        
    }
}
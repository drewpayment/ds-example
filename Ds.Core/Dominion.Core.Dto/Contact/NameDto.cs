using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Future Consideration: Consolidate with Person.
namespace Dominion.Core.Dto.Contact
{
    public class NameDto
    {
        public string FirstName           { get; set; }       //([A-Za-z\-] ?)*[A-Za-z\-] string: required:yes
        public string MiddleInitial       { get; set; }       //([A-Za-z\-] ?)*[A-Za-z\-] string: required:no
        public string LastName            { get; set; }       //([A-Za-z\-] ?)*[A-Za-z\-] string: required:yes
        public string Suffix              { get; set; }       //([A-Za-z\-] ?)*[A-Za-z\-] string: required:no
    }
}

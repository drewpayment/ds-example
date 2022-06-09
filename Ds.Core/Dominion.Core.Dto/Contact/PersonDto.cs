using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Future Consideration: Consolidate with Person.
namespace Dominion.Core.Dto.Contact
{
    [Serializable]
    public class PersonDto
    {
        public int    PersonId              { get; set; } 
        public string FirstName             { get; set; }       //([A-Za-z\-] ?)*[A-Za-z\-] string: required:yes
        public string MiddleInitial         { get; set; }       //([A-Za-z\-] ?)*[A-Za-z\-] string: required:no
        public string LastName              { get; set; }       //([A-Za-z\-] ?)*[A-Za-z\-] string: required:yes

        public string Phone                 { get; set; }
        public string EmailAddress          { get; set; }
        public string Title                 { get; set; }
        //public string Suffix              { get; set; }       //([A-Za-z\-] ?)*[A-Za-z\-] string: required:no
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Location;

//Future Consideration: Consolidate with Person & Address
namespace Dominion.Core.Dto.Contact
{
    public class DominionContactDto : AddressExtendedDto
    {
        public  int     DominionContactId          { get; set; }

        public  string  FirstName                  { get; set; }
        public  string  MiddleInitial              { get; set; }
        public  string  LastName                   { get; set; }
        
        public  string  CompanyName                { get; set; }
        public  string  TaxpayerName               { get; set; }
        
        public  string  FederalIdNumber            { get; set; }
        
        //public  string  AddressLine1               { get; set; }
        //public  string  AddressLine2               { get; set; }
        public  string  CityName                    { get; set; }
        //public  int     StateId                    { get; set; }
        //public  string  PostalCode                 { get; set; }
        //public  int     CountryId                  { get; set; }
        

        public  string  Email                      { get; set; }
        
        public  string  PhoneNumber                { get; set; }
        public  string  PhoneExtension             { get; set; }
        public  string  Fax                        { get; set; }
    }
}

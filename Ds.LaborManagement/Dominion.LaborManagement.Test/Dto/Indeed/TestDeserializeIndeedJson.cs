using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.LaborManagement.Dto.ApplicantTracking.IndeedApply;
using Dominion.Utility.DataExport.Exporters;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Dominion.LaborManagement.Test.Dto.Indeed
{
    [TestFixture]
    public class TestDeserializeIndeedJson
    {
        private IndeedApplication _app;

        [SetUp]
        public void SetUp()
        {
            // Although this class file is in the same directory as the json file, the interpreted code that needs this file is in a different location.
            JObject o = JObject.Parse(File.ReadAllText("..\\..\\Dto\\Indeed\\TestIndeedPost.json"));

            _app = o.ToObject<IndeedApplication>();
        }

        [Test]
        public void Test_Can_Deserialize_Provided_Json()
        {
            // Yay we didn't blow up!  Good enough to pass this test.
        }

        #region Analytics




        [Test]
        public void Test_Analytics_Has_Correct_Device()
        {
            var device = _app.Analytics.Device;
            Assert.AreEqual("desktop", device);
        }

        [Test]
        public void Test_Analytics_Has_Correct_Ip()
        {
            var ip = _app.Analytics.Ip;
            Assert.AreEqual("67.79.201.170", ip);
        }

        [Test]
        public void Test_Analytics_Has_Correct_Referer()
        {
            var actual = _app.Analytics.Referer;
            Assert.AreEqual("https://apply.indeed.com/indeedapply/button-generate-tests", actual);
        }

        [Test]
        public void Test_Analytics_Has_Correct_UserAgent()
        {
            var actual = _app.Analytics.UserAgent;
            Assert.AreEqual(
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36",
                actual);
        }

        #endregion

        #region Applicant

        [Test]
        public void Test_Applicant_Has_Correct_CoverLetter()
        {
            var actual = _app.Applicant.Coverletter;
            Assert.AreEqual("I'm writing this cover letter to express my interest.", actual);
        }

        [Test]
        public void Test_Applicant_Has_Correct_Email()
        {
            var actual = _app.Applicant.Email;
            Assert.AreEqual("john.doe@email.com", actual);
        }

        [Test]
        public void Test_Applicant_Has_Correct_FullName()
        {
            var actual = _app.Applicant.FullName;
            Assert.AreEqual("John Doe", actual);
        }

        [Test]
        public void Test_Applicant_Has_Correct_PhoneNumber()
        {
            var actual = _app.Applicant.PhoneNumber;
            Assert.AreEqual("5555555555", actual);
        }

        #endregion

        #region Resume

        [Test]
        public void Test_File_Has_Correct_ContentType()
        {
            var actual = _app.Applicant.Resume.File.ContentType;
            Assert.AreEqual("application/pdf", actual);
        }

        [Test]
        public void Test_File_Has_Correct_Data()
        {
            var actual = _app.Applicant.Resume.File.Data;
            Assert.AreEqual(
                "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vNtjdzLvPL+bd3f63qr76PslD1QeVDzUedjwo8WPbTIn2fEhn6H+Rwse3RvmDj/9Kfen9yNFj8mPK0b1R5vHHMaOjfuNX3my8MnI0+yn0xPFP6v+XPvM/Nl3v3j+0j8ZOznyXPz806+bXmi+2Pdy9sueqbCph6+Er6Zfl7zRfLP/Lett37uYd6PT+e+x7ys/WHzo/hj08f4n4adPvwHJ4vTiCmVuZHN0cmVhbQplbmRvYmoKNiAwIG9iagpbL0lDQ0Jhc2VkIDUgMCBSXQplbmRvYmoKNyAwIG9iago8PAogIC9UeXBlIC9NZXRhZGF0YQogIC9TdWJ0eXBlIC9YTUwKICAvTGVuZ3RoIDEyIDAgUgo+PgpzdHJlYW0KPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz48eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIj4KPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4KPHJkZjpEZXNjcmlwdGlvbiB4bWxuczpwZGY9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGRmLzEuMy8iIHJkZjphYm91dD0iIj4KPHBkZjpLZXl3b3Jkcz5JbmRlZWQgUmVzdW1lPC9wZGY6S2V5d29yZHM+CjxwZGY6UHJvZHVjZXI+QXBhY2hlIEZPUCBWZXJzaW9uIDEuMDwvcGRmOlByb2R1Y2VyPgo8cGRmOlBERlZlcnNpb24+MS40PC9wZGY6UERGVmVyc2lvbj4KPC9yZGY6RGVzY3JpcHRpb24+CjxyZGY6RGVzY3JpcHRpb24geG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIiByZGY6YWJvdXQ9IiI+CjxkYzpjcmVhdG9yPkluZGVlZDwvZGM6Y3JlYXRvcj4KPGRjOnRpdGxlPkluZGVlZCBSZXN1bWU8L2RjOnRpdGxlPgo8ZGM6c3ViamVjdD5JbmRlZWQgUmVzdW1lPC9kYzpzdWJqZWN0Pgo8ZGM6ZGF0ZT4yMDE3LTEyLTE0VDEyOjQ5OjIzLTA2OjAwPC9kYzpkYXRlPgo8L3JkZjpEZXNj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CmVuZG9iagoxNyAwIG9iagpbCjE2IDAgUgpdCmVuZG9iagoxNCAwIG9iago8PAogIC9SZXNvdXJjZXMgMyAwIFIKICAvVHlwZSAvUGFnZQogIC9NZWRpYUJveCBbMCAwIDYxMiA3OTJdCiAgL0Nyb3BCb3ggWzAgMCA2MTIgNzkyXQogIC9CbGVlZEJveCBbMCAwIDYxMiA3OTJdCiAgL1RyaW1Cb3ggWzAgMCA2MTIgNzkyXQogIC9QYXJlbnQgMSAwIFIKICAvQW5ub3RzIDE3IDAgUgogIC9Db250ZW50cyAxOCAwIFIKPj4KCmVuZG9iagoxOSAwIG9iago1NjcKZW5kb2JqCjIwIDAgb2JqCjw8CiAgL1R5cGUgL0ZvbnQKICAvU3VidHlwZSAvVHlwZTEKICAvQmFzZUZvbnQgL0hlbHZldGljYQogIC9FbmNvZGluZyAvV2luQW5zaUVuY29kaW5nCj4+CgplbmRvYmoKMjEgMCBvYmoKPDwKICAvVHlwZSAvRm9udAogIC9TdWJ0eXBlIC9UeXBlMQogIC9CYXNlRm9udCAvSGVsdmV0aWNhLUJvbGQKICAvRW5jb2RpbmcgL1dpbkFuc2lFbmNvZGluZwo+PgoKZW5kb2JqCjEgMCBvYmoKPDwgL1R5cGUgL1BhZ2VzCi9Db3VudCAyCi9LaWRzIFs4IDAgUiAxNCAwIFIgXSA+PgplbmRvYmoKMiAwIG9iago8PAogIC9UeXBlIC9DYXRhbG9nCiAgL1BhZ2VzIDEgMCBSCiAgL01ldGFkYXRhIDcgMCBSCiAgL0xhbmcgKGVuLUdCKQogIC9QYWdlTGFiZWxzIDkgMCBSCj4+CgplbmRvYmoKMyAwIG9iago8PAovRm9udCA8PAogIC9GMSAyMCAwIFIKICAvRjMgMjEgMCBSCj4+Ci9Qcm9jU2V0IFsgL1BERiAvSW1hZ2VCIC9JbWFnZUMgL1RleHQgXQovQ29sb3JTcGFjZSA8PAogIC9EZWZhdWx0UkdCIDYgMCBSCj4+Cj4+CmVuZG9iago5IDAgb2JqCjw8IC9OdW1zIFswIDw8IC9QICgxKSA+PgogMSA8PCAvUCAoMikgPj4KXSA+PgoKZW5kb2JqCnhyZWYKMCAyMgowMDAwMDAwMDAwIDY1NTM1IGYgCjAwMDAwMDY3ODQgMDAwMDAgbiAKMDAwMDAwNjg0OSAwMDAwMCBuIAowMDAwMDA2OTU3IDAwMDAwIG4gCjAwMDAwMDAwMTUgMDAwMDAgbiAKMDAwMDAwMDIwMiAwMDAwMCBuIAowMDAwMDAyODgwIDAwMDAwIG4gCjAwMDAwMDI5MTMgMDAwMDAgbiAKMDAwMDAwNTE5NiAwMDAwMCBuIAowMDAwMDA3MDk0IDAwMDAwIG4gCjAwMDAwMDM5MzIgMDAwMDAgbiAKMDAwMDAwNTM4OCAwMDAwMCBuIAowMDAwMDA1NDA5IDAwMDAwIG4gCjAwMDAwMDU0MjkgMDAwMDAgbiAKMDAwMDAwNjMzNSAwMDAwMCBuIAowMDAwMDA1NDUwIDAwMDAwIG4gCjAwMDAwMDU1MjkgMDAwMDAgbiAKMDAwMDAwNjMwOCAwMDAwMCBuIAowMDAwMDA1NjY1IDAwMDAwIG4gCjAwMDAwMDY1NDUgMDAwMDAgbiAKMDAwMDAwNjU2NSAwMDAwMCBuIAowMDAwMDA2NjcyIDAwMDAwIG4gCnRyYWlsZXIKPDwKL1NpemUgMjIKL1Jvb3QgMiAwIFIKL0luZm8gNCAwIFIKL0lEIFs8MjI4RjVGNkQ1RTI2OTk5RjlBOERFMERDNDA5MzZEMDQ+IDwyMjhGNUY2RDVFMjY5OTlGOUE4REUwREM0MDkzNkQwND5dCj4+CnN0YXJ0eHJlZgo3MTU2CiUlRU9GCg==",
                actual);
        }

        [Test]
        public void Test_File_Has_Correct_FileName()
        {
            var actual = _app.Applicant.Resume.File.FileName;
            Assert.AreEqual("John_Doe.pdf", actual);
        }

        [Test]
        public void Test_Resume_Has_Correct_HrXml()
        {
            var actual = _app.Applicant.Resume.HrXml;
            Assert.AreEqual("\n\n  \n    SomeId1\n    SomeId2\n  \n  \n    \n      \n        John Doe        John        Doe      \n      \n        \n          US\n          TX\n          Austin\n        \n      \n    \n    \n      \n        CircleBack Inc        \n                    \n            CircleBack Inc          \n          Responsibilities\nI did everything from business analytics, helpdesk, full account management for paying clients, data provisioning, and marketing automation.          \n            \n              US\n              DC\n              Washington\n            \n          \n        \n          2013-07\n        \n        \n          2015-03\n        \n        \n      \n      \n        testing Inc        \n                    \n            testing Inc          \n          this was a testing job for computers          \n            \n              US\n              CA\n              Riverside\n            \n          \n        \n          2011-01\n        \n        \n          2013-03\n        \n        \n      \n      \n        Indeed        \n                    \n            Indeed          \n          Responsibilities\ntesting, product management, sales.          \n            \n              US\n              TX\n              Austin\n            \n          \n        \n      \n    \n      \n        \n           \n            Johns Hopkins University\n          \n          \n            International studies\n            \n              \n                2014\n              \n              \n                2018\n              \n            \n          \n        \n        \n           \n            Pinkerton Academy\n          \n          \n            high school. this is\n          \n        \n      \n  \n\n", actual);
        }

        [Test]
        public void Test_Resume_Has_Correct_Html()
        {
            var actual = _app.Applicant.Resume.Html;
            Assert.AreEqual(@"\n\n    \n        \n        \n        \n    \n\n\n\n\n\n\n\n\n\n
      \n    
      \n        
      \n            
      John Doe

      \n            \n            
      This is my headline.  I'm great
      \n            \n            \n                
      tronan@indeed.com | 5555555555
        \n            \n            \n            
      Austin, TX 78758
      \n            \n            \n            
      \n                Here is my Summary,  Its a bit more detailed.  Again I'm great.\n            
      \n            \n        
      \n    
      \n    \n    
      \n        
      \n            
      Work Experience
      \n        
      \n    
      \n    \n        
      \n            
      \n                \n                
      Customer Success Manager
      \n                \n                \n                
      \n                    CircleBack Inc\n                     - \n                    Washington, DC\n                
      \n                \n                \n                
      July 2013 to March 2015
      \n                \n\n                \n                
      \n                    Responsibilities 
      I did everything from business analytics, helpdesk, full account management for paying clients, data provisioning, and marketing automation.\n                
      \n                \n            
      \n        
      \n    \n        
      \n            
      \n                \n                
      tester
      \n                \n                \n                
      \n                    testing Inc\n                     - \n                    Riverside, CA\n                
      \n                \n                \n                
      January 2011 to March 2013
      \n                \n\n                \n                
      \n                    this was a testing job for computers\n                
      \n                \n            
      \n        
      \n    \n        
      \n            
      \n                \n                
      Analyst
      \n                \n                \n                
      \n                    Indeed\n                     - \n                    Austin, TX\n                
      \n                \n                \n\n                \n                
      \n                    Responsibilities 
      testing, product management, sales.\n                
      \n                \n            
      \n        
      \n    \n    \n\n    \n    
      \n        
      \n            
      Education
      \n        
      \n    
      \n    \n        
      \n            
      \n                \n                    \n                    
      International studies
      \n                    \n                \n                \n                
      Johns Hopkins University\n                    \n                
      \n                \n                \n                
      \n                    2014\n                    to\n                    \n                        2018\n                    \n                
      \n                \n            
      \n        
      \n    \n        
      \n            
      \n                \n                    \n                    
      high school. this is
      \n                    \n                \n                \n                
      Pinkerton Academy\n                    \n                
      \n                \n                \n            
      \n        
      \n    \n    \n\n    \n        \n        \n        \n    \n        \n        \n        \n    \n        \n        \n        \n    \n        \n        \n        \n    \n\n    \n    
      \n        
      \n            
      Skills
      \n        
      \n    
      \n        
      \n            
      \n                
      \n                    Microsoft Office (4 years), Testing (1 year), HTML (5 years)\n                
\n            
\n        
\n    \n\n    \n\n    \n        
\n            
\n                
Military Service
\n            
\n        
\n        \n            
\n                
\n                    \n                    
Service Country: United States
\n                    \n                    \n                    
Branch: coast guard
\n                    \n                    \n                    
Rank: colonel
\n                    \n                    \n                    
January 2012 to January 2014
\n                    \n\n                    \n\n                    \n                
\n            
\n        \n    \n\n    \n        
\n            
\n                
Awards
\n            
\n        
\n        \n            
\n                
\n                    \n                    
best aware
\n                    \n                    \n                    
January 1988
\n                    \n\n                    \n                    
\n                        award is great\n                    
\n                    \n                
\n            
\n        \n            
\n                
\n                    \n                    
#1 tester
\n                    \n                    \n                    
January 2016
\n                    \n\n                    \n                    
\n                        best tester in the organization.\n                    
\n                    \n                
\n            
\n        \n    \n\n    \n        
\n            
\n                
Certifications
\n            
\n        
\n        \n            
\n                
\n                    \n                    
Driver's License
\n                    \n                    \n                    
February 2007 to Present
\n                    \n\n                    \n                    
\n                        never expiring license\n                    
\n                    \n                
\n            
\n        \n    \n\n    \n        
\n            
\n                
Groups
\n            
\n        
\n        \n            
\n                
\n                    \n                    
Bread Lovers Member
\n                    \n                    \n                    
January 2017 to Present
\n                    \n\n                    \n                    
\n                        Group for  bread lovers.\n                    
\n                    \n                
\n            
\n        \n    \n\n    \n\n    \n        
\n            
\n                
Publications
\n            
\n        
\n        \n            
\n                
\n                    \n                    
Publication Example
\n                    \n                    \n                    
\n                        https://en.wikipedia.org/wiki/Publication\n                    
\n                    \n                    \n                    
January 2014
\n                    \n\n                    \n                    
\n                        This is an example of what a publication would look like.\n                    
\n                    \n                
\n            
\n        \n    \n\n    \n    
\n        
\n            
Additional Information
\n        
\n    
\n    
\n        
\n            
This is a text box where I can write whatever additional information seems important.
\n        
\n    
\n    \n\n    
\n        
\n            \n        
\n    
\n\n
\n\n\n".Replace("\n", "\\n"), actual.Replace("\n", "\\n")); // verbatim string escapes some but not all '\' in '\n'
        }

        [Test]
        public void Test_Resume_Json_Has_Correct_AdditionalInfo()
        {
            var actual = _app.Applicant.Resume.Json.AdditionalInfo;
            Assert.AreEqual("This is a text box where I can write whatever additional information seems important.", actual);
        }

        [Test]
        public void Test_Resume_Json_Associations_Has_Correct_Length()
        {
            var actual = _app.Applicant.Resume.Json.Associations;
            Assert.AreEqual(1, actual.Total);
        }

        [Test]
        public void Test_Resume_Json_Associations_First_Has_Correct_Description()
        {
            var actual = _app.Applicant.Resume.Json.Associations.Values.First();
            Assert.AreEqual("Group for  bread lovers.", actual.Description);
        }

        [Test]
        public void Test_Resume_Json_Associations_First_Has_Correct_EndCurrent()
        {
            var actual = _app.Applicant.Resume.Json.Associations.Values.First();
            Assert.AreEqual(true, actual.EndCurrent);
        }

        [Test]
        public void Test_Resume_Json_Associations_First_Has_Correct_StartDateMonth()
        {
            var actual = _app.Applicant.Resume.Json.Associations.Values.First();
            Assert.AreEqual(1, actual.StartDateMonth);
        }

        [Test]
        public void Test_Resume_Json_Associations_First_Has_Correct_StartDateYear()
        {
            var actual = _app.Applicant.Resume.Json.Associations.Values.First();
            Assert.AreEqual(2017, actual.StartDateYear);
        }

        [Test]
        public void Test_Resume_Json_Associations_First_Has_Correct_Title()
        {
            var actual = _app.Applicant.Resume.Json.Associations.Values.First();
            Assert.AreEqual("Bread Lovers Member", actual.Title);
        }

        [Test]
        public void Test_Resume_Json_Awards_Has_Correct_Total()
        {
            var actual = _app.Applicant.Resume.Json.Awards.Total;
            Assert.AreEqual(2, actual);
        }

        [Test]
        public void Test_Resume_Json_Awards_First_Has_Correct_DateMonth()
        {
            var actual = _app.Applicant.Resume.Json.Awards.Values.First().DateMonth;
            Assert.AreEqual(1, actual);
        }

        [Test]
        public void Test_Resume_Json_Awards_First_Has_Correct_DateYear()
        {
            var actual = _app.Applicant.Resume.Json.Awards.Values.First().DateYear;
            Assert.AreEqual(1988, actual);
        }

        [Test]
        public void Test_Resume_Json_Awards_First_Has_Correct_Description()
        {
            var actual = _app.Applicant.Resume.Json.Awards.Values.First().Description;
            Assert.AreEqual("award is great", actual);
        }

        [Test]
        public void Test_Resume_Json_Awards_First_Has_Correct_Title()
        {
            var actual = _app.Applicant.Resume.Json.Awards.Values.First().Title;
            Assert.AreEqual("best aware", actual);
        }

        [Test]
        public void Test_Resume_Json_Certifications_First_Has_Correct_Description()
        {
            var actual = _app.Applicant.Resume.Json.Certifications.Values.First().Description;
            Assert.AreEqual("never expiring license", actual);
        }

        [Test]
        public void Test_Resume_Json_Certifications_First_Has_Correct_EndCurrent()
        {
            var actual = _app.Applicant.Resume.Json.Certifications.Values.First().EndCurrent;
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Test_Resume_Json_Certifications_First_Has_Correct_StartDateMonth()
        {
            var actual = _app.Applicant.Resume.Json.Certifications.Values.First().StartDateMonth;
            Assert.AreEqual(2, actual);
        }

        [Test]
        public void Test_Resume_Json_Certifications_First_Has_Correct_StartDateYear()
        {
            var actual = _app.Applicant.Resume.Json.Certifications.Values.First().StartDateYear;
            Assert.AreEqual(2007, actual);
        }

        [Test]
        public void Test_Resume_Json_Certifications_First_Has_Correct_Title()
        {
            var actual = _app.Applicant.Resume.Json.Certifications.Values.First().Title;
            Assert.AreEqual("Driver's License", actual);
        }

        [Test]
        public void Test_Resume_Json_Certifications_First_EndDateMonth_Is_Null_When_Indeed_Does_Not_Provide_It()
        {
            var actual = _app.Applicant.Resume.Json.Certifications.Values.First().EndDateMonth;
            Assert.Null(actual);
        }

        [Test]
        public void Test_Resume_Json_Certifications_First_EndDateYear_Is_Null_When_Indeed_Does_Not_Provide_It()
        {
            var actual = _app.Applicant.Resume.Json.Certifications.Values.First().EndDateYear;
            Assert.Null(actual);
        }

        [Test]
        public void Test_Resume_Json_Certifications_First_StartDateMonth_Is_Null_When_Indeed_Does_Not_Provide_It()
        {
            var actual = _app.Applicant.Resume.Json.Certifications.Values.Last().StartDateMonth;
            Assert.Null(actual);
        }

        [Test]
        public void Test_Resume_Json_Certifications_First_StartDateYear_Is_Null_When_Indeed_Does_Not_Provide_It()
        {
            var actual = _app.Applicant.Resume.Json.Certifications.Values.Last().StartDateYear;
            Assert.Null(actual);
        }

        [Test]
        public void Test_Resume_Json_Educations_First_Has_Correct_Degree()
        {
            var actual = _app.Applicant.Resume.Json.Educations.Values.First().Degree;
            Assert.AreEqual("International studies", actual);
        }

        [Test]
        public void Test_Resume_Json_Educations_First_Has_Correct_EndCurrent()
        {
            var actual = _app.Applicant.Resume.Json.Educations.Values.First().EndCurrent;
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Test_Resume_Json_Educations_First_Has_Correct_EndDateYear()
        {
            var actual = _app.Applicant.Resume.Json.Educations.Values.First().EndDateYear;
            Assert.AreEqual(2018, actual);
        }

        [Test]
        public void Test_Resume_Json_Educations_First_EndDateYear_Is_Null_When_Indeed_Does_Not_Provide_It()
        {
            var actual = _app.Applicant.Resume.Json.Educations.Values.Last().EndDateYear;
            Assert.Null(actual);
        }

        [Test]
        public void Test_Resume_Json_Educations_First_Has_Correct_StartDateYear()
        {
            var actual = _app.Applicant.Resume.Json.Educations.Values.First().StartDateYear;
            Assert.AreEqual(2014, actual);
        }

        [Test]
        public void Test_Resume_Json_Educations_First_StartDateYear_Is_Null_When_Indeed_Does_Not_Provide_It()
        {
            var actual = _app.Applicant.Resume.Json.Educations.Values.Last().StartDateYear;
            Assert.Null(actual);
        }

        [Test]
        public void Test_Resume_Json_Educations_First_Has_Correct_Field()
        {
            var actual = _app.Applicant.Resume.Json.Educations.Values.First().Field;
            Assert.AreEqual("Computer Science", actual);
        }

        [Test]
        public void Test_Resume_Json_Educations_First_Has_Correct_School()
        {
            var actual = _app.Applicant.Resume.Json.Educations.Values.First().School;
            Assert.AreEqual("Johns Hopkins University", actual);
        }

        [Test]
        public void Test_Resume_Json_Educations_First_Has_Correct_Location()
        {
            var actual = _app.Applicant.Resume.Json.Educations.Values.First().Location;
            Assert.AreEqual("Austin, TX", actual);
        }

        [Test]
        public void Test_Resume_Json_Link_Has_Correct_Url()
        {
            var actual = _app.Applicant.Resume.Json.Links.Values.First().Url;
            Assert.AreEqual("http://www.indeed.com", actual);
        }

        [Test]
        public void Test_Resume_Json_Patents_First_Title()
        {
            var actual = _app.Applicant.Resume.Json.Patents.Values.First().Title;
            Assert.AreEqual("A Patent", actual);
        }

        [Test]
        public void Test_Resume_Json_Patents_First_Description()
        {
            var actual = _app.Applicant.Resume.Json.Patents.Values.First().Description;
            Assert.AreEqual("A description!", actual);
        }

        [Test]
        public void Test_Resume_Json_Patents_First_PatentNumber()
        {
            var actual = _app.Applicant.Resume.Json.Patents.Values.First().PatentNumber;
            Assert.AreEqual("D12583548", actual);
        }

        [Test]
        public void Test_Resume_Json_Patents_First_Url()
        {
            var actual = _app.Applicant.Resume.Json.Patents.Values.First().Url;
            Assert.AreEqual("http://patents.com", actual);
        }

        [Test]
        public void Test_Resume_Json_Patents_First_DateMonth()
        {
            var actual = _app.Applicant.Resume.Json.Patents.Values.First().DateMonth;
            Assert.AreEqual(9, actual);
        }

        [Test]
        public void Test_Resume_Json_Patents_First_DateMonth_Is_Null_When_Indeed_Has_Not_Provided_It()
        {
            var actual = _app.Applicant.Resume.Json.Patents.Values.Last().DateMonth;
            Assert.Null(actual);
        }

        [Test]
        public void Test_Resume_Json_Patents_First_DateYear_Is_Correct()
        {
            var actual = _app.Applicant.Resume.Json.Patents.Values.First().DateYear;
            Assert.AreEqual(2007, actual);
        }

        [Test]
        public void Test_Resume_Json_Patents_First_DateYear_Is_Null_When_Indeed_Has_Not_Provided_It()
        {
            var actual = _app.Applicant.Resume.Json.Patents.Values.Last().DateYear;
            Assert.Null(actual);
        }

        [Test]
        public void Test_Resume_Json_Publications_First_Title()
        {
            var actual = _app.Applicant.Resume.Json.Publications.Values.First().Title;
            Assert.AreEqual("Publication Example", actual);
        }

        [Test]
        public void Test_Resume_Json_Publications_First_Description()
        {
            var actual = _app.Applicant.Resume.Json.Publications.Values.First().Description;
            Assert.AreEqual("This is an example of what a publication would look like.", actual);
        }

        [Test]
        public void Test_Resume_Json_Publications_First_Url()
        {
            var actual = _app.Applicant.Resume.Json.Publications.Values.First().Url;
            Assert.AreEqual("https://en.wikipedia.org/wiki/Publication", actual);
        }

        [Test]
        public void Test_Resume_Json_Publications_First_DateDay()
        {
            var actual = _app.Applicant.Resume.Json.Publications.Values.First().DateDay;
            Assert.AreEqual(9, actual);
        }

        [Test]
        public void Test_Resume_Json_Publications_First_DateDay_Is_Null_When_Indeed_Has_Not_Provided_It()
        {
            var actual = _app.Applicant.Resume.Json.Publications.Values.Last().DateDay;
            Assert.Null(actual);
        }

        [Test]
        public void Test_Resume_Json_Publications_First_DateMonth()
        {
            var actual = _app.Applicant.Resume.Json.Publications.Values.First().DateMonth;
            Assert.AreEqual(1, actual);
        }

        [Test]
        public void Test_Resume_Json_Publications_First_DateMonth_Is_Null_When_Indeed_Has_Not_Provided_It()
        {
            var actual = _app.Applicant.Resume.Json.Publications.Values.Last().DateMonth;
            Assert.Null(actual);
        }

        [Test]
        public void Test_Resume_Json_Publications_First_DateYear()
        {
            var actual = _app.Applicant.Resume.Json.Publications.Values.First().DateYear;
            Assert.AreEqual(2014, actual);
        }

        [Test]
        public void Test_Resume_Json_Publications_First_DateYear_Is_Null_When_Indeed_Has_Not_Provided_It()
        {
            var actual = _app.Applicant.Resume.Json.Publications.Values.Last().DateYear;
            Assert.Null(actual);
        }

        [Test]
        public void Test_Resume_Json_MilitaryService_First_Branch()
        {
            var actual = _app.Applicant.Resume.Json.MilitaryServices.Values.First().Branch;
            Assert.AreEqual("coast guard", actual);
        }

        [Test]
        public void Test_Resume_Json_MilitaryService_First_Commendations()
        {
            var actual = _app.Applicant.Resume.Json.MilitaryServices.Values.First().Commendations;
            Assert.AreEqual("A commendation", actual);
        }

        [Test]
        public void Test_Resume_Json_MilitaryService_First_ServiceBranch()
        {
            var actual = _app.Applicant.Resume.Json.MilitaryServices.Values.First().ServiceBranch;
            Assert.AreEqual("coast guard service branch", actual);
        }

        [Test]
        public void Test_Resume_Json_MilitaryService_First_Rank()
        {
            var actual = _app.Applicant.Resume.Json.MilitaryServices.Values.First().Rank;
            Assert.AreEqual("colonel", actual);
        }

        [Test]
        public void Test_Resume_Json_MilitaryService_First_Description()
        {
            var actual = _app.Applicant.Resume.Json.MilitaryServices.Values.First().Description;
            Assert.AreEqual("A description!", actual);
        }

        [Test]
        public void Test_Resume_Json_MilitaryService_First_StartDateMonth()
        {
            var actual = _app.Applicant.Resume.Json.MilitaryServices.Values.First().StartDateMonth;
            Assert.AreEqual(1, actual);
        }

        [Test]
        public void Test_Resume_Json_MilitaryService_First_StartDateMonth_Is_Null_When_Indeed_Has_Not_Provided_It()
        {
            var actual = _app.Applicant.Resume.Json.MilitaryServices.Values.Last().StartDateMonth;
            Assert.Null(actual);
        }

        [Test]
        public void Test_Resume_Json_MilitaryService_First_StartDateYear()
        {
            var actual = _app.Applicant.Resume.Json.MilitaryServices.Values.First().StartDateYear;
            Assert.AreEqual(2012, actual);
        }

        [Test]
        public void Test_Resume_Json_MilitaryService_First_StartDateYear_Is_Null_When_Indeed_Has_Not_Provided_It()
        {
            var actual = _app.Applicant.Resume.Json.MilitaryServices.Values.Last().StartDateYear;
            Assert.Null(actual);
        }

        [Test]
        public void Test_Resume_Json_MilitaryService_First_EndDateMonth()
        {
            var actual = _app.Applicant.Resume.Json.MilitaryServices.Values.First().EndDateMonth;
            Assert.AreEqual(1, actual);

        }

        [Test]
        public void Test_Resume_Json_MilitaryService_First_EndDateMonth_Is_Null_When_Indeed_Has_Not_Provided_It()
        {
            var actual = _app.Applicant.Resume.Json.MilitaryServices.Values.Last().EndDateMonth;
            Assert.Null(actual);
        }

        [Test]
        public void Test_Resume_Json_MilitaryService_First_EndDateYear()
        {
            var actual = _app.Applicant.Resume.Json.MilitaryServices.Values.First().EndDateYear;
            Assert.AreEqual(2014, actual);
        }

        [Test]
        public void Test_Resume_Json_MilitaryService_First_EndDateYear_Is_Null_When_Indeed_Has_Not_Provided_It()
        {
            var actual = _app.Applicant.Resume.Json.MilitaryServices.Values.Last().EndDateYear;
            Assert.Null(actual);
        }

        [Test]
        public void Test_Resume_Json_MilitaryService_First_EndCurrent()
        {
            var actual = _app.Applicant.Resume.Json.MilitaryServices.Values.First().EndCurrent;
            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Test_Resume_Json_FirstName()
        {
            var actual = _app.Applicant.Resume.Json.FirstName;
            Assert.AreEqual("John", actual);
        }

        [Test]
        public void Test_Resume_Json_LastName()
        {
            var actual = _app.Applicant.Resume.Json.LastName;
            Assert.AreEqual("Doe", actual);
        }

        [Test]
        public void Test_Resume_Json_Headline()
        {
            var actual = _app.Applicant.Resume.Json.Headline;
            Assert.AreEqual("This is my headline.  I'm great", actual);
        }

        [Test]
        public void Test_Resume_Json_Summary()
        {
            var actual = _app.Applicant.Resume.Json.Summary;
            Assert.AreEqual("Here is my Summary,  Its a bit more detailed.  Again I'm great.", actual);
        }

        [Test]
        public void Test_Resume_Json_PublicProfileUrl()
        {
            var actual = _app.Applicant.Resume.Json.PublicProfileUrl;
            Assert.AreEqual("http://mahprofile.com", actual);
        }

        [Test]
        public void Test_Resume_Json_AdditionalInfo()
        {
            var actual = _app.Applicant.Resume.Json.AdditionalInfo;
            Assert.AreEqual("This is a text box where I can write whatever additional information seems important.", actual);
        }

        [Test]
        public void Test_Resume_Json_PhoneNumber()
        {
            var actual = _app.Applicant.Resume.Json.PhoneNumber;
            Assert.AreEqual("5555555555", actual);
        }

        [Test]
        public void Test_Resume_Json_Location_City()
        {
            var actual = _app.Applicant.Resume.Json.Location.City;
            Assert.AreEqual("Austin, TX", actual);
        }

        [Test]
        public void Test_Resume_Json_Location_Country()
        {
            var actual = _app.Applicant.Resume.Json.Location.Country;
            Assert.AreEqual("US", actual);
        }

        [Test]
        public void Test_Resume_Json_Location_PostalCode()
        {
            var actual = _app.Applicant.Resume.Json.Location.PostalCode;
            Assert.AreEqual("78758", actual);
        }

        [Test]
        public void Test_Resume_Json_Skills()
        {
            var actual = _app.Applicant.Resume.Json.Skills;
            Assert.AreEqual("Microsoft Office (4 years), Testing (1 year), HTML (5 years)", actual);
        }

        #endregion

        #region Questions

        [Test]
        public void Test_First_Question_Has_Correct_Data()
        {
            var result = _app.Questions.Questions.First();

            Assert.IsEmpty(result.HierarchicalOptions);
            Assert.AreEqual("1", result.Id);
            Assert.AreEqual(40, result.Limit);
            Assert.IsEmpty(result.Options);
            Assert.AreEqual("Street", result.Question);
            Assert.True(result.Required == true);
            Assert.AreEqual("text", result.Type);
        }

        [Test]
        public void Test_First_Hierarchical_Question_Has_Correct_Data()
        {
            var result = _app.Questions.Questions.ToArray()[3];
            
            Assert.IsNotEmpty(result.HierarchicalOptions);

            var firstHierOption = result.HierarchicalOptions.First();

            // condition
            Assert.False(firstHierOption.Condition.Exclude);
            Assert.AreEqual("3", firstHierOption.Condition.Id);
            Assert.AreEqual("US", firstHierOption.Condition.Values.First());

            // id
            Assert.AreEqual("4", firstHierOption.Id);

            // options
            Assert.AreEqual(56, firstHierOption.Options.Count());
            Assert.AreEqual("Alabama", firstHierOption.Options.First().Label);
            Assert.AreEqual("AL", firstHierOption.Options.First().Value);


            // root level properties
            Assert.AreEqual("3", result.Id);

            Assert.AreEqual(2, result.Options.Count());
            Assert.AreEqual("United States", result.Options.First().Label);
            Assert.AreEqual("US", result.Options.First().Value);

            Assert.AreEqual("Country/State", result.Question);
            Assert.True(result.Required == true);
            Assert.AreEqual("hierarchical", result.Type);
        }

        #endregion

        #region Answers

        [Test]
        public void Test_First_Answer_Is_Single_Answer()
        {
            var result = _app.Questions.Answers.First();

            Assert.AreEqual("1", result.Id);
            Assert.AreEqual("2600 Esperanza Crossing", result.GetValues().First());
        }

        [Test]
        public void Test_FirstHierarchichal_Answer_Has_Correct_Values()
        {
            var result = _app.Questions.Answers.ToArray()[3];
            Assert.AreEqual(2, result.HierarchicalAnswers.Count());
            Assert.AreEqual("3", result.HierarchicalAnswers.First().Id);
            Assert.AreEqual("US", result.HierarchicalAnswers.First().GetValues().First());
            Assert.AreEqual("4", result.HierarchicalAnswers.Last().Id);
            Assert.AreEqual("TX", result.HierarchicalAnswers.Last().GetValues().First());

            Assert.AreEqual("3", result.Id);
            Assert.AreEqual("US", result.GetValues().First());
            Assert.AreEqual("TX", result.GetValues().Last());
        }

        #endregion
    }
}

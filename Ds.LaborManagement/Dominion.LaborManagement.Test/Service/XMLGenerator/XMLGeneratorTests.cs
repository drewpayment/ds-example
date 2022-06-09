using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Dominion.LaborManagement.Service.Internal;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.Utility.DataGeneration;
using NUnit.Framework;

namespace Dominion.LaborManagement.Test.Service.XMLGenerator
{
    [TestFixture]
    public class XMLGeneratorTests
    {

        private IndeedXMLGeneratorService _service { get; set; }


        [SetUp]
        public void SetUp()
        {
            _service = new IndeedXMLGeneratorService();
        }

        private XDocument resultFromDefaultValues()
        {
            return _service.BuildIndeedXML(new [] { new IndeedXmlJobPost()
            {
                Title = "",
                Date = DateTime.MinValue,
                ReferenceNumber = 0,
                Company = "",
                City = "",
                State = "",
                Country = "",
                PostalCode = "",
                Description = "",
                Salary = "",
                Education = "",
                JobType = "",
                Category = "",
                Experience = 0,
                ClientCode = "",
                SourceName = ""
            } }, "", DateTime.Now).Data;
        }

        [Test]
        public void Test_XMLGenerator_BuildIndeedXML_Returns_Something()
        {
            var result = resultFromDefaultValues();

            Assert.NotNull(result);
        }

        [Test]
        public void Test_XMLGenerator_BuildIndeedXML_Returns_Document_With_Xml_Version_1()
        {
            var result = resultFromDefaultValues();


            Assert.AreEqual("1.0", result.Declaration.Version);
        }

        [Test]
        public void Test_XMLGenerator_BuildIndeedXML_Returns_Document_With_Xml_Encoding_utf8()
        {
            var result = resultFromDefaultValues();


            Assert.AreEqual("utf-8", result.Declaration.Encoding);
        }

        [Test]
        public void Test_XMLGenerator_BuildIndeedXML_Declaration_Matches_Required_First_Line_Exactly()
        {
            var result = resultFromDefaultValues();


            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>", result.Declaration.ToString());
        }

        [Test]
        public void Test_XMLGenerator_BuildIndeedXML_Root_Is_Source_Element()
        {
            var result = resultFromDefaultValues();

            Assert.NotNull(result.Root);
            Assert.AreEqual(new XElement("source").Name.ToString(), result.Root.Name.ToString());
        }

        [Test]
        public void Test_XMLGenerator_BuildIndeedXML_First_Content_Element_Is_Publisher_With_Dominion_ATS_As_Value()
        {
            var result = resultFromDefaultValues();

            Assert.AreEqual(new XElement("publisher", "Dominion ATS").ToString(), result.Root.DescendantNodes().First().ToString());
        }

        [Test]
        public void Test_XMLGenerator_BuildIndeedXML_Whole_Document_With_Fake_Data_Correct()
        {
            var post = new IndeedXmlJobPost()
            {
                Title = "Sales Executive",
                Date = new DateTime(2005, 12, 10, 22, 49, 39),
                ReferenceNumber = 123131,
                Company = "Big ABC Corporation",
                City = "Phoenix",
                State = "AZ",
                Country = "US",
                PostalCode = "85003",
                Description = @"Do you have 5-7 years of sales experience? Are you
relentless at closing the deal? Are you ready for an exciting and
high-speed career in sales? If so, we want to hear from you!

We provide competitive compensation, including stock options and a full
benefit plan. As a fast-growing business, we offer excellent
opportunities for exciting and challenging work. As our company
continues to grow, you can expect unlimited career advancement!
",
                Salary = "$70K per year",
                Education = "Bachelors",
                JobType = "fulltime",
                Category = "Sales Management, Executive",
                Experience = 5,
                ClientCode = "TestCode",
                ClientEmail = "careers@dominionsystems.com",
                SourceName = "sourceName",
                ResumeRequiredId = 2,
                ApplicationId = 1
            };

            _service = new IndeedXMLGeneratorService();

            XDocument result = _service.BuildIndeedXML( new [] { post }, "56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9", new DateTime(2004, 12, 10, 22, 49, 39)).Data;

            var expected = @"<?xml version=""1.0"" encoding=""utf-8""?><source><publisher>Dominion ATS</publisher><publisherurl>https://www.dominionsystems.com/applicant-tracking-system/</publisherurl><lastBuildDate>Fri, 10 Dec 2004 22:49:39 EDT</lastBuildDate><job><title><![CDATA[Sales Executive]]></title><date><![CDATA[Sat, 10 Dec 2005 22:49:39 EDT]]></date><referencenumber><![CDATA[123131]]></referencenumber><url><![CDATA[http://www.localhost.com/DominionPayroll/applicantPostingListNL.aspx?code=TestCode&posting=123131]]></url><company><![CDATA[Big ABC Corporation]]></company><sourcename><![CDATA[sourceName]]></sourcename><city><![CDATA[Phoenix]]></city><state><![CDATA[AZ]]></state><country><![CDATA[US]]></country><postalcode><![CDATA[85003]]></postalcode><email><![CDATA[careers@dominionsystems.com]]></email><description><![CDATA[Do you have 5-7 years of sales experience? Are you
relentless at closing the deal? Are you ready for an exciting and
high-speed career in sales? If so, we want to hear from you!

We provide competitive compensation, including stock options and a full
benefit plan. As a fast-growing business, we offer excellent
opportunities for exciting and challenging work. As our company
continues to grow, you can expect unlimited career advancement!]]></description><salary><![CDATA[$70K per year]]></salary><education><![CDATA[Bachelors]]></education><jobtype><![CDATA[fulltime]]></jobtype><category><![CDATA[Sales Management, Executive]]></category><experience><![CDATA[5+ years]]></experience><indeed-apply-data><![CDATA[indeed-apply-apiToken=56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9&indeed-apply-jobTitle=Sales+Executive&indeed-apply-jobId=123131&indeed-apply-jobCompanyName=Big+ABC+Corporation&indeed-apply-jobLocation=Phoenix%2C+AZ&indeed-apply-jobUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2FapplicantPostingListNL.aspx%3Fcode%3DTestCode%26posting%3D123131&indeed-apply-postUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication&indeed-apply-phone=required&indeed-apply-coverletter=optional&indeed-apply-name=firstlastname&indeed-apply-questions=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication%2Fapplication%2F1%2Fposting%2F123131&indeed-apply-resume=hidden]]></indeed-apply-data></job></source>";

            using (var sw = new StringWriterWithEncoding(Encoding.UTF8))
            {
                using (var writer = XmlWriter.Create(sw, new XmlWriterSettings()
                {
                    NewLineHandling = NewLineHandling.None
                })) {

                    result.WriteTo(writer);
                }
                var actual = sw.ToString();
                StringAssert.AreEqualIgnoringCase(expected, actual);
            }
        }

        [Test]
        public void Test_XMLGenerator_BuildIndeedXML_Whole_Document_With_Changed_Data()
        {
            var post = new IndeedXmlJobPost()
            {
                Title = "b",
                Date = new DateTime(2006, 12, 10, 22, 49, 39),
                ReferenceNumber = 123131,
                Company = "b",
                City = "b",
                State = "Alberta",
                Country = "CA",
                PostalCode = "b",
                Description = "b",
                Salary = "b",
                Education = "b",
                JobType = "b",
                Category = "b",
                Experience = 5,
                ClientEmail = "careers@dominionsystems.com",
                ClientCode = "TestCode",
                SourceName = "sourceName",
                ResumeRequiredId = 2,
                ApplicationId = 1
            };

            _service = new IndeedXMLGeneratorService();

            XDocument result = _service.BuildIndeedXML(new [] { post }, "56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9", new DateTime(2006, 12, 10, 22, 49, 39)).Data;

            var expected = @"<?xml version=""1.0"" encoding=""utf-8""?><source><publisher>Dominion ATS</publisher><publisherurl>https://www.dominionsystems.com/applicant-tracking-system/</publisherurl><lastBuildDate>Sun, 10 Dec 2006 22:49:39 EDT</lastBuildDate><job><title><![CDATA[b]]></title><date><![CDATA[Sun, 10 Dec 2006 22:49:39 EDT]]></date><referencenumber><![CDATA[123131]]></referencenumber><url><![CDATA[http://www.localhost.com/DominionPayroll/applicantPostingListNL.aspx?code=TestCode&posting=123131]]></url><company><![CDATA[b]]></company><sourcename><![CDATA[sourceName]]></sourcename><city><![CDATA[b]]></city><state><![CDATA[Alberta]]></state><country><![CDATA[CA]]></country><postalcode><![CDATA[b]]></postalcode><email><![CDATA[careers@dominionsystems.com]]></email><description><![CDATA[b]]></description><salary><![CDATA[b]]></salary><education><![CDATA[b]]></education><jobtype><![CDATA[b]]></jobtype><category><![CDATA[b]]></category><experience><![CDATA[5+ years]]></experience><indeed-apply-data><![CDATA[indeed-apply-apiToken=56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9&indeed-apply-jobTitle=b&indeed-apply-jobId=123131&indeed-apply-jobCompanyName=b&indeed-apply-jobLocation=b%2C+Alberta&indeed-apply-jobUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2FapplicantPostingListNL.aspx%3Fcode%3DTestCode%26posting%3D123131&indeed-apply-postUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication&indeed-apply-phone=required&indeed-apply-coverletter=optional&indeed-apply-name=firstlastname&indeed-apply-questions=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication%2Fapplication%2F1%2Fposting%2F123131&indeed-apply-resume=hidden]]></indeed-apply-data></job></source>";

            using (var sw = new StringWriterWithEncoding(Encoding.UTF8))
            {
                using (var writer = XmlWriter.Create(sw, new XmlWriterSettings()
                {
                    NewLineHandling = NewLineHandling.None
                }))
                {

                    result.WriteTo(writer);
                }
                var actual = sw.ToString();
                StringAssert.AreEqualIgnoringCase(expected, actual);
            }
        }

        [Test]
        public void Test_XMLGenerator_BuildIndeedXML_Whole_Document_With_Two_Jobs()
        {
            var post1 = new IndeedXmlJobPost()
            {
                Title = "b",
                Date = new DateTime(2006, 12, 10, 22, 49, 39),
                ReferenceNumber = 123131,
                Company = "b",
                City = "b",
                State = "Alberta",
                Country = "CA",
                PostalCode = "b",
                Description = "b",
                Salary = "b",
                Education = "b",
                JobType = "b",
                Category = "b",
                Experience = 4,
                ClientCode = "TestCode",
                ClientEmail = "careers@dominionsystems.com",
                SourceName = "sourceName",
                ApplicationId = 1
            };

            var post2 = new IndeedXmlJobPost()
            {
                Title = "a", // applicantposting description
                Date = new DateTime(2006, 12, 10, 22, 49, 39), // applicantposting publishstart (if that is null use publisheddate?)
                ReferenceNumber = 123132, // applicantposting postingid
                Company = "a", // client name
                City = "a", // clientdivision
                State = "FL", // clientdivision
                Country = "US", // clientdivision
                PostalCode = "a", // clientdivision
                Description = "a", // applicantposting jobrequirements
                Salary = "a", // applicantposting
                Education = "a", // applicantposting
                JobType = "a", // applicantjobtype
                Category = "a", // applicantpostingcategory
                Experience = 5, // desired experience for job, years of employment
                ClientCode = "TestCode",
                ClientEmail = "careers@dominionsystems.com",
                SourceName = "sourceName",
                ResumeRequiredId = 2,
                ApplicationId = 1
            };

            _service = new IndeedXMLGeneratorService();

            //email ? maybe we need to add this field?

            XDocument result = _service.BuildIndeedXML(new [] { post1, post2 }, "56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9", new DateTime(2006, 12, 10, 22, 49, 39)).Data;

            var expected = @"<?xml version=""1.0"" encoding=""utf-8""?><source><publisher>Dominion ATS</publisher><publisherurl>https://www.dominionsystems.com/applicant-tracking-system/</publisherurl><lastBuildDate>Sun, 10 Dec 2006 22:49:39 EDT</lastBuildDate><job><title><![CDATA[b]]></title><date><![CDATA[Sun, 10 Dec 2006 22:49:39 EDT]]></date><referencenumber><![CDATA[123131]]></referencenumber><url><![CDATA[http://www.localhost.com/DominionPayroll/applicantPostingListNL.aspx?code=TestCode&posting=123131]]></url><company><![CDATA[b]]></company><sourcename><![CDATA[sourceName]]></sourcename><city><![CDATA[b]]></city><state><![CDATA[Alberta]]></state><country><![CDATA[CA]]></country><postalcode><![CDATA[b]]></postalcode><email><![CDATA[careers@dominionsystems.com]]></email><description><![CDATA[b]]></description><salary><![CDATA[b]]></salary><education><![CDATA[b]]></education><jobtype><![CDATA[b]]></jobtype><category><![CDATA[b]]></category><experience><![CDATA[4+ years]]></experience><indeed-apply-data><![CDATA[indeed-apply-apiToken=56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9&indeed-apply-jobTitle=b&indeed-apply-jobId=123131&indeed-apply-jobCompanyName=b&indeed-apply-jobLocation=b%2C+Alberta&indeed-apply-jobUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2FapplicantPostingListNL.aspx%3Fcode%3DTestCode%26posting%3D123131&indeed-apply-postUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication&indeed-apply-phone=required&indeed-apply-coverletter=optional&indeed-apply-name=firstlastname&indeed-apply-questions=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication%2Fapplication%2F1%2Fposting%2F123131&indeed-apply-resume=hidden]]></indeed-apply-data></job><job><title><![CDATA[a]]></title><date><![CDATA[Sun, 10 Dec 2006 22:49:39 EDT]]></date><referencenumber><![CDATA[123132]]></referencenumber><url><![CDATA[http://www.localhost.com/DominionPayroll/applicantPostingListNL.aspx?code=TestCode&posting=123132]]></url><company><![CDATA[a]]></company><sourcename><![CDATA[sourceName]]></sourcename><city><![CDATA[a]]></city><state><![CDATA[FL]]></state><country><![CDATA[US]]></country><postalcode><![CDATA[a]]></postalcode><email><![CDATA[careers@dominionsystems.com]]></email><description><![CDATA[a]]></description><salary><![CDATA[a]]></salary><education><![CDATA[a]]></education><jobtype><![CDATA[a]]></jobtype><category><![CDATA[a]]></category><experience><![CDATA[5+ years]]></experience><indeed-apply-data><![CDATA[indeed-apply-apiToken=56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9&indeed-apply-jobTitle=a&indeed-apply-jobId=123132&indeed-apply-jobCompanyName=a&indeed-apply-jobLocation=a%2C+FL&indeed-apply-jobUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2FapplicantPostingListNL.aspx%3Fcode%3DTestCode%26posting%3D123132&indeed-apply-postUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication&indeed-apply-phone=required&indeed-apply-coverletter=optional&indeed-apply-name=firstlastname&indeed-apply-questions=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication%2Fapplication%2F1%2Fposting%2F123132&indeed-apply-resume=hidden]]></indeed-apply-data></job></source>";

            using (var sw = new StringWriterWithEncoding(Encoding.UTF8))
            {
                using (var writer = XmlWriter.Create(sw, new XmlWriterSettings()
                {
                    NewLineHandling = NewLineHandling.None
                }))
                {

                    result.WriteTo(writer);
                }
                var actual = sw.ToString();
                StringAssert.AreEqualIgnoringCase(expected, actual);
            }
        }

        
        [Test]
        public void Test_Indeed_Apply_Data_Element_Has_Correct_Name()
        {
            var post = new IndeedXmlJobPost()
            {
                Title = "b",
                Date = new DateTime(2006, 12, 10, 22, 49, 39),
                ReferenceNumber = 123131,
                Company = "b",
                City = "b",
                State = "Alberta",
                Country = "CA",
                PostalCode = "b",
                Description = "b",
                Salary = "b",
                Education = "b",
                JobType = "b",
                Category = "b",
                Experience = 4,
                ClientCode = "TestCode",
                ClientEmail = "careers@dominionsystems.com",
                SourceName = "sourceName",
                ResumeRequiredId = 2
            };

            _service = new IndeedXMLGeneratorService();

            XDocument result = _service.BuildIndeedXML(new[] { post }, "", new DateTime(2006, 12, 10, 22, 49, 39)).Data;

            var expected =
                "<indeed-apply-data></indeed-apply-data>";

            var actual = result.Root.DescendantsAndSelf().Last().Name.LocalName;

            Assert.AreEqual("indeed-apply-data", actual);
        }

        /// <summary>
        /// The 'expected' strings below were tested by generating a string using Indeed's xml indeed apply generator tool. <see href="https://apply.indeed.com/indeedapply/button-generate-tests"/>
        /// This tool requires our real token and string.  Since this tool is created and managed by Indeed it should be ok to put our secret in this online tool.  
        /// But for any other testing using an online tool that is not created/managed by Indeed DO NOT PROVIDE OUR REAL SECRET
        /// </summary>
        [Test]
        public void Test_Indeed_Apply_Data_Element_Data_Is_Wrapped_In_CDATA()
        {
            var post = new IndeedXmlJobPost()
            {
                Title = "b",
                Date = new DateTime(2006, 12, 10, 22, 49, 39),
                ReferenceNumber = 123131,
                Company = "b",
                City = "b",
                State = "Alberta",
                Country = "CA",
                PostalCode = "b",
                Description = "b",
                Salary = "b",
                Education = "b",
                JobType = "b",
                Category = "b",
                Experience = 4,
                ClientCode = "TestCode",
                ClientEmail = "careers@dominionsystems.com",
                SourceName = "sourceName",
                ResumeRequiredId = 2
            };

            _service = new IndeedXMLGeneratorService();

            XDocument result = _service.BuildIndeedXML(new[] { post }, "", new DateTime(2006, 12, 10, 22, 49, 39)).Data;

            var expected =
                "<indeed-apply-data><![CDATA[indeed-apply-apiToken=56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9&indeed-apply-jobTitle=b&indeed-apply-jobId=123131&indeed-apply-jobCompanyName=b&indeed-apply-jobLocation=Alberta%2C+CA&indeed-apply-jobUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2FapplicantPostingListNL.aspx%3Fcode%3DTIME%26posting%3D2939%2F&indeed-apply-email=df267f68d955a490f61202b5b423fe3c&indeed-apply-postUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication%2F]]></indeed-apply-data>";

            var actual = result.Root.DescendantsAndSelf().Last().FirstNode.NodeType;

            Assert.AreEqual(XmlNodeType.CDATA, actual);
        }

        [Test]
        public void Test_Indeed_Apply_Data_Element_Value_Has_Correct_Value()
        {
            var post = new IndeedXmlJobPost()
            {
                Title = "b",
                Date = new DateTime(2006, 12, 10, 22, 49, 39),
                ReferenceNumber = 123131,
                Company = "b",
                City = "awesomeCiy",
                State = "Alberta",
                Country = "CA",
                PostalCode = "b",
                Description = "b",
                Salary = "b",
                Education = "b",
                JobType = "b",
                Category = "b",
                Experience = 4,
                ClientCode = "TIME",
                ClientEmail = "careers@dominionsystems.com",
                SourceName = "sourceName",
                ResumeRequiredId = 2,
                ApplicationId = 1
            };

            _service = new IndeedXMLGeneratorService();

            XDocument result = _service.BuildIndeedXML(new[] { post }, "56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9", new DateTime(2006, 12, 10, 22, 49, 39)).Data;

            var expected =
                "<indeed-apply-data><![CDATA[indeed-apply-apiToken=56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9&indeed-apply-jobTitle=b&indeed-apply-jobId=123131&indeed-apply-jobCompanyName=b&indeed-apply-jobLocation=awesomeCiy%2C+Alberta&indeed-apply-jobUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2FapplicantPostingListNL.aspx%3Fcode%3DTIME%26posting%3D123131&indeed-apply-postUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication&indeed-apply-phone=required&indeed-apply-coverletter=optional&indeed-apply-name=firstlastname&indeed-apply-questions=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication%2Fapplication%2F1%2Fposting%2F123131&indeed-apply-resume=hidden]]></indeed-apply-data>";

            var actual = result.Root.DescendantsAndSelf().Last().ToString();

            StringAssert.AreEqualIgnoringCase(expected, actual);
        }

        [Test]
        public void Test_Indeed_Apply_Data_Element_Value_Has_Resume_Optional_When_ResumeRequiredId_Is_1()
        {
            var post = new IndeedXmlJobPost()
            {
                Title = "b",
                Date = new DateTime(2006, 12, 10, 22, 49, 39),
                ReferenceNumber = 123131,
                Company = "b",
                City = "awesomeCiy",
                State = "Alberta",
                Country = "CA",
                PostalCode = "b",
                Description = "b",
                Salary = "b",
                Education = "b",
                JobType = "b",
                Category = "b",
                Experience = 4,
                ClientCode = "TIME",
                ClientEmail = "careers@dominionsystems.com",
                SourceName = "sourceName",
                ResumeRequiredId = 1,
                ApplicationId = 1
            };

            _service = new IndeedXMLGeneratorService();

            XDocument result = _service.BuildIndeedXML(new[] { post }, "56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9", new DateTime(2006, 12, 10, 22, 49, 39)).Data;

            var expected =
                "<indeed-apply-data><![CDATA[indeed-apply-apiToken=56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9&indeed-apply-jobTitle=b&indeed-apply-jobId=123131&indeed-apply-jobCompanyName=b&indeed-apply-jobLocation=awesomeCiy%2C+Alberta&indeed-apply-jobUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2FapplicantPostingListNL.aspx%3Fcode%3DTIME%26posting%3D123131&indeed-apply-postUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication&indeed-apply-phone=required&indeed-apply-coverletter=optional&indeed-apply-name=firstlastname&indeed-apply-questions=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication%2Fapplication%2F1%2Fposting%2F123131&indeed-apply-resume=optional]]></indeed-apply-data>";

            var actual = result.Root.DescendantsAndSelf().Last().ToString();

            StringAssert.AreEqualIgnoringCase(expected, actual);
        }

        [TestCase(3)]
        [TestCase(4)]
        public void Test_Indeed_Apply_Data_Element_Value_Has_Resume_Required_When_ResumeRequiredId_Is_3_Or_4(int resumeRequiredId)
        {
            var post = new IndeedXmlJobPost()
            {
                Title = "b",
                Date = new DateTime(2006, 12, 10, 22, 49, 39),
                ReferenceNumber = 123131,
                Company = "b",
                City = "awesomeCiy",
                State = "Alberta",
                Country = "CA",
                PostalCode = "b",
                Description = "b",
                Salary = "b",
                Education = "b",
                JobType = "b",
                Category = "b",
                Experience = 4,
                ClientCode = "TIME",
                ClientEmail = "careers@dominionsystems.com",
                SourceName = "sourceName",
                ResumeRequiredId = resumeRequiredId,
                ApplicationId = 1
            };

            _service = new IndeedXMLGeneratorService();

            XDocument result = _service.BuildIndeedXML(new[] { post }, "56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9", new DateTime(2006, 12, 10, 22, 49, 39)).Data;

            var expected =
                "<indeed-apply-data><![CDATA[indeed-apply-apiToken=56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9&indeed-apply-jobTitle=b&indeed-apply-jobId=123131&indeed-apply-jobCompanyName=b&indeed-apply-jobLocation=awesomeCiy%2C+Alberta&indeed-apply-jobUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2FapplicantPostingListNL.aspx%3Fcode%3DTIME%26posting%3D123131&indeed-apply-postUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication&indeed-apply-phone=required&indeed-apply-coverletter=optional&indeed-apply-name=firstlastname&indeed-apply-questions=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication%2Fapplication%2F1%2Fposting%2F123131&indeed-apply-resume=required]]></indeed-apply-data>";

            var actual = result.Root.DescendantsAndSelf().Last().ToString();

            StringAssert.AreEqualIgnoringCase(expected, actual);
        }

        [Test]
        public void Test_Indeed_Apply_Data_Element_Value_Has_Resume_Hidden_When_ResumeRequiredId_Is_2()
        {
            var post = new IndeedXmlJobPost()
            {
                Title = "b",
                Date = new DateTime(2006, 12, 10, 22, 49, 39),
                ReferenceNumber = 123131,
                Company = "b",
                City = "awesomeCiy",
                State = "Alberta",
                Country = "CA",
                PostalCode = "b",
                Description = "b",
                Salary = "b",
                Education = "b",
                JobType = "b",
                Category = "b",
                Experience = 4,
                ClientCode = "TIME",
                ClientEmail = "careers@dominionsystems.com",
                SourceName = "sourceName",
                ResumeRequiredId = 2,
                ApplicationId = 1
            };

            _service = new IndeedXMLGeneratorService();

            XDocument result = _service.BuildIndeedXML(new[] { post }, "56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9", new DateTime(2006, 12, 10, 22, 49, 39)).Data;

            var expected =
                "<indeed-apply-data><![CDATA[indeed-apply-apiToken=56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9&indeed-apply-jobTitle=b&indeed-apply-jobId=123131&indeed-apply-jobCompanyName=b&indeed-apply-jobLocation=awesomeCiy%2C+Alberta&indeed-apply-jobUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2FapplicantPostingListNL.aspx%3Fcode%3DTIME%26posting%3D123131&indeed-apply-postUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication&indeed-apply-phone=required&indeed-apply-coverletter=optional&indeed-apply-name=firstlastname&indeed-apply-questions=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication%2Fapplication%2F1%2Fposting%2F123131&indeed-apply-resume=hidden]]></indeed-apply-data>";

            var actual = result.Root.DescendantsAndSelf().Last().ToString();

            StringAssert.AreEqualIgnoringCase(expected, actual);
        }

        [Test]
        public void Test_Indeed_Apply_Data_Element_Value_Has_Phone_Required()
        {
            var post = new IndeedXmlJobPost()
            {
                Title = "b",
                Date = new DateTime(2006, 12, 10, 22, 49, 39),
                ReferenceNumber = 123131,
                Company = "b",
                City = "awesomeCiy",
                State = "Alberta",
                Country = "CA",
                PostalCode = "b",
                Description = "b",
                Salary = "b",
                Education = "b",
                JobType = "b",
                Category = "b",
                Experience = 4,
                ClientCode = "TIME",
                ClientEmail = "careers@dominionsystems.com",
                SourceName = "sourceName",
                ResumeRequiredId = 2,
                ApplicationId = 1
            };

            _service = new IndeedXMLGeneratorService();

            XDocument result = _service.BuildIndeedXML(new[] { post }, "56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9", new DateTime(2006, 12, 10, 22, 49, 39)).Data;

            var expected =
                "<indeed-apply-data><![CDATA[indeed-apply-apiToken=56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9&indeed-apply-jobTitle=b&indeed-apply-jobId=123131&indeed-apply-jobCompanyName=b&indeed-apply-jobLocation=awesomeCiy%2C+Alberta&indeed-apply-jobUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2FapplicantPostingListNL.aspx%3Fcode%3DTIME%26posting%3D123131&indeed-apply-postUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication&indeed-apply-phone=required&indeed-apply-coverletter=optional&indeed-apply-name=firstlastname&indeed-apply-questions=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication%2Fapplication%2F1%2Fposting%2F123131&indeed-apply-resume=hidden]]></indeed-apply-data>";

            var actual = result.Root.DescendantsAndSelf().Last().ToString();

            StringAssert.AreEqualIgnoringCase(expected, actual);
        }

        [Test]
        public void Test_Indeed_Apply_Data_Element_Value_Has_CoverLetter_Optional()
        {
            var post = new IndeedXmlJobPost()
            {
                Title = "b",
                Date = new DateTime(2006, 12, 10, 22, 49, 39),
                ReferenceNumber = 123131,
                Company = "b",
                City = "awesomeCiy",
                State = "Alberta",
                Country = "CA",
                PostalCode = "b",
                Description = "b",
                Salary = "b",
                Education = "b",
                JobType = "b",
                Category = "b",
                Experience = 4,
                ClientCode = "TIME",
                ClientEmail = "careers@dominionsystems.com",
                SourceName = "sourceName",
                ResumeRequiredId = 2,
                ApplicationId = 1
            };

            _service = new IndeedXMLGeneratorService();

            XDocument result = _service.BuildIndeedXML(new[] { post }, "56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9", new DateTime(2006, 12, 10, 22, 49, 39)).Data;

            var expected =
                "<indeed-apply-data><![CDATA[indeed-apply-apiToken=56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9&indeed-apply-jobTitle=b&indeed-apply-jobId=123131&indeed-apply-jobCompanyName=b&indeed-apply-jobLocation=awesomeCiy%2C+Alberta&indeed-apply-jobUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2FapplicantPostingListNL.aspx%3Fcode%3DTIME%26posting%3D123131&indeed-apply-postUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication&indeed-apply-phone=required&indeed-apply-coverletter=optional&indeed-apply-name=firstlastname&indeed-apply-questions=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication%2Fapplication%2F1%2Fposting%2F123131&indeed-apply-resume=hidden]]></indeed-apply-data>";

            var actual = result.Root.DescendantsAndSelf().Last().ToString();

            StringAssert.AreEqualIgnoringCase(expected, actual);
        }

        [Test]
        public void Test_Indeed_Apply_Data_Element_Value_Has_Name_Split()
        {
            var post = new IndeedXmlJobPost()
            {
                Title = "b",
                Date = new DateTime(2006, 12, 10, 22, 49, 39),
                ReferenceNumber = 123131,
                Company = "b",
                City = "awesomeCiy",
                State = "Alberta",
                Country = "CA",
                PostalCode = "b",
                Description = "b",
                Salary = "b",
                Education = "b",
                JobType = "b",
                Category = "b",
                Experience = 4,
                ClientCode = "TIME",
                ClientEmail = "careers@dominionsystems.com",
                SourceName = "sourceName",
                ResumeRequiredId = 2,
                ApplicationId = 1
            };

            _service = new IndeedXMLGeneratorService();

            XDocument result = _service.BuildIndeedXML(new[] { post }, "56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9", new DateTime(2006, 12, 10, 22, 49, 39)).Data;

            var expected =
                "<indeed-apply-data><![CDATA[indeed-apply-apiToken=56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9&indeed-apply-jobTitle=b&indeed-apply-jobId=123131&indeed-apply-jobCompanyName=b&indeed-apply-jobLocation=awesomeCiy%2C+Alberta&indeed-apply-jobUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2FapplicantPostingListNL.aspx%3Fcode%3DTIME%26posting%3D123131&indeed-apply-postUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication&indeed-apply-phone=required&indeed-apply-coverletter=optional&indeed-apply-name=firstlastname&indeed-apply-questions=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication%2Fapplication%2F1%2Fposting%2F123131&indeed-apply-resume=hidden]]></indeed-apply-data>";

            var actual = result.Root.DescendantsAndSelf().Last().ToString();

            StringAssert.AreEqualIgnoringCase(expected, actual);
        }

        [Test]
        public void Test_Indeed_Apply_Data_Element_Value_Has_Correct_QuestionUrl()
        {
            var post = new IndeedXmlJobPost()
            {
                Title = "b",
                Date = new DateTime(2006, 12, 10, 22, 49, 39),
                ReferenceNumber = 123131,
                Company = "b",
                City = "awesomeCiy",
                State = "Alberta",
                Country = "CA",
                PostalCode = "b",
                Description = "b",
                Salary = "b",
                Education = "b",
                JobType = "b",
                Category = "b",
                Experience = 4,
                ClientCode = "TIME",
                ClientEmail = "careers@dominionsystems.com",
                SourceName = "sourceName",
                ResumeRequiredId = 2,
                ApplicationId = 1
            };

            _service = new IndeedXMLGeneratorService();

            XDocument result = _service.BuildIndeedXML(new[] { post }, "56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9", new DateTime(2006, 12, 10, 22, 49, 39)).Data;

            var expected =
                "<indeed-apply-data><![CDATA[indeed-apply-apiToken=56ab16b9a477e0ffe8f45d2bf22fe0ffd5fc2a105b2f3986719feebbbca844f9&indeed-apply-jobTitle=b&indeed-apply-jobId=123131&indeed-apply-jobCompanyName=b&indeed-apply-jobLocation=awesomeCiy%2C+Alberta&indeed-apply-jobUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2FapplicantPostingListNL.aspx%3Fcode%3DTIME%26posting%3D123131&indeed-apply-postUrl=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication&indeed-apply-phone=required&indeed-apply-coverletter=optional&indeed-apply-name=firstlastname&indeed-apply-questions=http%3A%2F%2Fwww.localhost.com%2FDominionPayroll%2Fapi%2Findeedapplication%2Fapplication%2F1%2Fposting%2F123131&indeed-apply-resume=hidden]]></indeed-apply-data>";

            var actual = result.Root.DescendantsAndSelf().Last().ToString();

            StringAssert.AreEqualIgnoringCase(expected, actual);
        }
    }
}

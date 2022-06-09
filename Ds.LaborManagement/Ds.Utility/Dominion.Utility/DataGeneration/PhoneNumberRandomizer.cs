//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Dominion.Utility.TestDataGeneration
//{
//    public class PhoneNumberRandomizer
//    {
//        public static string BasicUsaFormat()
//        {
//            var areaCode = StringRandomizer.GenerateFromChars(RandomStringCharacterSets.NumericOnly, 3);
//            var prefix = StringRandomizer.GenerateFromChars(RandomStringCharacterSets.NumericOnly, 3);
//            var number = StringRandomizer.GenerateFromChars(RandomStringCharacterSets.NumericOnly, 4);

//            return string.Format("{0}-{1}-{2}", areaCode, prefix, number);
//        }
//    }
//}

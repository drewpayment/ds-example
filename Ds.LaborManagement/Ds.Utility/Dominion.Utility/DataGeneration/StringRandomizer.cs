using System;

namespace Dominion.Utility.DataGeneration
{
    public class StringRandomizer
    {
        #region Variables And Properties

        private static Random _random = new Random();
        private static string _alphaNumericAll = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private static string _alphaNumericOnlyUpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static string _alphaNumericOnlyLowerCase = "abcdefghijklmnopqrstuvwxyz0123456789";

        private static string _alphaAll = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private static string _alphaUpperOnly = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string _alphaLowerOnly = "abcdefghijklmnopqrstuvwxyz";

        private static string _numericOnly = "0123456789";

        private static string _gender = "MF";

        #endregion

        #region Methods

        public static string GenerateFromChars(RandomStringCharacterSets charSet, int maxChars)
        {
            return GenerateFromChars(charSet, 1, maxChars);
        }

        public static string GenerateFromChars(RandomStringCharacterSets charSet, int minChars, int maxChars)
        {
            var stringChars = new char[maxChars];

            for(int i = 0; i < stringChars.Length; i++)
                stringChars[i] = GetChar(charSet);

            return new string(stringChars);
        }

        public static string PhoneBasicUsaFormat()
        {
            var areaCode = StringRandomizer.GenerateFromChars(RandomStringCharacterSets.NumericOnly, 3);
            var prefix = StringRandomizer.GenerateFromChars(RandomStringCharacterSets.NumericOnly, 3);
            var number = StringRandomizer.GenerateFromChars(RandomStringCharacterSets.NumericOnly, 4);

            return string.Format("{0}-{1}-{2}", areaCode, prefix, number);
        }

        public static string SSN()
        {
            var part1 = StringRandomizer.GenerateFromChars(RandomStringCharacterSets.NumericOnly, 3);
            var part2 = StringRandomizer.GenerateFromChars(RandomStringCharacterSets.NumericOnly, 2);
            var part3 = StringRandomizer.GenerateFromChars(RandomStringCharacterSets.NumericOnly, 4);

            return string.Format("{0}-{1}-{2}", part1, part2, part3);
        }

        public static string GenderSingleLetter()
        {
            var gender = GenerateFromChars(RandomStringCharacterSets.Gender, 1);
            return gender;
        }

        private static char GetChar(RandomStringCharacterSets charSet)
        {
            switch(charSet)
            {
                //SINGLE LETTER GENDER 
                case RandomStringCharacterSets.Gender:
                    return _gender[_random.Next(_gender.Length)];

                //NUMERIC
                case RandomStringCharacterSets.NumericOnly:
                    return _numericOnly[_random.Next(_numericOnly.Length)];

                //ALPHA
                case RandomStringCharacterSets.AlphaAll:
                    return _alphaAll[_random.Next(_alphaAll.Length)];

                case RandomStringCharacterSets.AlphaUpperOnly:
                    return _alphaUpperOnly[_random.Next(_alphaUpperOnly.Length)];

                case RandomStringCharacterSets.AlphaLowerOnly:
                    return _alphaLowerOnly[_random.Next(_alphaLowerOnly.Length)];

                //ALPHA NUMERIC
                case RandomStringCharacterSets.AlphaNumericUpperOnly:
                    return _alphaNumericOnlyUpperCase[_random.Next(_alphaNumericOnlyUpperCase.Length)];

                case RandomStringCharacterSets.AlphaNumericLowerOnly:
                    return _alphaNumericOnlyLowerCase[_random.Next(_alphaNumericOnlyLowerCase.Length)];

                //DEFAULT TO ALPHA NUMERIC ALL
                case RandomStringCharacterSets.AlphaNumericAll:
                default:
                    return _alphaNumericAll[_random.Next(_alphaNumericAll.Length)];
            }
        }

        #endregion
    }
    
    public enum RandomStringCharacterSets
    {
        AlphaNumericAll,
        AlphaNumericUpperOnly,
        AlphaNumericLowerOnly,
        AlphaAll,
        AlphaUpperOnly,
        AlphaLowerOnly,
        NumericOnly,
        Gender
    }

}

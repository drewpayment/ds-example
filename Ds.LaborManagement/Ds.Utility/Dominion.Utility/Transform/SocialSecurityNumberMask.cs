using System;
using Dominion.Utility.Constants;

namespace Dominion.Utility.Transform
{
    /// <summary>
    /// SSN mask options.
    /// </summary>
    public enum SocialSecurityNumberMaskOption
    {
        /// <summary>
        /// eg: xxx-xx-xxxx
        /// </summary>
        MaskAll, 

        /// <summary>
        /// eg: xxx-xx-1234
        /// </summary>
        MaskAllButLastDigits
    }

    /// <summary>
    /// Container class used to define SSN mask settings such as what digits to mask and what mask character to use.
    /// </summary>
    public class SocialSecurityNumberMaskSettings
    {
        #region VARIABLES & PROPERTIES

        public const string DEFAULT_MASK_CHARACTER = "x";
        public const SocialSecurityNumberMaskOption DEFAULT_MASK_OPTION = SocialSecurityNumberMaskOption.MaskAll;

        public string MaskCharacter { get; set; }
        public SocialSecurityNumberMaskOption MaskOption { get; set; }

        public string MaskedRegex
        {
            get
            {
                switch (MaskOption)
                {
                    case SocialSecurityNumberMaskOption.MaskAll:
                        return string.Format("{0}{0}{0}-{0}{0}-{0}{0}{0}{0}", MaskCharacter);
                    case SocialSecurityNumberMaskOption.MaskAllButLastDigits:
                        return string.Format("{0}{0}{0}-{0}{0}-$3", MaskCharacter);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion

        #region CONSTRUCTOR

        public SocialSecurityNumberMaskSettings()
        {
            MaskCharacter = DEFAULT_MASK_CHARACTER;
            MaskOption = DEFAULT_MASK_OPTION;
        }

        #endregion
    }

    /// <summary>
    /// SSN mask transformer. 
    /// </summary>
    public class SocialSecurityNumberMask : RegexTransformer
    {

        /// <summary>
        /// Instantiates a new SocialSecurityNumberMask transformer w/ default mask settings.
        /// </summary>
        public SocialSecurityNumberMask()
            : this(new SocialSecurityNumberMaskSettings())
        {
        }

        /// <summary>
        /// Instantiates a new SocialSecurityNumberMask transformer.
        /// </summary>
        /// <param name="maskSettings">Mask settings to use to mask a given SSN.</param>
        public SocialSecurityNumberMask(SocialSecurityNumberMaskSettings maskSettings)
            : base(ValidationConstants.SSN_REG_MASK, maskSettings.MaskedRegex)
        {
        }
    }
}
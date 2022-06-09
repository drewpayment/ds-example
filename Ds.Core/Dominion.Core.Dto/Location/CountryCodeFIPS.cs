using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Location
{
    /// <summary>
    /// Codes from the U.S. standard FIPS PUB 10-4
    /// FIPS: Codes from the U.S. standard FIPS PUB 10-4
    /// https://en.wikipedia.org/wiki/List_of_FIPS_country_codes
    /// http://data.okfn.org/data/core/country-codes
    /// highFix: jay: add this to the database
    /// </summary>
    public enum CountryCodeFIPS
    {
        None = 0,

        /// <summary>
        /// Afghanistan
        /// </summary>
        AF = 1,

        /// <summary>
        /// Akrotiri
        /// </summary>
        AX = 2,

        /// <summary>
        /// Albania
        /// </summary>
        AL = 3,

        /// <summary>
        /// Algeria
        /// </summary>
        AG = 4,

        /// <summary>
        /// American Samoa
        /// </summary>
        AQ = 5,

        /// <summary>
        /// Andorra
        /// </summary>
        AN = 6,

        /// <summary>
        /// Angola
        /// </summary>
        AO = 7,

        /// <summary>
        /// Anguilla
        /// </summary>
        AV = 8,

        /// <summary>
        /// Antarctica
        /// </summary>
        AY = 9,

        /// <summary>
        /// Antigua and Barbuda
        /// </summary>
        AC = 10,

        /// <summary>
        /// Argentina
        /// </summary>
        AR = 11,

        /// <summary>
        /// Armenia
        /// </summary>
        AM = 12,

        /// <summary>
        /// Aruba
        /// </summary>
        AA = 13,

        /// <summary>
        /// Ashmore and Cartier Islands
        /// </summary>
        AT = 14,

        /// <summary>
        /// Australia
        /// </summary>
        AS = 15,

        /// <summary>
        /// Austria
        /// </summary>
        AU = 16,

        /// <summary>
        /// Azerbaijan
        /// </summary>
        AJ = 17,

        /// <summary>
        /// Bahamas
        /// </summary>
        BF = 18,

        /// <summary>
        /// Bahrain
        /// </summary>
        BA = 19,

        /// <summary>
        /// Baker Island
        /// </summary>
        FQ = 20,

        /// <summary>
        /// Bangladesh
        /// </summary>
        BG = 21,

        /// <summary>
        /// Barbados
        /// </summary>
        BB = 22,

        /// <summary>
        /// Belarus
        /// </summary>
        BO = 23,

        /// <summary>
        /// Belgium
        /// </summary>
        BE = 24,

        /// <summary>
        /// Belize
        /// </summary>
        BH = 25,

        /// <summary>
        /// Benin
        /// </summary>
        BN = 26,

        /// <summary>
        /// Bermuda
        /// </summary>
        BD = 27,

        /// <summary>
        /// Bhutan
        /// </summary>
        BT = 28,

        /// <summary>
        /// Bolivia
        /// </summary>
        BL = 29,

        /// <summary>
        /// Bosnia-Herzegovina
        /// </summary>
        BK = 30,

        /// <summary>
        /// Botswana
        /// </summary>
        BC = 31,

        /// <summary>
        /// Bouvet Island
        /// </summary>
        BV = 32,

        /// <summary>
        /// Brazil
        /// </summary>
        BR = 33,

        /// <summary>
        /// British Indian Ocean Territory
        /// </summary>
        IO = 34,

        /// <summary>
        /// British Virgin Islands
        /// </summary>
        VI = 35,

        /// <summary>
        /// Brunei
        /// </summary>
        BX = 36,

        /// <summary>
        /// Bulgaria
        /// </summary>
        BU = 37,

        /// <summary>
        /// Burkina Faso
        /// </summary>
        UV = 38,

        /// <summary>
        /// Burma
        /// </summary>
        BM = 39,

        /// <summary>
        /// Burundi
        /// </summary>
        BY = 40,

        /// <summary>
        /// Cambodia
        /// </summary>
        CB = 41,

        /// <summary>
        /// Cameroon
        /// </summary>
        CM = 42,

        /// <summary>
        /// Canada
        /// </summary>
        CA = 43,

        /// <summary>
        /// Cape Verde
        /// </summary>
        CV = 44,

        /// <summary>
        /// Cayman Islands
        /// </summary>
        CJ = 45,

        /// <summary>
        /// Central African Republic
        /// </summary>
        CT = 46,

        /// <summary>
        /// Chad
        /// </summary>
        CD = 47,

        /// <summary>
        /// Chile
        /// </summary>
        CI = 48,

        /// <summary>
        /// China
        /// </summary>
        CH = 49,

        /// <summary>
        /// Christmas Island
        /// </summary>
        KT = 50,

        /// <summary>
        /// Clipperton Island
        /// </summary>
        IP = 51,

        /// <summary>
        /// Cocos (Keeling) Islands
        /// </summary>
        CK = 52,

        /// <summary>
        /// Colombia
        /// </summary>
        CO = 53,

        /// <summary>
        /// Comoros
        /// </summary>
        CN = 54,

        /// <summary>
        /// Congo (Brazzaville)
        /// </summary>
        CF = 55,

        /// <summary>
        /// Congo (Kinshasa)
        /// </summary>
        CG = 56,

        /// <summary>
        /// Cook Islands
        /// </summary>
        CW = 57,

        /// <summary>
        /// Coral Sea Islands
        /// </summary>
        CR = 58,

        /// <summary>
        /// Costa Rica
        /// </summary>
        CS = 59,

        /// <summary>
        /// Cote D'Ivoire (Ivory Coast)
        /// </summary>
        IV = 60,

        /// <summary>
        /// Croatia
        /// </summary>
        HR = 61,

        /// <summary>
        /// Cuba
        /// </summary>
        CU = 62,

        /// <summary>
        /// Curacao
        /// </summary>
        UC = 63,

        /// <summary>
        /// Cyprus
        /// </summary>
        CY = 64,

        /// <summary>
        /// Czech Republic
        /// </summary>
        EZ = 65,

        /// <summary>
        /// Denmark
        /// </summary>
        DA = 66,

        /// <summary>
        /// Dhekelia
        /// </summary>
        DX = 67,

        /// <summary>
        /// Djibouti
        /// </summary>
        DJ = 68,

        /// <summary>
        /// Dominica
        /// </summary>
        DO = 69,

        /// <summary>
        /// Dominican Republic
        /// </summary>
        DR = 70,

        /// <summary>
        /// East Timor
        /// </summary>
        TT = 71,

        /// <summary>
        /// Ecuador
        /// </summary>
        EC = 72,

        /// <summary>
        /// Egypt
        /// </summary>
        EG = 73,

        /// <summary>
        /// El Salvador
        /// </summary>
        ES = 74,

        /// <summary>
        /// Equatorial Guinea
        /// </summary>
        EK = 75,

        /// <summary>
        /// Eritrea
        /// </summary>
        ER = 76,

        /// <summary>
        /// Estonia
        /// </summary>
        EN = 77,

        /// <summary>
        /// Ethiopia
        /// </summary>
        ET = 78,

        /// <summary>
        /// Falkland Islands (Islas Malvinas)
        /// </summary>
        FK = 79,

        /// <summary>
        /// Faroe Islands
        /// </summary>
        FO = 80,

        /// <summary>
        /// Federated States of Micronesia
        /// </summary>
        FM = 81,

        /// <summary>
        /// Fiji
        /// </summary>
        FJ = 82,

        /// <summary>
        /// Finland
        /// </summary>
        FI = 83,

        /// <summary>
        /// France
        /// </summary>
        FR = 84,

        /// <summary>
        /// French Polynesia
        /// </summary>
        FP = 85,

        /// <summary>
        /// French Southern and Antarctic Lands
        /// </summary>
        FS = 86,

        /// <summary>
        /// Gabon
        /// </summary>
        GB = 87,

        /// <summary>
        /// The Gambia
        /// </summary>
        GA = 88,

        /// <summary>
        /// Georgia
        /// </summary>
        GG = 89,

        /// <summary>
        /// Germany
        /// </summary>
        GM = 90,

        /// <summary>
        /// Ghana
        /// </summary>
        GH = 91,

        /// <summary>
        /// Gibraltar
        /// </summary>
        GI = 92,

        /// <summary>
        /// Greece
        /// </summary>
        GR = 93,

        /// <summary>
        /// Greenland
        /// </summary>
        GL = 94,

        /// <summary>
        /// Grenada
        /// </summary>
        GJ = 95,

        /// <summary>
        /// Guam
        /// </summary>
        GQ = 96,

        /// <summary>
        /// Guatemala
        /// </summary>
        GT = 97,

        /// <summary>
        /// Guernsey
        /// </summary>
        GK = 98,

        /// <summary>
        /// Guinea
        /// </summary>
        GV = 99,

        /// <summary>
        /// Guinea-Bissau
        /// </summary>
        PU = 100,

        /// <summary>
        /// Guyana
        /// </summary>
        GY = 101,

        /// <summary>
        /// Haiti
        /// </summary>
        HA = 102,

        /// <summary>
        /// Heard Island and McDonald Islands
        /// </summary>
        HM = 103,

        /// <summary>
        /// Holy See
        /// </summary>
        VT = 104,

        /// <summary>
        /// Honduras
        /// </summary>
        HO = 105,

        /// <summary>
        /// Hong Kong
        /// </summary>
        HK = 106,

        /// <summary>
        /// Howland Island
        /// </summary>
        HQ = 107,

        /// <summary>
        /// Hungary
        /// </summary>
        HU = 108,

        /// <summary>
        /// Iceland
        /// </summary>
        IC = 109,

        /// <summary>
        /// India
        /// </summary>
        IN = 110,

        /// <summary>
        /// Indonesia
        /// </summary>
        ID = 111,

        /// <summary>
        /// Iran
        /// </summary>
        IR = 112,

        /// <summary>
        /// Iraq
        /// </summary>
        IZ = 113,

        /// <summary>
        /// Ireland
        /// </summary>
        EI = 114,

        /// <summary>
        /// Israel
        /// </summary>
        IS = 115,

        /// <summary>
        /// Italy
        /// </summary>
        IT = 116,

        /// <summary>
        /// Jamaica
        /// </summary>
        JM = 117,

        /// <summary>
        /// Jan Mayen
        /// </summary>
        JN = 118,

        /// <summary>
        /// Japan
        /// </summary>
        JA = 119,

        /// <summary>
        /// Jarvis Island
        /// </summary>
        DQ = 120,

        /// <summary>
        /// Jersey
        /// </summary>
        JE = 121,

        /// <summary>
        /// Johnston Atoll
        /// </summary>
        JQ = 122,

        /// <summary>
        /// Jordan
        /// </summary>
        JO = 123,

        /// <summary>
        /// Kazakhstan
        /// </summary>
        KZ = 124,

        /// <summary>
        /// Kenya
        /// </summary>
        KE = 125,

        /// <summary>
        /// Kingman Reef
        /// </summary>
        KQ = 126,

        /// <summary>
        /// Kiribati
        /// </summary>
        KR = 127,

        /// <summary>
        /// Korea, Democratic People's Republic of (North)
        /// </summary>
        KN = 128,

        /// <summary>
        /// Korea, Republic of (South)
        /// </summary>
        KS = 129,

        /// <summary>
        /// Kosovo
        /// </summary>
        KV = 130,

        /// <summary>
        /// Kuwait
        /// </summary>
        KU = 131,

        /// <summary>
        /// Kyrgyzstan
        /// </summary>
        KG = 132,

        /// <summary>
        /// Laos
        /// </summary>
        LA = 133,

        /// <summary>
        /// Latvia
        /// </summary>
        LG = 134,

        /// <summary>
        /// Lebanon
        /// </summary>
        LE = 135,

        /// <summary>
        /// Lesotho
        /// </summary>
        LT = 136,

        /// <summary>
        /// Liberia
        /// </summary>
        LI = 137,

        /// <summary>
        /// Libya
        /// </summary>
        LY = 138,

        /// <summary>
        /// Liechtenstein
        /// </summary>
        LS = 139,

        /// <summary>
        /// Lithuania
        /// </summary>
        LH = 140,

        /// <summary>
        /// Luxembourg
        /// </summary>
        LU = 141,

        /// <summary>
        /// Macau
        /// </summary>
        MC = 142,

        /// <summary>
        /// Macedonia
        /// </summary>
        MK = 143,

        /// <summary>
        /// Madagascar
        /// </summary>
        MA = 144,

        /// <summary>
        /// Malawi
        /// </summary>
        MI = 145,

        /// <summary>
        /// Malaysia
        /// </summary>
        MY = 146,

        /// <summary>
        /// Maldives
        /// </summary>
        MV = 147,

        /// <summary>
        /// Mali
        /// </summary>
        ML = 148,

        /// <summary>
        /// Malta
        /// </summary>
        MT = 149,

        /// <summary>
        /// Man, Isle of
        /// </summary>
        IM = 150,

        /// <summary>
        /// Marshall Islands
        /// </summary>
        RM = 151,

        /// <summary>
        /// Mauritania
        /// </summary>
        MR = 152,

        /// <summary>
        /// Mauritius
        /// </summary>
        MP = 153,

        /// <summary>
        /// Mexico
        /// </summary>
        MX = 154,

        /// <summary>
        /// Midway Islands
        /// </summary>
        MQ = 155,

        /// <summary>
        /// Moldova
        /// </summary>
        MD = 156,

        /// <summary>
        /// Monaco
        /// </summary>
        MN = 157,

        /// <summary>
        /// Mongolia
        /// </summary>
        MG = 158,

        /// <summary>
        /// Montenegro
        /// </summary>
        MJ = 159,

        /// <summary>
        /// Montserrat
        /// </summary>
        MH = 160,

        /// <summary>
        /// Morocco
        /// </summary>
        MO = 161,

        /// <summary>
        /// Mozambique
        /// </summary>
        MZ = 162,

        /// <summary>
        /// Namibia
        /// </summary>
        WA = 163,

        /// <summary>
        /// Nauru
        /// </summary>
        NR = 164,

        /// <summary>
        /// Navassa Island
        /// </summary>
        BQ = 165,

        /// <summary>
        /// Nepal
        /// </summary>
        NP = 166,

        /// <summary>
        /// Netherlands
        /// </summary>
        NL = 167,

        /// <summary>
        /// New Caledonia
        /// </summary>
        NC = 168,

        /// <summary>
        /// New Zealand
        /// </summary>
        NZ = 169,

        /// <summary>
        /// Nicaragua
        /// </summary>
        NU = 170,

        /// <summary>
        /// Niger
        /// </summary>
        NG = 171,

        /// <summary>
        /// Nigeria
        /// </summary>
        NI = 172,

        /// <summary>
        /// Niue
        /// </summary>
        NE = 173,

        /// <summary>
        /// Norfolk Island
        /// </summary>
        NF = 174,

        /// <summary>
        /// Northern Mariana Islands
        /// </summary>
        CQ = 175,

        /// <summary>
        /// Norway
        /// </summary>
        NO = 176,

        /// <summary>
        /// Oman
        /// </summary>
        MU = 177,

        /// <summary>
        /// Other Country
        /// </summary>
        OC = 178,

        /// <summary>
        /// Pakistan
        /// </summary>
        PK = 179,

        /// <summary>
        /// Palau
        /// </summary>
        PS = 180,

        /// <summary>
        /// Palmyra Atoll
        /// </summary>
        LQ = 181,

        /// <summary>
        /// Panama
        /// </summary>
        PM = 182,

        /// <summary>
        /// Papua-New Guinea
        /// </summary>
        PP = 183,

        /// <summary>
        /// Paracel Islands
        /// </summary>
        PF = 184,

        /// <summary>
        /// Paraguay
        /// </summary>
        PA = 185,

        /// <summary>
        /// Peru
        /// </summary>
        PE = 186,

        /// <summary>
        /// Philippines
        /// </summary>
        RP = 187,

        /// <summary>
        /// Pitcairn Islands
        /// </summary>
        PC = 188,

        /// <summary>
        /// Poland
        /// </summary>
        PL = 189,

        /// <summary>
        /// Portugal
        /// </summary>
        PO = 190,

        /// <summary>
        /// Puerto Rico
        /// </summary>
        RQ = 191,

        /// <summary>
        /// Qatar
        /// </summary>
        QA = 192,

        /// <summary>
        /// Romania
        /// </summary>
        RO = 193,

        /// <summary>
        /// Russia
        /// </summary>
        RS = 194,

        /// <summary>
        /// Rwanda
        /// </summary>
        RW = 195,

        /// <summary>
        /// Saint Barthelemy
        /// </summary>
        TB = 196,

        /// <summary>
        /// Saint Martin
        /// </summary>
        RN = 197,

        /// <summary>
        /// Samoa
        /// </summary>
        WS = 198,

        /// <summary>
        /// San Marino
        /// </summary>
        SM = 199,

        /// <summary>
        /// Sao Tome and Principe
        /// </summary>
        TP = 200,

        /// <summary>
        /// Saudi Arabia
        /// </summary>
        SA = 201,

        /// <summary>
        /// Senegal
        /// </summary>
        SG = 202,

        /// <summary>
        /// Serbia
        /// </summary>
        RI = 203,

        /// <summary>
        /// Seychelles
        /// </summary>
        SE = 204,

        /// <summary>
        /// Sierra Leone
        /// </summary>
        SL = 205,

        /// <summary>
        /// Singapore
        /// </summary>
        SN = 206,

        /// <summary>
        /// Sint Maarten
        /// </summary>
        NN = 207,

        /// <summary>
        /// Slovakia
        /// </summary>
        LO = 208,

        /// <summary>
        /// Slovenia
        /// </summary>
        SI = 209,

        /// <summary>
        /// Solomon Islands
        /// </summary>
        BP = 210,

        /// <summary>
        /// Somalia
        /// </summary>
        SO = 211,

        /// <summary>
        /// South Africa
        /// </summary>
        SF = 212,

        /// <summary>
        /// South Georgia and the South Sandwich Islands
        /// </summary>
        SX = 213,

        /// <summary>
        /// South Sudan
        /// </summary>
        OD = 214,

        /// <summary>
        /// Spain
        /// </summary>
        SP = 215,

        /// <summary>
        /// Spratly Islands
        /// </summary>
        PG = 216,

        /// <summary>
        /// Sri Lanka
        /// </summary>
        CE = 217,

        /// <summary>
        /// St. Helena
        /// </summary>
        SH = 218,

        /// <summary>
        /// St. Kitts and Nevis
        /// </summary>
        SC = 219,

        /// <summary>
        /// St. Lucia Island
        /// </summary>
        ST = 220,

        /// <summary>
        /// St. Pierre and Miquelon
        /// </summary>
        SB = 221,

        /// <summary>
        /// St. Vincent and the Grenadines
        /// </summary>
        VC = 222,

        /// <summary>
        /// Sudan
        /// </summary>
        SU = 223,

        /// <summary>
        /// Suriname
        /// </summary>
        NS = 224,

        /// <summary>
        /// Svalbard
        /// </summary>
        SV = 225,

        /// <summary>
        /// Swaziland
        /// </summary>
        WZ = 226,

        /// <summary>
        /// Sweden
        /// </summary>
        SW = 227,

        /// <summary>
        /// Switzerland
        /// </summary>
        SZ = 228,

        /// <summary>
        /// Syria
        /// </summary>
        SY = 229,

        /// <summary>
        /// Taiwan
        /// </summary>
        TW = 230,

        /// <summary>
        /// Tajikistan
        /// </summary>
        TI = 231,

        /// <summary>
        /// Tanzania
        /// </summary>
        TZ = 232,

        /// <summary>
        /// Thailand
        /// </summary>
        TH = 233,

        /// <summary>
        /// Togo
        /// </summary>
        TO = 234,

        /// <summary>
        /// Tokelau
        /// </summary>
        TL = 235,

        /// <summary>
        /// Tonga
        /// </summary>
        TN = 236,

        /// <summary>
        /// Trinidad and Tobago
        /// </summary>
        TD = 237,

        /// <summary>
        /// Tunisia
        /// </summary>
        TS = 238,

        /// <summary>
        /// Turkey
        /// </summary>
        TU = 239,

        /// <summary>
        /// Turkmenistan
        /// </summary>
        TX = 240,

        /// <summary>
        /// Turks and Caicos Islands
        /// </summary>
        TK = 241,

        /// <summary>
        /// Tuvalu
        /// </summary>
        TV = 242,

        /// <summary>
        /// Uganda
        /// </summary>
        UG = 243,

        /// <summary>
        /// Ukraine
        /// </summary>
        UP = 244,

        /// <summary>
        /// United Arab Emirates
        /// </summary>
        AE = 245,

        /// <summary>
        /// United Kingdom (England, Northern Ireland, Scotland, and Wales)
        /// </summary>
        UK = 246,

        /// <summary>
        /// Uruguay
        /// </summary>
        UY = 247,

        /// <summary>
        /// Uzbekistan
        /// </summary>
        UZ = 248,

        /// <summary>
        /// Vanuatu
        /// </summary>
        NH = 249,

        /// <summary>
        /// Venezuela
        /// </summary>
        VE = 250,

        /// <summary>
        /// Vietnam
        /// </summary>
        VM = 251,

        /// <summary>
        /// Virgin Islands
        /// </summary>
        VQ = 252,

        /// <summary>
        /// Wake Island
        /// </summary>
        WQ = 253,

        /// <summary>
        /// Wallis and Futuna
        /// </summary>
        WF = 254,

        /// <summary>
        /// Western Sahara
        /// </summary>
        WI = 255,

        /// <summary>
        /// Yemen (Aden)
        /// </summary>
        YM = 256,

        /// <summary>
        /// Zambia
        /// </summary>
        ZA = 257,

        /// <summary>
        /// Zimbabwe
        /// </summary>
        ZI = 258,
    }
}
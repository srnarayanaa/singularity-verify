// ==++==
//
//   Copyright (c) Microsoft Corporation.  All rights reserved.
//
// ==--==
////////////////////////////////////////////////////////////////////////////
//
//  Class:    CharacterInfo
//
//  Purpose:  This class implements a set of methods for retrieving
//            character type information.  Character type information is
//            independent of culture and region.
//
////////////////////////////////////////////////////////////////////////////

namespace System.Globalization
{

    using System;
    using System.Runtime.CompilerServices;

    internal class CharacterInfo : System.Object
    {
        private CharacterInfo() {}   // Prevent from being created

        //--------------------------------------------------------------------//
        //                        Internal Information                        //
        //--------------------------------------------------------------------//

        //
        internal const char  HIGH_SURROGATE_START  = '\ud800';
        internal const char  HIGH_SURROGATE_END    = '\udbff';
        internal const char  LOW_SURROGATE_START   = '\udc00';
        internal const char  LOW_SURROGATE_END     = '\udfff';

        internal static bool IsHighSurrogate(char c)
        {
            return ((c >= CharacterInfo.HIGH_SURROGATE_START) && (c <= CharacterInfo.HIGH_SURROGATE_END));
        }
        internal static bool IsLowSurrogate(char c)
        {
            return ((c >= CharacterInfo.LOW_SURROGATE_START) && (c <= CharacterInfo.LOW_SURROGATE_END));
        }

        ////////////////////////////////////////////////////////////////////////
        //
        //  IsLetter
        //
        //  Determines if the given character is an alphabetic character.
        //
        ////////////////////////////////////////////////////////////////////////

        public static bool IsLetter(char ch) {
            UnicodeCategory uc = GetUnicodeCategory(ch);
            return (uc == UnicodeCategory.UppercaseLetter
                    || uc == UnicodeCategory.LowercaseLetter
                    || uc == UnicodeCategory.TitlecaseLetter
                    || uc == UnicodeCategory.ModifierLetter
                    || uc == UnicodeCategory.OtherLetter);
        }


        ////////////////////////////////////////////////////////////////////////
        //
        //  IsLower
        //
        //  Determines if the given character is a lower case character.
        //
        ////////////////////////////////////////////////////////////////////////

        public static bool IsLower(char c)
        {
            return (GetUnicodeCategory(c) == UnicodeCategory.LowercaseLetter);
        }


        //=================================IsMark==========================
        //Action: Returns true if and only if the character c has one of the properties NonSpacingMark, SpacingCombiningMark,
        // or EnclosingMark. Marks usually modify one or more other
        //Returns:
        //Arguments:
        //Exceptions:
        //============================================================================  

        public static bool IsMark(char ch) {
            UnicodeCategory uc = GetUnicodeCategory(ch);
            return (uc == UnicodeCategory.NonSpacingMark
                    || uc == UnicodeCategory.SpacingCombiningMark
                    || uc == UnicodeCategory.EnclosingMark);
        }

        //=================================IsNumber==========================
        //Action: Returns true if and only if the character c has one of the properties DecimalDigitNumber,
        // LetterNumber, or OtherNumber. Use the GetNumericValue method to determine the numeric value.
        //Returns:
        //Arguments:
        //Exceptions:
        //Note: GetNuemricValue is not yet implemented.
        //============================================================================  
        public static bool IsNumber(char ch) {
            UnicodeCategory uc = GetUnicodeCategory(ch);
            return (uc == UnicodeCategory.DecimalDigitNumber
                    || uc == UnicodeCategory.LetterNumber
                    || uc == UnicodeCategory.OtherNumber);
        }

        //=================================IsDigit==========================
        //Action: Returns true if and only if the character c has the property DecimalDigitNumber.
        // Use the GetNumericValue method to determine the numeric value.
        //Returns:
        //Arguments:
        //Exceptions:
        //Note: GetNuemricValue is not yet implemented.
        //============================================================================  

        public static bool IsDigit(char ch) {
            UnicodeCategory uc = GetUnicodeCategory(ch);
            return (uc == UnicodeCategory.DecimalDigitNumber);
        }

        //=================================IsSeparator==========================
        //Action: Returns true if and only if the character c has one of the properties SpaceSeparator, LineSeparator
        //or ParagraphSeparator.
        //Returns:
        //Arguments:
        //Exceptions:
        //============================================================================  
        public static bool IsSeparator(char ch) {
            UnicodeCategory uc = GetUnicodeCategory(ch);
            return (uc == UnicodeCategory.SpaceSeparator || uc == UnicodeCategory.LineSeparator || uc == UnicodeCategory.ParagraphSeparator);
        }

        //=================================IsControl==========================
        //Action: Returns true if and only if the character c has the property Control.
        //Returns:
        //Arguments:
        //Exceptions:
        //============================================================================  

        public static bool IsControl(char ch) {
            return (GetUnicodeCategory(ch) == UnicodeCategory.Control);
        }

        //=================================IsSurrogate==========================
        //Action: Returns true if and only if the character c has the property PrivateUse.
        //Returns:
        //Arguments:
        //Exceptions:
        //============================================================================  

        public static bool IsSurrogate(char ch) {
            return (GetUnicodeCategory(ch) == UnicodeCategory.Surrogate);
        }


        ////////////////////////////////////////////////////////////////////////
        //
        //  IsPunctuation
        //
        //  Determines if the given character is a punctuation character.
        //
        ////////////////////////////////////////////////////////////////////////

        public static bool IsPunctuation(char ch)
        {
            UnicodeCategory uc = GetUnicodeCategory(ch);
            switch (uc) {
                case UnicodeCategory.ConnectorPunctuation:
                case UnicodeCategory.DashPunctuation:
                case UnicodeCategory.OpenPunctuation:
                case UnicodeCategory.ClosePunctuation:
                case UnicodeCategory.InitialQuotePunctuation:
                case UnicodeCategory.FinalQuotePunctuation:
                case UnicodeCategory.OtherPunctuation:
                    return (true);
            }
            return (false);
        }


        ////////////////////////////////////////////////////////////////////////
        //
        //  IsSymbol
        //
        //  Determines if the given character is a symbol character.
        //
        ////////////////////////////////////////////////////////////////////////

        public static bool IsSymbol(char ch)
        {
            UnicodeCategory uc = GetUnicodeCategory(ch);
            return (uc == UnicodeCategory.MathSymbol
                    || uc == UnicodeCategory.CurrencySymbol
                    || uc == UnicodeCategory.ModifierSymbol
                    || uc == UnicodeCategory.OtherSymbol);
        }


        ////////////////////////////////////////////////////////////////////////
        //
        //  IsTitleCase
        //
        //  Determines if the given character is a title case character.
        //  The title case characters are:
        //      \u01c5  (Dz with Caron)
        //      \u01c8  (Lj)
        //      \u01cb  (Nj)
        //      \u01f2  (Dz)
        //  A title case character is a concept that is useful for only 4
        //  Unicode characters.  This has to do with the mapping of some
        //  Cyrillic/Serbian characters to the Latin alphabet.  For example,
        //  the Cyrillic character LJ looks just like 'L' and 'J' in the Latin
        //  alphabet.  However, when LJ is used in a book title, the
        //  capitalized form is 'Lj' instead of 'LJ'.
        //
        //  These 4 characters are the only ones in Unicode that have a
        //  character type of both Upper and Lower case.
        //
        ////////////////////////////////////////////////////////////////////////

        internal static bool IsTitleCase(char c)
        {
            return (GetUnicodeCategory(c) == UnicodeCategory.TitlecaseLetter);
        }


        ////////////////////////////////////////////////////////////////////////
        //
        //  IsUpper
        //
        //  Determines if the given character is an upper case character.
        //
        ////////////////////////////////////////////////////////////////////////

        public static bool IsUpper(char c)
        {
            return (GetUnicodeCategory(c) == UnicodeCategory.UppercaseLetter);
        }


        ////////////////////////////////////////////////////////////////////////
        //
        //  WhiteSpaceChars
        //
        ////////////////////////////////////////////////////////////////////////

        internal static readonly char [] WhitespaceChars =
        { '\x9', '\xA', '\xB', '\xC', '\xD', '\x20', '\xA0',
          '\x2000', '\x2001', '\x2002', '\x2003', '\x2004', '\x2005',
          '\x2006', '\x2007', '\x2008', '\x2009', '\x200A', '\x200B',
          '\x3000', '\xFEFF' };

        // BUGBUG YSLin: Leave this for now because System.Char still has this function.

        ////////////////////////////////////////////////////////////////////////
        //
        //  IsWhiteSpace
        //
        //  Determines if the given character is a white space character.
        //
        ////////////////////////////////////////////////////////////////////////

        public static bool IsWhiteSpace(char c)
        {
            // NOTENOTE YSLin:
            // There are characters which belong to UnicodeCategory.Control but are considered as white spaces.
            // We use code point comparisons for these characters here as a temporary fix.
            // The compiler should be smart enough to do a range comparison to optimize this (U+0009 ~ U+000d).
            // Also provide a shortcut here for the space character (U+0020)
            switch (c) {
                case ' ':
                case '\x0009' :
                case '\x000a' :
                case '\x000b' :
                case '\x000c' :
                case '\x000d' :
                case '\x0085' :
                    return (true);
            }

            // In Unicode 3.0, U+2028 is the only character which is under the category "LineSeparator".
            // And U+2029 is the only character which is under the category "ParagraphSeparator".
            switch ((int)GetUnicodeCategory(c)) {
                case ((int)UnicodeCategory.SpaceSeparator):
                case ((int)UnicodeCategory.LineSeparator):
                case ((int)UnicodeCategory.ParagraphSeparator):
                    return (true);
            }

            return (false);
        }

        //=================================IsCombiningCharacter==========================
        //Action: Returns if the specified character is a combining character.
        //Returns:
        //  TRUE if ch is a combining character.
        //Arguments:
        //  ch  a Unicode character
        //Exceptions:
        //  None
        //Notes:
        //  Used by StringInfo.ParseCombiningCharacter.
        //============================================================================  
        internal static bool IsCombiningCharacter(char ch) {
            UnicodeCategory uc = CharacterInfo.GetUnicodeCategory(ch);
            return (
                uc == UnicodeCategory.NonSpacingMark ||
                uc == UnicodeCategory.SpacingCombiningMark ||
                uc == UnicodeCategory.EnclosingMark
                );
        }


        private static byte[] level1CategoryTable = new byte[256] {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
            16, 17, 18, 19, 20, 21, 22, 23, 24, 8, 8, 8, 8, 8, 25, 26,
            27, 28, 29, 30, 31, 32, 33, 34, 35, 8, 8, 8, 8, 8, 36, 37,
            38, 39, 40, 41, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21,
            21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 42, 21, 21,
            21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21,
            21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21,
            21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21,
            21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21,
            21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 43,
            21, 21, 21, 21, 44, 8, 8, 8, 8, 8, 8, 8, 21, 21, 21, 21,
            21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21,
            21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21,
            21, 21, 21, 21, 21, 21, 21, 45, 46, 46, 46, 46, 46, 46, 46, 46,
            47, 47, 47, 47, 47, 47, 47, 47, 47, 47, 47, 47, 47, 47, 47, 47,
            47, 47, 47, 47, 47, 47, 47, 47, 47, 21, 48, 49, 21, 50, 51, 52
        };
        private static ushort[] level2CategoryTable = new ushort[848] {
            0, 0, 16, 32, 48, 64, 80, 96, 0, 0, 112, 128, 144, 160, 176, 192,
            208, 208, 208, 224, 240, 208, 208, 256, 272, 288, 304, 320, 336, 352, 208, 368,
            208, 208, 384, 400, 416, 176, 176, 176, 176, 176, 432, 448, 464, 464, 480, 416,
            496, 496, 496, 496, 512, 416, 528, 544, 560, 576, 592, 176, 608, 624, 208, 640,
            144, 144, 144, 176, 176, 176, 208, 208, 656, 208, 208, 208, 672, 208, 208, 688,
            416, 416, 416, 704, 144, 720, 736, 176, 752, 768, 784, 800, 816, 832, 848, 864,
            880, 896, 912, 848, 928, 944, 960, 976, 832, 832, 832, 832, 832, 992, 1008, 1024,
            1040, 1056, 1072, 496, 1088, 416, 416, 416, 832, 832, 1104, 1120, 416, 416, 416, 416,
            416, 416, 416, 416, 416, 416, 416, 416, 416, 416, 416, 416, 416, 416, 416, 416,
            1136, 832, 832, 1152, 1168, 1184, 1200, 1216, 1232, 1248, 1264, 1280, 1296, 1312, 1328, 1344,
            1360, 1248, 1264, 1376, 1392, 1408, 1424, 1440, 1456, 1472, 1264, 1488, 1504, 1520, 1536, 416,
            1232, 1248, 1264, 1552, 1568, 1584, 1600, 1616, 1632, 1648, 1664, 1680, 1696, 1712, 1728, 1744,
            1760, 1776, 1264, 1792, 1808, 1824, 1600, 416, 1840, 1776, 1264, 1856, 1872, 1888, 1600, 416,
            1840, 1776, 1264, 1904, 1920, 1712, 1600, 416, 1936, 1952, 832, 1968, 1984, 2000, 416, 2016,
            912, 832, 832, 2032, 2048, 2064, 416, 416, 2080, 2096, 2112, 2128, 2144, 2160, 416, 416,
            2176, 2192, 2208, 2224, 2240, 832, 848, 2256, 2272, 2288, 496, 2304, 2320, 416, 416, 416,
            832, 832, 2336, 2352, 2368, 2384, 416, 416, 416, 416, 144, 144, 2400, 832, 832, 2416,
            832, 832, 832, 832, 832, 2432, 832, 832, 832, 832, 2448, 832, 832, 832, 832, 2464,
            2480, 832, 832, 832, 2496, 2496, 832, 832, 2496, 832, 2512, 2528, 2528, 2480, 2512, 832,
            2512, 2528, 832, 832, 2480, 848, 2544, 2560, 416, 416, 832, 832, 832, 832, 832, 2576,
            912, 832, 832, 832, 832, 832, 832, 832, 832, 832, 832, 832, 832, 832, 832, 832,
            832, 832, 832, 832, 832, 832, 832, 832, 832, 832, 832, 832, 832, 832, 832, 832,
            832, 832, 832, 832, 832, 832, 2592, 2608, 2624, 2640, 832, 832, 832, 832, 2656, 2672,
            416, 416, 416, 416, 416, 416, 416, 416, 832, 832, 832, 2688, 2704, 2720, 2736, 416,
            2752, 2736, 832, 832, 2768, 832, 832, 2784, 832, 832, 2800, 416, 416, 416, 416, 416,
            208, 208, 208, 208, 208, 208, 208, 208, 208, 2816, 208, 208, 208, 208, 208, 2832,
            2848, 2864, 2848, 2848, 2864, 2880, 2848, 432, 2896, 2896, 2896, 2912, 2928, 2944, 2960, 2976,
            2992, 3008, 3024, 3040, 3056, 416, 3072, 3088, 3104, 416, 3120, 416, 416, 3136, 3152, 416,
            3168, 3184, 3200, 3216, 416, 3232, 3248, 3248, 3264, 3280, 3296, 3312, 3328, 3344, 3312, 3360,
            3376, 3376, 3376, 3376, 3376, 3376, 3376, 3376, 3376, 3376, 3376, 3376, 3376, 3376, 3376, 3392,
            3408, 3312, 3424, 3312, 3312, 3312, 3312, 3440, 3312, 3456, 416, 416, 416, 416, 416, 416,
            3312, 3312, 3472, 416, 3456, 416, 3488, 3488, 3488, 3504, 3312, 3312, 3312, 3312, 3520, 416,
            3312, 3312, 3312, 3312, 3312, 3312, 3312, 3312, 3312, 3536, 3312, 3552, 3568, 3312, 3312, 3584,
            3312, 3600, 3312, 3312, 3312, 3312, 3616, 3632, 416, 416, 416, 416, 416, 416, 416, 416,
            3648, 3312, 3664, 3312, 3680, 3696, 3712, 3728, 3488, 3744, 3312, 3760, 416, 416, 416, 416,
            3312, 3312, 3312, 3312, 3312, 3312, 3312, 3312, 3312, 3312, 3312, 3312, 3312, 3312, 3312, 3312,
            416, 416, 416, 416, 416, 416, 416, 416, 3312, 3776, 3312, 3312, 3312, 3312, 3312, 3360,
            3312, 3312, 3312, 3312, 3312, 3312, 3312, 3312, 3312, 3312, 3312, 3312, 3312, 3536, 416, 3792,
            3808, 3824, 3840, 3856, 912, 832, 832, 832, 832, 3872, 912, 832, 832, 832, 832, 3888,
            3904, 832, 1072, 912, 832, 832, 832, 832, 2512, 3920, 832, 2784, 416, 416, 416, 416,
            3312, 3936, 3952, 3312, 3360, 416, 3312, 3968, 3952, 3312, 3312, 1616, 3792, 3312, 3312, 3984,
            3312, 3312, 3312, 3312, 3312, 3312, 3312, 4000, 3312, 3312, 3312, 3312, 3312, 4016, 3312, 3984,
            832, 832, 832, 832, 832, 832, 832, 832, 832, 832, 832, 4032, 416, 416, 416, 416,
            832, 832, 832, 832, 832, 832, 832, 832, 832, 832, 4032, 416, 416, 416, 416, 416,
            832, 832, 832, 832, 832, 832, 832, 832, 1072, 3312, 4048, 4064, 4080, 416, 416, 416,
            832, 832, 832, 832, 832, 832, 832, 832, 832, 832, 4096, 416, 416, 416, 416, 416,
            4112, 4112, 4112, 4112, 4112, 4112, 4112, 4112, 4112, 4112, 4112, 4112, 4112, 4112, 4112, 4112,
            4128, 4128, 4128, 4128, 4128, 4128, 4128, 4128, 4128, 4128, 4128, 4128, 4128, 4128, 4128, 4128,
            832, 832, 4144, 416, 416, 416, 416, 416, 416, 416, 416, 416, 416, 416, 416, 416,
            4160, 4176, 4192, 4208, 4224, 832, 832, 832, 832, 832, 832, 4240, 416, 4256, 832, 832,
            832, 832, 832, 4272, 416, 832, 832, 832, 832, 4288, 832, 832, 2784, 416, 416, 4304,
            416, 416, 4320, 4336, 4352, 4368, 4384, 4400, 832, 832, 832, 832, 832, 832, 832, 4416,
            4432, 32, 48, 64, 80, 4448, 4464, 4480, 832, 4496, 832, 2512, 4512, 4528, 4544, 4560
        };
        private static byte[] level3CategoryTable = new byte[4576] {
            14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
            11, 24, 24, 24, 26, 24, 24, 24, 20, 21, 24, 25, 24, 19, 24, 24,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 24, 24, 25, 25, 25, 24,
            24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 24, 21, 27, 18,
            27, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 20, 25, 21, 25, 14,
            11, 24, 26, 26, 26, 26, 28, 28, 27, 28, 1, 22, 25, 19, 28, 27,
            28, 25, 10, 10, 27, 1, 28, 24, 27, 10, 1, 23, 10, 10, 10, 24,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 25, 1, 1, 1, 1, 1, 1, 1, 1,
            0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1,
            0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0,
            1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1,
            0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 1,
            1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0,
            0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 1, 0,
            0, 1, 0, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0,
            1, 0, 0, 0, 1, 0, 1, 0, 0, 1, 1, 4, 0, 1, 1, 1,
            4, 4, 4, 4, 0, 2, 1, 0, 2, 1, 0, 2, 1, 0, 1, 0,
            1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 1,
            1, 0, 2, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1,
            29, 29, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1,
            0, 1, 0, 1, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 29, 29,
            3, 3, 3, 3, 3, 3, 3, 3, 3, 27, 27, 3, 3, 3, 3, 3,
            3, 3, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27,
            3, 3, 3, 3, 3, 27, 27, 27, 27, 27, 27, 27, 27, 27, 3, 29,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 29,
            5, 5, 5, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            29, 29, 29, 29, 27, 27, 29, 29, 29, 29, 3, 29, 29, 29, 24, 29,
            29, 29, 29, 29, 27, 27, 0, 24, 0, 0, 0, 29, 0, 29, 0, 0,
            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 29, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 29,
            1, 1, 0, 0, 0, 1, 1, 1, 29, 29, 0, 1, 0, 1, 0, 1,
            1, 1, 1, 1, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            0, 1, 28, 5, 5, 5, 5, 29, 7, 7, 29, 29, 0, 1, 0, 1,
            0, 0, 1, 0, 1, 29, 29, 0, 1, 29, 29, 0, 1, 29, 29, 29,
            0, 1, 0, 1, 0, 1, 29, 29, 0, 1, 29, 29, 29, 29, 29, 29,
            29, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 29, 29, 3, 24, 24, 24, 24, 24, 24,
            29, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 29, 24, 19, 29, 29, 29, 29, 29,
            29, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            5, 5, 29, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 29, 5, 5, 5, 24, 5,
            24, 5, 5, 24, 5, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 29, 29, 29, 29, 29,
            4, 4, 4, 24, 24, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 24, 29, 29, 29,
            29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 24, 29, 29, 29, 24,
            29, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 5, 5, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 24, 24, 24, 24, 29, 29,
            5, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 24, 4, 5, 5, 5, 5, 5, 5, 5, 7, 7, 5,
            5, 5, 5, 5, 5, 3, 3, 5, 5, 28, 5, 5, 5, 5, 29, 29,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 4, 4, 4, 28, 28, 29,
            24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 29, 15,
            4, 5, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 29, 29, 29,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 29, 29, 29, 29, 29,
            4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            5, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            29, 5, 5, 6, 29, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 29, 29, 5, 4, 6, 6,
            6, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 5, 29, 29,
            4, 5, 5, 5, 5, 29, 29, 29, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 5, 5, 24, 24, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            24, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            29, 5, 6, 6, 29, 4, 4, 4, 4, 4, 4, 4, 4, 29, 29, 4,
            4, 29, 29, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 29, 4, 4, 4, 4, 4, 4,
            4, 29, 4, 29, 29, 29, 4, 4, 4, 4, 29, 29, 5, 29, 6, 6,
            6, 5, 5, 5, 5, 29, 29, 6, 6, 29, 29, 6, 6, 5, 29, 29,
            29, 29, 29, 29, 29, 29, 29, 6, 29, 29, 29, 29, 4, 4, 29, 4,
            4, 4, 5, 5, 29, 29, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            4, 4, 26, 26, 10, 10, 10, 10, 10, 10, 28, 29, 29, 29, 29, 29,
            29, 29, 5, 29, 29, 4, 4, 4, 4, 4, 4, 29, 29, 29, 29, 4,
            4, 29, 4, 4, 29, 4, 4, 29, 4, 4, 29, 29, 5, 29, 6, 6,
            6, 5, 5, 29, 29, 29, 29, 5, 5, 29, 29, 5, 5, 5, 29, 29,
            29, 29, 29, 29, 29, 29, 29, 29, 29, 4, 4, 4, 4, 29, 4, 29,
            29, 29, 29, 29, 29, 29, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            5, 5, 4, 4, 4, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            29, 5, 5, 6, 29, 4, 4, 4, 4, 4, 4, 4, 29, 4, 29, 4,
            4, 4, 29, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 29, 4, 4, 29, 4, 4, 4, 4, 4, 29, 29, 5, 4, 6, 6,
            6, 5, 5, 5, 5, 5, 29, 5, 5, 6, 29, 6, 6, 5, 29, 29,
            4, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            4, 29, 29, 29, 29, 29, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            4, 29, 4, 4, 29, 29, 4, 4, 4, 4, 29, 29, 5, 4, 6, 5,
            6, 5, 5, 5, 29, 29, 29, 6, 6, 29, 29, 6, 6, 5, 29, 29,
            29, 29, 29, 29, 29, 29, 5, 6, 29, 29, 29, 29, 4, 4, 29, 4,
            4, 4, 29, 29, 29, 29, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            28, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            29, 29, 5, 6, 29, 4, 4, 4, 4, 4, 4, 29, 29, 29, 4, 4,
            4, 29, 4, 4, 4, 4, 29, 29, 29, 4, 4, 29, 4, 29, 4, 4,
            29, 29, 29, 4, 4, 29, 29, 29, 4, 4, 4, 29, 29, 29, 4, 4,
            4, 4, 4, 4, 4, 4, 29, 4, 4, 4, 29, 29, 29, 29, 6, 6,
            5, 6, 6, 29, 29, 29, 6, 6, 6, 29, 6, 6, 6, 5, 29, 29,
            29, 29, 29, 29, 29, 29, 29, 6, 29, 29, 29, 29, 29, 29, 29, 29,
            29, 29, 29, 29, 29, 29, 29, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            10, 10, 10, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            29, 6, 6, 6, 29, 4, 4, 4, 4, 4, 4, 4, 4, 29, 4, 4,
            4, 29, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 29, 4, 4, 4, 4, 4, 29, 29, 29, 29, 5, 5,
            5, 6, 6, 6, 6, 29, 5, 5, 5, 29, 5, 5, 5, 5, 29, 29,
            29, 29, 29, 29, 29, 5, 5, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            29, 29, 6, 6, 29, 4, 4, 4, 4, 4, 4, 4, 4, 29, 4, 4,
            4, 4, 4, 4, 29, 4, 4, 4, 4, 4, 29, 29, 29, 29, 6, 5,
            6, 6, 6, 6, 6, 29, 5, 6, 6, 29, 6, 6, 5, 5, 29, 29,
            29, 29, 29, 29, 29, 6, 6, 29, 29, 29, 29, 29, 29, 29, 4, 29,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 29, 29, 29, 29, 6, 6,
            6, 5, 5, 5, 29, 29, 6, 6, 6, 29, 6, 6, 6, 5, 29, 29,
            29, 29, 6, 6, 29, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 29, 29, 29, 4, 4, 4, 4, 4, 4,
            4, 4, 29, 4, 4, 4, 4, 4, 4, 4, 4, 4, 29, 4, 29, 29,
            4, 4, 4, 4, 4, 4, 4, 29, 29, 29, 5, 29, 29, 29, 29, 6,
            6, 6, 5, 5, 5, 29, 5, 29, 6, 6, 6, 6, 6, 6, 6, 6,
            29, 29, 6, 6, 24, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            4, 5, 4, 4, 5, 5, 5, 5, 5, 5, 5, 29, 29, 29, 29, 26,
            4, 4, 4, 4, 4, 4, 3, 5, 5, 5, 5, 5, 5, 5, 5, 24,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 24, 24, 29, 29, 29, 29,
            29, 4, 4, 29, 4, 29, 29, 4, 4, 29, 4, 29, 29, 4, 29, 29,
            29, 29, 29, 29, 4, 4, 4, 4, 29, 4, 4, 4, 4, 4, 4, 4,
            29, 4, 4, 4, 29, 4, 29, 4, 29, 29, 4, 4, 29, 4, 4, 4,
            4, 5, 4, 4, 5, 5, 5, 5, 5, 5, 29, 5, 5, 4, 29, 29,
            4, 4, 4, 4, 4, 29, 3, 29, 5, 5, 5, 5, 5, 5, 29, 29,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 29, 29, 4, 4, 29, 29,
            4, 28, 28, 28, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
            24, 24, 24, 28, 28, 28, 28, 28, 5, 5, 28, 28, 28, 28, 28, 28,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 10, 10, 10, 10, 10, 10,
            10, 10, 10, 10, 28, 5, 28, 5, 28, 5, 20, 21, 20, 21, 6, 6,
            4, 4, 4, 4, 4, 4, 4, 4, 29, 4, 4, 4, 4, 4, 4, 4,
            29, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6,
            5, 5, 5, 5, 5, 24, 5, 5, 4, 4, 4, 4, 29, 29, 29, 29,
            5, 5, 5, 5, 5, 5, 5, 5, 29, 5, 5, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 29, 28, 28,
            28, 28, 28, 28, 28, 28, 5, 28, 28, 28, 28, 28, 28, 29, 29, 28,
            4, 4, 29, 4, 4, 4, 4, 4, 29, 4, 4, 29, 6, 5, 5, 5,
            5, 6, 5, 29, 29, 29, 5, 5, 6, 5, 29, 29, 29, 29, 29, 29,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 24, 24, 24, 24, 24, 24,
            4, 4, 4, 4, 4, 4, 6, 6, 5, 5, 29, 29, 29, 29, 29, 29,
            0, 0, 0, 0, 0, 0, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            4, 4, 4, 4, 4, 4, 4, 29, 29, 29, 29, 24, 29, 29, 29, 29,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 29, 29, 29, 29, 29, 4,
            4, 4, 4, 29, 29, 29, 29, 29, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 29, 29, 29, 29, 29, 29,
            4, 4, 4, 4, 4, 4, 4, 29, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 29, 4, 29, 4, 4, 4, 4, 29, 29,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 29,
            4, 29, 4, 4, 4, 4, 29, 29, 4, 4, 4, 4, 4, 4, 4, 29,
            29, 24, 24, 24, 24, 24, 24, 24, 24, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 29, 29, 29,
            4, 4, 4, 4, 4, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 24, 24, 4,
            4, 4, 4, 4, 4, 4, 4, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            11, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 20, 21, 29, 29, 29,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 24, 24, 24, 10, 10,
            10, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            4, 4, 4, 4, 6, 6, 6, 5, 5, 5, 5, 5, 5, 5, 6, 6,
            6, 6, 6, 6, 6, 6, 5, 6, 6, 5, 5, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 24, 24, 24, 24, 24, 24, 24, 26, 24, 29, 29, 29,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 29, 29, 29, 29, 29, 29,
            24, 24, 24, 24, 24, 24, 19, 24, 24, 24, 24, 15, 15, 15, 15, 29,
            4, 4, 4, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 29, 29, 29, 29, 29, 29, 29, 29,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 5, 29, 29, 29, 29, 29, 29,
            0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 29, 29, 29, 29,
            0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 29, 29, 29, 29, 29, 29,
            1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0,
            1, 1, 1, 1, 1, 1, 29, 29, 0, 0, 0, 0, 0, 0, 29, 29,
            1, 1, 1, 1, 1, 1, 1, 1, 29, 0, 29, 0, 29, 0, 29, 0,
            1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2,
            1, 1, 1, 1, 1, 29, 1, 1, 0, 0, 0, 0, 2, 27, 1, 27,
            27, 27, 1, 1, 1, 29, 1, 1, 0, 0, 0, 0, 2, 27, 27, 27,
            1, 1, 1, 1, 29, 29, 1, 1, 0, 0, 0, 0, 29, 27, 27, 27,
            1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 27, 27, 27,
            29, 29, 1, 1, 1, 29, 1, 1, 0, 0, 0, 0, 2, 27, 27, 29,
            11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 15, 15, 15, 15,
            19, 19, 19, 19, 19, 19, 24, 24, 22, 23, 20, 22, 22, 23, 20, 22,
            24, 24, 24, 24, 24, 24, 24, 24, 12, 13, 15, 15, 15, 15, 15, 11,
            24, 24, 24, 24, 24, 24, 24, 24, 24, 22, 23, 24, 24, 24, 24, 18,
            18, 24, 24, 24, 25, 20, 21, 29, 24, 24, 24, 24, 24, 24, 29, 29,
            29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 15, 15, 15, 15, 15, 15,
            10, 29, 29, 29, 10, 10, 10, 10, 10, 10, 25, 25, 25, 20, 21, 1,
            10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 25, 25, 25, 20, 21, 29,
            26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 7, 7, 7,
            7, 5, 7, 7, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            28, 28, 0, 28, 28, 28, 28, 0, 28, 28, 1, 0, 0, 0, 1, 1,
            0, 0, 0, 1, 28, 0, 28, 28, 28, 0, 0, 0, 0, 0, 28, 28,
            28, 28, 28, 28, 0, 28, 0, 28, 0, 28, 0, 0, 0, 0, 28, 1,
            0, 0, 28, 0, 1, 4, 4, 4, 4, 1, 28, 29, 29, 29, 29, 29,
            29, 29, 29, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,
            9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
            9, 9, 9, 9, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            25, 25, 25, 25, 25, 28, 28, 28, 28, 28, 25, 25, 28, 28, 28, 28,
            25, 28, 28, 25, 28, 28, 25, 28, 28, 28, 28, 28, 28, 28, 25, 28,
            28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
            28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 25, 25,
            28, 28, 25, 28, 25, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
            28, 28, 28, 28, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
            25, 25, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            28, 28, 28, 28, 28, 28, 28, 28, 25, 25, 25, 25, 28, 28, 28, 28,
            25, 25, 28, 28, 28, 28, 28, 28, 28, 20, 21, 28, 28, 28, 28, 28,
            28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 29, 28, 28, 28,
            28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 29, 29, 29, 29, 29,
            28, 28, 28, 28, 28, 28, 28, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,
            10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 28, 28, 28, 28,
            28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 10, 29, 29, 29, 29, 29,
            28, 28, 28, 28, 28, 28, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            28, 28, 28, 28, 28, 28, 28, 25, 28, 28, 28, 28, 28, 28, 28, 28,
            28, 25, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
            28, 28, 28, 28, 28, 28, 28, 28, 29, 29, 29, 29, 29, 29, 29, 29,
            28, 28, 28, 28, 29, 29, 29, 29, 29, 28, 28, 28, 28, 28, 28, 28,
            28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 25,
            28, 28, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            29, 28, 28, 28, 28, 29, 28, 28, 28, 28, 29, 29, 28, 28, 28, 28,
            28, 28, 28, 28, 28, 28, 28, 28, 29, 28, 28, 28, 28, 28, 28, 28,
            28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 29, 28, 29, 28,
            28, 28, 28, 29, 29, 29, 28, 29, 28, 28, 28, 28, 28, 28, 28, 29,
            29, 28, 28, 28, 28, 28, 28, 28, 29, 29, 29, 29, 29, 29, 29, 29,
            29, 29, 29, 29, 29, 29, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,
            10, 10, 10, 10, 28, 29, 29, 29, 28, 28, 28, 28, 28, 28, 28, 28,
            29, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 29,
            28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 29, 28, 28, 28, 28, 28,
            28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 29, 29, 29, 29,
            11, 24, 24, 24, 28, 3, 4, 9, 20, 21, 20, 21, 20, 21, 20, 21,
            20, 21, 28, 28, 20, 21, 20, 21, 20, 21, 20, 21, 19, 20, 21, 21,
            28, 9, 9, 9, 9, 9, 9, 9, 9, 9, 5, 5, 5, 5, 5, 5,
            19, 3, 3, 3, 3, 3, 28, 28, 9, 9, 9, 29, 29, 29, 28, 28,
            4, 4, 4, 4, 4, 29, 29, 29, 29, 5, 5, 27, 27, 3, 3, 29,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 18, 3, 3, 3, 29,
            29, 29, 29, 29, 29, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            28, 28, 10, 10, 10, 10, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
            28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 29, 29, 29,
            10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 28, 28, 28, 28, 28, 28,
            28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 29, 29, 29, 28,
            28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 29,
            28, 28, 28, 28, 28, 28, 28, 29, 29, 29, 29, 28, 28, 28, 28, 28,
            28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 29, 29,
            4, 4, 4, 4, 4, 4, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            28, 28, 29, 29, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
            28, 28, 28, 28, 29, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
            28, 29, 28, 28, 28, 29, 28, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            4, 4, 4, 4, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16,
            17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 29, 29,
            1, 1, 1, 1, 1, 1, 1, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            29, 29, 29, 1, 1, 1, 1, 1, 29, 29, 29, 29, 29, 4, 5, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 25, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 29, 4, 4, 4, 4, 4, 29, 4, 29,
            4, 4, 29, 4, 4, 29, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            29, 29, 29, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 20, 21,
            29, 29, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 29, 29, 29, 29,
            5, 5, 5, 5, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
            24, 19, 19, 18, 18, 20, 21, 20, 21, 20, 21, 20, 21, 20, 21, 20,
            21, 20, 21, 20, 21, 29, 29, 29, 29, 24, 24, 24, 24, 18, 18, 18,
            24, 24, 24, 29, 24, 24, 24, 24, 19, 20, 21, 20, 21, 20, 21, 24,
            24, 24, 25, 19, 25, 25, 25, 29, 24, 26, 24, 24, 29, 29, 29, 29,
            4, 4, 4, 29, 4, 29, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 29, 29, 15,
            29, 24, 24, 24, 26, 24, 24, 24, 20, 21, 24, 25, 24, 19, 24, 24,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 20, 25, 21, 25, 29,
            29, 24, 20, 21, 24, 18, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 3, 3,
            29, 29, 4, 4, 4, 4, 4, 4, 29, 29, 4, 4, 4, 4, 4, 4,
            29, 29, 4, 4, 4, 4, 4, 4, 29, 29, 4, 4, 4, 29, 29, 29,
            26, 26, 25, 27, 28, 26, 26, 29, 28, 25, 25, 25, 25, 28, 28, 29,
            29, 29, 29, 29, 29, 29, 29, 29, 29, 15, 15, 15, 28, 28, 29, 29
        };

        public static UnicodeCategory GetUnicodeCategory(char ch) {
            byte index1 = level1CategoryTable[ch >> 8];
            ushort index2 = level2CategoryTable[(index1<<4) + ((ch>>4) & 0xf)];
            byte result = level3CategoryTable[index2 + (ch & 0xf)];
            return (UnicodeCategory) result;
        }
    }
}

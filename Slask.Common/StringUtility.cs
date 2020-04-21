using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Slask.Common
{
    // CREATE TESTS
    public static class StringUtility
    {
        public static List<string> ToStringList(string text, string delimiter)
        {
            bool textIsInvalid = IsNullOrEmpty(text);
            bool delimiterIsInvalid = IsNullOrEmpty(delimiter);

            if (textIsInvalid || delimiterIsInvalid)
            {
                return new List<string>();
            }

            List<string> textList = text.Split(delimiter).ToList();

            for (int index = 0; index < textList.Count; ++index)
            {
                bool isEmpty = textList[index].Length == 0;

                if (isEmpty)
                {
                    textList.RemoveAt(index--);
                    continue;
                }

                textList[index] = textList[index].Replace(" ", string.Empty, StringComparison.CurrentCulture);
            }

            return textList;
        }

        public static string ToUpperNoSpaces(string text)
        {
            bool textIsInvalid = IsNullOrEmpty(text);

            if (textIsInvalid)
            {
                return text;
            }

            return text.ToUpper(CultureInfo.CurrentCulture).Replace(" ", string.Empty, StringComparison.CurrentCulture);
        }

        public static bool IsNullOrEmpty(string text)
        {
            bool textIsInvalid = text == null;

            if (textIsInvalid)
            {
                return true;
            }

            string textSansWhitespace = text.Replace(" ", string.Empty, StringComparison.CurrentCulture);
            bool textIsEmpty = textSansWhitespace.Length == 0;

            return textIsEmpty;
        }
    }
}

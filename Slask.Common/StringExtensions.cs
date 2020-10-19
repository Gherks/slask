using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Slask.Common
{
    public static class StringExtensions
    {
        public static List<string> ToStringList(this string text, string delimiter)
        {
            bool textIsInvalid = string.IsNullOrWhiteSpace(text);
            bool delimiterIsInvalid = string.IsNullOrWhiteSpace(delimiter);

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

                textList[index] = textList[index].Replace(" ", string.Empty);
            }

            return textList;
        }

        public static string ToUpperNoSpaces(this string text)
        {
            bool textIsInvalid = string.IsNullOrWhiteSpace(text);

            if (textIsInvalid)
            {
                return text;
            }

            return text.ToUpper().Replace(" ", string.Empty);
        }
    }
}

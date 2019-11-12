using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Slask.Common
{
    public static class StringUtility
    { 
        public static List<string> ToStringList(string text, string delimiter)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            List<string> textList = text.Split(delimiter).ToList();

            for (int index = 0; index < textList.Count; ++index)
            {
                textList[index] = textList[index].Replace(" ", string.Empty, StringComparison.CurrentCulture);
            }

            return textList;
        }

        public static string ToUpperNoSpaces(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return text.ToUpper(CultureInfo.CurrentCulture).Replace(" ", string.Empty, StringComparison.CurrentCulture);
        }
    }
}

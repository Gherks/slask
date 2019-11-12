using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Common
{
    public static class StringUtility
    { 
        public static List<string> StringToStringList(string text, string delimiter)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            List<string> textList = text.Split(delimiter).ToList();

            for (int index = 0; index < textList.Count; ++index)
            {
                textList[index] = textList[index].Replace(" ", string.Empty);
            }

            return textList;
        }
    }
}

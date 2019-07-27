using System;

namespace Slask.Common
{
    public class DateTimeHelper
    {
        public static DateTime Now => DateTimeProvider.Current.Now;
    }
}

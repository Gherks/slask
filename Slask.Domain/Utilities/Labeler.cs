namespace Slask.Domain.Utilities
{
    public static class Labeler
    {
        public static string GetLabelForIndex(int letterIndex)
        {
            const string lookup = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int letterCount = lookup.Length;

            if (letterIndex >= letterCount)
            {
                int endingIndex = letterIndex % letterCount;
                int restIndex = (letterIndex - endingIndex) / letterCount;
                return GetLabelForIndex(restIndex - 1) + GetLabelForIndex(endingIndex);
            }
            else
            {
                return lookup[letterIndex].ToString();
            }
        }
    }
}

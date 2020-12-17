namespace DartCore.Utilities
{
    public class StringUtilities
    {
        /// <summary>
        /// Puts '\n's between words if the line length exceeds the given limit.
        /// </summary>
        /// <param name="originalString">the raw string.</param>
        /// <param name="maxLineLength">max line length in characters.</param>
        /// <returns></returns>
        public static string IncreaseLinesIfNecessary(string originalString, int maxLineLength = 25)
        {
            if (maxLineLength <= 0) return originalString;
            
            var newWords = originalString.Split(' ');
            var originalWords = originalString.Split(' ');
        
            var letterCount = 0;
            for (var i = 0; i < originalWords.Length; i++)
            {
                if (letterCount >= maxLineLength)
                {
                    newWords[i] = originalWords[i].Insert(0, "\n");
                    letterCount = 0;
                }

                letterCount += originalWords[i].Length;
            }

            var newString = "";
            foreach (var word in newWords) newString += word + " ";
            return newString.Remove(newString.Length - 1);
        }
    }
}
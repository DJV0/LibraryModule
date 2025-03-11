namespace LibraryModule.Helpers
{
    internal class SearchingHelper
    {
        public static int BoyerMooreSearch(string text, string pattern)
        {
            int textLength = text.Length;
            int patternLength = pattern.Length;

            if (patternLength == 0) 
                return -1;

            int[] badChar = BuildBadCharacterTable(pattern);

            int shift = 0;
            while (shift <= (textLength - patternLength))
            {
                int j = patternLength - 1;

                while (j >= 0 && pattern[j] == text[shift + j])
                    j--;

                if (j < 0)
                    return shift;

                shift += Math.Max(1, j - badChar[text[shift + j]]);
            }

            return -1;
        }

        private static int[] BuildBadCharacterTable(string pattern)
        {
            const int alphabetSize = 256;
            int[] badChar = new int[alphabetSize];

            for (int i = 0; i < alphabetSize; i++)
                badChar[i] = -1;

            for (int i = 0; i < pattern.Length; i++)
                badChar[pattern[i]] = i;

            return badChar;
        }
    }
}

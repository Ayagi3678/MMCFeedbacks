using Unity.VisualScripting;

namespace MMCFeedbacks.Core
{
    public static class StringConversionUtility
    {
        public static string SplitLast(string str,char splitChar='/')
        {
            var splitString = str.Split(splitChar);
            return splitString[^1] ?? str;
        }
        
        public static string SeparateCamelCase(string input)
        {
            string separated = System.Text.RegularExpressions.Regex.Replace(input, "(?<=.)([A-Z])", " $1");
            return separated;
        }
    }
}
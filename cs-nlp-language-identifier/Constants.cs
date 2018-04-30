namespace LanguageIdentifier
{
    public class Constants
    {
        public static char[] DefaultSplitChars
        {
            get
            {
                return new char[15] { ' ', '\r', '\t', '\n', ',', ';', '.', ':', '[', ']', '(', ')', '{', '}', '"' };
            }
        }
    }
}

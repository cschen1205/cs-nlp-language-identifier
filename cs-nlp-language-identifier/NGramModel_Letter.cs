namespace LanguageIdentifier
{
    public class NGramModel_Letter : NGramModel<char>
    {
        public NGramModel_Letter(int N)
            : base(N)
        {
        }

        public void Parse4Gram(string word)
        {
            char[] chars = word.ToCharArray();
            Parse4Gram(chars);
        }
    }
}

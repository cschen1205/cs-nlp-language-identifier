using System;
using System.Collections.Generic;

namespace LanguageIdentifier
{
    /// <summary>
    /// base on probabilistic model
    /// </summary>
    public class NGramLanguageIdentifier 
    {
        protected Dictionary<string, NGramModel_Letter> mLangModels = new Dictionary<string, NGramModel_Letter>();
        protected int mN;
        public NGramLanguageIdentifier(int N=2)
        {
            mN = N;
        }

        public NGramModel_Letter FindModel(string language)
        {
            if (mLangModels.ContainsKey(language))
            {
                return mLangModels[language];
            }
            else
            {
                NGramModel_Letter model = new NGramModel_Letter(mN);
                mLangModels[language] = model;

                return model;
            }
        }

        public void ReadWord(string word, NGramModel_Letter model)
        {
            model.Parse4Gram(word);
        }

        public void ReadParagraph(string paragraph, string language)
        {
            ReadParagraph(paragraph, language, Constants.DefaultSplitChars);
        }

        public string GetLanguage(string paragraph)
        {
            return GetLanguage(paragraph, Constants.DefaultSplitChars);
        }

        public string GetLanguage(string paragraph, char[] chars)
        {
            Dictionary<string, Dictionary<string, NGram<char>>> ngram_group = new Dictionary<string, Dictionary<string, NGram<char>>>();
            string[] words = paragraph.Split(chars, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                NGramModel_Letter.ParseStoreGram(word.ToCharArray(), ngram_group, mN);
            }
            string selected_language = null;
            double min_distance = double.MaxValue;
            double distance = 0;
            foreach (string language in mLangModels.Keys)
            {
                NGramModel_Letter model = mLangModels[language];
                distance = model.GetDistanceSq(ngram_group);
                if (distance < min_distance)
                {
                    min_distance = distance;
                    selected_language = language;
                }
            }

            return selected_language;
        }

        public void ReadParagraph(string paragraph, string language, char[] chars)
        {
            string[] words = paragraph.Split(chars, StringSplitOptions.RemoveEmptyEntries);
            NGramModel_Letter model = FindModel(language);
            foreach (string word in words)
            {
                ReadWord(word, model);
            }
        }
    }
}

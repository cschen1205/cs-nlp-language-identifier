using System.Collections.Generic;

namespace LanguageIdentifier
{
    public class NGramModel<T>
    {
        protected int mN;
        protected Dictionary<string, Dictionary<string, NGram<T>>> mNGrams = new Dictionary<string, Dictionary<string, NGram<T>>>();

        public NGramModel(int N)
        {
            mN = N;
        }

        public Dictionary<string, NGram<T>> FindNGramWithEvidenceSetId(string evidence_set_id)
        {
            if (evidence_set_id == null)
            {
                evidence_set_id = "UniGram";
            }
            if (mNGrams.ContainsKey(evidence_set_id))
            {
                return mNGrams[evidence_set_id];
            }
            else
            {
                Dictionary<string, NGram<T>> ngrams = new Dictionary<string, NGram<T>>();
                mNGrams[evidence_set_id] = ngrams;
                return ngrams;
            }
        }

        public NGram<T> FindNGram(string evidence_set_id, string query_variable_value_id)
        {
            Dictionary<string, NGram<T>> ngrams = FindNGramWithEvidenceSetId(evidence_set_id);
            if (ngrams.ContainsKey(query_variable_value_id))
            {
                return ngrams[query_variable_value_id];
            }
            return null;
        }

        public void Parse4Gram(T[] sequence)
        {
            ParseStoreGram(sequence, mNGrams, mN);
        }

        public virtual double GetDistanceSq(Dictionary<string, Dictionary<string, NGram<T>>> ngram_group)
        {
            double sqr_sum = 0;
            double ngram_distance = 0;
            foreach (string evidence_set_id in ngram_group.Keys)
            {
                Dictionary<string, NGram<T>> this_ngrams_by_query_variable_value = FindNGramWithEvidenceSetId(evidence_set_id);

                Dictionary<string, NGram<T>> ngrams_by_query_variable_value = ngram_group[evidence_set_id];
                foreach (string query_variable_value_id in ngrams_by_query_variable_value.Keys)
                {
                    NGram<T> ngram = ngrams_by_query_variable_value[query_variable_value_id];
                    if (this_ngrams_by_query_variable_value.ContainsKey(query_variable_value_id))
                    {
                        NGram<T> this_ngram = this_ngrams_by_query_variable_value[query_variable_value_id];
                        ngram_distance = this_ngram.ConditionalProbability - ngram.ConditionalProbability;
                    }
                    else
                    {
                        ngram_distance = ngram.ConditionalProbability;
                    }
                    sqr_sum += ngram_distance * ngram_distance;
                }
            }

            return sqr_sum;
        }

        public static void ParseStoreGram(T[] sequence, Dictionary<string, Dictionary<string, NGram<T>>> ngram_group, int N)
        {
            int L = sequence.Length;
            int K = L - N+1;

            T query_variable_value;
            T[] evidence_variable_values;
            string query_variable_value_id;
            string ngram_id;
            string evidence_set_id;

            for (int k = 0; k < K; ++k)
            {
                SubSequence(sequence, k, N, out query_variable_value, out evidence_variable_values);
                ngram_id = NGram<T>.CreateNGramSignature(query_variable_value, evidence_variable_values, out query_variable_value_id, out evidence_set_id);
                Dictionary<string, NGram<T>> ngrams = null;

                if (evidence_set_id == null)
                {
                    evidence_set_id = "UniGram";
                }
                if (ngram_group.ContainsKey(evidence_set_id))
                {
                    ngrams = ngram_group[evidence_set_id];
                }
                else
                {
                    ngrams = new Dictionary<string, NGram<T>>();
                    ngram_group[evidence_set_id] = ngrams;
                }

                NGram<T> ngram = null;
                if (ngrams.ContainsKey(query_variable_value_id))
                {
                    ngram = ngrams[query_variable_value_id];
                }
                else
                {
                    ngram = new NGram<T>(N, query_variable_value, evidence_variable_values);
                    ngrams[query_variable_value_id] = ngram;
                }
                ngram.EvidenceJointSupportLevel++;
                ngram.JoinSupportLevel++;
            }
        }

        private static void SubSequence(T[] sequence, int k, int N, out T query_variable_value, out T[] evidence_variable_values)
        {
            int evidence_length = N - 1;

            evidence_variable_values = null;
            if (N > 1)
            {
                evidence_variable_values = new T[evidence_length];
            }

            for (int i = 0; i < evidence_length; ++i)
            {
                evidence_variable_values[i] = sequence[k + i];
            }

            query_variable_value = sequence[k + N-1];
            
        }
    }
}

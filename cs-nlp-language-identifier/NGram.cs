using System.Text;

namespace LanguageIdentifier
{
    public class NGram<T>
    {
        protected int mN;
        protected T[] mEvidenceVariableValues;
        protected T mQueryVariableValue;
        protected int mJoinSupportLevel = 0;
        protected int mEvidenceJointSupportLevel = 0;
        protected string mID;
        protected string mQueryVariableValueID;
        protected string mEvidenceSetID;

        public NGram(int N, T query_variable_value, T[] evidence_variable_values)
        {
            mQueryVariableValue = query_variable_value;
            mEvidenceVariableValues = evidence_variable_values;
            mN = N;
            mID = CreateNGramSignature(mQueryVariableValue, mEvidenceVariableValues, out mQueryVariableValueID, out mEvidenceSetID);
        }

        public bool IsUniGram
        {
            get { return mEvidenceVariableValues == null; }
        }

        public static string CreateNGramSignature(T query_variable_value, T[] evidence_variable_values, out string query_variable_value_id, out string evidence_set_id)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0}", query_variable_value);

            query_variable_value_id = sb.ToString();
            
            evidence_set_id = CreateEvidenceSignature(evidence_variable_values);

            if (evidence_set_id != null)
            {
                sb.Append("|");
                sb.Append(evidence_set_id);
            }

            return sb.ToString();
        }

        public static string CreateEvidenceSignature(T[] evidence_variable_values)
        {
            if (evidence_variable_values == null) return null;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < evidence_variable_values.Length; ++i)
            {
                if (i != 0) sb.Append(",");
                sb.AppendFormat("{0}", evidence_variable_values[i]);
            }

            return sb.ToString();
        }

        public int JoinSupportLevel
        {
            get { return mJoinSupportLevel; }
            set { mJoinSupportLevel = value; }
        }

        public int EvidenceJointSupportLevel
        {
            get { return mEvidenceJointSupportLevel; }
            set { mEvidenceJointSupportLevel = value; }
        }

        public string ID
        {
            get
            {
                return mID;
            }
        }

        public string EvidenceSetID
        {
            get
            {
                return mEvidenceSetID;
            }
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is NGram<T>)
            {
                NGram<T> rhs = obj as NGram<T>;
                return ID == rhs.ID;
            }
            return false;
        }

        public double ConditionalProbability
        {
            get
            {
                if (mEvidenceJointSupportLevel != 0)
                {
                    return (double)mJoinSupportLevel / mEvidenceJointSupportLevel;
                }
                return 0;
            }
        }

        public override string ToString()
        {
            if (mEvidenceVariableValues != null)
            {
                return string.Format("P({0}) = {1:0.00}", ID, ConditionalProbability);
            }
            else
            {
                return ID;
            }
        }
    }
}

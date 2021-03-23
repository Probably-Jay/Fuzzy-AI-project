using System.Collections.Generic;
using UnityEngine;
namespace FuzzyLogic
{
  

    /// <summary>
    /// A fuzzy number
    /// </summary>
    internal class FuzzyNumber
    {


        /// <summary>
        /// Number of states a fuzzy number can be in
        /// </summary>
        public const int NumberOfMemberships = 5;

        /// <summary>
        /// The satates that a fuzzy number can be in, and their normalised values
        /// </summary>
        public static readonly Dictionary<FuzzyUtility.FuzzyStates, float> NormalisedStateValues = new Dictionary<FuzzyUtility.FuzzyStates, float>() 
        {
            {FuzzyUtility.FuzzyStates.LP,   1f }
            , {FuzzyUtility.FuzzyStates.MP, 0.5f }
            , {FuzzyUtility.FuzzyStates.Z,  0f }
            , {FuzzyUtility.FuzzyStates.MN, -0.5f }
            , {FuzzyUtility.FuzzyStates.LN, -1f }
        };


        private float[] values = new float[NumberOfMemberships];

        /// <summary>
        /// Indexer to the membership-value of this number for each state
        /// </summary>
        /// <param name="state">A <see cref="FuzzyLogic.FuzzyUtility.FuzzyStates"/></param>
        /// <returns>A float value representing membership</returns>
        public float this[FuzzyUtility.FuzzyStates state]
        {
            get { return values[(int)state]; }
            set { values[(int)state] = value; }
        }

        /// <summary>
        /// The sum of the membership amounts
        /// </summary>
        public float Sum
        {
            get
            {
                float total = 0;
                foreach (var value in values)
                {
                    total += value;
                }
                return total;
            }
        }

        /// <summary>
        /// Logical-Not operator, <c>1 - value</c>
        /// </summary>
        public static FuzzyNumber operator !(FuzzyNumber n)
        {
            var negation = new FuzzyNumber();
            foreach (FuzzyUtility.FuzzyStates state in System.Enum.GetValues(typeof(FuzzyUtility.FuzzyStates)))
            {
                negation[state] = 1f - n[state];
            }
            return negation;
        }

        /// <summary>
        /// Logical-Or operator, <see cref="UnityEngine.Mathf.Max(float, float)"/> of <c>value</c>
        /// </summary>
        public static FuzzyNumber operator |(FuzzyNumber a,FuzzyNumber b)
        {
            var or = new FuzzyNumber();
            foreach (FuzzyUtility.FuzzyStates state in System.Enum.GetValues(typeof(FuzzyUtility.FuzzyStates)))
            {
                or[state] = Mathf.Max(a[state], b[state]);
            }
            return or;
        }

        /// <summary>
        /// Logical-And operator, <see cref="UnityEngine.Mathf.Min(float, float)"/> of <c>value</c>
        /// </summary>
        public static FuzzyNumber operator &(FuzzyNumber a,FuzzyNumber b)
        {
            var and = new FuzzyNumber();
            foreach (FuzzyUtility.FuzzyStates state in System.Enum.GetValues(typeof(FuzzyUtility.FuzzyStates)))
            {
                and[state] = Mathf.Min(a[state], b[state]);
            }
            return and;
        }


    }
}


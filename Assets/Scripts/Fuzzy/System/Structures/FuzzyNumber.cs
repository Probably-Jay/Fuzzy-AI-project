using System.Collections.Generic;
using UnityEngine;
namespace FuzzyLogic
{
  

    /// <summary>
    /// A fuzzy number
    /// </summary>
    internal class FuzzyNumber
    {



        public const int NumberOfMemberships = 5;

        public static readonly Dictionary<FuzzyUtility.FuzzyStates, float> NormalisedStateValues = new Dictionary<FuzzyUtility.FuzzyStates, float>() 
        {
            {FuzzyUtility.FuzzyStates.LP,   1f }
            , {FuzzyUtility.FuzzyStates.MP, 0.5f }
            , {FuzzyUtility.FuzzyStates.Z,  0f }
            , {FuzzyUtility.FuzzyStates.MN, -0.5f }
            , {FuzzyUtility.FuzzyStates.LN, -1f }
        };


        private float[] values = new float[NumberOfMemberships];

        public float this[FuzzyUtility.FuzzyStates state]
        {
            get { return values[(int)state]; }
            set { values[(int)state] = value; }
        }

        public void Normalise()
        {
            float mag = Magnitude;

            for (int i = 0; i < values.Length; i++)
            {
                values[i] /= mag;
            }

        }

        public float Magnitude
        {
            get
            {
                float total = 0;
                foreach (var value in values)
                {
                    total += value * value;
                }

                var mag = Mathf.Sqrt(total);
                return mag;
            }
        }  
        
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





        public static FuzzyNumber operator !(FuzzyNumber n)
        {
            var negation = new FuzzyNumber();
            foreach (FuzzyUtility.FuzzyStates state in System.Enum.GetValues(typeof(FuzzyUtility.FuzzyStates)))
            {
                negation[state] = 1f - n[state];
            }
            return negation;
        }     
        
        public static FuzzyNumber operator |(FuzzyNumber a,FuzzyNumber b)
        {
            var or = new FuzzyNumber();
            foreach (FuzzyUtility.FuzzyStates state in System.Enum.GetValues(typeof(FuzzyUtility.FuzzyStates)))
            {
                or[state] = Mathf.Max(a[state], b[state]);
            }
            return or;
        } 
        
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


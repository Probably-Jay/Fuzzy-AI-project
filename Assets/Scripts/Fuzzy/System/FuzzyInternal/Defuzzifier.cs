using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FuzzyLogic
{

    internal class Defuzzifier
    {

       // public FunctionCurve outputCurve;

        public enum DefuzificationMethod
        {
            Maximum
            , CenterOfMass
        }

        public DefuzificationMethod defuzificationMethod = DefuzificationMethod.CenterOfMass;

        public CrispOutput Defuzzify(FuzzyOutputData fuzzyOutput)
        {
            CrispOutput crispOutput = new CrispOutput();
            foreach (CrispOutput.Outputs variable in CrispOutput.OutputEnumvalues)
            {
                crispOutput[variable] = GetCrispValue(fuzzyOutput[variable]);
            }

            return crispOutput;
        }

        private float GetCrispValue(FuzzyNumber fuzzyNumber)
        {
            switch (defuzificationMethod)
            {
                case DefuzificationMethod.Maximum:
                    return Maximum(fuzzyNumber);
                case DefuzificationMethod.CenterOfMass:
                    return CenterOfMass(fuzzyNumber);
                default:
                    return -1;
            }
        }

        private float Maximum(FuzzyNumber fuzzyNumber)
        {
            float max = 0;
            FuzzyUtility.FuzzyStates maxState = FuzzyUtility.FuzzyStates.Z;

            foreach (FuzzyUtility.FuzzyStates state in System.Enum.GetValues(typeof(FuzzyUtility.FuzzyStates)))
            {
                float current = fuzzyNumber[state];
                if (current <= max)
                {
                    continue;
                }
                else
                {
                    maxState = state;
                    max = current;
                }
            }

            float meanOfMaximum = FuzzyNumber.NormalisedStateValues[maxState];

            return meanOfMaximum;

        }


        private float CenterOfMass(FuzzyNumber fuzzyNumber)
        {
            float weightedTotal = 0;
            float sum = fuzzyNumber.Sum;

            // weight each value 
            foreach (FuzzyUtility.FuzzyStates state in System.Enum.GetValues(typeof(FuzzyUtility.FuzzyStates)))
            {
                float weighedValue = fuzzyNumber[state] * FuzzyNumber.NormalisedStateValues[state];
                weighedValue /= sum;

                weightedTotal += weighedValue;
            }


            return weightedTotal;


        }
    }
}

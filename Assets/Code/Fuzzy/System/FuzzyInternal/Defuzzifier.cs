using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FuzzyLogic
{
    /// <summary>
    /// Class that produces <see cref="FuzzyLogic.CrispOutput"/> based on the <see cref="FuzzyLogic.FuzzyOutputData"/> given by <see cref="FuzzyLogic.InferenceEngine"/>
    /// </summary>
    internal class Defuzzifier
    {



        /// <summary>
        /// The method by which defuzzification will take place. 
        /// See <see cref="FuzzyLogic.FuzzySystem.DefuzzificationMethod"/>
        /// </summary>
        public FuzzySystem.DefuzzificationMethod defuzificationMethod = FuzzySystem.DefuzzificationMethod.CenterOfMass;

        /// <summary>
        /// Takes <see cref="FuzzyLogic.FuzzyOutputData"/> and from a call of <see cref="FuzzyLogic.InferenceEngine.ApplyRulset(FuzzyInputData)"/> 
        /// and returns a new into <see cref="FuzzyLogic.CrispOutput"/> using the current method of <see cref="FuzzyLogic.Defuzzifier.defuzificationMethod"/>
        /// </summary>
        /// <param name="fuzzyOutput">The fuzzy data to be defuzzified</param>
        /// <returns>A <see cref="FuzzyLogic.CrispOutput"/> which can be used by the external system</returns>
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
                case FuzzySystem.DefuzzificationMethod.Maximum:
                    return Maximum(fuzzyNumber);
                case FuzzySystem.DefuzzificationMethod.CenterOfMass:
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FuzzyLogic
{

    /// <summary>
    /// Takes <see cref="FuzzyLogic.CrispInput"/> and converts it into <see cref="FuzzyLogic.FuzzyInputData"/>
    /// </summary>
    internal class Fuzifier
    {

        public FunctionCurve inputFunction;

        /// <summary>
        /// Takes <see cref="FuzzyLogic.CrispInput"/> and turns it into <see cref="FuzzyLogic.FuzzyInputData"/> based on the evaluated value at <see cref="inputFunction"/>
        /// </summary>
        /// <param name="crispInput">Crisp data from sensors</param>
        /// <returns>Fuzzified data</returns>
        public FuzzyInputData Fuzzify(CrispInput crispInput)
        {
            FuzzyInputData fuzzy = new FuzzyInputData();

            // normalise input here??

            foreach (CrispInput.Inputs variable in System.Enum.GetValues(typeof(CrispInput.Inputs)))
            {
                foreach (FuzzyUtility.FuzzyStates state in System.Enum.GetValues(typeof(FuzzyUtility.FuzzyStates)))
                {
                    fuzzy[variable][state] = inputFunction[state].Evaluate(crispInput[variable]);
                }
            }

            // normalise output here??

            return fuzzy;

        }
    } 
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FuzzyLogic
{

    public class FuzzySystem : MonoBehaviour
    {
        private Fuzifier fuzifier = new Fuzifier();
        private InferenceEngine inferenceEngine = new InferenceEngine();
        private Defuzzifier defuzzifier = new Defuzzifier();

        private FunctionCurve functionCurve = null;
        private FuzzyRulesList fuzzyRulesList = null;


        public FunctionCurve FunctionCurve { get => functionCurve; 
            set
            {
                functionCurve = value;
                fuzifier.inputFunction = value;
            }
        }
        public FuzzyRulesList FuzzyRulesList { get => fuzzyRulesList;
            set
            {
                fuzzyRulesList = value;
                inferenceEngine.fuzzyRules = value;
            }
        }


        /// <summary>
        /// Compute fuzzy logic on the <paramref name="crispInput"/>, using the current <see cref="FunctionCurve"/> and <see cref="FuzzyRulesList"/>. Returns normalised values between <c>[-1,1]</c>
        /// </summary>
        public CrispOutput FuzzyCompute(CrispInput crispInput)
        {
            FuzzyInputData fuzzyInput = fuzifier.Fuzzify(crispInput);
            FuzzyOutputData fuzzyOutput = inferenceEngine.ApplyRulset(fuzzyInput);
            CrispOutput crispOutput = defuzzifier.Defuzzify(fuzzyOutput);
            return crispOutput;
        }

        /// <summary>
        /// Builds a new <see cref="FuzzyLogic.CrispInput"/> based on povided normalised <paramref name="rawInputs"/> within <c>[-1,1]</c>
        /// </summary>
        /// <param name="rawInputs">A list of raw "crisp" float inputs, within <c>[-1,1]</c></param>
        /// <returns>A new <see cref="FuzzyLogic.CrispInput"/></returns>
        public CrispInput BuildInput(float[] rawInputs) => Build(rawInputs);

        /// <summary>
        /// Builds a new <see cref="FuzzyLogic.CrispInput"/> based on povided <paramref name="rawInputs"/> from a clamped scale where [<paramref name="minValues"/>,<paramref name="maxValues"/>] 
        /// will be mapped to <c>[-1,1]</c>
        /// </summary>
        /// <param name="rawInputs">A list of raw "crisp" float inputs, within [<paramref name="minValues"/>,<paramref name="maxValues"/>]</param>
        /// <param name="minValues">A list of minimum values where <paramref name="rawInputs"/> will be mapped to <c>-1</c></param>
        /// <param name="maxValues">A list of maximum values where <paramref name="rawInputs"/> will be mapped to <c>1</c></param>
        /// <returns>A new normalised <see cref="FuzzyLogic.CrispInput"/></returns>
        public CrispInput BuildInputNormalised(float[] rawInputs, float[] minValues, float[] maxValues)
        {
            float[] normalisedInputs = new float[rawInputs.Length];
            for (int i = 0; i < rawInputs.Length; i++)
            {
                normalisedInputs[i] = FuzzyUtility.NormaliseValue(minValues[i], maxValues[i], rawInputs[i]);
            }
            return Build(normalisedInputs);
        }

        /// <summary>
        /// Builds a new <see cref="FuzzyLogic.CrispInput"/> based on povided <paramref name="rawInputs"/> from an uneven clamped scale where [<paramref name="minValues"/>,<paramref name="maxValues"/>] 
        /// will be mapped to <c>[-1,1]</c>, and <paramref name="neutralValues"/> represents neutral values wich will be mapped to <c>0</c>
        /// </summary>
        /// <param name="rawInputs">A list of raw "crisp" float inputs, within [<paramref name="minValues"/>,<paramref name="maxValues"/>]</param>
        /// <param name="minValues">A list of minimum values where <paramref name="rawInputs"/> will be mapped to <c>-1</c></param>
        /// <param name="neutralValues">A list of neutral values where <paramref name="rawInputs"/> will be mapped to <c>0</c></param>
        /// <param name="maxValues">A list of maximum values where <paramref name="rawInputs"/> will be mapped to <c>1</c></param>
        /// <returns>A new unevenly-normalised <see cref="FuzzyLogic.CrispInput"/></returns>
        public CrispInput BuildInputNormalisedUneven(float[] rawInputs, float[] minValues, float[] neutralValues, float[] maxValues)
        {
            float[] normalisedInputs = new float[rawInputs.Length];
            for (int i = 0; i < rawInputs.Length; i++)
            {
                normalisedInputs[i] = FuzzyUtility.NormaliseValueUneven(minValues[i], neutralValues[i], maxValues[i], rawInputs[i]);
            }
            return Build(normalisedInputs);
        }


        private CrispInput Build(float[] rawInputs)
        {
            if (rawInputs.Length > CrispInput.NumberOfVariables) 
                throw new ArgumentOutOfRangeException($"You cannot provide more data points than {nameof(CrispInput.NumberOfVariables)}: {CrispInput.NumberOfVariables}");

            return new CrispInput(rawInputs);
        }

        /// <summary>
        /// Returns a new <see cref="FuzzyLogic.CrispOutput"/> that was generated from a call of <see cref="FuzzyLogic.FuzzySystem.FuzzyCompute(CrispInput)"/>
        /// with a paramater generated by a call of <see cref="FuzzyLogic.FuzzySystem.BuildInputNormalised(float[], float[], float[])(float[], float[], float[], float[])"/>.
        /// Maps each value that was normalised to <c>[-1,1]</c> to again be between the clamped range in [<paramref name="minValues"/>,<paramref name="maxValues"/>]
        /// </summary>
        /// <param name="crispOutput">The input data to be un-normalised</param>
        /// <param name="minValues">The minumum values this function will return, will be mapped from <c>-1</c></param>
        /// <param name="maxValues">The maximum values this function will return, will be mapped from <c>1</c></param>
        /// <returns>A <see cref="FuzzyLogic.CrispOutput"/> containing un-Normalised values</returns>
        public CrispOutput UnNormaliseOutput(CrispOutput crispOutput, float[] minValues, float[] maxValues)
        {
            float[] unNormalisedOutputs = new float[CrispOutput.NumberOfVariables];
            for (int i = 0; i < CrispOutput.NumberOfVariables; i++)
            {
                unNormalisedOutputs[i] = FuzzyUtility.UnNormaliseValue(minValues[i], maxValues[i], crispOutput[i]);
            }
            return new CrispOutput(unNormalisedOutputs);
        }

        /// <summary>
        /// Returns a new <see cref="FuzzyLogic.CrispOutput"/> that was generated from a call of <see cref="FuzzyLogic.FuzzySystem.FuzzyCompute(CrispInput)"/>
        /// with a paramater generated by a call of <see cref="FuzzyLogic.FuzzySystem.BuildInputNormalisedUneven(float[], float[], float[], float[])"/>.
        /// Maps each value that was normalised unevenly to <c>[-1,1]</c> to again be between the clamped range in <c>[</c><paramref name="minValues"/><c>,</c><paramref name="maxValues"/><c>]</c> where the values in <paramref name="neutralValues"/> represents a neutral state        
        /// </summary>
        /// <param name="crispOutput">The input data to be un-normalised</param>
        /// <param name="minValues">The minumum values this function will return, will be mapped from <c>-1</c></param>
        /// <param name="neutralValues">The neutral values this function will return, will be mapped from <c>0</c></param>
        /// <param name="maxValues">The maximum values this function will return, will be mapped from <c>1</c></param>
        /// <returns>A <see cref="FuzzyLogic.CrispOutput"/> containing un-Normalised values</returns>
        public CrispOutput UnNormaliseOutputUneven(CrispOutput crispOutput, float[] minValues, float[] neutralValues, float[] maxValues)
        {
            float[] unNormalisedOutputs = new float[CrispOutput.NumberOfVariables];
            for (int i = 0; i < CrispOutput.NumberOfVariables; i++)
            {
                unNormalisedOutputs[i] = FuzzyUtility.UnNormaliseValueUneven(minValues[i], neutralValues[i], maxValues[i], crispOutput[i]);
            }
            return new CrispOutput(unNormalisedOutputs);
        }

    }
}

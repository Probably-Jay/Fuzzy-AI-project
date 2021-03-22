using UnityEngine;

namespace FuzzyLogic
{

    public static class FuzzyUtility
    {
        public enum FuzzyStates
        {
            LN
            , MN
            , Z
            , MP
            , LP
        }

        /// <summary>
        /// Clamped normalises <paramref name="value"/> to a <c>[-1,1]</c> scale based on the scale between <c>[</c><paramref name="valueMin"/><c>,</c><paramref name="valueMax"/><c>]</c>
        /// </summary>
        /// <param name="valueMin">The minumum value this <paramref name="value"/> can be expeceted to have, will be mapped to <c>-1</c></param>
        /// <param name="valueMax">The maximum value this <paramref name="value"/> can be expeceted to have, will be mapped to <c>1</c></param>
        /// <param name="value">The value to be normalised</param>
        /// <returns>Normalised represenation of <paramref name="value"/> within the range <c>[-1,1]</c></returns>
        public static float NormaliseValue(float valueMin, float valueMax, float value)
        {
            float zeroOne = Mathf.Clamp01(Mathf.InverseLerp(valueMin, valueMax, value));
            zeroOne *= 2f;
            zeroOne -= 1;
            return zeroOne;
        }

        /// <summary>
        /// Clamped normalises <paramref name="value"/> to a <c>[-1,1]</c> scale based on the uneven scale between <c>[</c><paramref name="valueMin"/><c>,</c><paramref name="valueMax"/><c>]</c>, where <paramref name="valueNeutral"/> represents a neutral state 
        /// </summary>
        /// <param name="valueMin">The minumum value this <paramref name="value"/> can be expeceted to have, will be mapped to <c>-1</c></param>
        /// <param name="valueNeutral">The value representing a neutral state of <paramref name="value"/>, will be mapped to <c>0</c></param>
        /// <param name="valueMax">The maximum value this <paramref name="value"/> can be expeceted to have, will be mapped to <c>1</c></param>
        /// <param name="value">The value to be normalised</param>
        /// <returns>Normalised represenation of <paramref name="value"/> within the range <c>[-1,1]</c></returns>
        public static float NormaliseValueUneven(float valueMin, float valueNeutral, float valueMax, float value)
        {
            if (value < valueNeutral)
            {
                float negZero = Mathf.Clamp01(Mathf.InverseLerp(valueMin, valueNeutral, value));
                negZero -= 1;
                return negZero;
            }
            else if (value > valueNeutral)
            {
                float zeroOne = Mathf.Clamp01(Mathf.InverseLerp(valueNeutral, valueMax, value));
                return zeroOne;
            }
            else return 0; // mapp neutral to 0
           
        }

        /// <summary>
        /// Returns a value that was normalised to <c>[-1,1]</c> to again be between the clamped range <c>[</c><paramref name="valueMin"/><c>,</c><paramref name="valueMax"/><c>]</c>
        /// </summary>
        /// <param name="valueMin">The minumum value this function will return, will be mapped from <c>-1</c></param>
        /// <param name="valueMax">The maximum value this function will return, will be mapped from <c>1</c></param>
        /// <param name="nValue">The normalised value to be projected, valid over range <c>[-1,1]</c></param>
        /// <returns>Un-Normalised value</returns>
        public static float UnNormaliseValue(float valueMin, float valueMax, float nValue)
        {
            if (!ValidInstruction(nValue)) return nValue;

            float negPosOne = Mathf.Clamp(nValue,-1,1);
            negPosOne += 1;
            negPosOne /= 2;
            return Mathf.Lerp(valueMin, valueMax, negPosOne);
        }

        /// <summary>
        /// Returns a value that was normalised unevenly to <c>[-1,1]</c> to again be between the clamped range <c>[</c><paramref name="valueMin"/><c>,</c><paramref name="valueMax"/><c>]</c> where <paramref name="valueNeutral"/> represents a neutral state
        /// </summary>
        /// <param name="valueMin">The minumum value this function will return, will be mapped from <c>-1</c></param>
        /// <param name="valueNeutral">The neutral value this function will return, will be mapped from <c>0</c></param>
        /// <param name="valueMax">The maximum value this function will return, will be mapped from <c>1</c></param>
        /// <param name="nValue">The normalised value to be projected, valid over range <c>[-1,1]</c></param>
        /// <returns>Un-Normalised value</returns>
        public static float UnNormaliseValueUneven(float valueMin, float valueNeutral, float valueMax, float nValue)
        {
            if (!ValidInstruction(nValue)) return nValue;
            if (nValue < 0)
            {
                float negZero = Mathf.Clamp(nValue, -1, 0);
                negZero += 1;
                return Mathf.Lerp(valueMin, valueNeutral, negZero);
            }
            else if (nValue > 0)
            {
                float zeroOne = Mathf.Clamp(nValue, 0, 1);
                return Mathf.Lerp(valueNeutral, valueMax, zeroOne);
            }
            else return valueNeutral;

        }

        /// <summary>
        /// If the fuzzy logic does not know what to do with an input (it has no rules relating to the provided argument) it will produce an invalid instruction, use this function to test for that
        /// </summary>
        /// <param name="value">The value returned by <see cref="FuzzyLogic.FuzzySystem.FuzzyCompute(CrispInput)"/></param>
        /// <returns>If the <paramref name="value"/> is valid</returns>
        public static bool ValidInstruction(float value) => !float.IsNaN(value);


    }
}


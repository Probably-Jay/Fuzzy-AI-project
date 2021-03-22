using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FuzzyLogic
{
    /// <summary>
    /// Input to the fuzzy system
    /// </summary>
    public class CrispInput 
    {
        
        internal CrispInput(float[] newValues)
        {
            variables = (float[])newValues.Clone();
        }

        public const int NumberOfVariables = 3;
        public enum Inputs
        {
            Input1
            , Input2
            , Input3
            //, Input4
            //, Input5
            //, Input6
            //, Input7
            //, Input8
            //, Input9
            //, Input10
        }


        private float[] variables = new float[NumberOfVariables];

        public float this[Inputs InputVariable]
        {
            get { return variables[(int)InputVariable]; }
            set { variables[(int)InputVariable] = value; }
        }
    } 
    
    
    /// <summary>
    /// Outputs to the fuzzy system
    /// </summary>
    public class CrispOutput
    {
        internal CrispOutput()
        {
            variables =  new float[NumberOfVariables];
        } 
        
        internal CrispOutput(float[] newValues)
        {
            variables = (float[])newValues.Clone();
        }


        public const int NumberOfVariables = 1;
        public enum Outputs
        {
            Output1
            //, Output2
            //, Output3
            //, Output4
            //, Output5
            //, Output6
            //, Output7
            //, Output8
            //, Output9
            //, Output10
        }


        private float[] variables;

        public float this[Outputs InputVariable]
        {
            get { return variables[(int)InputVariable]; }
            set { variables[(int)InputVariable] = value; }
        } 
        internal float this[int InputVariable]
        {
            get { return variables[InputVariable]; }
        }

        /// <summary>
        /// If <see cref="FuzzyLogic.FuzzySystem.FuzzyCompute(CrispInput)"/> does not have any rules that correspond to it's input, it will return a value representing "Take no action". Use this function to test for that 
        /// </summary>
        /// <param name="variable">The variable in question</param>
        /// <returns>If that variable represents an action that should be taken</returns>
        public bool OutputValid(Outputs variable) => FuzzyUtility.ValidInstruction(this[variable]);

        /// <summary>
        /// For use in <c>foreach</c> statements, get a list of each <see cref="FuzzyLogic.CrispOutput.Outputs"/>
        /// </summary>
        public static Outputs[] OutputEnumvalues => (Outputs[])System.Enum.GetValues(typeof(CrispOutput.Outputs));
       
    }
}

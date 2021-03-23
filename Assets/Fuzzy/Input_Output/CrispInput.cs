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
        /// <summary>
        /// Constructor
        /// </summary>
        public CrispInput()
        {
            variables = new float[NumberOfVariables];
        }
        internal CrispInput(float[] newValues)
        {
            variables = (float[])newValues.Clone();
        }

        /// <summary>
        /// The number of input variables in the system
        /// </summary>
        public const int NumberOfVariables = 5;

        /// <summary>
        /// An enum representing the input variables of the system
        /// </summary>
        public enum Inputs
        {
            Speed
            , ForwardDistance
            , RightDistance
            , LeftDistance
            , ForwardSurfaceNormal
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

        /// <summary>
        /// For use in <c>foreach</c> statements, get a list of each <see cref="FuzzyLogic.CrispInput.Inputs"/>
        /// </summary>
        public static Inputs[] InputEnumvalues => (Inputs[])System.Enum.GetValues(typeof(CrispInput.Inputs));
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

        /// <summary>
        /// The number of output variables in the system
        /// </summary>
        public const int NumberOfVariables = 2;

        /// <summary>
        /// An enum representing the output variables of the system
        /// </summary>
        public enum Outputs
        {
            ForwardBackwards
            , LeftRight
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
        /// If <see cref="FuzzyLogic.FuzzySystem.EvaluateFuzzyLogic(CrispInput)"/> does not have any rules that correspond to it's input, it will return a value representing "Take no action". Use this function to test for that 
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

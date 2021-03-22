using System.Collections;
namespace FuzzyLogic
{

    /// <summary>
    /// Structure holding the data after being fuzzified
    /// </summary>
    [System.Serializable]
    internal class FuzzyInputData
    {

        public FuzzyInputData()
        {
            values = new FuzzyNumber[NumberOfVariables];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = new FuzzyNumber();
            }
        }

        public const int NumberOfVariables = CrispInput.NumberOfVariables;

        private FuzzyNumber[] values;

        public FuzzyNumber this[CrispInput.Inputs InputVariable]
        {
            get { return values[(int)InputVariable]; }
            set { values[(int)InputVariable] = value; }
        }

    }
    
    /// <summary>
    /// Structure holding the fuzzy data after the rules are applied
    /// </summary>
    [System.Serializable]
    internal class FuzzyOutputData
    {
        public FuzzyOutputData()
        {
            values = new FuzzyNumber[NumberOfVariables];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = new FuzzyNumber();
            }
        }

        public const int NumberOfVariables = CrispOutput.NumberOfVariables;

        private FuzzyNumber[] values = new FuzzyNumber[NumberOfVariables];

        public FuzzyNumber this[CrispOutput.Outputs InputVariable]
        {
            get { return values[(int)InputVariable]; }
            set { values[(int)InputVariable] = value; }
        }

    }
}


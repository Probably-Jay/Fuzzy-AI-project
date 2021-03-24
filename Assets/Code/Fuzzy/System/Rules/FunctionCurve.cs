using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FuzzyLogic
{
    /// <summary>
    /// An input curve used to fuzzify the data, made up of multiple <see cref="UnityEngine.AnimationCurve"/>
    /// </summary>
    [CreateAssetMenu(menuName = "Rules/Fuzzy function curve")]
    public class FunctionCurve : ScriptableObject
    {

        [SerializeField] AnimationCurve[] curve;


        internal AnimationCurve this[FuzzyUtility.FuzzyStates state]
        {
            get { return curve[(int)state]; }
            set { curve[(int)state] = value; }
        }
    

    }
}
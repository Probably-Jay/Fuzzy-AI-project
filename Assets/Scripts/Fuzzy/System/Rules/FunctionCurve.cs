using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FuzzyLogic
{
    [CreateAssetMenu(menuName = "Rules/Fuzzy function curve")]
    public class FunctionCurve : ScriptableObject
    {

        [SerializeField] AnimationCurve[] curve;



        internal AnimationCurve this[FuzzyUtility.FuzzyStates state]
        {
            get { return curve[(int)state]; }
            set { curve[(int)state] = value; }
        }
        public AnimationCurve this[int state]
        {
            get { return curve[(int)state]; }
            set { curve[(int)state] = value; }
        }

    }
}
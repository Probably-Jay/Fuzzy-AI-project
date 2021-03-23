using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FuzzyLogic
{
    /// <summary>
    /// Applies the <see cref="FuzzyLogic.FuzzyRulesList"/> to <see cref="FuzzyLogic.FuzzyInputData"/> and returns <see cref="FuzzyLogic.FuzzyOutputData"/>
    /// </summary>
    internal class InferenceEngine
    {
        /// <summary>
        /// The rules which will be used to drive the inference engine
        /// </summary>
        public FuzzyRulesList fuzzyRules;

        /// <summary>
        /// Applies the rulset in <see cref="FuzzyLogic.InferenceEngine.fuzzyRules"/> to <paramref name="fuzzyInput"/> and produces a new <see cref="FuzzyLogic.FuzzyOutputData"/> based on these rules
        /// </summary>
        /// <param name="fuzzyInput">The fussified input given by <see cref="FuzzyLogic.Fuzzifier.Fuzzify(CrispInput)"/> on which the rules will be applied</param>
        /// <returns>A new <see cref="FuzzyLogic.FuzzyOutputData"/> based on these rules</returns>
        public FuzzyOutputData ApplyRulset(FuzzyInputData fuzzyInput)
        {
            List<FuzzyOutputData> unaggragatedFuzzyOuputs = new List<FuzzyOutputData>();

            ApplySimpleRules(fuzzyInput, unaggragatedFuzzyOuputs);
            ApplyLogicalRules(fuzzyInput, unaggragatedFuzzyOuputs);

            FuzzyOutputData outputData = AgragateFuzzyOutputs(unaggragatedFuzzyOuputs);

            return outputData;

        }


        private void ApplySimpleRules(FuzzyInputData fuzzyInput, List<FuzzyOutputData> unaggragatedFuzzyOuputs)
        {
            foreach (SimpleFuzzyRule rule in fuzzyRules.simpleRules)
            {
                FuzzyOutputData fuzzyOutput = new FuzzyOutputData();


                float value = GetValueOfPredicate(fuzzyInput, rule.predicate);

                fuzzyOutput[rule.consequent.output][rule.consequent.state] = value;


                unaggragatedFuzzyOuputs.Add(fuzzyOutput);
            }
        }

        private void ApplyLogicalRules(FuzzyInputData fuzzyInput, List<FuzzyOutputData> unaggragatedFuzzyOuputs)
        {
            foreach (LogicalFuzzyRule rule in fuzzyRules.logicRules)
            {
                FuzzyOutputData fuzzyOutput = new FuzzyOutputData();

                float predicate1 = GetValueOfPredicate(fuzzyInput, rule.predicate1);
                float predicate2 = GetValueOfPredicate(fuzzyInput, rule.predicate2);

                float value = ApplyLogicalRelationshipToPredicateValues(predicate1, predicate2, rule.logicalRelationship);

                fuzzyOutput[rule.consequent.output][rule.consequent.state] = value;


                unaggragatedFuzzyOuputs.Add(fuzzyOutput);
            }
        }

        private static float GetValueOfPredicate(FuzzyInputData fuzzyInput, FuzzyPredicate predicate)
        {
            float value = 0;
            switch (predicate.isOrIsNot)
            {
                case FuzzyPredicate.IsOrIsNot.Is:
                    value = ValueIsRule(fuzzyInput, predicate);
                    break;
                case FuzzyPredicate.IsOrIsNot.IsNot:
                    value = ValueIsNotRule(fuzzyInput, predicate);
                    break;
            }

            return value;
        }

        private float ApplyLogicalRelationshipToPredicateValues(float predicate1, float predicate2, FuzzyPredicate.Logic logicalRelationship)
        {
            float value = 0;
            switch (logicalRelationship)
            {
                case FuzzyPredicate.Logic.And:
                    value = RelationshipIsAnd(predicate1, predicate2);
                    break;
                case FuzzyPredicate.Logic.Or:
                    value = RelationshipIsOr(predicate1, predicate2);
                    break;
            }
            return value;
        }


        private static float ValueIsRule(FuzzyInputData fuzzyInput, FuzzyPredicate predicate) => fuzzyInput[predicate.input][predicate.state];
        private static float ValueIsNotRule(FuzzyInputData fuzzyInput, FuzzyPredicate predicate) => (!fuzzyInput[predicate.input])[predicate.state];


        private static float RelationshipIsAnd(float predicate1, float predicate2) => Mathf.Min(predicate1, predicate2);
        private static float RelationshipIsOr(float predicate1, float predicate2) => Mathf.Max(predicate1, predicate2);



        /// <summary>
        /// Agragates a list of <see cref="FuzzyLogic.FuzzyOutputData"/> by taking the maximum value of each <see cref="FuzzyLogic.FuzzyNumber.FuzzyStates"/> for each <see cref="FuzzyLogic.CrispOutput.Outputs"/> 
        /// </summary>
        /// <returns>A single <see cref="FuzzyLogic.FuzzyOutputData"/></returns>
        private FuzzyOutputData AgragateFuzzyOutputs(List<FuzzyOutputData> unaggragatedFuzzyOuputs)
        {
            FuzzyOutputData outputData = new FuzzyOutputData();

           
            foreach (CrispOutput.Outputs outputVariable in CrispOutput.OutputEnumvalues)
            {
                foreach (FuzzyUtility.FuzzyStates membershipState in System.Enum.GetValues(typeof(FuzzyUtility.FuzzyStates)))
                {
                    outputData[outputVariable][membershipState] = GetMaxVariableMembershipState(outputVariable, membershipState, unaggragatedFuzzyOuputs);
                }
            }
            return outputData;
        }

        /// <summary>
        /// Regular find max algorythm on the values provided
        /// </summary>
        private float GetMaxVariableMembershipState(CrispOutput.Outputs outputVariable, FuzzyUtility.FuzzyStates membershipState, List<FuzzyOutputData> unaggragatedFuzzyOuputs)
        {
            float cur;
            float max = 0;
            foreach (FuzzyOutputData fuzzyOutput in unaggragatedFuzzyOuputs)
            {
                cur = fuzzyOutput[outputVariable][membershipState];
                if (cur <= max)
                {
                    continue;
                }
                else
                {
                    max = cur;
                }
            }
            return max;
        }


    }


}

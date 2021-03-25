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
            foreach (FuzzyRulesList.SimpleFuzzyRule rule in fuzzyRules.simpleRules)
            {
                FuzzyOutputData fuzzyOutput = new FuzzyOutputData();


                float value = GetValueOfAnticedent(fuzzyInput, rule.anticedent);

                fuzzyOutput[rule.consequent.output][rule.consequent.state] = value;


                unaggragatedFuzzyOuputs.Add(fuzzyOutput);
            }
        }

        private void ApplyLogicalRules(FuzzyInputData fuzzyInput, List<FuzzyOutputData> unaggragatedFuzzyOuputs)
        {
            foreach (FuzzyRulesList.LogicalFuzzyRule rule in fuzzyRules.logicRules)
            {
                FuzzyOutputData fuzzyOutput = new FuzzyOutputData();

                float anticedent1 = GetValueOfAnticedent(fuzzyInput, rule.anticedent1);
                float anticedent2 = GetValueOfAnticedent(fuzzyInput, rule.anticedent2);

                float value = ApplyLogicalRelationshipToAnticedent1Values(anticedent1, anticedent2, rule.logicalRelationship);

                fuzzyOutput[rule.consequent.output][rule.consequent.state] = value;


                unaggragatedFuzzyOuputs.Add(fuzzyOutput);
            }
        }

        private static float GetValueOfAnticedent(FuzzyInputData fuzzyInput, FuzzyRulesList.FuzzyAnticedent anticedent)
        {
            float value = 0;
            switch (anticedent.isOrIsNot)
            {
                case FuzzyRulesList.FuzzyAnticedent.IsOrIsNot.Is:
                    value = ValueIsRule(fuzzyInput, anticedent);
                    break;
                case FuzzyRulesList.FuzzyAnticedent.IsOrIsNot.IsNot:
                    value = ValueIsNotRule(fuzzyInput, anticedent);
                    break;
            }

            return value;
        }

        private float ApplyLogicalRelationshipToAnticedent1Values(float anticedent1, float anticedent2, FuzzyRulesList.FuzzyAnticedent.LogicalRelationship logicalRelationship)
        {
            float value = 0;
            switch (logicalRelationship)
            {
                case FuzzyRulesList.FuzzyAnticedent.LogicalRelationship.And:
                    value = RelationshipIsAnd(anticedent1, anticedent2);
                    break;
                case FuzzyRulesList.FuzzyAnticedent.LogicalRelationship.Or:
                    value = RelationshipIsOr(anticedent1, anticedent2);
                    break;
            }
            return value;
        }


        private static float ValueIsRule(FuzzyInputData fuzzyInput, FuzzyRulesList.FuzzyAnticedent anticedent) => fuzzyInput[anticedent.input][anticedent.state];
        private static float ValueIsNotRule(FuzzyInputData fuzzyInput, FuzzyRulesList.FuzzyAnticedent anticedent) => (!fuzzyInput[anticedent.input])[anticedent.state];


        private static float RelationshipIsAnd(float anticedent1, float anticedent2) => Mathf.Min(anticedent1, anticedent2);
        private static float RelationshipIsOr(float anticedent1, float anticedent2) => Mathf.Max(anticedent1, anticedent2);



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

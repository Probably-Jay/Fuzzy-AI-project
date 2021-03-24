using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR

using UnityEditor;
using UnityEditorInternal;

#endif

namespace FuzzyLogic
{
    /// <summary>
    /// Scriptable object that holds a rule base for use by <see cref="FuzzyLogic.InferenceEngine"/>
    /// </summary>
    [CreateAssetMenu(menuName = "Rules/Fuzzy Rules")]
    public class FuzzyRulesList : ScriptableObject
    {
        /// <summary>
        /// The list of <see cref="FuzzyLogic.FuzzyRulesList.SimpleFuzzyRule"/>
        /// </summary>
        public SimpleFuzzyRule[] simpleRules;
        /// <summary>
        /// The list of <see cref="FuzzyLogic.FuzzyRulesList.LogicalFuzzyRule"/>
        /// </summary>
        public LogicalFuzzyRule[] logicRules;

        /// <summary>
        /// A fuzzy rule in the form (<see cref="FuzzyLogic.FuzzyRulesList.FuzzyAnticedent"/> ⇒ <see cref="FuzzyLogic.FuzzyRulesList.FuzzyConsequent"/>)
        /// </summary>
        [System.Serializable]
        public class SimpleFuzzyRule
        {
            public FuzzyAnticedent anticedent = new FuzzyAnticedent();

            public FuzzyConsequent consequent = new FuzzyConsequent();
        }

        /// <summary>
        /// A fuzzy rule in the form (<see cref="FuzzyLogic.FuzzyRulesList.FuzzyAnticedent"/> [<see cref="FuzzyLogic.FuzzyRulesList.FuzzyAnticedent.LogicalRelationship"/>] <see cref="FuzzyLogic.FuzzyRulesList.FuzzyAnticedent"/>  ⇒ <see cref="FuzzyLogic.FuzzyRulesList.FuzzyConsequent"/>)
        /// </summary>
        [System.Serializable]
        public class LogicalFuzzyRule
        {
            public FuzzyAnticedent anticedent1 = new FuzzyAnticedent();

            public FuzzyAnticedent.LogicalRelationship logicalRelationship;

            public FuzzyAnticedent anticedent2 = new FuzzyAnticedent();


            public FuzzyConsequent consequent = new FuzzyConsequent();
        }

        /// <summary>
        /// An anticedent phrase, in the form <see cref="FuzzyLogic.CrispInput.Inputs"/> [<see cref="FuzzyLogic.FuzzyRulesList.FuzzyAnticedent.IsOrIsNot"/>] in the state of <see cref="FuzzyLogic.FuzzyUtility.FuzzyStates"/> 
        /// </summary>
        [System.Serializable]
        public class FuzzyAnticedent 
        {
            public enum IsOrIsNot
            {
                Is /// The <see cref="FuzzyLogic.FuzzyRulesList.FuzzyAnticedent.input"/> is in the <see cref="FuzzyLogic.FuzzyRulesList.FuzzyAnticedent.state"/>
                , IsNot /// The <see cref="FuzzyLogic.FuzzyRulesList.FuzzyAnticedent.input"/> is not in the <see cref="FuzzyLogic.FuzzyRulesList.FuzzyAnticedent.state"/>
            }

            public enum LogicalRelationship
            {
                And /// Will take the minimum of the two <see cref="FuzzyLogic.FuzzyRulesList.FuzzyAnticedent"/> 
                , Or /// Will take the maximum of the two <see cref="FuzzyLogic.FuzzyRulesList.FuzzyAnticedent"/> 
            }

            public CrispInput.Inputs input;
            public FuzzyAnticedent.IsOrIsNot isOrIsNot;
            public FuzzyUtility.FuzzyStates state;
        }

        /// <summary>
        /// The consequent phrase, in the form <see cref="FuzzyLogic.CrispOutput.Outputs"/> is in the state of <see cref="FuzzyLogic.FuzzyUtility.FuzzyStates"/> 
        /// </summary>
        [System.Serializable]
        public class FuzzyConsequent
        {
            public CrispOutput.Outputs output;
            public FuzzyUtility.FuzzyStates state;
        }
    }




#if UNITY_EDITOR

    /// <summary>
    /// A custom editor to set up the <see cref="FuzzyLogic.FuzzyRulesList"/> <see cref="UnityEngine.ScriptableObject"/>
    /// </summary>
    [CustomEditor(typeof(FuzzyRulesList))]
    public class FuzzyRulesEditor : Editor
    {


        SerializedProperty simpleRules;
        SerializedProperty logicRules;

        ReorderableList simpleList;
        ReorderableList logicList;

        private void OnEnable()
        {
            simpleRules = serializedObject.FindProperty(nameof(FuzzyRulesList.simpleRules));
            logicRules = serializedObject.FindProperty(nameof(FuzzyRulesList.logicRules));

            simpleList = new ReorderableList(serializedObject, simpleRules, true, true, true, true);
            logicList = new ReorderableList(serializedObject, logicRules, true, true, true, true);

            simpleList.drawElementCallback = DrawSimpleListItems; 
            simpleList.drawHeaderCallback = DrawSimpleHeader;

            logicList.drawElementCallback = DrawLogicListItems;
            logicList.drawHeaderCallback = DrawLogicHeader; 
        }


        public override void OnInspectorGUI()
        {


            serializedObject.Update(); 

            simpleList.DoLayoutList(); 
            logicList.DoLayoutList(); 

          
            serializedObject.ApplyModifiedProperties();

        }

        void DrawSimpleHeader(Rect rect)
        {
            string name = "Simple rules";
            EditorGUI.LabelField(rect, name);
        }
        void DrawLogicHeader(Rect rect)
        {
            string name = "Logic rules";
            EditorGUI.LabelField(rect, name);
        }


        void DrawSimpleListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = simpleList.serializedProperty.GetArrayElementAtIndex(index);

            SerializedProperty anticedent1 = element.FindPropertyRelative(nameof(FuzzyRulesList.SimpleFuzzyRule.anticedent));
            SerializedProperty consiquent = element.FindPropertyRelative(nameof(FuzzyRulesList.SimpleFuzzyRule.consequent));


            EditorGUI.LabelField(new Rect(rect.x, rect.y, 15, EditorGUIUtility.singleLineHeight), "If");

            float currentOffset = 15;

            currentOffset += DrawAnticedent1(rect, anticedent1, currentOffset);

            DrawConsiquent(rect, consiquent, currentOffset);

        }

        void DrawLogicListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = logicList.serializedProperty.GetArrayElementAtIndex(index);

            SerializedProperty anticedent1 = element.FindPropertyRelative(nameof(FuzzyRulesList.LogicalFuzzyRule.anticedent1));
            SerializedProperty anticedent2 = element.FindPropertyRelative(nameof(FuzzyRulesList.LogicalFuzzyRule.anticedent2));
            SerializedProperty consiquent = element.FindPropertyRelative(nameof(FuzzyRulesList.LogicalFuzzyRule.consequent));


            EditorGUI.LabelField(new Rect(rect.x, rect.y, 15, EditorGUIUtility.singleLineHeight), "If");


            float currentOffset = 15;

            currentOffset += DrawAnticedent1(rect, anticedent1, currentOffset);

            currentOffset += DrawRelationship(rect, element, currentOffset);

            currentOffset += DrawAnticedent1(rect, anticedent2, currentOffset);


            DrawConsiquent(rect, consiquent, currentOffset);


        }


        private static float DrawAnticedent1(Rect rect, SerializedProperty anticedent1, float currentOffset)
        {
            EditorGUI.PropertyField(
                 new Rect(rect.x + currentOffset, rect.y, 110, EditorGUIUtility.singleLineHeight),
                 anticedent1.FindPropertyRelative(nameof(FuzzyRulesList.FuzzyAnticedent.input)),
                 GUIContent.none
                );

            EditorGUI.PropertyField(
                 new Rect(rect.x + currentOffset + 110, rect.y, 60, EditorGUIUtility.singleLineHeight),
                 anticedent1.FindPropertyRelative(nameof(FuzzyRulesList.FuzzyAnticedent.isOrIsNot)),
                 GUIContent.none
                );

            EditorGUI.PropertyField(
                 new Rect(rect.x + currentOffset + 170, rect.y, 40, EditorGUIUtility.singleLineHeight),
                 anticedent1.FindPropertyRelative(nameof(FuzzyRulesList.FuzzyAnticedent.state)),
                 GUIContent.none
                );
            return 215;
        }

        private static float DrawRelationship(Rect rect, SerializedProperty element, float currentOffset)
        {
            EditorGUI.PropertyField(
              new Rect(rect.x + currentOffset, rect.y, 50, EditorGUIUtility.singleLineHeight),
              element.FindPropertyRelative(nameof(FuzzyRulesList.LogicalFuzzyRule.logicalRelationship)),
              GUIContent.none
             );
            return 55;
        }

        private static void DrawConsiquent(Rect rect, SerializedProperty consiquent, float currentOffset)
        {
            EditorGUI.LabelField(new Rect(rect.x + currentOffset, rect.y, 30, EditorGUIUtility.singleLineHeight), "then");

            EditorGUI.PropertyField(
                 new Rect(rect.x + currentOffset + 35, rect.y, 100, EditorGUIUtility.singleLineHeight),
                 consiquent.FindPropertyRelative(nameof(FuzzyRulesList.FuzzyConsequent.output)),
                 GUIContent.none
                );

            EditorGUI.LabelField(new Rect(rect.x + currentOffset + 140, rect.y, 15, EditorGUIUtility.singleLineHeight), "is");

            EditorGUI.PropertyField(
                 new Rect(rect.x + currentOffset + 155, rect.y, 40, EditorGUIUtility.singleLineHeight),
                 consiquent.FindPropertyRelative(nameof(FuzzyRulesList.FuzzyConsequent.state)),
                 GUIContent.none
                );
        }

      





    }





#endif
}
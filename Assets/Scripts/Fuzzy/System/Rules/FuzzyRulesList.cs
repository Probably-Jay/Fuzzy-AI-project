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
    [CreateAssetMenu(menuName = "Rules/Fuzzy Rules")]
    public class FuzzyRulesList : ScriptableObject
    {

        public SimpleFuzzyRule[] simpleRules;
        public LogicalFuzzyRule[] logicRules;
    }

    [System.Serializable]
    public class SimpleFuzzyRule
    {
        public FuzzyPredicate predicate = new FuzzyPredicate();

        public FuzzyConsequent consequent = new FuzzyConsequent();
    }

    [System.Serializable]
    public class LogicalFuzzyRule
    {
        public FuzzyPredicate predicate1 = new FuzzyPredicate();

        public FuzzyPredicate.Logic logicalRelationship;

        public FuzzyPredicate predicate2 = new FuzzyPredicate();


        public FuzzyConsequent consequent = new FuzzyConsequent();
    }

    [System.Serializable]
    public class FuzzyPredicate 
    {
        public enum IsOrIsNot
        {
            Is
            , IsNot
        }

        public enum Logic
        {
            And
            , Or
        }

        public CrispInput.Inputs input;
        public FuzzyPredicate.IsOrIsNot isOrIsNot;
        public FuzzyUtility.FuzzyStates state;
    }
    [System.Serializable]

    public class FuzzyConsequent
    {
        public CrispOutput.Outputs output;
        public FuzzyUtility.FuzzyStates state;
    }





#if UNITY_EDITOR

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

            SerializedProperty predicate = element.FindPropertyRelative(nameof(SimpleFuzzyRule.predicate));
            SerializedProperty consiquent = element.FindPropertyRelative(nameof(SimpleFuzzyRule.consequent));


            EditorGUI.LabelField(new Rect(rect.x, rect.y, 15, EditorGUIUtility.singleLineHeight), "If");

            float currentOffset = 15;

            currentOffset += DrawPredicate(rect, predicate, currentOffset);

            DrawConsiquent(rect, consiquent, currentOffset);

        }

        void DrawLogicListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = logicList.serializedProperty.GetArrayElementAtIndex(index);

            SerializedProperty predicate1 = element.FindPropertyRelative(nameof(LogicalFuzzyRule.predicate1));
            SerializedProperty predicate2 = element.FindPropertyRelative(nameof(LogicalFuzzyRule.predicate2));
            SerializedProperty consiquent = element.FindPropertyRelative(nameof(LogicalFuzzyRule.consequent));


            EditorGUI.LabelField(new Rect(rect.x, rect.y, 15, EditorGUIUtility.singleLineHeight), "If");


            float currentOffset = 15;

            currentOffset += DrawPredicate(rect, predicate1, currentOffset);

            currentOffset += DrawRelationship(rect, element, currentOffset);

            currentOffset += DrawPredicate(rect, predicate2, currentOffset);


            DrawConsiquent(rect, consiquent, currentOffset);


        }


        private static float DrawPredicate(Rect rect, SerializedProperty predicate1, float currentOffset)
        {
            EditorGUI.PropertyField(
                 new Rect(rect.x + currentOffset, rect.y, 110, EditorGUIUtility.singleLineHeight),
                 predicate1.FindPropertyRelative(nameof(FuzzyPredicate.input)),
                 GUIContent.none
                );

            EditorGUI.PropertyField(
                 new Rect(rect.x + currentOffset + 110, rect.y, 60, EditorGUIUtility.singleLineHeight),
                 predicate1.FindPropertyRelative(nameof(FuzzyPredicate.isOrIsNot)),
                 GUIContent.none
                );

            EditorGUI.PropertyField(
                 new Rect(rect.x + currentOffset + 170, rect.y, 40, EditorGUIUtility.singleLineHeight),
                 predicate1.FindPropertyRelative(nameof(FuzzyPredicate.state)),
                 GUIContent.none
                );
            return 215;
        }

        private static float DrawRelationship(Rect rect, SerializedProperty element, float currentOffset)
        {
            EditorGUI.PropertyField(
              new Rect(rect.x + currentOffset, rect.y, 50, EditorGUIUtility.singleLineHeight),
              element.FindPropertyRelative(nameof(LogicalFuzzyRule.logicalRelationship)),
              GUIContent.none
             );
            return 55;
        }

        private static void DrawConsiquent(Rect rect, SerializedProperty consiquent, float currentOffset)
        {
            EditorGUI.LabelField(new Rect(rect.x + currentOffset, rect.y, 30, EditorGUIUtility.singleLineHeight), "then");

            EditorGUI.PropertyField(
                 new Rect(rect.x + currentOffset + 35, rect.y, 100, EditorGUIUtility.singleLineHeight),
                 consiquent.FindPropertyRelative(nameof(FuzzyConsequent.output)),
                 GUIContent.none
                );

            EditorGUI.LabelField(new Rect(rect.x + currentOffset + 140, rect.y, 15, EditorGUIUtility.singleLineHeight), "is");

            EditorGUI.PropertyField(
                 new Rect(rect.x + currentOffset + 155, rect.y, 40, EditorGUIUtility.singleLineHeight),
                 consiquent.FindPropertyRelative(nameof(FuzzyConsequent.state)),
                 GUIContent.none
                );
        }

      





    }





#endif
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Uitls
{
    [CustomEditor(typeof(CustomButton), true)]
    [CanEditMultipleObjects]
    public class CustomButtonEditor : ButtonEditor
    {
        SerializedProperty _animatableComponentProperty;
        SerializedProperty _dontInteractOnAnimationProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _animatableComponentProperty = serializedObject.FindProperty("_animatableComponent");
            _dontInteractOnAnimationProperty = serializedObject.FindProperty("_dontInteractOnAnimation");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(_animatableComponentProperty);
            EditorGUILayout.PropertyField(_dontInteractOnAnimationProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}

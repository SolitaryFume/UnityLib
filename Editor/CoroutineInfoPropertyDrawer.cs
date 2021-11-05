using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityLib;

namespace UnityLibEditor
{


    [UnityEditor.CustomPropertyDrawer(typeof(CoroutineInfo))]
    public class CoroutineInfoPropertyDrawer : UnityEditor.PropertyDrawer
    {
        private bool foldout;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            var time = property.FindPropertyRelative("CrateTime").stringValue;

            foldout = EditorGUI.Foldout(new Rect(rect.x,rect.y,rect.width,22), foldout, time);
            if (foldout)
            {
                var trace = property.FindPropertyRelative("Trace");
                for (int i = 0; i < trace.arraySize; i++)
                {
                    var log = trace.GetArrayElementAtIndex(i);
                    var r = new Rect(rect.x, rect.y + (i +1 )* 22, rect.width, 22);
                    EditorGUI.LabelField(r, trace.GetArrayElementAtIndex(i).stringValue);
                }
            }


            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if(foldout) 
            {
                var size = property.FindPropertyRelative("Trace").arraySize;
                return (size+1) * 22;
            }
            else
                return 22;
        }
    }
}
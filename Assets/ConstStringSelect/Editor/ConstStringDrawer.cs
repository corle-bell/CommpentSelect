//*************************************************
//----Author:       Cyy 
//
//----CreateDate:   2023-10-13 14:22:43
//
//----Desc:         Create By BM
//
//**************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Bm.Drawer
{
    [CustomPropertyDrawer(typeof(ConstStringSelectAttribute))]

    public class ConstStringDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConstStringSelectAttribute attr = (ConstStringSelectAttribute)attribute;

            Rect src = new Rect(position);
            src.width = position.width * 0.85f-18;
            EditorGUI.PropertyField(src, property);

            src.width = position.width*0.15f+18;
            src.x = position.width*0.85f;
            
            if (GUI.Button(src, "Select"))
            {
                ConstStringSelectWindow.Open(property, attr);
            }
        }
    }
}



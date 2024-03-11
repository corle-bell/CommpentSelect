//*************************************************
//----Author:       Cyy 
//
//----CreateDate:   2024-02-19 18:00:05
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
    [CustomPropertyDrawer(typeof(ClassVariableSelect))]

    public class ClassVariableSelectDrawer : PropertyDrawer
    {
        protected string[] SelectOption;
        protected int selectIndex = -1;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ClassVariableSelect attr = (ClassVariableSelect)attribute;
            var text = ObjectNames.NicifyVariableName(label.text);
            var typeDesc = $"[{(attr.Filter==null?"":attr.Filter.Name)}, {attr.NameFilter}]";
            Init(attr, property.stringValue);
            
            int i = selectIndex;
            selectIndex = EditorGUI.Popup(position, $"{text}--{typeDesc}", selectIndex, SelectOption);
            if (i != selectIndex && selectIndex >= 0)
            {
                property.stringValue = SelectOption[selectIndex];
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        private void Init(ClassVariableSelect attr, string current)
        {
            if (SelectOption==null || SelectOption.Length==0)
            {
                SelectOption = DrawerEditorHelper.GetClassVariableCache(attr);
                for (int i = 0; i < SelectOption.Length; i++)
                {
                    if (current.Equals(SelectOption[i]))
                    {
                        selectIndex = i;
                        break;
                    }
                }
            }
        }
    }
}

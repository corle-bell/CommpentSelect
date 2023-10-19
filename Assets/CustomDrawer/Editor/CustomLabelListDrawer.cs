//*************************************************
//----Author:       Cyy 
//
//----CreateDate:   2023-10-14 17:31:50
//
//----Desc:         Create By BM
//
//**************************************************
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Bm.Drawer
{
    [CustomPropertyDrawer(typeof(CustomLabelListAttribute))]
    public class CustomLabelListDrawer : PropertyDrawer
    {
        public int id = -1;
        public string [] name_list;
        public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label)
        {
            CustomLabelListAttribute clla = attribute as CustomLabelListAttribute;

            Init(property, clla);

            #if ODIN_INSPECTOR
            if (id<0)
            #endif
            {
                string tmp = DrawerUtils.GetValueInSquareBracket(property.propertyPath);
                id = int.Parse(tmp);
            }

            if (id>=0 && id<name_list.Length)
            {
                label.text = name_list[id];
            }
            EditorGUI.PropertyField(position, property, label);  
        }

        private void Init(SerializedProperty property, CustomLabelListAttribute clla)
        {
            if(name_list!=null)return;
            
            if (clla.EnumType!=null)
            {
                name_list = Enum.GetNames(clla.EnumType);
            }
            
            if (clla.FieldName!=null)
            {
                var component = property.serializedObject.targetObject;
                FieldInfo fieldInfo = component.GetType().GetField(clla.FieldName,
                    BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

                if (fieldInfo!=null)
                {
                    var obj = fieldInfo.GetValue(null);
                    var type = fieldInfo.ToString().ToLower();
                    if (obj is Array && type.Contains("string"))
                    {
                        string[] list = obj as string[];
                        name_list = list;
                    }
                    else if (DrawerUtils.IsList(fieldInfo.FieldType))
                    {
                        List<string> list = obj as List<string>;
                        if (id>=0 && id<list.Count)
                        {
                            name_list = list.ToArray();
                        }
                    }
                }
                
            }
        }

    }
}

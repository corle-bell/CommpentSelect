using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Bm.Drawer
{
    [CustomPropertyDrawer(typeof(ComponentSelectAttribute))]

    public class ComponentSelectDrawer : PropertyDrawer
    {
        public GameObject root;

        protected Component[] componentlist;
        protected string[] componentDesc;
        protected int selectIndex = -1;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            root = (property.serializedObject.targetObject as Component).gameObject;
            ComponentSelectAttribute attr = (ComponentSelectAttribute)attribute;

            switch (attr.style)
            {
                case Style.DropDown:
                    DrawDropDown(position, property);
                    break;
                case Style.PopUp:
                    DrawPopUp(position, property);
                    break;
            }


        }

        private void DrawPopUp(Rect position, SerializedProperty property)
        {
            Rect src = new Rect(position);
            src.width = position.width * 0.85f-18;
            EditorGUI.PropertyField(src, property);

            src.width = position.width*0.15f+18;
            src.x = position.width*0.85f;
            
            if (GUI.Button(src, "Select"))
            {
                ComponentSelectWindow.Open(property, attribute as ComponentSelectAttribute);
            }
        }

        private void DrawDropDown(Rect position, SerializedProperty property)
        {
            UpdateParams(property);
            int i = selectIndex;
            selectIndex = EditorGUI.Popup(position, property.name, selectIndex, componentDesc);
            if (i != selectIndex && selectIndex >= 0)
            {
                property.objectReferenceValue = componentlist[selectIndex];
            }
        }

        protected void UpdateParams(SerializedProperty property)
        {
            if (componentlist == null || componentlist.Length == 0)
            {
                ComponentSelectAttribute attr = (ComponentSelectAttribute)attribute;
                if (attr.type == null)
                {
                    if (attr.includeChildren)
                    {
                        componentlist = root.GetComponentsInChildren<Component>();
                    }
                    else
                    {
                        componentlist = root.GetComponents<Component>();
                    }
                }
                else
                {
                    System.Type t = attr.type;
                    if (attr.includeChildren)
                    {
                        componentlist = root.GetComponentsInChildren(t);
                    }
                    else
                    {
                        componentlist = root.GetComponents(t);
                    }
                }

                Dictionary<string, int> TypeCounter = new Dictionary<string, int>();
                Dictionary<int, int> IdCounter = new Dictionary<int, int>();
                for (int i = 0; i < componentlist.Length; i++)
                {
                    string key = componentlist[i].transform.HierarchyName()+"#"+componentlist[i].GetType();
                    int count = 0;
                    if (TypeCounter.TryGetValue(key, out count))
                    {
                        count++;
                        TypeCounter[key] = count;
                    }
                    else
                    {
                        TypeCounter.Add(key, count);
                    }
                    
                    IdCounter.Add(componentlist[i].GetInstanceID(), count);
                }
                
                componentDesc = new string[componentlist.Length];
                for (int i = 0; i < componentDesc.Length; i++)
                {
                    string key = componentlist[i].transform.HierarchyName()+"#"+componentlist[i].GetType();

                    if (TypeCounter[key]==0)
                    {
                        componentDesc[i] = DrawerUtils.FormatDesc(componentlist[i], root.name, attr.includeChildren);
                    }
                    else
                    {
                        componentDesc[i] = $"{DrawerUtils.FormatDesc(componentlist[i], root.name, attr.includeChildren)} [{IdCounter[componentlist[i].GetInstanceID()]}]";
                    }

                    if (componentlist[i] == property.objectReferenceValue)
                    {
                        selectIndex = i;
                    }
                }
                
                TypeCounter.Clear();
                IdCounter.Clear();
            }
        }



    }
}



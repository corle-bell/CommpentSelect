using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace ComponentSelect
{
    public class ComponentSelectWindow : EditorWindow
    {

        public static ComponentSelectWindow Open(SerializedProperty _serializedProperty, ComponentSelectAttribute _atr)
        {
            ComponentSelectWindow myWindow = EditorWindow.GetWindow(typeof(ComponentSelectWindow), false, "ComponentSelectWindow", true) as ComponentSelectWindow;
            myWindow.Init(_serializedProperty, _atr);
            myWindow.Show();
            return myWindow;
        }


        public SerializedProperty serializedProperty;
        protected Component[] componentlist;
        protected string[] componentDesc;
        protected int selectIndex = -1;
        public Component root;
        public string fieldName;
        public void Init(SerializedProperty _serializedProperty, ComponentSelectAttribute _atr)
        {
            serializedProperty = _serializedProperty;
            root = (_serializedProperty.serializedObject.targetObject as Component);
            fieldName = _serializedProperty.propertyPath;
            Debug.Log(fieldName);
            UpdateParams(_serializedProperty, _atr);
        }

        private void OnGUI()
        {
            for(int i=0; i<componentlist.Length; i++)
            {
                GUILayout.BeginHorizontal();
                if(i==selectIndex)
                {
                    GUILayout.Label(componentDesc[i]);
                }
                else
                {
                    if (GUILayout.Button(componentDesc[i]))
                    {
                        selectIndex = i;
                        SetComponent(componentlist[selectIndex]);
                        this.Close();
                    }
                }
                GUILayout.EndHorizontal();
            }
        }

        protected void SetComponent(Component _Component)
        {
            root.GetType().GetField(fieldName).SetValue(root, _Component);
            EditorUtility.SetDirty(root);
        }

        protected void UpdateParams(SerializedProperty property, ComponentSelectAttribute _atr)
        {
            if (componentlist == null || componentlist.Length == 0)
            {
                ComponentSelectAttribute attr = _atr;
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

                componentDesc = new string[componentlist.Length];
                for (int i = 0; i < componentDesc.Length; i++)
                {
                    componentDesc[i] = ComponentSelectUtils.FormatDesc(componentlist[i], root.name, attr.includeChildren);

                    if (componentlist[i] == property.objectReferenceValue)
                    {
                        selectIndex = i;
                    }
                }
            }
        }

    }

}


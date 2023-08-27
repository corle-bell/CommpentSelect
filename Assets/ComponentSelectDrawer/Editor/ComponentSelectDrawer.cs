using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ComponentSelect
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
                case Style.PopUp:
                    DrawPopUp(position, property);
                    break;
                case Style.Button:
                    DrawButton(position, property);
                    break;
            }


        }

        private void DrawButton(Rect position, SerializedProperty property)
        {
            Rect src = new Rect(position);
            src.width = position.width * 0.7f;
            EditorGUI.PropertyField(src, property);

            src.x = position.width -80;
            src.width = 80;
            if (GUI.Button(src, "Select"))
            {
                ComponentSelectWindow.Open(property, attribute as ComponentSelectAttribute);
            }
        }

        private void DrawPopUp(Rect position, SerializedProperty property)
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



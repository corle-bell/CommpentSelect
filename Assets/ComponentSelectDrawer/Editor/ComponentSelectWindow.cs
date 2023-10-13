using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace Bm.Drawer
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
        protected Texture2D[] iconList;
        protected string[] componentDesc;
        protected int selectIndex = -1;
        public Component root;
        public string fieldName;
        private Material m_material;
        public void Init(SerializedProperty _serializedProperty, ComponentSelectAttribute _atr)
        {
            serializedProperty = _serializedProperty;
            root = (_serializedProperty.serializedObject.targetObject as Component);
            fieldName = _serializedProperty.propertyPath;
            UpdateParams(_serializedProperty, _atr);
            
            m_material = new Material(Shader.Find("Hidden/Internal-Colored"));
            m_material.hideFlags = HideFlags.HideAndDontSave;
        }
    

        private void OnGUI()
        {
            float label_width = position.width*0.7f;
            float height = 30f;
            float spacex = 5f;
            float spacey = 5f;
            float childPadingLeft = position.width*0.2f;
            Rect rect = new Rect(0, 0, this.position.width, height);
            string titleString = "";
            float top = 0;
            for(int i=0; i<componentlist.Length; i++)
            {
                //绘制标题
                Rect icoRect = new Rect(rect);
                if (!titleString.Equals(componentDesc[i]))
                {
                    titleString = componentDesc[i];
                    
                    rect.y += height+spacey;
                    
                    
                    icoRect.width = rect.width;
                    icoRect.x = 0;
                    icoRect.height = height;
                    GUI.Label(icoRect, titleString, GUI.skin.customStyles[53]);

                    top = rect.y-height;
                    
                    icoRect.y += height + spacey;
                }

                //绘制折线
                Rect lineRect = new Rect(childPadingLeft*0.3f, height*0.5f, childPadingLeft*0.7f, position.height);
                GUI.BeginGroup(lineRect);
                
                GL.PushMatrix();
                m_material.SetPass(0);
                GL.Begin(GL.LINES);
                GL.Color(new Color(1f, 1f, 1f));
                
                DrawLine(new Vector2(0, top), new Vector2(0, icoRect.y));
                DrawLine(new Vector2(0, icoRect.y), new Vector2(childPadingLeft*0.7f, icoRect.y));
                
                GL.End();
                GL.PopMatrix();
                
                
                
                GUI.EndGroup();
                
              
                
                
                //绘制图片
                icoRect.x += childPadingLeft;
                
                icoRect.x += spacex;
                icoRect.width = icoRect.height = rect.height;
                GUI.DrawTexture(icoRect, iconList[i]);
                
                //绘制按钮
                icoRect.x += icoRect.height + spacex;
                icoRect.width = label_width;
                
                if(i==selectIndex)
                {
                    GUI.Label(icoRect, componentlist[i].GetType().ToString(), GUI.skin.customStyles[42]);
                }
                else
                {
                    if (GUI.Button(icoRect, componentlist[i].GetType().ToString()))
                    {
                        selectIndex = i;
                        SetComponent(componentlist[selectIndex]);
                        this.Close();
                    }
                }
                
                rect.y += height+spacey;
            }
        }

        private void DrawLine(Vector2 p0, Vector2 p1)
        {
            GL.Vertex(p0);
            GL.Vertex(p1);
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

                Texture2D defaultTex = EditorGUIUtility.FindTexture("d_cs script Icon");
                componentDesc = new string[componentlist.Length];
                iconList = new Texture2D[componentDesc.Length];
                for (int i = 0; i < componentDesc.Length; i++)
                {
                    componentDesc[i] = ComponentSelectUtils.FormatDesc(componentlist[i], root.name, attr.includeChildren, false);

                    var ic = FindTexture(componentlist[i].GetType());
                    iconList[i] = ic==null?defaultTex:ic as Texture2D;
                    if (componentlist[i] == property.objectReferenceValue)
                    {
                        selectIndex = i;
                    }
                }
            }
        }

        private Texture2D FindTexture(Type type)
        {
            var ass = Assembly.GetAssembly(typeof(Editor));
            var ttt = ass.GetType("UnityEditor.EditorGUIUtility");
            MethodInfo methodInfo = ttt.GetMethod("FindTextureByType", BindingFlags.Static | BindingFlags.NonPublic);
            if (methodInfo == null) return null;
            return methodInfo.Invoke(null, new object[] {type}) as Texture2D;
        }
    }

}


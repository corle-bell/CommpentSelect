//*************************************************
//----Author:       Cyy 
//
//----CreateDate:   2023-10-13 14:25:51
//
//----Desc:         Create By BM
//
//**************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;

namespace Bm.Drawer
{
    public class ConstStringSelectWindow : EditorWindow
    {

        public static ConstStringSelectWindow Open(SerializedProperty _serializedProperty, ConstStringSelectAttribute _atr)
        {
            ConstStringSelectWindow myWindow = EditorWindow.GetWindow(typeof(ConstStringSelectWindow), false, "ConstStringSelectWindow", true) as ConstStringSelectWindow;
            myWindow.Init(_serializedProperty, _atr);
            myWindow.Show();
            return myWindow;
        }

        struct FiledData
        {
            public string title;
            public string content;
        }

        public SerializedProperty serializedProperty;
        private FiledData [][] classList;
        private string[] classDesc;
        protected int classIndex = -1;
        protected int stringIndex = -1;
        protected int selectClass = -1;
        public Component root;
        public string fieldName;
        public object propertyRoot;
        private int ArrayIndex = -1;
        private Vector2 scroll;
        private bool autoClose;
        public void Init(SerializedProperty _serializedProperty, ConstStringSelectAttribute _atr)
        {
            serializedProperty = _serializedProperty;
            propertyRoot = root = (_serializedProperty.serializedObject.targetObject as Component);

            autoClose = _atr.isAutoClose;
            
            GetPropertyName(_serializedProperty.propertyPath);
            InitList(_serializedProperty.stringValue);
        }

        private void GetPropertyName(string _path)
        {
            _path = _path.Replace("Array.data", "");
            string[] name_parts = _path.Split(new[] { '.' });

            if (name_parts.Length == 1)
            {
                fieldName = name_parts[0];
                return;
            }

            if (name_parts.Length > 1)
            {
                if (_path.Contains("["))
                {
                    object obj = propertyRoot;
                    for (int i = 0; i < name_parts.Length; i++)
                    {
                        if (name_parts[i].StartsWith("["))
                        {
                            var key = DrawerUtils.GetValueInSquareBracket(name_parts[i]);
                            int id = int.Parse(key);
                            
                            if (i == name_parts.Length - 1) //string数组
                            {
                                ArrayIndex = id;
                                propertyRoot = obj;
                                return;
                            }
                            else if (i==name_parts.Length-2) //property在数组里
                            {
                                obj = (obj as Array).GetValue(id);
                               
                                fieldName = name_parts[name_parts.Length - 1];
                                propertyRoot = obj;
                                return;
                            }
                            
                            obj = (obj as Array).GetValue(id);
                        }
                        else
                        {
                            var t = obj.GetType();
                            var f = t.GetField(name_parts[i]);
                            obj = f.GetValue(obj);
                        }
                    }
                }
                else
                {
                    object obj = propertyRoot;
                    for (int i = 0; i < name_parts.Length-1; i++)
                    {
                        obj = obj.GetType().GetField(name_parts[i]).GetValue(obj);
                    }

                    propertyRoot = obj;
                    fieldName = name_parts[name_parts.Length - 1];
                }
            }
        }

        private void OnGUI()
        {
            float half_w = position.width*0.5f;
            
            selectClass = EditorGUILayout.Popup("Class", selectClass, classDesc);
            
            EditorGUILayout.Space(5);
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Filed", GUI.skin.customStyles[58], GUILayout.Width(half_w), GUILayout.Height(30f));
            GUILayout.Label("Content", GUI.skin.customStyles[58], GUILayout.Width(half_w), GUILayout.Height(30f));
            EditorGUILayout.EndHorizontal();
           
            if(selectClass<0 || selectClass>=classDesc.Length || classList==null)return;
            
            
            scroll = EditorGUILayout.BeginScrollView(scroll);
            for (int i = 0; i < classList[selectClass].Length; i++)
            {
                var data = classList[selectClass][i];
                EditorGUILayout.BeginHorizontal();
                

                if (i==stringIndex && selectClass==classIndex)
                {
                    Color tc = GUI.color;
                    GUI.color = Color.green;
                    GUILayout.Label(data.title, GUI.skin.customStyles[1], GUILayout.Width(half_w), GUILayout.Height(30f));
                    GUILayout.Label(data.content, GUI.skin.customStyles[1], GUILayout.Width(half_w), GUILayout.Height(30f));
                    GUI.color = tc;
                }
                else
                {
                    
                    if (GUILayout.Button(data.title, GUI.skin.customStyles[1], GUILayout.Width(half_w), GUILayout.Height(30f))
                        || 
                        GUILayout.Button(data.content, GUI.skin.customStyles[1], GUILayout.Width(half_w), GUILayout.Height(30f)))
                    {
                        stringIndex = i;
                        classIndex = selectClass;
                        SetString(data.content);

                        if (autoClose)
                        {
                            this.Close();
                        }
                    }
                }
               
                
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }

      
        protected void SetString(string _text)
        {
            if (ArrayIndex>=0)
            {
                (propertyRoot as string[])[ArrayIndex] = _text;
            }
            else
            {
                propertyRoot.GetType().GetField(fieldName).SetValue(propertyRoot, _text);
            }
            EditorUtility.SetDirty(root);
        }

        protected void InitList(string _default)
        {
            //这里的代码 引用自 https://www.cnblogs.com/xxj-jing/archive/2011/09/29/2890100.html
            //加载程序集信息
            Assembly ass = Assembly.Load("Assembly-CSharp");
            Type[] types = ass.GetExportedTypes(); //还是用这个比较好，得到的都是自定义的类型

            // 验证指定自定义属性（使用的是 4.0 的新语法，匿名方法实现的，不知道的同学查查资料吧！）
            Func<System.Attribute[], bool> IsAtt1 = o =>
            {
                foreach (System.Attribute a in o)
                {
                    if (a is ConstStringContentAttribute)
                        return true;
                }
                return false;
            };

            //查找具有 Attribute.Atts.Att1 特性的类型（使用的是 linq 语法）
            Type[] CosType = types.Where(o =>
            {
                return IsAtt1(System.Attribute.GetCustomAttributes(o, true));
            }).ToArray();

            classList = new FiledData[CosType.Length][];
            classDesc = new string[classList.Length];
            for (int i=0; i<CosType.Length; i++)
            {
                bool isSelect = false;
                Type t = CosType[i];
                classDesc[i] = t.Name;
                classList[i] = InitCSC(t, out isSelect, _default);
                if (isSelect)
                {
                    selectClass = classIndex = i;
                }
            }

            if (selectClass < 0 && classDesc.Length > 0)
            {
                selectClass = 0;
            }
        }

        private FiledData[] InitCSC(Type t, out bool _isSelect, string _default)
        {
            _isSelect = false;
            List<FiledData> ret = new List<FiledData>();
            
            FieldInfo[] fields = t.GetFields();
 
            Type typeString = typeof(string);

            object oValue = null;
 
            foreach (FieldInfo fi in fields)
            {
                // 不是字符串类型 //
                if (fi.FieldType != typeString)
                    continue;
 
                // 不是常量 //
                if (!fi.IsStatic)
                    continue;
 
                if (!fi.IsLiteral)
                    continue;
 
 
                string s = fi.GetValue(null) as string;

                if (_default.Equals(s))
                {
                    stringIndex = ret.Count;
                    _isSelect = true;
                }

                FiledData fd = new FiledData();
                fd.content = s;
                fd.title = fi.Name;
                ret.Add(fd);
            }
            return ret.ToArray();
        }
    }

}
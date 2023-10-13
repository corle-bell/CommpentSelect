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
        private Material m_material;


        private Vector2 scroll; 
        public void Init(SerializedProperty _serializedProperty, ConstStringSelectAttribute _atr)
        {
            serializedProperty = _serializedProperty;
            root = (_serializedProperty.serializedObject.targetObject as Component);
            fieldName = _serializedProperty.propertyPath;

            InitList(_serializedProperty.stringValue);
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
                    }
                }
               
                
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }

      
        protected void SetString(string _text)
        {
            root.GetType().GetField(fieldName).SetValue(root, _text);
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
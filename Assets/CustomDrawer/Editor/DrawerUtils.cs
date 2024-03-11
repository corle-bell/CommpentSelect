using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using BmFramework.Core;
using UnityEditor;

namespace Bm.Drawer
{
    public class DrawerUtils
    {
        public static string FormatDesc(Component component, string _root, bool includeChildren, bool includeType=true)
        {
            var fullName = component.transform.HierarchyName().Split(new char[] { '/' });
            int index = Array.IndexOf(fullName, _root);
            string name = "";
            for (int i = index; i < fullName.Length; i++)
            {
                name += fullName[i];
                name += i < fullName.Length - 1 ? "->" : "";
            }

            if (includeType)
            {
                return includeChildren ? $"{name}/{component.GetType()}" : $"{component.GetType()}";
            }
            else
            {
                return $"{name}";
            }
        }

        public static string GetValueInSquareBracket(string _text)
        {
            Regex rgx = new Regex(@"(?i)(?<=\[)(.*)(?=\])");//中括号[]
            return rgx.Match(_text).Value;
        }
        
        public static string[] CreateVariablePath(Type Class, Type filter, string _nameFilter, bool MatchValue)
        {
            var subTypeList = new List<Type>();
            var ret = new List<string>();
            var assembly = Class.Assembly; // 获取当前父类所在的程序集
            var assemblyAllTypes = assembly.GetTypes(); // 获取该程序集中的所有类型

            foreach (var itemType in assemblyAllTypes) // 遍历所有类型进行查找
            {
                var baseType = itemType.BaseType; // 获取元素类型的基类
                if (baseType != null && MatchValue==Regex.IsMatch(itemType.Name, _nameFilter)) // 如果有基类
                {
                    if (baseType.Name == Class.Name) // 如果基类就是给定的父类
                    {
                        subTypeList.Add(itemType); // 加入子类表中
                    }
                }
            }

            foreach (var t in subTypeList)
            {
                ret.AddRange(CreateVariablePathByType(t.Name,t, filter));
            }

            return ret.ToArray();
        }
        
        public static string[] CreateVariablePathByType(string parent, Type Class, Type filter)
        {
            List<string> ret = new List<string>();
            FieldInfo[] fields = Class.GetFields(BindingFlags.Public | BindingFlags.Instance);

            if (filter == null)
            {
                foreach (FieldInfo field in fields)
                {
                    if (field.FieldType.IsClass)
                    {
                        string[] path = CreateVariablePathByType($"{parent}", field.FieldType, filter);
                        if (path != null && path.Length > 0)
                        {
                            ret.AddRange(path);
                        }
                    }
                    else
                    {
                        ret.Add($"{parent}/{field.Name}");
                    }
                }
            }
            else
            {
                foreach (FieldInfo field in fields)
                {
                    if (field.FieldType.Name==filter.Name)
                    {
                        ret.Add($"{parent}/{field.Name}");
                    }
                    else if (field.FieldType.IsClass)
                    {
                        string[] path = CreateVariablePathByType($"{parent}", field.FieldType, filter);
                        if (path != null && path.Length > 0)
                        {
                            ret.AddRange(path);
                        }
                    }
                }
            }
            
            return ret.ToArray();
        }
        
        /// <summary>
        /// 判断类型是否为可操作的列表类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsList(Type type)
        {
            if (typeof(System.Collections.IList).IsAssignableFrom(type))
            {
                return true;
            }
            foreach (var it in type.GetInterfaces())
            {
                if (it.IsGenericType && typeof(IList<>) == it.GetGenericTypeDefinition())
                    return true;
            }
            return false;
        }
        
        public static int FindMax(int [] _arr)
        {
            int ret = int.MinValue;
            for (int i=0; i<_arr.Length; i++)
            {
                ret = _arr[i] > ret?_arr[i]:ret;
            }

            return ret;
        }

    }
    
    
    [InitializeOnLoad]
    public static class DrawerEditorHelper
    {
        private static Dictionary<string, string[]> ClassVariableCache = new Dictionary<string, string[]>();
        //初始化类时,注册事件处理函数
        static DrawerEditorHelper()
        {
            ClassVariableCache.Clear();
        }

        public static string[] GetClassVariableCache(ClassVariableSelect attr)
        {
            string key = $"{attr.Class}{attr.Filter}{attr.NameFilter}";
            string[] ret;
            if (ClassVariableCache.TryGetValue(key, out ret))
            {
                return ret;
            }

            ret = DrawerUtils.CreateVariablePath(attr.Class, attr.Filter, attr.NameFilter, attr.MatchValue);
            ClassVariableCache.Add(key, ret);
            return ret;
        }
    }

}

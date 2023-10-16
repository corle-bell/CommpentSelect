using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    }
    
   

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ComponentSelect
{
    public class ComponentSelectUtils
    {
        public static string FormatDesc(Component component, string _root, bool includeChildren)
        {
            var fullName = component.transform.HierarchyName().Split(new char[] { '/' });
            int index = Array.IndexOf(fullName, _root);
            string name = "";
            for (int i = index; i < fullName.Length; i++)
            {
                name += fullName[i];
                name += i < fullName.Length - 1 ? "->" : "";
            }
            return includeChildren ? $"{name}/{component.GetType()}" : $"{component.GetType()}";
        }
    }

}

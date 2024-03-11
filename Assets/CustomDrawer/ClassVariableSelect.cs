//*************************************************
//----Author:       Cyy 
//
//----CreateDate:   2024-02-19 17:56:27
//
//----Desc:         Create By BM
//
//**************************************************

using System;
using UnityEngine;

namespace Bm.Drawer
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ClassVariableSelect : PropertyAttribute
    {
        public Type Class;
        public Type Filter;
        public string NameFilter;
        public bool MatchValue;
        public ClassVariableSelect(Type _Class, Type _Filter, string _NameFilter="", bool _MatchValue=true)
        {
            Class = _Class;
            Filter = _Filter;
            MatchValue = _MatchValue;
            NameFilter = _NameFilter;
        }
    }
}
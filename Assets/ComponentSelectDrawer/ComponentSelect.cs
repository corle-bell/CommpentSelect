//*************************************************
//----Author:       Cyy 
//
//----CreateDate:   2023-08-15 11:35:46
//
//----Desc:         Create By BM
//
//**************************************************
using UnityEngine;
using System;


namespace ComponentSelect
{
    public enum Style
    {
        PopUp,
        Button,
    }
    
    
    [AttributeUsage(AttributeTargets.Field)]
    public class ComponentSelectAttribute : PropertyAttribute {
 
        public Type type;
        public bool includeChildren;
        public Style style;
        public ComponentSelectAttribute(bool _includeChildren=false, Type type=null, Style _style = Style.PopUp) {
            this.type = type;
            includeChildren = _includeChildren;
            style = _style;
        }
 
    }
    

}
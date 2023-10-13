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


namespace Bm.Drawer
{
    public enum Style
    {
        DropDown,
        PopUp,
    }
    
    
    [AttributeUsage(AttributeTargets.Field)]
    public class ComponentSelectAttribute : PropertyAttribute {
 
        public Type type;
        public bool includeChildren;
        public Style style;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_includeChildren"> is Include Children Component </param>
        /// <param name="type"> Select Type </param>
        /// <param name="_style"> Drawer Style </param>
        public ComponentSelectAttribute(bool _includeChildren=false, Type type=null, Style _style = Style.PopUp) {
            this.type = type;
            includeChildren = _includeChildren;
            style = _style;
        }
 
    }
    

}
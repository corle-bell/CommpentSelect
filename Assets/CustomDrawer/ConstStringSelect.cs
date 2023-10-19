//*************************************************
//----Author:       Cyy 
//
//----CreateDate:   2023-10-13 13:49:42
//
//----Desc:         Create By BM
//
//**************************************************
using UnityEngine;
using System;


namespace Bm.Drawer
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ConstStringSelectAttribute : PropertyAttribute 
    {
        public bool isAutoClose;
        
        public ConstStringSelectAttribute(bool _isAutoClose=true) {
            this.isAutoClose = _isAutoClose;
        }
    }
    
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class ConstStringContentAttribute : System.Attribute
    {
        
    }
}
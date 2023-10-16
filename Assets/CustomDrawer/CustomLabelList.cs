//*************************************************
//----Author:       Cyy 
//
//----CreateDate:   2023-10-14 17:30:21
//
//----Desc:         Create By BM
//
//**************************************************
using UnityEngine;
using System;


namespace Bm.Drawer
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CustomLabelListAttribute : PropertyAttribute
    {
        public Type EnumType;
        public string FieldName;
        public CustomLabelListAttribute(Type _enumType)
        {
            EnumType = _enumType;
            FieldName = null;
        }
        
        public CustomLabelListAttribute(string _field)
        {
            EnumType = null;
            FieldName = _field;
        }
    }
}
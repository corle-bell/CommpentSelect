//*************************************************
//----Author:       Cyy 
//
//----CreateDate:   2024-01-31 16:19:22
//
//----Desc:         Create By BM
//
//**************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bm.Drawer
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowTimeStampAttribute : PropertyAttribute
    {
        public string format;
        public int coefficient;
        public bool isLocal = false;
        public ShowTimeStampAttribute(string _format="yyyy-MM-dd HH:mm:ss", int _coefficient=1, bool _isisLocal=false)
        {
            format = _format;
            coefficient = _coefficient;
            isLocal = _isisLocal;
        }
        
        public ShowTimeStampAttribute(string _format, bool _isisLocal=false)
        {
            format = _format;
            coefficient = 1;
            isLocal = _isisLocal;
        }
        
        public ShowTimeStampAttribute(int _coefficient, bool _isisLocal=false)
        {
            format = "yyyy-MM-dd HH:mm:ss";
            coefficient = _coefficient;
            isLocal = _isisLocal;
        }
        
        public ShowTimeStampAttribute(bool _isisLocal=false)
        {
            format = "yyyy-MM-dd HH:mm:ss";
            coefficient = 1;
            isLocal = _isisLocal;
        }
    }
}
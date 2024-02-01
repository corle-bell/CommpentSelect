//*************************************************
//----Author:       Cyy 
//
//----CreateDate:   2024-01-31 16:24:07
//
//----Desc:         Create By BM
//
//**************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEditor;

namespace Bm.Drawer
{
    [CustomPropertyDrawer(typeof(ShowTimeStampAttribute))]

    public class ShowTimeStampDrawer : PropertyDrawer
    {
        public DateTimeOffset dateTimeOffset;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowTimeStampAttribute attr = (ShowTimeStampAttribute)attribute;

            Rect src = new Rect(position);
            src.width = position.width * 0.7f;
            string timeStr = Format(property.intValue * attr.coefficient, attr.format, attr.isLocal);
            var outStr = EditorGUI.DelayedTextField(src, label.text, timeStr);
            
            if (!outStr.Equals(timeStr))
            {
                if (attr.isLocal)
                {
                    dateTimeOffset = DateTimeOffset.Parse(outStr).ToUniversalTime();   
                }
                else
                {
                    dateTimeOffset = DateTimeOffset.ParseExact(outStr, attr.format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                }
                
                property.intValue = (int)dateTimeOffset.ToUnixTimeSeconds()/attr.coefficient;
            }
            
            src.x += src.width+18f;
            src.width = position.width * 0.3f - 18f;
            property.intValue = EditorGUI.DelayedIntField(src, property.intValue);
        }

        private string Format(int _time, string _format, bool isLocal)
        {
            dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(_time);
            if (isLocal)
            {
                dateTimeOffset = dateTimeOffset.ToLocalTime();
            }
            return dateTimeOffset.ToString(_format);
        }
    }
}



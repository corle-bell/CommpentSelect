using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bm.Drawer;

public enum UIName
{
    Loading,
    Game,
    Shop,
    Setting
}

public class ClassTest : MonoBehaviour
{
    [ComponentSelect(true, typeof(Component), Style.DropDown)]
    public Component script0;

    [ComponentSelect(true, typeof(ClassBasic))]
    public ClassBasic script1;

    [ComponentSelect(true, typeof(ClassA))]
    public Component script2;

    [ComponentSelect(true, typeof(Component), Style.PopUp)]
    public Component script3;


    [ConstStringSelect]
    public string text0;
    
    [ConstStringSelect]
    public string text1;

    [CustomLabelList(typeof(UIName))]
    public string[] UiTitle;
    
    
    [CustomLabelList("_UIOrder_Labels")]
    public int[] UiOrder;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"script0 = {script0}");
        Debug.Log($"script1 = {script1}");
        Debug.Log($"script2 = {script2}");
        Debug.Log($"script3 = {script3}");
        
        Debug.Log($"text0 = {text0}");
        Debug.Log($"text1 = {text1}");
        
        
        Debug.Log($"{UIName.Loading} = {UiTitle[(int)UIName.Loading]}");
        Debug.Log($"{UIName.Shop} = {UiTitle[(int)UIName.Shop]}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #if UNITY_EDITOR
    private static string[] _UIOrder_Labels = new string[]
    {
        "String_Loading",
        "String_Game",
        "String_Shop",
        "String_Setting",
    };
    #endif
}

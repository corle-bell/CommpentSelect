using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bm.Drawer;

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
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"script0 = {script0}");
        Debug.Log($"script1 = {script1}");
        Debug.Log($"script2 = {script2}");
        Debug.Log($"script3 = {script3}");
        
        Debug.Log($"text0 = {text0}");
        Debug.Log($"text1 = {text1}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

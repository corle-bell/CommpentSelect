
# ShowTimeStamp
格式化显示时间戳

``` javascript
[ShowTimeStamp("yyyy/MM/dd HH:mm:ss", 60,true)]
public int TimeTicks1 = 1706777351;
```

参数1：显示格式 

参数2：时间戳比例, 60的话就是相当于，以分钟作为时间戳 

参数3：是否显示为当前时区 

### Editor界面显示
![Image text](https://github.com/corle-bell/ComponentSelect/blob/main/Screenshoot/ShowTimeStamp.png)
# ComponentSelect

选择脚本选择器

``` javascript
[ComponentSelect(true, typeof(Component), Style.PopUp)]
```

第一个参数为是否包含子物体的脚本

第二个参数是脚本的类型

第三个参数是Drawer类型,两种可选。下拉列表，和弹窗,默认是弹窗。

### Editor界面显示

![Image text](https://github.com/corle-bell/ComponentSelect/blob/main/Screenshoot/QQ截图20230827114033.png)
### 特性使用代码示例
![Image text](https://github.com/corle-bell/ComponentSelect/blob/main/Screenshoot/Code.png)
### 弹窗界面展示
![Image text](https://github.com/corle-bell/ComponentSelect/blob/main/Screenshoot/PopUp.png)
### 下拉列表展示
![Image text](https://github.com/corle-bell/ComponentSelect/blob/main/Screenshoot/DropDown.png)



# ConstStringSelect
常量字符串选择器

``` javascript
[ConstStringContent]
public class UIStringDefine
{
    public const string UI_Title = "This is Title";
    public const string UI_Hp = "This is Hp";
    public const string UI_Level = "This is Hp";
}

public class ClassTest : MonoBehaviour
{
    [ConstStringSelect]
    public string text0;
}
```

### Editor界面显示
![Image text](https://github.com/corle-bell/ComponentSelect/blob/main/Screenshoot/ConstStringSelect.png)



### 特性使用
![Image text](https://github.com/corle-bell/ComponentSelect/blob/main/Screenshoot/ConstStringSelect_Code.png)


# CustomLabelList
自定义数组标题
兼容Odin插件

``` javascript

public enum UIName
{
    Loading,
    Game,
    Shop,
    Setting
}

[CustomLabelList(typeof(UIName))]
public string[] UiTitle;


[CustomLabelList("_UIOrder_Labels")]
public int[] UiOrder;

private static string[] _UIOrder_Labels = new string[]
{
	"String_Loading",
	"String_Game",
	"String_Shop",
	"String_Setting",
};
```

### Editor界面显示
![Image text](https://github.com/corle-bell/ComponentSelect/blob/main/Screenshoot/CustomLabelList_Editor.png)

![Image text](https://github.com/corle-bell/ComponentSelect/blob/main/Screenshoot/CustomLabelList_Editor_Odin.png)
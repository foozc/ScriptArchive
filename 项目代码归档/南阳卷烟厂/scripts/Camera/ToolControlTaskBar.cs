using System;
using System.Runtime.InteropServices;
//control the task bar hide or show
//liuyanlei
public class ToolControlTaskBar
{
    [DllImport("user32.dll")]   //这里是引入 user32.dll 库， 这个库是windows系统自带的。
    public static extern int ShowWindow(int hwnd, int nCmdShow); //这是显示任务栏
    [DllImport("user32.dll")]
    public static extern int FindWindow(string lpClassName, string lpWindowName); //这是隐藏任务栏
    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();

    private const int SW_HIDE = 0;  //hied task bar
    private const int SW_RESTORE = 8;//show task bar
                                     // Use this for initialization
    
    /// <summary>
    /// show TaskBar
    /// </summary>
    public static void ShowTaskBar()
    {
        ShowWindow(FindWindow("Button", null), SW_RESTORE);
        ShowWindow(FindWindow("Shell_TrayWnd", null), SW_RESTORE);
    }
    /// <summary>
    /// Hide TaskBar
    /// </summary>
    public static void HideTaskBar()
    {
        ShowWindow(FindWindow("Button", null), SW_HIDE);
        ShowWindow(FindWindow("Shell_TrayWnd", null), SW_HIDE);
    }
}
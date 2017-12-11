using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]

public class OpenFileDialogTest
{
	public int structSize = 0;
	public IntPtr dlgOwner = IntPtr.Zero;
	public IntPtr instance = IntPtr.Zero;
	public String filter = null;
	public String customFilter = null;
	public int maxCustFilter = 0;
	public int filterIndex = 0;
	public String file = null;
	public int maxFile = 0;
	public String fileTitle = null;
	public int maxFileTitle = 0;
	public String initialDir = null;
	public String title = null;
	public int flags = 0;
	public short fileOffset = 0;
	public short fileExtension = 0;
	public String defExt = null;
	public IntPtr custData = IntPtr.Zero;
	public IntPtr hook = IntPtr.Zero;
	public String templateName = null;
	public IntPtr reservedPtr = IntPtr.Zero;
	public int reservedInt = 0;
	public int flagsEx = 0;
}
public class WindowDll
{
	[DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
	public static extern bool GetOpenFileName([In, Out] OpenFileDialogTest ofn);
	public static bool GetOpenFileName1([In, Out] OpenFileDialogTest ofn)
	{
		return GetOpenFileName(ofn);
	}
}
public class OpenFile : MonoBehaviour
{
	private string openWindowName;
	private string fileSuffix;
	public enum FileType
	{
		AllType = 0,
		Ppt = 1,
		Word = 2,
		Excel=3,
		Vedio=4,
        Image=5
	}

    public void OpenFileByType(FileType FileType, Action<string, string> action, string option)
    {
        switch (FileType)
        {
            case FileType.AllType:
                openWindowName = "AllType";
                fileSuffix = "All Files\0*.*\0\0";
                break;
            case FileType.Ppt:
                openWindowName = "Ppt";
                fileSuffix = "PPT文件(*.pptx;*.ppt;*.pptm;*.ppsx)\0*.pptx;*.ppt;*.pptm;*.ppsx";
                break;
            case FileType.Word:
                openWindowName = "Word";
                fileSuffix = "Word文件(*.docx;*.docm;*.docm;*.dotx;*.doc)\0*.docx;*.docm;*.docm;*.dotx;*.doc";
                break;
            case FileType.Excel:
                openWindowName = "Excel";
                fileSuffix = "Excel文件(*.xlsm;*.xlsm;*.xls;*.xltx;*.xlsx)\0*.xlsm;*.xlsm;*.xls;*.xltx;*.xlsx";
                break;
            case FileType.Vedio:
                openWindowName = "Vedio";
                fileSuffix = "视频文件(*.avi;*.wmv;*.mov;*.mp4)\0*.avi;*.wmv;*.mov;*.mp4";
                break;
            case FileType.Image:
                openWindowName = "Image";
                fileSuffix = "图片文件(*.jpg;*.png)\0*.jpg;*.png";
                break;
            default:
                break;
        }
        OpenFileDiaLog(action, option);
    }

    public void OpenFileByType(FileType FileType, Action<string> action)
	{
		switch (FileType)
		{
			case FileType.AllType:
				openWindowName = "AllType";
				fileSuffix = "All Files\0*.*\0\0";
				break;
			case FileType.Ppt:
				openWindowName = "Ppt";
				fileSuffix = "PPT文件(*.pptx;*.ppt;*.pptm;*.ppsx)\0*.pptx;*.ppt;*.pptm;*.ppsx";
				break;
			case FileType.Word:
				openWindowName = "Word";
				fileSuffix = "Word文件(*.docx;*.docm;*.docm;*.dotx;*.doc)\0*.docx;*.docm;*.docm;*.dotx;*.doc";
				break;
			case FileType.Excel:
				openWindowName = "Excel";
				fileSuffix = "Excel文件(*.xlsm;*.xlsm;*.xls;*.xltx;*.xlsx)\0*.xlsm;*.xlsm;*.xls;*.xltx;*.xlsx";
				break;
			case FileType.Vedio:
				openWindowName = "Vedio";
				fileSuffix = "视频文件(*.avi;*.mov;*.mp4)\0*.avi;*.mov;*.mp4";
				break;
            case FileType.Image:
                openWindowName = "Image";
                fileSuffix = "图片文件(*.jpg;*.png)\0*.jpg;*.png";
                break;
			default:
				break;
		}
		OpenFileDiaLog(action);
	}

    public void OpenFileDiaLog(Action<string, string> action, string option)
    {
        OpenFileDialogTest ofn = new OpenFileDialogTest();
        ofn.structSize = Marshal.SizeOf(ofn);
        //ofn.filter = "三菱(*.gxw)\0*.gxw\0西门子(*.mwp)\0*.mwp\0All Files\0*.*\0\0";
        ofn.filter = fileSuffix;
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        string path = Application.streamingAssetsPath;
        path = path.Replace('/', '\\');//默认路径  
        ofn.initialDir = path;
        ofn.title = openWindowName;
        //ofn.defExt = "JPG";//显示文件的类型  
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR  
        if (WindowDll.GetOpenFileName(ofn))
        {
            if (action != null)
                action(ofn.file, option);
            Debug.Log(ofn.file);
        }
    }


    public void OpenFileDiaLog(Action<string> action)
	{
		OpenFileDialogTest ofn = new OpenFileDialogTest();
		ofn.structSize = Marshal.SizeOf(ofn);
		//ofn.filter = "三菱(*.gxw)\0*.gxw\0西门子(*.mwp)\0*.mwp\0All Files\0*.*\0\0";
		ofn.filter =fileSuffix;
		ofn.file = new string(new char[256]);
		ofn.maxFile = ofn.file.Length;
		ofn.fileTitle = new string(new char[64]);
		ofn.maxFileTitle = ofn.fileTitle.Length;
		string path = Application.streamingAssetsPath;
		path = path.Replace('/', '\\');//默认路径  
		ofn.initialDir = path;
		ofn.title = openWindowName;
		//ofn.defExt = "JPG";//显示文件的类型  
		ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR  
		if (WindowDll.GetOpenFileName(ofn))
		{
            if (action != null)
                action(ofn.file);
		}
	}

    public void myAction(string fileName)
    {
        Debug.Log(fileName);
    }

	public void test()
	{
		OpenFileByType(FileType.Ppt, myAction);
	}
	public void test2()
	{
		OpenFileByType(FileType.Word, myAction);
	}
	public void test3()
	{
		OpenFileByType(FileType.Excel, myAction);
	}
	public void test4()
	{
		OpenFileByType(FileType.Vedio, myAction);
	}
}
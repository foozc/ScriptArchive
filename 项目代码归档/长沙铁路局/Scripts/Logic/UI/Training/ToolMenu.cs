using Assets.Scripts.Configs;
using Assets.Scripts.Controller;
using Assets.Scripts.VO;
using PluginEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Training;
using Utility;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:工具菜单控制
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.Training
{
    public class ToolMenu : MonoBehaviour
    {
        public UIScrollBar scrollBar;
        public GameObject toolModePrefab;
        public UIGrid grid;
        public UISprite toolFollow;
        public UISprite toolTitle;
        public GameObject toolPanel;

        private Dictionary<string, ToolModel> toolModes;
        private string currentTool = "";
        private GameObject currentHintTool;
        private Dictionary<string, Texture2D> textureDic;
        private bool isShowToolMenu = true;
        public UIAtlas atlas;
        private Action<string> action;
        private bool isInit = false;
        private bool isClickTool = false;
		public GameObject lastName=null;
        public void addToolItem(ToolModel toolModel)
        {
            if (toolModes == null) toolModes = new Dictionary<string, ToolModel>();
            if (!toolModes.ContainsKey(toolModel.Id))
                toolModes.Add(toolModel.Id, toolModel);
            GameObject obj = ResourceManager.getInstance().loadUISpawnPool(toolModePrefab, null);
            toolModel.ToolMenuItem = obj;
            obj.name = toolModel.Id;
            UIEventListener.Get(obj).onClick += toolMenuItemClick;
            UISprite toolIcon = obj.transform.Find("toolIcon").GetComponent<UISprite>();
            toolIcon.atlas = atlas;
            toolIcon.spriteName = toolModel.SpriteName;
            toolIcon.MakePixelPerfect();
            obj.transform.Find("toolName").GetComponent<UILabel>().text = toolModel.Name;
            grid.AddChild(obj.transform);
            obj.transform.localPosition = Vector3.zero;
            grid.repositionNow = true;
            grid.Reposition();
            scrollBar.value = 0;
        }

        public void initToolMenu(Action<string> action)
		{
			isShowToolMenu = false;
			showToolMenu();
			scrollBar.value = 0;
			if (!isInit)
                StartCoroutine(loadTexture());
            isInit = true;
            this.action = action;
		}

        private void createAtlas()
        {
            if (atlas == null)
            {
                atlas = this.gameObject.AddComponent<UIAtlas>();
            }
            List<UISpriteData> list = new List<UISpriteData>();
            Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            int maxSize = 4096;
            List<Texture2D> texture2Ds = new List<Texture2D>(textureDic.Values);
            List<string> textNames = new List<string>(textureDic.Keys);
            Rect[] rects = tex.PackTextures(texture2Ds.ToArray(), 2, maxSize);
            atlas.spriteMaterial = new Material(Shader.Find("Unlit/Transparent Colored"));
            atlas.spriteMaterial.mainTexture = tex;
            for(int i = 0; i < textNames.Count; i++)
            {
                UISpriteData data = new UISpriteData();
                Rect rect = NGUIMath.ConvertToPixels(rects[i], tex.width, tex.height, true);
                data.x = Mathf.RoundToInt(rect.x);
                data.y = Mathf.RoundToInt(rect.y);
                data.width = Mathf.RoundToInt(rect.width);
                data.height = Mathf.RoundToInt(rect.height);
                data.name = textNames[i];
                list.Add(data);
            }
            atlas.spriteList = list;

        }

        public void addToolItems(List<ToolModel> toolModels)
        {
			foreach (ToolModel toolModel in toolModels)
			{
				addToolItem(toolModel);
			}
        }

        public void mouseClick()
        {
            if (!isClickTool)
            {
                currentTool = "";
                toolFollow.gameObject.SetActive(false);
            }
            else isClickTool = false;
        }
        
        public void toolMenuItemClick(GameObject obj)
        {
            isClickTool = true;
            if (action != null)
                action(obj.name);
            PluginEventManager.FireEvent("RecordTools", toolModes[obj.name].SpriteName);
            toolFollow.gameObject.SetActive(true);
            currentTool = obj.name;
            toolFollow.atlas = atlas;
            toolFollow.spriteName = toolModes[obj.name].SpriteName;
		}

        public void cancelToolFollow()
        {
            toolFollow.gameObject.SetActive(false);
        }
        
        IEnumerator loadTexture()
        {
            DirectoryInfo dir = new DirectoryInfo(GlobalConfig.toolTexturePath);//初始化一个DirectoryInfo类的对象
            GetAllFiles(dir);
            if (textureDic == null) textureDic = new Dictionary<string, Texture2D>();
            foreach (DictionaryEntry de in ht)
            {
                string path = GlobalConfig.toolTexturePath + "/" + de.Key;
                path = path.Replace('/', '\\');
                WWW www = new WWW("file://" + path);
                while (!www.isDone)
                {
                    yield return null;
                }
                if (www != null)
                {
                    textureDic.Add(de.Key.ToString().Substring(0, de.Key.ToString().LastIndexOf('.')), www.texture);
                }
                www.Dispose();
            }
            ht.Clear();
            ht = null;
            createAtlas();

            //string pathTool = Application.streamingAssetsPath + "/tools.txt";
            //addToolItems(FileUtils.loadObjectFromJsonFile<List<ToolModel>>(pathTool));

            ToolsDBHelper toolDB = new ToolsDBHelper();
            //FileUtils.saveObjectToJsonFile(toolDB.getTools(), pathTool);
            addToolItems(toolDB.getTools());
        }
        Hashtable ht = new Hashtable();
        public void GetAllFiles(DirectoryInfo dir)
        {
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();   //初始化一个FileSystemInfo类型的实例
            foreach (FileSystemInfo i in fileinfo)              //循环遍历fileinfo下的所有内容
            {
                if (i is DirectoryInfo)             //当在DirectoryInfo中存在i时
                {
                    GetAllFiles((DirectoryInfo)i);  //获取i下的所有文件
                }
                else
                {
                    string str = i.FullName;        //记录i的绝对路径
                    string fileName = str.Substring(str.LastIndexOf("\\") + 1);
                    string fileType = str.Substring(str.LastIndexOf(".") + 1, (str.Length - str.LastIndexOf(".") - 1));
                    if (!fileType.Equals("meta"))
                    {
                        if (ht.Contains(fileName))
                        {
                            ht[fileName] = fileName;
                        }
                        else
                        {
                            ht.Add(fileName, fileName);
                        }
                    }
                }
            }
        }

        public void destroyToolItem()
        {
            toolFollow.gameObject.SetActive(false);
            currentTool = null;
        }

        public bool hintToolItem(string id, bool show)
        {
            if (!toolModes.ContainsKey(id))
                return false;
            if (currentHintTool != null)
            {
                currentHintTool.gameObject.SetActive(false);
                currentHintTool = null;
            }
            if (!show)
                return true;
            currentHintTool = toolModes[id].ToolMenuItem.transform.Find("particle").gameObject;
            currentHintTool.gameObject.SetActive(true);
            return true;
        }

        public void showToolMenu()
        {
            if (isShowToolMenu)
            {
                isShowToolMenu = false;
                toolTitle.spriteName = "gongjulam-tiaotou-shouqi";
				toolTitle.GetComponent<UIButton>().normalSprite = "gongjulam-tiaotou-shouqi";
			}
            else
            {
                isShowToolMenu = true;
                toolTitle.spriteName = "gongjulam-tiaotou-xiala";
				toolTitle.GetComponent<UIButton>().normalSprite = "gongjulam-tiaotou-xiala";
			}
        }

        public void scrollBarTop()
        {
            float size = scrollBar.barSize;
            scrollBar.value -= size / 10;
        }

        public void scrollBarBottom()
        {
            float size = scrollBar.barSize;
            scrollBar.value += size / 10;
        }
		public string spriteName;
		public void hindTool(string toolItemName,bool isHigh)
		{
			spriteName = toolItemName;
			//if (isHigh)
			//	lastName = toolItemName;
			//else
			//	lastName = null;
			if (lastName != null)
                lastName.SetActive(false);
            foreach (var item in toolModes)
			{
				if (item.Value.SpriteName.Equals(toolItemName))
				{
                    lastName = item.Value.ToolMenuItem.transform.Find("particle").gameObject;
					print(item.Value.ToolMenuItem.transform.parent.parent.parent.parent);
					if (item.Value.ToolMenuItem.transform.parent.parent.parent.parent.localScale.y<1.0f)
					{
						return;
					}
					item.Value.ToolMenuItem.transform.Find("particle").gameObject.SetActive(isHigh);
                }
			}
		}

        public void canelHindTool()
        {
            if (lastName != null)
            {
                lastName.SetActive(false);
                lastName = null;
            }
        }
	}
}

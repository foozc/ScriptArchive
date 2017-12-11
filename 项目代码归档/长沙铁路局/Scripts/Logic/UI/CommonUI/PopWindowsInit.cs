using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Logic.UI.CommonUI;
using Assets.Scripts.Controller;
using Assets.Scripts.Logic.UI.Training;
using System;

public class PopWindowsInit : MonoBehaviour
{
	private Action<string> itemClickAction;
	private MyTreeMenu treeMenu;
	private List<UISprite> popMenuItem = new List<UISprite>();
	private List<string> value = new List<string>();
	public UIGrid grid;
	public UIScrollBar scrollBar;
	public UIScrollView scrollView;
	public GameObject popItem;
	private string[] itemSprite = { "neirong", "neirong_p" };
	void Start()
	{
		init();
	}
	private void init()
	{
		treeMenu = GameObject.Find("TreeMenu").GetComponent<MyTreeMenu>();
		rest();
		foreach (Transform item in treeMenu.treeGrid.transform)
		{
			if (item.GetComponent<MyTreeMenuItem>().getNodeLevel() == 1)
			{
				//if (item.GetComponent<MyTreeMenuItem>().name.Equals("1"))
				//{
				//	continue;
				//}
				//else
				//{
					GameObject obj = ResourceManager.getInstance().loadUISpawnPool(popItem, null);
					grid.AddChild(obj.transform);
					obj.transform.position = Vector3.zero;
					obj.transform.localScale=new Vector3(1,1,1);
					obj.name = item.GetComponent<MyTreeMenuItem>().name;
                    obj.transform.Find("name").GetComponent<UILabel>().text = item.GetComponent<MyTreeMenuItem>().getItemVO().name;
					value.Add(item.GetComponent<MyTreeMenuItem>().getItemVO().value);
					UIEventListener.Get(obj).onClick = popItemClick;
					popMenuItem.Add(obj.GetComponent<UISprite>());
				//}
			}
		}
		grid.repositionNow = true;
		grid.Reposition();
		scrollBar.value = 0;
	}
	public void setItemClick(Action<string> itemClickAction)
	{
			this.itemClickAction = itemClickAction;
	}
	private void popItemClick(GameObject Obj)
	{
		foreach (UISprite item in popMenuItem)
		{
			item.spriteName = itemSprite[0];
		}
		Obj.GetComponent<UISprite>().spriteName = itemSprite[1];
		if (itemClickAction!=null)
		{
			itemClickAction(value[popMenuItem.IndexOf(Obj.GetComponent<UISprite>())]);
		}
	}
	public void rest()
	{
		int count = grid.GetChildList().Count;
		List<Transform> trs = grid.GetChildList();
		for (int i = 0; i < count; i++)
		{
			ResourceManager.getInstance().destroyUILoad(trs[i].gameObject);
		}
	}
}

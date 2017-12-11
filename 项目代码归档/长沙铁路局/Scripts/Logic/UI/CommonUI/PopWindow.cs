using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Scripts.Logic.UI.Training;
using Training;
using Assets.Scripts.Controller;
public class PopWindow : MonoBehaviour
{
	public TrainUI tui;
	public FlowOperateMenu flows;
	private Dictionary<int, FlowOperateMenu.FlowItem> flow = null;
	public UIGrid grid;
	public UIScrollBar scrollBar;
	public UIScrollView scrollView;
	public GameObject popItem;
	private string[] itemSprite = { "neirong", "neirong_p" };
	private List<UISprite> popMenuItem = new List<UISprite>();
	void Start()
	{
		//init();
	}
	public void init()
	{
		if (popMenuItem.Count!=0)
		{
			popMenuItem.Clear();
			foreach (Transform item in grid.transform)
			{
				Destroy(item.gameObject);
			}
		}
		flow = flows.getflows();
		for (int i = 0; i < flow.Count; i++)
		{
			GameObject obj = ResourceManager.getInstance().loadUISpawnPool(popItem, null);
			grid.AddChild(obj.transform);
			obj.transform.position = Vector3.zero;
			obj.transform.localScale = new Vector3(1, 1, 1);
			obj.name = flow[i].index.ToString();
            obj.transform.Find("name").GetComponent<UILabel>().text = flow[i].name.text;
			UIEventListener.Get(obj).onClick = popItemClick;
			popMenuItem.Add(obj.GetComponent<UISprite>());
		}
		grid.repositionNow = true;
		grid.Reposition();
		scrollBar.value = 0;
	}
	private void popItemClick(GameObject Obj)
	{
		if (popMenuItem.Count!=0)
		{
			foreach (UISprite item in popMenuItem)
			{
				item.spriteName = itemSprite[0];
			}
			Obj.GetComponent<UISprite>().spriteName = itemSprite[1];
			tui.flowOperateMenuClick(popMenuItem.IndexOf(Obj.GetComponent<UISprite>()));
		}
	}
}

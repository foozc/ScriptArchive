using UnityEngine;
using System.Collections;
using Assets.Scripts.Logic.UI.CommonUI;

public class DragScrollViewChild : MyUIDragScrollView
{
	public UILabel ub;
	public void OnScroll(float delta)
	{
		if (scrollView.panel.GetViewSize().y > ub.localSize.y)
		{
			scrollView.transform.localPosition = new Vector3(scrollView.transform.localPosition.x,0,scrollView.transform.localPosition.z);
		}
		else
		{
			base.OnScroll(delta);
		}
	}
}

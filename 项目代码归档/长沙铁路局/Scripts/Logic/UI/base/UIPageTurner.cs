using UnityEngine;
using System.Collections;

public class UIPageTurner : MonoBehaviour {

	#region INI
	public UIButton right_btn;
	public UIButton left_btn;
	public UILabel num;
	public Transform[] pages;
	#endregion

	int i;

	void OnEnable(){
		i = 0;
		activeI(i);
	}

	void Start () {
		right_btn.onClick.Add(new EventDelegate(()=>{
			if((i+1)<pages.Length)
			activeI(++i);
		}));
		left_btn.onClick.Add(new EventDelegate(()=>{
			if((i)>0)
			activeI(--i);
		}));
	}
	
	void Update () {
	
	}

	void activeI(int i)
	{
		for (int k = 0; k < pages.Length; k++) {
			if(k == i)pages[k].gameObject.SetActive(true);
			else pages[k].gameObject.SetActive(false);
		}
		num.text = (i+1) +"/"+ pages.Length.ToString();
	}
}

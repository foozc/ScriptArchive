using UnityEngine;
using System.Collections;

public class FilledScreen : MonoBehaviour
{
	void Start()
	{
		UIRoot root = GameObject.FindObjectOfType<UIRoot>();
		if (root != null)
		{
			float s = (float)root.activeHeight / Screen.height;
			int height = Mathf.CeilToInt(Screen.height * s);
			int width = Mathf.CeilToInt(Screen.width * s);
			gameObject.GetComponent<UIWidget>().SetRect(-width/2,-height/2,width,height);
		}
	}
	void Update()
	{

	}
}

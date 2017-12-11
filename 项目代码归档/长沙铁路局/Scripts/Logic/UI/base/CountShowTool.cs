using UnityEngine;
using System.Collections;

public class CountShowTool : MonoBehaviour {

	public Transform[] tras;

	public void setNum(int i){
		for(int x=0; x<tras.Length; x++){
			if(x<i) tras[x].gameObject.SetActive(true);
			else tras[x].gameObject.SetActive(false);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBg : MonoBehaviour {

	// Use this for initialization
	public void BgChange()
    {
        Debug.Log(UIButton.current.GetComponent<UIButton>());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using Assets.Scripts.Load;
public class StopOperation : MonoBehaviour {
    private TweenAlpha TA;
    public UILabel label;
    public UIScrollBar USB;
    public UILabel songName;
    private TextuerPropModel TPM;
    // Use this for initialization
    void Start () {


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //在中间停止时回调函数
    public void StopCallbackFunction(int Index)
    {
        TA = this.GetComponent<TweenAlpha>();
        TPM = TextuerPropModel.GetInstance();
        USB.value = 0;
        label.enabled = false;
        TA.enabled = true;
        TA.ResetToBeginning();
        TA.PlayForward();

        songName.text = TPM.listTextuerProp[Index].MusicName;
        label.text = TPM.listContent[Index];
        label.enabled = true;

    }
}

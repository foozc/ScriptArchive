using UnityEngine;
using System.Collections;
using Assets.Scripts.Controller;

public class ModelCognition : MonoBehaviour
{
    public GameObject HighLight;
	public GameObject arrow;
    // Use this for initialization
    public void OnMouseover()
    {
		AudioManager.getInstance().AudioStop(AudioManager.MusicNumType.realtimeMusic);
		string musicPath=null;
		string text = null;
		switch (HighLight.name)
		{
			case "zuozhuangyouzhaung-H"://L（R）
				musicPath = "2.jiegourenzhi_nr";
				text = "L或者R表示左装或右装。";
				break;
			case "jianchexingcheng-H"://160
				musicPath = "2.jiegourenzhi_160";
				text = "160表示检测行程。";
				break;
			case "dongcheng-H"://220
				musicPath = "2.jiegourenzhi_200";
				text = "200表示动程。";
				break;
			case "zhongguoxing-H"://c
				musicPath = "2.jiegourenzhi_C";
				text = "C表示中国型。";
				break;
			case "gunzhusigang-H"://k
				musicPath = "2.jiegourenzhi_k";
				text = "K表示滚珠丝杠。";
				break;
			case "SIEMENS-H"://s
				musicPath = "2.jiegourenzhi-s";
				text = "s表示西门子。";
				break;
			case "baochili-H"://700
				musicPath = "2.jiegourenzhi_700";
				text = "700表示保持力。";
				break;
		}
		AudioManager.getInstance().AudioPlay("sbrz/" + musicPath, AudioManager.MusicNumType.realtimeMusic);
		UIManager.getInstance().equipKnowUI.CallTextInfo(text);
		HighLight.SetActive(true);
		arrow.SetActive(true);
		print("in");
    }
    public void OnMouseOut()
    {
		AudioManager.getInstance().AudioStop(AudioManager.MusicNumType.realtimeMusic);
		HighLight.SetActive(false);
		arrow.SetActive(false);
		print("out");
    }
}

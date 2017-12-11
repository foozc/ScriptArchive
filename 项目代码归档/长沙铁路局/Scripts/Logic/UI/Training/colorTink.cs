using UnityEngine;
using System.Collections;

public class colorTink : MonoBehaviour {

    private float TimeLong;
    float delta;
    private bool flag;

    public delegate void OnFinished();

    /// <summary>
    /// Delegate to trigger when the spring finishes.
    /// </summary>

    public OnFinished onFinished;

	// Use this for initialization
	void Start () {
        this.transform.GetComponent<UISprite>().color = new Color(255f/255f, 255f/255f, 255f/255f, 100f/255f);
        TimeLong = 2.0f;
	}
	
	// Update is called once per frame
    void Update () 
    {
        if (flag)
        {
            if (TimeLong > delta)
            {
                delta += Time.deltaTime;
                this.transform.GetComponent<UISprite>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, (float)(delta / TimeLong));
            }
            else
            {
                //delta = 0.0f;
                this.transform.GetComponent<UISprite>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
                enabled = false;
            
            }
        }
        else
        {
            if (delta>0)
            {
                delta -= Time.deltaTime;
                this.transform.GetComponent<UISprite>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, (float)(delta / TimeLong));
            }
            else
            {
                //delta = 0.0f;
                this.transform.GetComponent<UISprite>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0f / 255f);
                enabled = false;
                if (onFinished != null)
                {
                    onFinished();
                }
            }
        }

        
      


    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    /// <param name="pos"></param>
    /// <param name="strength"></param>
    static public void Begin(GameObject go, bool flag, OnFinished onFinished = null)
    {
        colorTink sp = go.GetComponent<colorTink>();
        if (sp == null) sp = go.AddComponent<colorTink>();
        sp.onFinished = onFinished;
        if (!sp.enabled)
        {
            sp.enabled = true;
        }
        sp.flag = flag;
        if (flag)
            sp.delta = 0.0f;
        else
            sp.delta = 1.0f;
    }
}

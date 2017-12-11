using UnityEngine;
using System.Collections;

public class control_panel : MonoBehaviour {

    private int WhichButton;
    private bool isdown = false;
    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        if (isdown)
        {
            GameObject fpc = GameObject.Find("First Person Controller");
            if (fpc != null)
            {
                if (WhichButton == 1)
                {
                    fpc.transform.Translate(Vector3.forward * Time.deltaTime);
                }
                else if (WhichButton == 2)
                {
                    fpc.transform.Translate(Vector3.left * Time.deltaTime);
                }
                else if (WhichButton == 3)
                {
                    fpc.transform.Translate(Vector3.right * Time.deltaTime);
                }
                else if (WhichButton == 4)
                {
                    fpc.transform.Translate(Vector3.back * Time.deltaTime);
                }
            }
        }
    }
    public void forwardBt_down()
    {
        isdown = true;
        WhichButton = 1;
    }

    public void leftBt_down()
    {
        isdown = true;
        WhichButton = 2;
    }
    public void rightBt_down()
    {
        isdown = true;
        WhichButton = 3;
    }
    public void backBt_down()
    {
        isdown = true;
        WhichButton = 4;
    }
    public void OnRelease()
    {
        isdown = false;
        WhichButton = 0;
    }
}

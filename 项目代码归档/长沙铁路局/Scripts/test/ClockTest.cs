using UnityEngine;
using System.Collections;
using Assets.Scripts.Tools;

public class ClockTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Clock clock = new Clock(0, 0, 1, 0, 100, 0);
        ClockManager.getInstance().addClock(clock);
		
        clock.setTick(tick);


		GetComponent<UITool>().setPlayBtnClick(fall);
    }

    public void tick(Clock c, Clock.Stage s, ulong counter, float clockTime)
    {
        Debug.Log(clockTime);
    }

	public void fall()
	{

	}
}

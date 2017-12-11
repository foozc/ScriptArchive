using UnityEngine;
using System.Collections;
using Assets.Scripts.Tools;
using System;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:倒计时
*Author:作者
*
*/
public class CountDown : MonoBehaviour
{
    public UILabel textLabel;
    private Action timeEndEvent;
    private string time;
    private bool isPause = false;
    private int totalSeconds;
    private string hours;
    private string minutes;
    private string seconds;
    private Clock clock;
    private int currentSeconds;

    public void setTotalSeconds(int totalSeconds)
    {
        this.totalSeconds = totalSeconds;
        currentSeconds = totalSeconds;
        setTimeOfText();
    }
    private void setTimeOfText()
    {
        hours = (currentSeconds / 3600).ToString();
        minutes = (currentSeconds / 60 % 60).ToString();
        seconds = (currentSeconds % 60).ToString();
        if (currentSeconds / 3600 < 10)
        {
            hours = "0" + hours;
        }
        if (currentSeconds / 60 % 60 < 10)
        {
            minutes = "0" + minutes;
        }
        if (currentSeconds % 60 < 10)
        {
            seconds = "0" + seconds;
        }
        textLabel.text = hours + ":" + minutes + ":" + seconds;
    }
    public void setTimePause()
    {
        isPause = true;
    }
    public void onRestart()
    {
		onStop();
		currentSeconds = totalSeconds;
        setTimeOfText();
		this.gameObject.SetActive(true);
        startTimer();
    }

    public void setTimeGo()
    {
        isPause = false;
    }
    public void onStop()
    {
        if (clock != null)
            ClockManager.getInstance().RemoveClock(clock);
        clock = null;
    }

    public void startTimer()
    {
        clock = new Clock(0, 0, 1, 0, 100, 0);
        ClockManager.getInstance().addClock(clock);
        clock.setTick(tick);
    }
    public void tick(Clock c, Clock.Stage s, ulong counter, float clockTime)
    {
        if (currentSeconds > 0 && !isPause)
        {
            currentSeconds--;
            setTimeOfText();
        }
        else if (currentSeconds == 0)
        {
            if (timeEndEvent != null)
            {
                timeEndEvent();
				currentSeconds = -1;
			}
        }
    }
    public void setTimeEndEvent(Action timeEndEvent)
    {
        this.timeEndEvent = timeEndEvent;
    }

    public int getCurrentTime()
    {
        if (totalSeconds < currentSeconds)
            return 0;
        else
            return totalSeconds - currentSeconds;
    }
}

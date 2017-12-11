using UnityEngine;
using System.Collections;
using Assets.Scripts.Logic.Movies;
using System.Collections.Generic;
using Assets.Scripts.Logic;
using System;
using RenderHeads.Media.AVProVideo;
using Assets.Scripts.Controller;

public class VideoDisplay : MonoBehaviour
{
    public GameObject movie;
    public UISprite playBtn;
    public UISprite playBigBtn;
    public UISlider slider;

    private bool changeProgress = false;
    private bool isPlay = false;
    public void init()
    {
        movie.SetActive(true);
        playBtn.spriteName = "bofanganniu";
        playBtn.GetComponent<UIButton>().normalSprite = playBtn.spriteName;
        playBigBtn.spriteName = "shiping_bofanganniu";
        playBigBtn.GetComponent<UIButton>().normalSprite = playBigBtn.spriteName;
        slider.value = 0;
        isPlay = false;
    }

    public void playMovie(string movie)
    {
        MovieManager.getInstance().playMovie(movie);
        MovieManager.getInstance().setMovieProgressAction(updateSliderValue);
        playBtn.spriteName = "zantinganniu";
        playBtn.GetComponent<UIButton>().normalSprite = playBtn.spriteName;
        playBigBtn.spriteName = "shiping_zantinganniu";
        playBigBtn.GetComponent<UIButton>().normalSprite = playBigBtn.spriteName;
        playBigBtn.gameObject.SetActive(false);
        isPlay = true;
    }

    public void playMovie(string movie, Action<string> action)
    {
        MovieManager.getInstance().playMovie(movie,action,false);
        MovieManager.getInstance().setMovieProgressAction(updateSliderValue);
        playBtn.spriteName = "zantinganniu";
        playBtn.GetComponent<UIButton>().normalSprite = playBtn.spriteName;
        playBigBtn.spriteName = "shiping_zantinganniu";
        playBigBtn.GetComponent<UIButton>().normalSprite = playBigBtn.spriteName;
        playBigBtn.gameObject.SetActive(false);
        isPlay = true;
    }

    public void playClick()
    {
        if (isPlay)
        {
            MovieManager.getInstance().OnPause();
            playBigBtn.gameObject.SetActive(true);
            playBtn.spriteName = "bofanganniu";
            playBtn.GetComponent<UIButton>().normalSprite = playBtn.spriteName;
            playBigBtn.spriteName = "shiping_bofanganniu";
            playBigBtn.GetComponent<UIButton>().normalSprite = playBigBtn.spriteName;
        }
        else
        {
            MovieManager.getInstance().OnResume();
            playBtn.spriteName = "zantinganniu";
            playBigBtn.gameObject.SetActive(false);
            playBtn.GetComponent<UIButton>().normalSprite = playBtn.spriteName;
            playBigBtn.spriteName = "shiping_zantinganniu";
            playBigBtn.GetComponent<UIButton>().normalSprite = playBigBtn.spriteName;
        }
        isPlay = !isPlay;
    }

    public void updateSliderValue(float value)
    {
        if (!changeProgress)
            slider.value = value;
    }
    
    public void movieSliderChange(float value)
    {
        MovieManager.getInstance().OnVideoSliderDown(value);
        MovieManager.getInstance().OnVideoSliderUp();
    }

}

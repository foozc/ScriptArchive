using UnityEngine;
using System.Collections;
using Assets.Scripts.Logic.Movies;


public class VideoDisplayCharacter : MonoBehaviour {

    public GameObject uiPanel;
    public UISprite playBtn;
    public UISprite playBigBtn;
    public UISlider slider;
    public UILabel textLabel;
    public UIScrollBar textScrollBar;
    public UIScrollView textScrollView;

    private bool changeProgress = false;
    private bool isPlay = false;
    public void init()
    {
        uiPanel.SetActive(true);
        playBtn.spriteName = "bofanganniu";
        MovieManager.getInstance().setMovieProgressAction(updateSliderValue);
        playBtn.GetComponent<UIButton>().normalSprite = playBtn.spriteName;
        playBigBtn.spriteName = "shiping_bofanganniu";
        playBigBtn.GetComponent<UIButton>().normalSprite = playBigBtn.spriteName;
        slider.value = 0;
        textLabel.text = "";
        isPlay = false;
    }


    public void play(string movieName, string text)
    {
        MovieManager.getInstance().playMovie(movieName);
        text = text.Replace("\\n", "\n");
        //text = text.Replace("\u3000 (1)", "(1)");
        //text = text.Replace("\u3000 (2)", "(2)");
        //text = text.Replace("\u3000 (3)", "(3)");
        //text = text.Replace("\u3000 (4)", "(4)");
        //text = "\u3000" + text;
        textLabel.text = text;
        playBigBtn.gameObject.SetActive(false);
        playBtn.spriteName = "zantinganniu";
        playBtn.GetComponent<UIButton>().normalSprite = playBtn.spriteName;
        playBigBtn.spriteName = "shiping_zantinganniu";
        playBigBtn.GetComponent<UIButton>().normalSprite = playBigBtn.spriteName;
        isPlay = true;
        textScrollBar.value = 0;
    }

    public void playClick()
    {
        if (isPlay)
        {
            MovieManager.getInstance().OnPause();
            playBtn.spriteName = "bofanganniu";
            playBtn.GetComponent<UIButton>().normalSprite = playBtn.spriteName;
            playBigBtn.spriteName = "shiping_bofanganniu";
            playBigBtn.GetComponent<UIButton>().normalSprite = playBigBtn.spriteName;
            playBigBtn.gameObject.SetActive(true);
        }
        else
        {
            MovieManager.getInstance().OnResume();
            playBigBtn.gameObject.SetActive(false);
            playBtn.spriteName = "zantinganniu";
            playBtn.GetComponent<UIButton>().normalSprite = playBtn.spriteName;
            playBigBtn.spriteName = "shiping_zantinganniu";
            playBigBtn.GetComponent<UIButton>().normalSprite = playBigBtn.spriteName;
        }
        isPlay = !isPlay;
    }

    public void scrollBarChange()
    {
        if (textScrollView.panel.GetViewSize().y > textScrollView.bounds.size.y)
        {
            textScrollView.ResetPosition();
            textScrollBar.value = 0;
        }
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

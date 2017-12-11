using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System;
public class TabTween : MonoBehaviour
{	
    public Image[] bg = null;
    public Image[] around = null;
    public Image[] img = null;
    public Image[] image = null;
    
    public GameObject[] bimage = null;
    private Tween t, q;
    
    // Use this for initialization

    public void closeNew(GameObject game)
    {
        game.SetActive(false);
        t.Kill();
        q.Kill();
        foreach (Image item in image)
        {
            item.gameObject.SetActive(false);
            //Material lastMat = item.GetComponent<Image>().material;
            ////Tween h = lastMat.DOFade(1, 1);
            //Material m = Instantiate(lastMat) as Material;
            //item.GetComponent<Image>().material = m;

            ///init material
            //m.DOFade(1, 0);

        }
        foreach (Image item in around)
        {
            item.gameObject.SetActive(false);
            item.GetComponent<RectTransform>().localScale = new Vector3(0f, 0f, 1f);
        }
        foreach (Image item in bg)
        {
            item.gameObject.SetActive(false);
        }
        foreach (GameObject item in bimage)
        {
            item.SetActive(false);
        }
        foreach(Image item in img)
        {
            item.gameObject.SetActive(true);
        }

    }

    public void startAnimation(string index)
    {
        t.Kill();
        q.Kill();
        foreach (Image item in img)
        {
            item.gameObject.SetActive(false);
        }
        foreach (Image item in image)
        {
            item.gameObject.SetActive(false);
            //Material lastMat = item.GetComponent<Image>().material;
            ////Tween h = lastMat.DOFade(1, 1);
            //Material m = Instantiate(lastMat) as Material;
            //item.GetComponent<Image>().material = m;

            ///init material
            //m.DOFade(1, 0);

        }
        foreach (Image item in around)
        {
            item.gameObject.SetActive(false);
            item.GetComponent<RectTransform>().localScale = new Vector3(0f, 0f, 1f);
        }
        foreach (Image item in bg)
        {
            item.gameObject.SetActive(false);
        }
        foreach (GameObject item in bimage)
        {
            item.SetActive(false);
        }
        this.showTab(index);
        this.showAround(index);
    }

    private void showTab(string index)
	{
		int id = Convert.ToInt32(index);
		
        if (image[id]!=null)
		{
            img[id].gameObject.SetActive(true);
            bimage[id].SetActive(true);
            bg[id].gameObject.SetActive(true);
            //bg[id].rectTransform.DOScale(new Vector2(1.5f, 1.5f), 0f);
            bg[id].rectTransform.DOScale(new Vector2(3f, 3f), 0f);//big
            image[id].gameObject.SetActive(true);
            //image[id].rectTransform.DOScale(new Vector2(1f, 1f), 0f);//big
            Material lastMat = image[id].GetComponent<Image>().material;
            //Tween h = lastMat.DOFade(1, 1);
            Material m = Instantiate(lastMat) as Material;
			image[id].GetComponent<Image>().material = m;
            
            ///init material
            m.DOFade(1, 0);

            ///set material animation
            t = m.DOFade(0, 1);            
            t.SetLoops(-1, LoopType.Yoyo);
		}
	}
   

    private void showAround(string index)
    {
        int id = Convert.ToInt32(index);
       
        if (around[id] != null)
        {
            around[id].gameObject.SetActive(true);
            Resources.UnloadUnusedAssets();
            //q = around[id].rectTransform.DOScale(new Vector2(1.5f, 1.5f), 2f);
            q = around[id].rectTransform.DOScale(new Vector2(3f, 3f), 2f);//big
            q.SetLoops(-1);
        }

    }

	// Update is called once per frame
	void Update () {
		
	}
}

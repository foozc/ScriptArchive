using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Load;
public class glrc : MonoBehaviour
{

    private Vector2 last_p = new Vector3(-380, 0, 0);
    private Vector2 now_p = new Vector3(0, 0, 0);
    private Vector2 next_p = new Vector3(380, 0, 0);

    public Transform item_style;
    public RollStop Rs;
    private int upItem;
    private int content_id = 1;

    private TextuerPropModel TextuerName;

public List<GameObject> ListUS;

// Use this for initialization
void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (upItem != Rs.Item)
        {
            content_id = 0;

      
            setsadas();
            upItem = Rs.Item;

            updateDotPos();
            orgShow();  

        }

    }




    //public UIAtlas one;
    public void next()
    {

        if (node_content.childCount > 1) return;
        next_btn.enabled = false;
        last_btn.enabled = false;
        int old_id = content_id;
        Transform now_img = node_content.GetChild(0);
        content_id++;
        if (content_id >= TextuerName.listTextuerProp[upItem].num)
        {
            content_id = 0;
        }


        GameObject next_img = NGUITools.AddChild(node_content.gameObject, item_style.gameObject);
        next_img.SetActive(true);
        next_img.name = "next";
        TweenPosition now_tp = now_img.GetComponent<TweenPosition>();
        now_tp.from = now_p; now_tp.to = last_p;
      //  next_img.GetComponent<UISprite>().spriteName = TextuerName.listTextuerProp[upItem].texture1 + (content_id+1).ToString();

        Texture t = Resources.Load<Texture>("content/" + TextuerName.listTextuerProp[upItem].texture1 + (content_id + 1).ToString());
        next_img.GetComponent<UITexture>().mainTexture = t;

        TweenPosition next_tp = next_img.GetComponent<TweenPosition>();
        next_tp.from = next_p; next_tp.to = now_p;
        now_tp.enabled = true; now_tp.ResetToBeginning(); now_tp.PlayForward();
        next_tp.enabled = true; next_tp.ResetToBeginning(); next_tp.PlayForward();

        EventDelegate.Callback del = delegate
        {
            DotChangeColor();
            Destroy(now_img.gameObject);
            next_img.name = "now";
            next_btn.enabled = true;
            last_btn.enabled = true;
        };
        next_tp.SetOnFinished(del);

    }

    public UIButton next_btn;
    public UIButton last_btn;
    public void last()
    {

        if (node_content.childCount > 1) return;
        next_btn.enabled = false;
        last_btn.enabled = false;
        int old_id = content_id;
        Transform now_img = node_content.GetChild(0);
        content_id--;
        if (content_id < 0)
        {
            content_id = TextuerName.listTextuerProp[upItem].num - 1;
        }


        GameObject last_img = NGUITools.AddChild(node_content.gameObject, item_style.gameObject);
        last_img.SetActive(true);
        last_img.name = "last";
        TweenPosition now_tp = now_img.GetComponent<TweenPosition>();
        now_tp.from = now_p; now_tp.to = next_p;

        //last_img.GetComponent<UISprite>().spriteName = TextuerName.listTextuerProp[upItem].texture1 + (content_id+1).ToString();

        Texture t = Resources.Load<Texture>("content/" + TextuerName.listTextuerProp[upItem].texture1 + (content_id + 1).ToString());
        last_img.GetComponent<UITexture>().mainTexture = t;

        TweenPosition last_tp = last_img.GetComponent<TweenPosition>();
        last_tp.from = last_p; last_tp.to = now_p;
        now_tp.enabled = true; now_tp.ResetToBeginning(); now_tp.PlayForward();
        last_tp.enabled = true; last_tp.ResetToBeginning(); last_tp.PlayForward();

        EventDelegate.Callback del = delegate
        {
            DotChangeColor();
            Destroy(now_img.gameObject);
            last_img.name = "now";
            next_btn.enabled = true;
            last_btn.enabled = true;
        };

        last_tp.SetOnFinished(del);
    }

    public Transform node_content;
    public void orgShow()
    {
        int sum = node_content.childCount;
        if (sum != 1)
        {
            node_content.DestroyChildren();
            GameObject now_img1 = NGUITools.AddChild(node_content.gameObject, item_style.gameObject);
            now_img1.SetActive(true);
            now_img1.transform.localPosition = now_p;
            Texture t = Resources.Load<Texture>("content/" + TextuerName.listTextuerProp[upItem].texture1 + "1");
            now_img1.GetComponent<UITexture>().mainTexture = t;
            now_img1.name = "now";
            next_btn.enabled = true;
            last_btn.enabled = true;
        }
        else
        {
            Transform img = node_content.GetChild(0);
            img.gameObject.SetActive(true);
            img.localPosition = now_p;
            Texture t = Resources.Load<Texture>("content/" + TextuerName.listTextuerProp[upItem].texture1 + "1");
            img.GetComponent<UITexture>().mainTexture = t;
        }
    }
    private void DotChangeColor()
    {
        for (int i = 0; i < TextuerName.listTextuerProp[upItem].num; i++)
        {
            if (i == content_id)
            {

                ListUS[i].GetComponent<UISprite>().spriteName = "dian1";
            }
            else
            {
                ListUS[i].GetComponent<UISprite>().spriteName = "dian2";
            }
        }
    }
    private void updateDotPos()
    {
        for (int i = 0; i < TextuerName.listTextuerProp[upItem].num; i++)
        {
            if (i == 0)
            {
                ListUS[i].GetComponent<UISprite>().spriteName = "dian1";
            }
            else
            {
                ListUS[i].GetComponent<UISprite>().spriteName = "dian2";
            }
        }

    }

    void setsadas()
    {
        if(TextuerName.listTextuerProp[upItem].num ==2)
        {
            ListUS[2].SetActive(false);
            ListUS[3].SetActive(false);
        }

        if (TextuerName.listTextuerProp[upItem].num == 3)
        {
            ListUS[3].SetActive(false);
        }
    }
   public void chushihua()
    {

        TextuerName = TextuerPropModel.GetInstance();
        upItem = Rs.Item;
        content_id = 0;
        setsadas();
        orgShow();
    } 

}

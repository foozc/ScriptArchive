using Assets.Scripts.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Logic.UI.Training;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:动态列表控制
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.CommonUI
{
    public class DynamicListUI : UIbase
    {
        public DynamicLabelGrid grid;
        public UIScrollBar scrollBar;
        public UISprite topBtn;
        public UISprite bottomBtn;
        public UIScrollView scrollView;
        public UILabel titleNumber;
        public UILabel titleName;
		public UIButton showButton;
		public UISprite title;
		public UISprite numberSprite;
		public UISprite Foreground;
		public UISprite Background;

		private bool isShowDynamicList = false;
        private List<UILabel> items;
        private UILabel currentLabel;
        public override int Depth
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        void Awake()
        {
            items = new List<UILabel>();
            EventDelegate.Add(scrollBar.onChange, scrollBarChange);
        }

        public void updateTitle(string number, string name)
        {
            titleName.text = name;
            titleNumber.text = number;
        }

        public void showSpInfoUIParm(bool show)
        {
            if (show)
            {
                isShowDynamicList = true;
                SpringPosition sp = SpringPosition.Begin(gameObject, new Vector3(178f, 0f, 0f), 15f);
            }
            else
            {
                isShowDynamicList = false;
                SpringPosition sp = SpringPosition.Begin(gameObject, new Vector3(-280f, 0f, 0f), 15f);
            }
        }
        
        public void showSpInfoUI()
        {
            if (isShowDynamicList)
            {
				SpringPosition sb = SpringPosition.Begin(showButton.gameObject, new Vector3(-35, 16f, 0f), 15f);
				isShowDynamicList = false;
                SpringPosition sp = SpringPosition.Begin(gameObject, new Vector3(-280f, 0f, 0f), 15f);
            }
            else
            {
				SpringPosition sb = SpringPosition.Begin(showButton.gameObject, new Vector3(-357, 16f, 0f), 15f);
				isShowDynamicList = true;
                SpringPosition sp = SpringPosition.Begin(gameObject, new Vector3(178f, 0f, 0f), 15f);
            }
        }

        /// <summary>
        /// 添加培训下一步操作提示
        /// </summary>
        /// <param name="text"></param>
        public void addItem(string text)
        {
            GameObject labelObj = ResourceManager.getInstance().loadUISpawnPool("Prefabs/UI/MyUI/DlItem", grid.gameObject);
            labelObj.transform.localPosition = Vector3.zero;
            labelObj.name = (items.Count + 1).ToString();
            UILabel label = labelObj.GetComponent<UILabel>();
            label.text = text;
            items.Add(label);
            grid.AddChild(labelObj.transform);

            if(this.currentLabel != null)
                this.currentLabel.color = Color.black;
            this.currentLabel = label;
            this.currentLabel.color = Color.red;

            grid.repositionNow = true;
            grid.Reposition();
            updateScrollBar();
        }

        /// <summary>
        /// 更新列表中最后一条信息
        /// </summary>
        /// <param name="text"></param>
        public void updateItemLast(string text)
        {
            items[items.Count - 1].text = text;
        }

        private void updateScrollBar()
        {
            scrollView.UpdatePosition();
            scrollView.UpdateScrollbars(true);
            if (scrollView.panel.GetViewSize().y > grid.transform.localScale.y)
            {
                scrollBar.value = 0;
            }
            else
                scrollBar.value = 1;
        }
        
        public void scrollBarTop()
        {
            float size = scrollBar.barSize;
            scrollBar.value -= size / 10;
        }

        public void scrollBarBottom()
        {
            float size = scrollBar.barSize;
            scrollBar.value += size / 10;
        }

        public void scrollBarChange()
        {
            scrollView.UpdatePosition();
            //scrollView.UpdateScrollbars(true);
            if (scrollView.panel.GetViewSize().y > scrollView.bounds.size.y)
            {
                scrollBar.value = 0;
            }
            else scrollBar.value = 1;
            if(scrollBar.alpha == 1)
            {
                topBtn.alpha = 1;
                topBtn.GetComponent<BoxCollider>().enabled = true;
                bottomBtn.alpha = 1;
                bottomBtn.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                topBtn.alpha = 0;
                topBtn.GetComponent<BoxCollider>().enabled = false;
                bottomBtn.alpha = 0;
                bottomBtn.GetComponent<BoxCollider>().enabled = false;
            }
        }
        

        public void reset()
        {
			titleNumber.text = "00";
			titleName.text = null;
			clearChildren(grid.transform);
            scrollBar.value = 0;
        }
		public void resetPostion()
		{
			isShowDynamicList = true;
			showSpInfoUI();
		}
		public void initDynamicListUI(TrainUI.TrainMode trainMode)
		{
			switch (trainMode)
			{
				case TrainUI.TrainMode.Study:
					this.gameObject.SetActive(true);
					showButton.gameObject.SetActive(true);
					showButton.GetComponent<UIButton>().normalSprite = "neirongxianshikuang-xianshianniu";
					showButton.GetComponent<UIButton>().hoverSprite = "neirongxianshikuang-xianshianniu";
					title.spriteName = "buzhoutishi-xuexi-biaotikuang";
					numberSprite.spriteName = "buzhoutishi-xuexi-xulie";
					Foreground.spriteName = "buzhoutishi-xuexi-huagan";
					Background.spriteName = "buzhoutishi-xuexi-BJ";
					bottomBtn.spriteName = "buzhoutishi-xuexi-xia";
					topBtn.spriteName = "buzhoutishi-xuexi-shang";
					showButton.GetComponent<UISprite>().spriteName = "neirongxianshikuang-xianshianniu";
					break;
				case TrainUI.TrainMode.Training:
					this.gameObject.SetActive(true);
					showButton.gameObject.SetActive(true);
					showButton.GetComponent<UIButton>().normalSprite = "buzhoutishi-lianxi-showButton";
					showButton.GetComponent<UIButton>().hoverSprite = "buzhoutishi-lianxi-showButton";
					title.spriteName = "buzhoutishi-lianxi-beijng";
					numberSprite.spriteName = "buzhoutishi-lianxi-xuhaokuang";
					Foreground.spriteName = "buzhoutishi-lianxi-huagan";
					Background.spriteName = "buzhoutishi-lianxi-BJ";
					bottomBtn.spriteName = "buzhoutishi-lianxi-xia";
					topBtn.spriteName = "buzhoutishi-lianxi-shang";
					showButton.GetComponent<UISprite>().spriteName = "buzhoutishi-lianxi-showButton";
					break;
				case TrainUI.TrainMode.Exam:
					this.gameObject.SetActive(false);
					showButton.gameObject.SetActive(false);
					break;
			}
		}
		public bool getIsShow()
		{
			return isShowDynamicList;
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Logic.UI
{
    public class PageTurning:UIbase
    {
        public bool LoopPageNum = true;  //页码显示是否为循环模式，即当前页码增加到最大后自动从最小开始循环/

        private UISprite turnLeft;
        private UISprite turnRight;
        private UILabel pageLabel;

        private int curNum = 1;  //当前页码/
        private int maxNum = 1;  //最大页码/
        private Action<bool,int> rightBtnClicked;  //是否向右翻页的委托，传出bool值和当前页码整数值/


        public override int Depth
        {
            get
            {
                int depth = this.GetComponent<UIWidget>().depth;
                Debug.Log("highestDepth=" + this.pageLabel.depth + ";  lowestDepth=" + depth);
                return depth;
            }
            set
            {
                adjustDepth(value);
            }
        }

        private void adjustDepth(int lowestDepthValue)
        {
            this.GetComponent<UIWidget>().depth = lowestDepthValue;
            this.turnLeft.depth = ++lowestDepthValue;
            this.turnRight.depth = lowestDepthValue;
            this.pageLabel.depth = ++lowestDepthValue;
        }


        void Awake()
        {
            this.turnLeft = this.transform.Find("turnLeft").GetComponent<UISprite>();
            this.turnRight = this.transform.Find("turnRight").GetComponent<UISprite>();
            this.pageLabel = this.transform.Find("pageNumber").GetComponent<UILabel>();

            UIEventListener.Get(this.turnLeft.gameObject).onClick = ToLeft;
            UIEventListener.Get(this.turnRight.gameObject).onClick = ToRight;
        }


        public void init(int curPageNum,int maxPageNum,Action<bool,int> rightBtnOnClick)
        {
            if (maxPageNum < 1)
            {
                if (curPageNum >= 1)
                    maxPageNum = curPageNum;
                else
                    curPageNum = maxPageNum = 1;
            }
            else if(curPageNum<1)
            {
                curPageNum = 1;
            }
            else if(curPageNum>maxPageNum)
            {
                curPageNum = maxPageNum;
            }

            this.curNum = curPageNum;
            this.maxNum = maxPageNum;
            this.rightBtnClicked = rightBtnOnClick;
            showPageNum(this.curNum, this.maxNum);
        }

        public void resetPageNum(int curPageNum, int maxPageNum)
        {
            if (maxPageNum < 1)
            {
                if (curPageNum >= 1)
                    maxPageNum = curPageNum;
                else
                    curPageNum = maxPageNum = 1;
            }
            else if (curPageNum < 1)
            {
                curPageNum = 1;
            }
            else if (curPageNum > maxPageNum)
            {
                curPageNum = maxPageNum;
            }

            this.curNum = curPageNum;
            this.maxNum = maxPageNum;
            showPageNum(this.curNum, this.maxNum);
        }

        private void ToLeft(GameObject go)
        {
            if(this.curNum>1)
            {
                this.curNum--;
                showPageNum(this.curNum, this.maxNum);
                if (rightBtnClicked != null) rightBtnClicked(false,this.curNum);
            }
            else
            {
                if (LoopPageNum)
                {
                    this.curNum = this.maxNum;
                    showPageNum(this.curNum, this.maxNum);
                    if (rightBtnClicked != null) rightBtnClicked(false,this.curNum);
                }
            }            
        }

        private void ToRight(GameObject go)
        {
            if(this.curNum<this.maxNum)
            {
                this.curNum++;
                showPageNum(this.curNum, this.maxNum);
                if (rightBtnClicked != null) rightBtnClicked(true,this.curNum);
            }
            else
            {
                if (LoopPageNum)
                {
                    this.curNum = 1;
                    showPageNum(this.curNum, this.maxNum);
                    if (rightBtnClicked != null) rightBtnClicked(true,this.curNum);
                }
            }  
        }

        private void showPageNum(int curPageNum, int maxPageNum)
        {
            this.pageLabel.text = curPageNum.ToString() + " / " + maxPageNum.ToString();
        }
    }
}

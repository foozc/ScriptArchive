using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Logic.UI
{

    /// <summary>
    /// 生成星星
    /// </summary>
    [RequireComponent(typeof(UIWidget))]
    public class Stars:UIbase
    {
        public string brightStarName = "";
        public string grayStarName = "";
        public string brokenStarName = "";
        private int starsCount = 0;   //生成的星星总数/
        private List<UISprite> starsList = new List<UISprite>();   //生成的sprite将存储到这个list中/
        private UIWidget widget;  //当前对象的widget/

        public override int Depth
        {
            get
            {
                int depth = this.widget.depth;
                Debug.Log("highestDepth=" + (depth+1) + ";  lowestDepth=" + depth);
                return depth;
            }
            set
            {
                init().adjustDepth(value);
            }
        }

        private void adjustDepth(int lowestDepthValue)
        {
            this.widget.depth = lowestDepthValue;
            foreach (UISprite item in starsList)
            {
                item.depth = lowestDepthValue + 1;
            }
        }


        /// <summary>
        /// 私有初始化方法
        /// </summary>
        /// <returns></returns>
        private Stars init()
        {
            if (starsList.Count == 0)
            {
                this.widget = this.GetComponent<UIWidget>();
                if (this.transform.childCount > 0)
                {
                    this.starsCount = this.transform.childCount;
                    //starsList = this.transform.GetComponentsInChildren<UISprite>().ToDynList();

                    starsList = new List<UISprite>(this.transform.GetComponentsInChildren<UISprite>());
                }
            }
            return this;
        }


        /// <summary>
        /// 初始化，此初始化方法可重复执行
        /// </summary>
        /// <param name="totalStarNum">要生成的星星总数，当使用静态初始化方式时，此数等于star当前已存在的子物体数，不会执行动态生成sprite的方法</param>
        public Stars init(int totalStarNum, int brightNum, int brokenNum)
        {
            init();
            if (starsCount==0)
            {
                this.starsCount = totalStarNum;
                starsList = addSprites(totalStarNum, this.gameObject);
            }
            setStarLevel(brightNum, brokenNum);
            return this;
        }

        /// <summary>
        /// 设置星级（即设置多少亮星，多少灰星，多少碎星）
        /// </summary>
        /// <param name="brightNum">亮星数量</param>
        /// <param name="brokenNum">碎星数量</param>
        private void setStarLevel(int brightNum, int brokenNum)
        {
            for (int i = 0; i < starsCount;i++ )
            {
                if(i<brightNum)
                {
                    this.setUISprite(starsList[i], brightStarName);
                }
                else if(i<starsCount-brokenNum)
                {
                    this.setUISprite(starsList[i], grayStarName);
                }
                else
                {
                    this.setUISprite(starsList[i], brokenStarName);
                }
            }
        }


        /// <summary>
        /// 在指定父物体下面添加指定数量的sprite并进行横向排列
        /// </summary>
        /// <param name="count">要生成的sprite的数量</param>
        /// <param name="parent">挂载sprite的父物体</param>
        /// <param name="atlas">要生成的sprite所在的图集</param>
        /// <param name="spritename">要生成的sprite的名字</param>
        /// <returns></returns>
        private List<UISprite> addSprites(int count,GameObject parent)
        {
            if(count<1)count=1;

            List<UISprite> sprites = new List<UISprite>();
            UIAtlas atlas = UIDownLoad.Instance.getAtlas(grayStarName);  //通过sprite名称获取图集，有可能得到的结果为null/
            if (atlas == null) atlas = Resources.Load("test/UItest/Maps", typeof(UIAtlas)) as UIAtlas;
            for (int i = 0; i < count; i++)
            {
                UISprite sprite = NGUITools.AddSprite(parent, atlas, grayStarName);
                //sprite.MakePixelPerfect();
                sprite.width = 16;
                sprite.height = 16;
                sprite.depth = this.Depth + 1;
                sprites.Add(sprite);
            }

            UIGrid grid = this.gameObject.AddComponent<UIGrid>();
            grid.cellWidth = 17;
            grid.cellHeight =17;
            grid.pivot = UIWidget.Pivot.Center;
            grid.Reposition();

            return sprites;
        }

    }
}

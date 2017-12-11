using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:Gird排序类
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.CommonUI
{
    public class DynamicLabelGrid : UIGrid
    {
        /// <summary>
        /// 两个Label之间的间隙
        /// </summary>
        public float labelSpace;

        public Arrangement myArrangement = Arrangement.Horizontal;

        public new enum Arrangement
        {
            Horizontal,
            Vertical
        }

        protected override void ResetPosition(List<Transform> list)
        {
            mReposition = false;

            // Epic hack: Unparent all children so that we get to control the order in which they are re-added back in
            // EDIT: Turns out this does nothing.
            //for (int i = 0, imax = list.Count; i < imax; ++i)
            //	list[i].parent = null;

            int x = 0;
            int y = 0;
            int maxX = 0;
            int maxY = 0;

            float curX = 0;
            float curY = 0;

            Transform myTrans = transform;

            // Re-add the children in the same order we have them in and position them accordingly
            for (int i = 0, imax = list.Count; i < imax; ++i)
            {
                Transform t = list[i];
                UILabel label = t.GetComponent<UILabel>();
                if(label == null)
                {
                    Debug.LogError("排序内容不是UILabel子物体，无法排列");
                    return;
                }

                Vector3 pos = t.localPosition;
                pos.z = label.transform.localPosition.z;
                float depth = pos.z;

                //if (arrangement == Arrangement.CellSnap)
                //{
                //    if (cellWidth > 0) pos.x = Mathf.Round(pos.x / cellWidth) * cellWidth;
                //    if (cellHeight > 0) pos.y = Mathf.Round(pos.y / cellHeight) * cellHeight;
                //}
                //else 

                //pos = (arrangement == Arrangement.Horizontal) ?
                //    new Vector3(cellWidth * x, -cellHeight * y, depth) :
                //    new Vector3(cellWidth * y, -cellHeight * x, depth);
                if (myArrangement == Arrangement.Horizontal)
                {
                    if(label.pivot == UIWidget.Pivot.BottomLeft || label.pivot == UIWidget.Pivot.Left || label.pivot == UIWidget.Pivot.TopLeft)
                    {
                        pos.y = 0;
                        pos.x = curX;
                        curX += (label.width + labelSpace);
                    }
                    else if (label.pivot == UIWidget.Pivot.Bottom || label.pivot == UIWidget.Pivot.Top || label.pivot == UIWidget.Pivot.Center)
                    {
                        pos.y = 0;
                        pos.x = curX + label.width / 2f;
                        curX += (label.width + labelSpace);

                    }else if(label.pivot == UIWidget.Pivot.TopRight || label.pivot == UIWidget.Pivot.BottomRight || label.pivot == UIWidget.Pivot.Right)
                    {
                        pos.y = 0;
                        pos.x = curX + label.width;
                        curX += (label.width + labelSpace);
                    }


                    maxX = Mathf.Max(maxX, label.width);
                    maxY = Mathf.Max(maxY, label.height);
                }
                else
                {
                    if (label.pivot == UIWidget.Pivot.Top || label.pivot == UIWidget.Pivot.TopLeft || label.pivot == UIWidget.Pivot.TopRight)
                    {
                        pos.x = 0;
                        pos.y = curY;
                        curY -= (label.height + labelSpace);
                    }
                    else if (label.pivot == UIWidget.Pivot.Bottom || label.pivot == UIWidget.Pivot.BottomLeft || label.pivot == UIWidget.Pivot.BottomRight)
                    {
                        pos.x = 0;
                        pos.y = curY - label.height;
                        curY += (label.height+ labelSpace);

                    }
                    else if (label.pivot == UIWidget.Pivot.Center || label.pivot == UIWidget.Pivot.Right || label.pivot == UIWidget.Pivot.Left)
                    {
                        pos.x = 0;
                        pos.y = curY - label.height / 2f;
                        curY -= (label.height + labelSpace);
                    }
                }

                if (animateSmoothly && Application.isPlaying)
                {
                    SpringPosition sp = SpringPosition.Begin(t.gameObject, pos, 15f);
                    sp.updateScrollView = true;
                    sp.ignoreTimeScale = true;
                }
                else t.localPosition = pos;


                //if (++x >= maxPerLine && maxPerLine > 0)
                //{
                //    x = 0;
                //    ++y;
                //}
            }
            
            
            // Apply the origin offset
            if (pivot != UIWidget.Pivot.TopLeft)
            {
                Vector2 po = NGUIMath.GetPivotOffset(pivot);

                float fx, fy;

                if (myArrangement == Arrangement.Horizontal)
                {
                    fx = Mathf.Lerp(0f, maxX * cellWidth, po.x);
                    fy = Mathf.Lerp(-maxY * cellHeight, 0f, po.y);
                }
                else
                {
                    fx = Mathf.Lerp(0f, maxY * cellWidth, po.x);
                    fy = Mathf.Lerp(-maxX * cellHeight, 0f, po.y);
                }

                for (int i = 0; i < myTrans.childCount; ++i)
                {
                    Transform t = myTrans.GetChild(i);
                    SpringPosition sp = t.GetComponent<SpringPosition>();

                    if (sp != null)
                    {
                        sp.target.x -= fx;
                        sp.target.y -= fy;
                    }
                    else
                    {
                        Vector3 pos = t.localPosition;
                        pos.x -= fx;
                        pos.y -= fy;
                        t.localPosition = pos;
                    }
                }
            }
        
        
        }
    }
}

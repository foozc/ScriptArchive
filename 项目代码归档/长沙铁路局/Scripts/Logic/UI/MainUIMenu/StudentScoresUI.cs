using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Assets.Scripts.Controller;
using Assets.Scripts.Logic.UI.Training;
using Assets.Scripts.VO;
using Assets.Scripts.DBLogic;
using Assets.Scripts.Configs;
using PluginEvent;
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:学生分数UI
*Author:作者
*
*/
namespace Assets.Scripts.Logic.UI.MainUIMenu
{
    public class StudentScoresUI : SingletonUI<StudentScoresUI>
    {
        public TableListUI tableListUI;
        public UIGrid treeGrid;
        private int countnum = 0;
        // Use this for initialization
        void Start()
        {


        }
        // Update is called once per frame
        void Update()
        {

        }

        public void setStudentScore()
        {
            List<ListItemBase<Score>> listItems = new List<ListItemBase<Score>>();
            treeGrid.onCustomSort = treeSort;
            clearChildren(treeGrid.transform);

            ScoreDBHelper helper = new ScoreDBHelper();
            List<Score> scores = helper.getScore(MySession.Id);
            countnum = 0;
            foreach (Score score in scores)
            {
                if (countnum++ >= 4)
                    break;
                //GameObject obj = getPrefab("MyUI/ScoreItem1");

                GameObject menuItem = ResourceManager.getInstance().loadUISpawnPool("Prefabs/UI/MyUI/ScoreItem1", treeGrid.gameObject);
                //GameObject menuItem = generate(treeGrid.transform, obj, Vector3.one, Vector3.zero, Vector3.zero);
                menuItem.transform.Find("name").GetComponent<UILabel>().text = score.User.Name;
                menuItem.transform.Find("sno").GetComponent<UILabel>().text = score.User.Id.ToString();
                menuItem.transform.Find("SubOne").GetComponent<UILabel>().text = score.SubOne.ToString("0.0");
                menuItem.transform.Find("SubTwo").GetComponent<UILabel>().text = score.SubTwo.ToString("0.0");
                menuItem.transform.Find("Score").GetComponent<UILabel>().text = score.Score1.ToString("0.0");
                menuItem.transform.Find("Date").GetComponent<UILabel>().text = score.Date.ToString("yyyy-MM-dd ");
                
                menuItem.name = score.Date.ToString();
                menuItem.name = score.Date.ToString();
            }
            treeGrid.repositionNow = true;
            treeGrid.Reposition();
            //foreach (Score score in scores)
            //{
            //    ScoreListItem item = new ScoreListItem(score);
            //    listItems.Add(item);
            //}
            //tableListUI.loadAllItem<Score>(listItems);
        }

        private int treeSort(Transform item1, Transform item2)
        {
            DateTime dt1 = Convert.ToDateTime(item1.name);
            DateTime dt2 = Convert.ToDateTime(item1.name);
            if (DateTime.Compare(dt1, dt2) > 0)
                return 1;
            else return -1;
        }

        /// <summary>
        /// 获取ui预制物
        /// </summary>
        /// <returns></returns>
        private  GameObject getPrefab(string prefabName)
        {
            GameObject pre = null;
            if (!GlobalConfig.isDevilop)
            {
                pre = UIDownLoad.Instance.getUIPrefab(prefabName);
                if (pre == null) pre = Resources.Load("Prefabs/UI/" + prefabName, typeof(GameObject)) as GameObject;
            }
            else
            {
                pre = Resources.Load("Prefabs/UI/" + prefabName, typeof(GameObject)) as GameObject;
            }
            return pre;
        }

        /// <summary>
        /// 在指定父物体下面根据给定的大小、位置、旋转生成子物体
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="preObj"></param>
        /// <param name="localScale"></param>
        /// <param name="localPos"></param>
        /// <param name="localEulerAngle"></param>
        /// <returns></returns>
        private GameObject generate(Transform parentNode, GameObject preObj, Vector3 localScale, Vector3 localPos, Vector3 localEulerAngle)
        {
            GameObject obj = Instantiate(preObj) as GameObject;
            obj.transform.parent = parentNode;
            obj.transform.localScale = localScale;
            obj.transform.localPosition = localPos;
            obj.transform.localEulerAngles = localEulerAngle;
            return obj;
        }

        protected void clearChildren(Transform root)
        {
            int childrenNum = root.childCount;
            for (int i = 0; i < childrenNum; i++)
            {
                ResourceManager.getInstance().getUIPool.Despawn(root.GetChild(i).transform);
            }
        }
    }
}


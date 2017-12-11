using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Logic.UI
{
    /// <summary>
    /// 选项卡类型
    /// </summary>
    public enum TabType
    {
        TweenScale = 0,   //变尺寸/
        TweenColor,        //变颜色/
        Hide             //隐藏
    }

    /// <summary>
    /// 每个选项卡按钮对象的命名很重要，最后一位必须为数字，数字按序排列不要重复
    /// </summary>
    public class TabStrip : UIbase
    {
        public List<UIToggle> tabs = new List<UIToggle>();    //当前选项卡组中所有的UItoggle对象/
        private List<UILabel> labs = new List<UILabel>();     //当前选项卡组中所有的UItoggle对象对应的子对象中的UILabel/ 
        private UIWidget widget;
        private bool oneShot = false;   //第一个选项卡初始化时使用一次/
        public TabType activeState = TabType.TweenScale;    //选项卡激活状态使用哪种状态:0为变高，1为不变高/
        public Color defTextColor = Color.black; //默认的文字颜色/
        public Color actTextColor = Color.white;   //激活的文字颜色/
        public Vector2 tabBtnSize = new Vector2(90,30);//选项卡尺寸/
        private int defHeight = 32;      //默认的选项卡按钮高度/
        private int actHeight = 50;      //激活的选项卡按钮高度/
        private UIToggle currentActive;  //当前处于激活状态的uitoggle/

        public override int Depth
        {
            get
            {
                Debug.Log("highestDepth=" + (this.widget.depth+2) + ";  lowestDepth=" + this.widget.depth);
                return widget.depth;
            }
            set
            {
                adjustDepth(value);  //这个方法会把父物体上的widget本身的depth一起设置了/
            }
        }
        public void Start()
        {
            //setTabActive(0);
        }

        private void adjustDepth(int lowestDepthValue)
        {
            this.widget.depth = lowestDepthValue;
            for(int i=0;i<tabs.Count;i++)
            {
                tabs[i].GetComponent<UIWidget>().depth = lowestDepthValue + 1;
                if(labs.Count>0)labs[i].depth = lowestDepthValue + 2;
            }
        }
        

        #region   静态初始化，子物体须提前安排好
        /// <summary>
        /// tab静态初始化，子物体须提前安排好
        /// </summary>
        /// <param name="tabGroupID"></param>
        /// <param name="tabJustActived"></param>
        /// <returns></returns>
        public TabStrip init(int tabGroupID, Action<int> tabJustActived,int index)
        {
            this.widget = this.transform.GetComponent<UIWidget>();
            defHeight = Mathf.FloorToInt(tabBtnSize.y);         //默认高度取值/
            actHeight = Mathf.FloorToInt(tabBtnSize.y * 1.2f);  //激活高度取值/
            oneShot = true;
            UIToggle tabTogOriginal = null;  //初始第一个tab/

            //tabs = this.GetComponentsInChildren<UIToggle>().ToList();  //获取tabs列表,此方法在对象隐藏情况下无法执行/
            
            for (int i = 0; i < tabs.Count; i++)   //给每个选项卡添加值改变的监听,有没有给每个选项卡添加监听是一回事，每个选项卡被点击时执行的是不是同一个方法是另外一回事/
            {
                //设置tab的状态/
                UIToggle tabTog = tabs[i];
                //tabTog.GetComponent<UIButton>().isEnabled = true;
                tabTog.name = tabTog.name + i.ToString();
                tabTog.group = tabGroupID;
				if (i == index)
				{
					//this.defTextColor = tabTog.transform.Find("Label").GetComponent<UILabel>().color;
					tabTogOriginal = tabTog;
					tabTogOriginal.startsActive = true;  //设置第一个选项卡的起始默认状态为true/
					setTabActiveState(tabTogOriginal);   //onchange未触发前默认设置选项卡1为激活状态/
				}
				//设置tab值改变时候的监听/
				tabTog.onChange.Add(new EventDelegate(() =>
                {
                    if (oneShot)
                    {
                        setTabDefaultState(tabTogOriginal);
                        oneShot = false;
                    } 
                    if (UIToggle.current.value)
                    {
                        setTabActiveState(UIToggle.current);
						tabJustActived(int.Parse(UIToggle.current.name.Substring(UIToggle.current.name.Length - 1)));
					}
					else
                    {
                        setTabDefaultState(UIToggle.current);
                    }
                }));
            }
            return this;
        }
        #endregion


        #region  动态初始化，根据文字内容多少生成相等数量的tab
       /// <summary>
        /// tab动态初始化，根据文字内容多少生成相等数量的tab
       /// </summary>
       /// <param name="tabGroupID">tab组id</param>
       /// <param name="tabText">tab上的文字名</param>
       /// <param name="tabSprite">tab上的图片名，按顺序依次是：normal、hover、pressed、disable，list长度应该为4，不足的用"-1"补齐，至少要有normal</param>
       /// <param name="tabJustActived">tab被激活时候执行的委托</param>
       /// <returns></returns>
        public TabStrip init(int tabGroupID, List<string> tabText,List<string> tabSprite,Action<int> tabJustActived)
        {
            this.widget = this.transform.GetComponent<UIWidget>();  
            defHeight = Mathf.FloorToInt(tabBtnSize.y);         //默认高度取值/
            actHeight = Mathf.FloorToInt(tabBtnSize.y * 1.2f);  //激活高度取值/
            oneShot = true;
            UIToggle tabTogOriginal = null;  //初始第一个tab/

            if (tabs.Count > 0)  //如果tabs下面有子物体，全部清除/
            {
                clearChildren(this.transform);
                tabs.Clear();
            }

            for (int i = 0; i < tabText.Count; i++)   //给每个选项卡添加值改变的监听,有没有给每个选项卡添加监听是一回事，每个选项卡被点击时执行的是不是同一个方法是另外一回事/
            {
                GameObject tabObj = generate(this.transform, getPrefab(UIDownLoad.Prefab_TabPre_Cell), Vector3.one, Vector3.zero, Vector3.zero);
                tabObj.name = "tab" + i.ToString();  //给tab调整名字/
                //给每个tab设置sprite图片/
                setUISprite(tabObj.GetComponent<UISprite>(),tabSprite[0]);  //首先设置好图集/
                UIButton uiBtn = tabObj.GetComponent<UIButton>();
                uiBtn.normalSprite = tabSprite[0];
                if(!tabSprite[1].Equals("-1")) uiBtn.hoverSprite = tabSprite[1];
                if (!tabSprite[2].Equals("-1")) uiBtn.hoverSprite = tabSprite[2];
                if (!tabSprite[3].Equals("-1")) uiBtn.hoverSprite = tabSprite[3];
                //设置tab的状态/
                UIToggle tabTog = tabObj.GetComponent<UIToggle>();
                tabTog.GetComponent<UIWidget>().SetRect(0, 0, tabBtnSize.x, tabBtnSize.y);
                tabTog.group = tabGroupID;
                if (i == 0)
                { 
                    tabTogOriginal = tabTog;
                    tabTogOriginal.startsActive = true;  //设置第一个选项卡的起始默认状态为true/
                    setTabActiveState(tabTogOriginal);   //onchange未触发前默认设置选项卡1为激活状态/
                }
                //UILabel childLabel = tabObj.transform.Find("Label").GetComponent<UILabel>();
                //childLabel.text = tabText[i];
                //childLabel.color = defTextColor;
                //设置tab值改变时候的监听/
                tabTog.onChange.Add(new EventDelegate(() =>
                {
                    if (oneShot)
                    {
                        setTabDefaultState(tabTogOriginal);
                        oneShot = false;
                    }
                    if (UIToggle.current.value)
                    {
                        setTabActiveState(UIToggle.current);
                        tabJustActived(int.Parse(UIToggle.current.name.Substring(UIToggle.current.name.Length - 1)));
                    }
                    else
                    {
                        setTabDefaultState(UIToggle.current);
                    }
                }));

                tabs.Add(tabTog);
                //labs.Add(childLabel);
            }
            adjustDepth(widget.depth);   //调整深度值/
            alignment(tabBtnSize);  //对元素进行排列/
            return this;
        }
        #endregion


        /// <summary>
        /// 将UIgrid下的子物体排列整齐
        /// </summary>
        private void alignment(Vector2 childSize)
        {
            UIGrid grid = this.transform.GetComponent<UIGrid>();
            if (grid == null) grid = this.gameObject.AddComponent<UIGrid>();
            grid.arrangement = UIGrid.Arrangement.Horizontal;
            grid.pivot = UIWidget.Pivot.TopLeft;
            grid.cellWidth = childSize.x;
            grid.cellHeight = childSize.y;
            grid.Reposition();
        }

        /// <summary>
        /// 设置toggle组id
        /// </summary>
        /// <param name="id">要放到一组的选项卡须设置一个统一的不为0的id值</param>
        public void setTabGroupID(int id)
        {
            foreach(UIToggle t in tabs)
            {
                t.group = id;             
            }
        }

        /// <summary>
        /// 选项卡按钮的默认状态
        /// </summary>
        /// <param name="current"></param>
        private void setTabDefaultState(UIToggle current)
        {
            if (disableTabTest(current))
                return;
            UIButton btn = current.GetComponent<UIButton>();
            btn.enabled = true;
            btn.SetState(UIButtonColor.State.Normal,true);
            int index = int.Parse(current.gameObject.name.Substring(current.name.Length - 1));
            current.GetComponent<UISprite>().depth = 12 - index;
            switch (activeState)
            {
                case TabType.TweenScale:
                    TweenHeight.Begin(current.GetComponent<UIWidget>(), 0f, defHeight);   //改变选项卡高度/
                    break;
                case TabType.TweenColor:
                    //btn.transform.Find("Label").GetComponent<UILabel>().color = defTextColor;  //文字变成正常色/
                    break;
            }
        }

        private bool disableTabTest(UIToggle current)
        {
            int toggle = tabs.FindIndex(delegate (UIToggle to)
            {
                if (to == current)
                    return true;
                else return false;
            });
            if (toggle != -1 && toggle == disableTab)
                return true;
            else return false;
        }

        /// <summary>
        /// 选项卡按钮的激活状态
        /// </summary>
        /// <param name="current"></param>
        private void setTabActiveState(UIToggle current)
        {
            if (disableTabTest(current))
                return;
            UIButton btn = current.GetComponent<UIButton>();
            btn.enabled = false;
            btn.SetState(UIButtonColor.State.Pressed, true);
            current.GetComponent<UISprite>().depth = 15;
            currentActive = current;

            switch(activeState)
            {
                case TabType.TweenScale:                  
                    TweenHeight.Begin(current.GetComponent<UIWidget>(), 0f, actHeight);   //改变选项卡高度/
                    break;
                case TabType.TweenColor:
                    //btn.transform.Find("Label").GetComponent<UILabel>().color = actTextColor;  //文字变成激活色/
                    break;
            }
           
        }

        /// <summary>
        /// 设置激活状态
        /// </summary>
        /// <param name="index"></param>
        public void setTabActiveState(int index)
        {
            index = Mathf.Clamp(index, 0, tabs.Count);  //限制index的范围/
            if (index != disableTab)
            {
                if (currentActive != null)
                    setTabDefaultState(currentActive);
                else setTabDefaultState(tabs[0]);
                Debug.Log(tabs[index].name);
                setTabActiveState(tabs[index]);
            }
            tabs[index].value = true;          
        }

        private int disableTab = -1;
        public void setTabDisableState(int index)
        {
            disableTab = index;
            index = Mathf.Clamp(index, 0, tabs.Count);
            tabs[index].GetComponent<UIButton>().isEnabled = false;
        }

        /// <summary>
        /// tab索引值从0开始计数，当count大于总数时，显示所有tab标签
        /// </summary>
        /// <param name="count">表示需要显示的数量</param>
        /// <param name="state"></param>
        private void setTabActive(int count)
        {
            if (tabs.Count >= count)
            {
                for (int i = 0; i < count; i++ )
                {
                    tabs[i].gameObject.SetActive(true);
                }
            }
            for (int i = count; i < tabs.Count; i++)
            {
                tabs[i].gameObject.SetActive(false);
            }
        }

        public List<UILabel> getAllLabs()
        {
            return labs;
        }
		public void restTabStrip()
        {
            if(disableTab != -1)
                tabs[disableTab].GetComponent<UIButton>().isEnabled = true;
            disableTab = -1;
            currentActive.GetComponent<UIButton>().SetState(UIButton.State.Normal, false);
		}
    }
}

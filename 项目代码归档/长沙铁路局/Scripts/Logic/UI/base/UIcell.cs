using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Logic.UI
{
    public class UIcell<S, E> : UIbtn<S, E>
    {
        private bool isInited = false;      //是否已经初始化过/

        protected UISprite cdMask;         //产生冷却遮罩效果的对象/
        protected GameObject fxObj;        //uicell上带的特效对象/
        protected UIWidget widget;         //widget对象/
        protected UILabel propNum;         //label对象:用于显示道具数量/
        protected UISprite propIcon;       //用于显示道具图标/
        protected UISprite lockIcon;       //用于显示锁定图标/
        protected UISprite qualityIcon;    //道具品质外框,用于显示道具品质/
        protected UILabel strengthNum;     //道具强化加成数量显示，主要用于装备强化量显示/
        protected UISprite background;     //格子背景图/
        protected int mouseBtnIndex = 0;   //鼠标按键对应的序号，与unity默认的一致：0-左键单击，1-右键单击，2-中键单击/
        protected bool isMouseBtnDown = false;  //鼠标按键是否按下/
        protected bool isCursorOver = false;    //光标是否在ui元素上方/

        protected const float standardTime = 0.2f;  //两次单击之间的时间间隔小于此值被视为双击/
        protected int clickCount = 0;       //鼠标点击数量，用于区分鼠标左键的单击和双击/

        protected E evtMsg = default(E);    //事件要传递出去的参数/ 
        protected int indexInGroup = -1;   //当前cell在群组中的索引，如果在背包中，应该与其在背包中的cell的list中的下标一致/

        protected float cd_Time = 0f;           //冷却时间/
        protected bool cd_Ok = true;            //是否已冷却好/
        protected float cd_frequency = 0.02f;   //每多少秒cd遮罩发生一次变化，即变化频率/

        protected bool isLocked = false;    //当前uicell是否锁定状态/

        //protected DragDropCell cellCanDrag = null;  //当前Cell的gameobject对象的拖动功能/


        public override int Depth
        {
            get
            {
                return this.widget.depth;
            }
            set
            {
                this.widget.depth = value;
            }
        }

        public override event UIEventHandler<S, E> SingleClick;
        public override event UIEventHandler<S, E> MouseEnter;
        public override event UIEventHandler<S, E> MouseExit;
        public virtual event UIEventHandler<S, E> DoubleClick;
        public virtual event UIEventHandler<S, E> RightMouseBtnClick;

        protected virtual void init()
        {
            if (!isInited)
            {
                this.cdMask = this.transform.Find("cdMask").GetComponent<UISprite>();
                this.fxObj = this.transform.Find("particle").gameObject;
                this.widget = this.GetComponent<UIWidget>();
                this.propNum = this.transform.Find("propNum").GetComponent<UILabel>();
                this.propIcon = this.transform.Find("propIcon").GetComponent<UISprite>();
                this.lockIcon = this.transform.Find("lockIcon").GetComponent<UISprite>();
                this.qualityIcon = this.transform.Find("propQuality").GetComponent<UISprite>();
                this.strengthNum = this.transform.Find("StrengthNum").GetComponent<UILabel>();
                this.background = this.transform.Find("background").GetComponent<UISprite>();
                
                //设置粒子系统的渲染层级，保证粒子系统渲染在UI层上面/
                ParticleSystem particleSystem = this.fxObj.GetComponent<ParticleSystem>();
                particleSystem.GetComponent<Renderer>().sortingLayerName = "Particle";
                particleSystem.GetComponent<Renderer>().sortingOrder = -1;

                //对当前对象的鼠标交互事件的监听/
                UIEventListener.Get(gameObject).onHover = btnHasMouseHover;   //onHover可以引发鼠标进入、退出、点击事件/
                UIEventListener.Get(gameObject).onDrag = btnOnDrag;
                UIEventListener.Get(gameObject).onDragOut = btnOnDragOut;
                UIEventListener.Get(gameObject).onDragOver = btnOnDragOver;
                UIEventListener.Get(gameObject).onDrop = btnOnDrop;

                isInited = true;
            }
        }

        /// <summary>
        /// 给当前对象赋值，即道具图片要设为与值相对应的道具图片、锁定状态的得锁定等等……/
        /// </summary>
        /// <param name="evtArgs"></param>
        public virtual void setValue(E evtArgs = default(E))
        {
            this.evtMsg = evtArgs;
        }


        ///// <summary>
        ///// 当前对象是否可拖拽(拖拽时复制一份出来)
        ///// </summary>
        ///// <param name="canDrag"></param>
        //public void setDragAbility(bool canDrag)
        //{
        //    if (cellCanDrag == null)
        //        cellCanDrag = this.GetComponent<DragDropCell>();
        //    if (cellCanDrag != null)
        //        cellCanDrag.enabled = canDrag;
        //}


        #region  对onDrag、onDragOut、onDragOver、onDrop的监听
        private void btnOnDrag(GameObject go, Vector2 delta)
        {
            // Debug.Log(Time.time+"::::onDrag:"+go.name+";  "+delta);
        }

        private void btnOnDragOut(GameObject go)
        {
            Debug.Log(Time.time + "::::onDragOut:" + go.name);
            //有两种情况：1、鼠标移出对象边界  2、鼠标在对象范围内释放
        }

        private void btnOnDragOver(GameObject go)
        {
            Debug.Log(Time.time + "::::onDragOver:" + go.name);
            //在对象范围内按下鼠标时触发1次，不分左右键
        }

        private void btnOnDrop(GameObject go, GameObject draggedObject)
        {
            //go是被drop的物体对象，draggedObject是被拖动的物体对象
            Debug.Log(Time.time + "::::onDrop:" + go.name + ";    " + draggedObject.name);
        }
        #endregion

        #region  UIcell主要的功能方法

        /// <summary>
        /// 当前对象在群组中的位置索引,与其在list中的下标值一致
        /// </summary>
        public int IndexInGroup
        {
            get { return indexInGroup; }
            set { indexInGroup = value; }
        }

        /// <summary>
        /// 用于事件传递的参数
        /// </summary>
        public E EvtMsg
        {
            get { return evtMsg; }
            set { this.setValue(value); }
        }

        /// <summary>
        /// 隐藏或显示按钮上的特效
        /// </summary>
        public void showFX(bool show)
        {
            if (this.fxObj == null) this.fxObj = this.transform.Find("particle").gameObject;
            this.fxObj.SetActive(show);
        }

        /// <summary>
        /// 设置CD时间（用于游戏过程中技能升级时更改cd时间）
        /// </summary>
        public float CDTime
        {
            get{
                return this.cd_Time;
            }
            set{
                this.cd_Time = value;
            }          
        }

        /// <summary>
        /// 开始冷却
        /// </summary>
        public void startCD()
        {
            if (cd_Time <= 0) return;
            cd_Ok = false;
            if (cdMask == null) this.cdMask = this.transform.Find("cdMask").GetComponent<UISprite>();
            cdMask.fillAmount = 1f;
            cdMask.gameObject.SetActive(true);
            StartCoroutine(coldDown(cd_frequency / this.cd_Time));
        }

        private IEnumerator coldDown(float deltaValue)
        {
            float cdValue = 1f;
            while(cdValue - deltaValue>0)
            {
                cdValue -= deltaValue;
                cdMask.fillAmount = cdValue;
                yield return new WaitForSeconds(cd_frequency);
            }
            cdMask.fillAmount = 0f;
            cd_Ok = true;
            cdMask.gameObject.SetActive(false);
        }

        /// <summary>
        /// 冷却状态属性
        /// </summary>
        public bool CDState
        {
            get { return cd_Ok; }
            private set 
            {
                cd_Ok = value;
                if (!cd_Ok) startCD();
            }
        }

        /// <summary>
        /// 设置道具图标
        /// </summary>
        public void setPropIcon(string iconNameInAtlas)
        {
            if (this.propIcon == null) this.propIcon = this.transform.Find("propIcon").GetComponent<UISprite>();
            if (string.IsNullOrEmpty(iconNameInAtlas))
            {
                this.propIcon.gameObject.SetActive(false);
            }
            else
            {
                this.propIcon.gameObject.SetActive(true);
                setUISprite(propIcon, iconNameInAtlas);  //有真实数据可用时解除此处注释/
            }           
        }

        /// <summary>
        /// 设置道具数量,为-1时隐藏数量数据
        /// </summary>
        /// <param name="propCount"></param>
        public void setPropNum(string propCount)
        {
            if (propNum == null) propNum = this.transform.Find("propNum").GetComponent<UILabel>();
            if (!string.IsNullOrEmpty(propCount) && !propCount.Equals("-1"))
            {
                propNum.gameObject.SetActive(true);
                propNum.text = propCount;
            }
            else
            {
                propNum.text = "";
                propNum.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// UIcell当前锁定状态的属性
        /// </summary>
        public bool LockState
        {
            get{return isLocked;}
            set
            {
                if (this.lockIcon == null) this.lockIcon = this.transform.Find("lockIcon").GetComponent<UISprite>();
                lockIcon.gameObject.SetActive(value);
                isLocked = value;
            }
        }

        /// <summary>
        /// 设置格子背景图
        /// </summary>
        /// <param name="bgSpriteName"></param>
        public void setBackGround(string bgSpriteName)
        {
            if (this.background == null) this.background = this.transform.Find("background").GetComponent<UISprite>();
            setUISprite(this.background,bgSpriteName);
        }

        /// <summary>
        /// 显示道具品质,-1时，隐藏品质控件
        /// </summary>
        public void showQuality(string iconNameInAtlas)
        {
            if(this.qualityIcon==null) this.qualityIcon = this.transform.Find("propQuality").GetComponent<UISprite>();
            if (!string.IsNullOrEmpty(iconNameInAtlas) && !iconNameInAtlas.Equals("-1"))
            {
                qualityIcon.gameObject.SetActive(true);
                setUISprite(qualityIcon, iconNameInAtlas);
            }
            else
            {
                qualityIcon.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 显示强化加成值,-1时隐藏控件
        /// </summary>
        public void showStrengthNum(string strengthPlus)
        {
            if (strengthNum == null) strengthNum = this.transform.Find("StrengthNum").GetComponent<UILabel>();
            if (!string.IsNullOrEmpty(strengthPlus) && !strengthPlus.Equals("-1"))
            {
                if (!strengthNum.gameObject.activeSelf)
                    strengthNum.gameObject.SetActive(true);
                strengthNum.text = strengthPlus;
            }
            else
            {
                strengthNum.text = "";
                strengthNum.gameObject.SetActive(false);
            }
        }

        #endregion

        #region   发布鼠标左键单击、左键双击、右键单击事件，传递事件参数
        protected virtual void leftMouseBtnClicked(GameObject go)
        {
            Debug.Log("左键单击  " + Time.time);
        }

        protected virtual void leftMouseBtnDoubleClicked(GameObject go)
        {
            Debug.Log("左键双击  " + Time.time);
        }

        protected virtual void rightMouseBtnClicked(GameObject go)
        {
            Debug.Log("右键单击  " + Time.time);
        }

        protected virtual void btnHasMouseEnter(GameObject go)
        {
            Debug.Log("鼠标进入   " + Time.time);
        }

        protected virtual void btnHasMouseExit(GameObject go)
        {
            Debug.Log("鼠标退出   " + Time.time);
        }
        #endregion

        #region  判断鼠标左键单击、左键双击、右键单击的逻辑
        private void btnHasMouseHover(GameObject go, bool state)
        {
            if (state)
            {
                isCursorOver = true;

                //if (isMouseBtnDown)
                //{
                //    switch (mouseBtnIndex)
                //    {
                //        case 1:
                //            rightMouseBtnClicked(gameObject);      //右键单击/
                //            break;
                //        default:
                //            break;
                //    }
                //    isMouseBtnDown = false;
                //}
                //else
                //{
                    btnHasMouseEnter(gameObject);   //鼠标进入/
                //}
            }
            else
            {
                isCursorOver = false;
                btnHasMouseExit(gameObject);    //鼠标退出/
            }
        }

        void Update()
        {
            if (!isCursorOver) return;
            if (Input.GetMouseButtonDown(0))
            {
                isMouseBtnDown = true;
                mouseBtnIndex = 0;

                clickCount++;
                if (clickCount == 1)
                {
                    StartCoroutine(waitDoubleClick(Time.time));
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                isMouseBtnDown = true;
                mouseBtnIndex = 1;

                mouseRightClickCount++;
                if (mouseRightClickCount == 1)
                {
                    StartCoroutine(waitRigthDoubleClick(Time.time));
                }
            }
        }

        private int mouseRightClickCount = 0;
        private IEnumerator waitRigthDoubleClick(float startTime)
        {
            float endTime = startTime + standardTime;
            while (Time.time < endTime)
            {
                if (mouseRightClickCount == 2)
                {
                    //leftMouseBtnDoubleClicked(gameObject);   //右键双击/
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
            mouseRightClickCount = 0;
            if (Time.time >= endTime)
            {
                rightMouseBtnClicked(gameObject);     //右键单击/
            }
        }

        private IEnumerator waitDoubleClick(float startTime)
        {
            float endTime = startTime + standardTime;
            while (Time.time < endTime)
            {
                if (clickCount == 2)
                {
                    leftMouseBtnDoubleClicked(gameObject);   //左键双击/
                    break;  
                }                    
                yield return new WaitForEndOfFrame();
            }
            clickCount = 0;
            if (Time.time >= endTime) 
            {
                leftMouseBtnClicked(gameObject);     //左键单击/
            }
        }

        #endregion

    }
}

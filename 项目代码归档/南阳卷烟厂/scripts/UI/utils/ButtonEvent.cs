using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIBuilder
{
    /// <summary>
    /// 按钮事件组件
    /// </summary>
    public class ButtonEvent : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        public bool scaleWhenClick = true;
        public CallBack OnClickDown;      //按下回调
        public CallBack OnClickUp;        //弹起回调
        public CallBack OnClick;          //点击按钮回调  

        private bool startDrag = false;     //开始拖动
        private Transform dragPanelContentTransform;            
        private bool needCheckScrollWithDragpanel = true;      //是否需要检测与dragpanel同步滚动
        
        enum AnimationState
        {
            NONE,                       //默认态
            SCALE_OUT,                  //向外放大
            SCALE_IN                    //向内缩小
        }

        private bool isInArea = false;      //是否在区域内
        private bool isPressed = false;     //是否在点击
        private AnimationState animationState = AnimationState.NONE;


        public delegate void CallBack(GameObject gameObject);

        public void Start()
        {
            EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>();

            if (eventTrigger != null && eventTrigger.triggers != null)
            {
                EventTrigger.TriggerEvent trigger1 = new EventTrigger.TriggerEvent();
                trigger1.AddListener((eventData) => OnTouchDown());
                EventTrigger.Entry pointDownEntry = new EventTrigger.Entry() { callback = trigger1, eventID = EventTriggerType.PointerDown };

                EventTrigger.TriggerEvent trigger2 = new EventTrigger.TriggerEvent();
                trigger2.AddListener((eventData) => OnTouchUp());
                EventTrigger.Entry pointUpEntry = new EventTrigger.Entry() { callback = trigger2, eventID = EventTriggerType.PointerUp };

                //EventTrigger.TriggerEvent trigger3 = new EventTrigger.TriggerEvent();
                //trigger3.AddListener((eventData) => OnTouchClick());
                //EventTrigger.Entry pointClickEntry = new EventTrigger.Entry() { callback = trigger3, eventID = EventTriggerType.PointerClick };

                EventTrigger.TriggerEvent trigger3 = new EventTrigger.TriggerEvent();
                trigger3.AddListener((eventData) => OnTouchEnter());
                EventTrigger.Entry pointEnterEntry = new EventTrigger.Entry() { callback = trigger3, eventID = EventTriggerType.PointerEnter };

                EventTrigger.TriggerEvent trigger4 = new EventTrigger.TriggerEvent();
                trigger4.AddListener((eventData) => OnTouchExit());
                EventTrigger.Entry pointExitEntry = new EventTrigger.Entry() { callback = trigger4, eventID = EventTriggerType.PointerExit };

                eventTrigger.triggers.Add(pointDownEntry);
                eventTrigger.triggers.Add(pointUpEntry);
                //eventTrigger.triggers.Add(pointClickEntry);
                eventTrigger.triggers.Add(pointEnterEntry);
                eventTrigger.triggers.Add(pointExitEntry);
            }
        }

        private void Update()
        {
            if (animationState != AnimationState.NONE && scaleWhenClick)
            {
                Vector3 orgLocalScale = gameObject.transform.localScale;
                if (animationState == AnimationState.SCALE_OUT)
                {
                    if (orgLocalScale.x < 1.1f)                             //放大到1.2倍
                    {
                        orgLocalScale += new Vector3(0.01f, 0.01f, 0);
                        gameObject.transform.localScale = orgLocalScale;
                    }
                    else
                    {
                        animationState = AnimationState.NONE;
                    }
                }
                if (animationState == AnimationState.SCALE_IN)              //缩小到原始大小
                {
                    if (orgLocalScale.x > 1)
                    {
                        orgLocalScale -= new Vector3(0.01f, 0.01f, 0);
                        gameObject.transform.localScale = orgLocalScale;
                    }
                    else
                    {
                        animationState = AnimationState.NONE;
                    }
                }
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!IsEnable())
            {
                return;
            }

            OnTouchClick();
            //playClickEffect();

            string callbackName = gameObject.name.Substring(0, 1).ToUpper() + gameObject.name.Substring(1);
            SendMessageUpwards(callbackName+"OnClick", gameObject, SendMessageOptions.DontRequireReceiver);
        }

        private void OnTouchDown()
        {            
            if (!IsEnable())
            {
                return;
            }
            isPressed = true;
            if (isInArea)
            {
                animationState = AnimationState.SCALE_OUT;
            }
            if (OnClickDown != null)
            {
                OnClickDown(gameObject);
            }
        }

        private void OnTouchUp()
        {
            if (!IsEnable())
            {
                return;
            }
            isPressed = false;
            animationState = AnimationState.SCALE_IN;

            if (OnClickUp != null)
            {
                OnClickUp(gameObject);
            }
        }

        private void OnTouchEnter()
        {
            if (!IsEnable())
            {
                return;
            }
            isInArea = true;    
            if (isPressed)
            {
                animationState = AnimationState.SCALE_OUT;
            }
        }

        private void OnTouchExit()
        {
            if (!IsEnable())
            {
                return;
            }
            isInArea = false;
            animationState = AnimationState.SCALE_IN;       
        }

        private void OnTouchClick()
        {
            if (OnClick != null)
            {
                OnClick(gameObject);
            }
        }

        //private void playClickEffect()
        //{
        //    GameObject uiView = GameObject.Find("UIView");
        //    if (uiView)
        //    {
        //        SoundEffectControl soundEffectControl = uiView.GetComponent<SoundEffectControl>();
        //        AudioSource audio = uiView.GetComponent<AudioSource>();
        //        if (audio != null && soundEffectControl != null && soundEffectControl.buttonClickClip != null && SoundEffectControl.effectOn)
        //        {
        //            audio.PlayOneShot(soundEffectControl.buttonClickClip);
        //        }
        //    }
        //}

        private bool IsEnable()
        {
            bool enable = true;
            Button btnComponent = gameObject.GetComponent<Button>();
            Toggle toggleComponent = gameObject.GetComponent<Toggle>();
            if (btnComponent)
            {
                enable = btnComponent.interactable;
            }
            else if (toggleComponent)
            {
                enable = toggleComponent.interactable;
            }
            return enable;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            startDrag = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            startDrag = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (startDrag && needCheckScrollWithDragpanel)
            {
                if (dragPanelContentTransform == null)
                {
                    Transform parentTransform = transform.parent;
                    while (parentTransform && parentTransform.name != "content")
                    {
                        parentTransform = parentTransform.parent;
                    }
                    if(!parentTransform)        //找不到content，不属于Dragpanel中，则不需要检测与dragpanel同步滚动
                    {
                        needCheckScrollWithDragpanel = false; 
                    }
                    else
                    {
                        dragPanelContentTransform = parentTransform;
                    }
                }             
                if (dragPanelContentTransform && dragPanelContentTransform.parent)      //dragpanenl中
                {
                    if (dragPanelContentTransform.parent.name.IndexOf("_h_") > 0)      //横向滚动条
                    {
                        dragPanelContentTransform.localPosition += new Vector3(eventData.delta.x, 0, 0);
                    }
                    else if (dragPanelContentTransform.parent.name.IndexOf("_v_") > 0)      //纵向滚动条
                    {
                        dragPanelContentTransform.localPosition += new Vector3(0, eventData.delta.y, 0);
                    }
                }
            }
        }
    }

}

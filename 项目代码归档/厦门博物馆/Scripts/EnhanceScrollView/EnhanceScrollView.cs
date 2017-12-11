using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using Assets.Scripts.Load;
public class EnhanceScrollView : MonoBehaviour
{
    public enum InputSystemType
    {
        NGUIAndWorldInput, // use EnhanceScrollViewDragController.cs to get the input(keyboard and touch)
        UGUIInput,         // use UDragEnhanceView for each item to get drag event
    }

    // Input system type(NGUI or 3d world, UGUI)
    public InputSystemType inputType = InputSystemType.NGUIAndWorldInput;
    // Control the item's scale curve
    public AnimationCurve scaleCurve;
    // Control the position curve
    public AnimationCurve positionCurve;
    // Control the "depth"'s curve(In 3d version just the Z value, in 2D UI you can use the depth(NGUI))
    // NOTE:
    // 1. In NGUI set the widget's depth may cause performance problem
    // 2. If you use 3D UI just set the Item's Z position
    public AnimationCurve depthCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));
    // The start center index
    [Tooltip("The Start center index")]
    public int startCenterIndex = 0;
    // Offset width between item
    public float cellWidth = 10f;
    [Range(0.1f, 1)]
    public float cellWidthDeviant = 1f;
    private float totalHorizontalWidth = 500.0f;
    // vertical fixed position value 
    public float yFixedPositionValue = 46.0f;

    // Lerp duration
    public float lerpDuration = 0.2f;
    private float mCurrentDuration = 0.0f;
    private int mCenterIndex = 0;
    public bool enableLerpTween = true;

    // center and preCentered item
    private EnhanceItem curCenterItem;
    private EnhanceItem preCenterItem;
    // if we can change the target item
    private bool canChangeItem = true;
    private float dFactor = 0.2f;

    // originHorizontalValue Lerp to horizontalTargetValue
    private float originHorizontalValue = 0.1f;
    public float curHorizontalValue = 0.5f;



    // "depth" factor (2d widget depth or 3d Z value)
    private int depthFactor = 5;

    // Drag enhance scroll view
    [Tooltip("Camera for drag ray cast")]
    public Camera sourceCamera;
    public EnhanceScrollViewDragController dragController;

    private bool IsDemandMode = true;

    public Action<int> act;//回调函数
    public Action<int> act2;//回调函数

    private UITexture fish1;

    public RollStop RS;
    public GameObject SO;
    private TextuerPropModel TextuerName;


    //private GameObject buttonClick; //按钮

    public void EnableDrag(bool isEnabled)
    {
        if (isEnabled)
        {
            if (inputType == InputSystemType.NGUIAndWorldInput)
            {
                if (sourceCamera == null)
                {
                    Debug.LogError("## Source Camera for drag scroll view is null ##");
                    return;
                }

                if (dragController == null)
                    //dragController = gameObject.AddComponent<EnhanceScrollViewDragController>();
                    dragController.enabled = true;
                // set the camera and mask
                dragController.SetTargetCameraAndMask(sourceCamera, (1 << LayerMask.NameToLayer("UI")));
            }
        }
        else
        {
            if (dragController != null)
                dragController.enabled = false;
        }
    }

    // targets enhance item in scroll view
    public List<EnhanceItem> listEnhanceItems;
    // sort to get right index
    private List<EnhanceItem> listSortedItems = new List<EnhanceItem>();
    


    private EnhanceScrollView instance;
    public EnhanceScrollView GetInstance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;

    }

    void Start()
    {

   

        //RollStop RS = RollStop.getInstance();

        // RS.Load();
        
        //TextuerName = TextuerPropModel.GetInstance();
        //InstantiateTextuer(TextuerName.TEYP);

        //canChangeItem = true;
        //int count = listEnhanceItems.Count;
        //dFactor = (Mathf.RoundToInt((1f / count) * 10000f)) * 0.0001f;
        //mCenterIndex = count / 2;
        //if (count % 2 == 0)
        //    mCenterIndex = count / 2 - 1;
        //int index = 0;
        //for (int i = count - 1; i >= 0; i--)
        //{
        //    listEnhanceItems[i].CurveOffSetIndex = i;
        //    listEnhanceItems[i].CenterOffSet = dFactor * (mCenterIndex - index);
        //    //listEnhanceItems[i].SetSelectState(false);
        //    GameObject obj = listEnhanceItems[i].gameObject;

        //    if (inputType == InputSystemType.NGUIAndWorldInput)
        //    {
        //        DragEnhanceView script = obj.GetComponent<DragEnhanceView>();
        //        if (script != null)
        //            script.SetScrollView(this);
        //    }
        //    else
        //    {
        //        UDragEnhanceView script = obj.GetComponent<UDragEnhanceView>();
        //        if (script != null)
        //            script.SetScrollView(this);
        //    }
        //    index++;
        //}

        //// set the center item with startCenterIndex
        //if (startCenterIndex < 0 || startCenterIndex >= count)
        //{
        //    Debug.LogError("## startCenterIndex < 0 || startCenterIndex >= listEnhanceItems.Count  out of index ##");
        //    startCenterIndex = mCenterIndex;
        //}

        //// sorted items
        //listSortedItems = new List<EnhanceItem>(listEnhanceItems.ToArray());
        //totalHorizontalWidth = cellWidth * count * cellWidthDeviant;
        //curCenterItem = listEnhanceItems[startCenterIndex];
        //curHorizontalValue = 0.5f - curCenterItem.CenterOffSet;
        //LerpTweenToTarget(0f, curHorizontalValue, false);

        //// 
        //// enable the drag actions
        //// 
        //EnableDrag(true);

        //UITexture ME1 = Resources.Load<UITexture>("Prefabs/fuzi1");
        ////curCenterItem.GetComponent<UITexture>().mainTexture = Resources.Load<Texture>("Texture/centre" + curCenterItem.CurveOffSetIndex);

        //fish1 = Instantiate(ME1, new Vector3(0, -1, 0), this.transform.rotation) as UITexture;
        //fish1.mainTexture = Resources.Load<Texture>("song/" + TextuerName.listTextuerProp[curCenterItem.Number].textureSong);
        //fish1.depth = 99;
        //fish1.transform.parent = transform;
        //fish1.transform.localScale = new Vector3(1, 1, 1);
        //fish1.transform.localPosition = new Vector3(0, -1, 0);
        //setAct(RS.getCurrentTextuer);
        //setAct2(SO.GetComponent<StopOperation>().StopCallbackFunction);
        //depthFactor = listEnhanceItems.Count;

        //if (act != null)
        //{

        //    act(curCenterItem.Number);
        //}
        //if (act2 != null)
        //{
        //    act2(curCenterItem.Number);
        //}
        //SO.GetComponent<glrc>().chushihua();
    }

    private void LerpTweenToTarget(float originValue, float targetValue, bool needTween = false)
    {
        if (!needTween)
        {
            SortEnhanceItem();
            originHorizontalValue = targetValue;
            UpdateEnhanceScrollView(targetValue);
            this.OnTweenOver();
        }
        else
        {
            originHorizontalValue = originValue;
            curHorizontalValue = targetValue;
            mCurrentDuration = 0.0f;
        }
        enableLerpTween = needTween;
    }

    public void DisableLerpTween()
    {
        this.enableLerpTween = false;
    }

    /// 
    /// Update EnhanceItem state with curve fTime value
    /// 
    public void UpdateEnhanceScrollView(float fValue)
    {
        for (int i = 0; i < listEnhanceItems.Count; i++)
        {
            EnhanceItem itemScript = listEnhanceItems[i];
            float xValue = GetXPosValue(fValue, itemScript.CenterOffSet);
            float scaleValue = GetScaleValue(fValue, itemScript.CenterOffSet);
            float depthCurveValue = depthCurve.Evaluate(fValue + itemScript.CenterOffSet);
            itemScript.UpdateScrollViewItems(xValue, depthCurveValue, depthFactor, listEnhanceItems.Count, yFixedPositionValue, scaleValue);
        }

    }

    void Update()
    {
        if (enableLerpTween && IsDemandMode)
            TweenViewToTarget();

        totalHorizontalWidth = listEnhanceItems.Count * cellWidth * cellWidthDeviant;
    }

    private void TweenViewToTarget()
    {


        mCurrentDuration += Time.deltaTime;
        if (mCurrentDuration > lerpDuration)
            mCurrentDuration = lerpDuration;

        float percent = mCurrentDuration / lerpDuration;
        float value = Mathf.Lerp(originHorizontalValue, curHorizontalValue, percent);
        UpdateEnhanceScrollView(value);
        if (mCurrentDuration >= lerpDuration)
        {
            canChangeItem = true;
            enableLerpTween = false;
            OnTweenOver();
            exchangeTextuer();
            if (act != null)
            {

                act(curCenterItem.Number);
            }
            if (act2 != null)
            {
                act2(curCenterItem.Number);
            }
        }
        else
        {

        }
    }

    private void OnTweenOver()
    {
        if (preCenterItem != null)
        {
           // preCenterItem.SetSelectState(false);

        }
        if (curCenterItem != null)
        {
           // curCenterItem.SetSelectState(true);
        }
    }
    // Get the evaluate value to set item's scale
    private float GetScaleValue(float sliderValue, float added)
    {
        float scaleValue = scaleCurve.Evaluate(sliderValue + added);
        return scaleValue;
    }

    // Get the X value set the Item's position
    private float GetXPosValue(float sliderValue, float added)
    {
        float evaluateValue = positionCurve.Evaluate(sliderValue + added) * totalHorizontalWidth - totalHorizontalWidth / 2;
        return evaluateValue;
    }

    private int GetMoveCurveFactorCount(EnhanceItem preCenterItem, EnhanceItem newCenterItem)
    {
        SortEnhanceItem();
        int factorCount = Mathf.Abs(newCenterItem.RealIndex) - Mathf.Abs(preCenterItem.RealIndex);
        return Mathf.Abs(factorCount);
    }

    // sort item with X so we can know how much distance we need to move the timeLine(curve time line)
    public int SortPosition(EnhanceItem a, EnhanceItem b) { return a.transform.localPosition.x.CompareTo(b.transform.localPosition.x); }
    private void SortEnhanceItem()
    {
        listSortedItems.Sort(SortPosition);
        for (int i = listSortedItems.Count - 1; i >= 0; i--)
            listSortedItems[i].RealIndex = i;
    }

    public void SetHorizontalTargetItemIndex(EnhanceItem selectItem)
    {
        if (!canChangeItem)
            return;

        if (curCenterItem == selectItem)
        {
            exchangeTextuer();
            return;
        }

        canChangeItem = false;
        preCenterItem = curCenterItem;
        curCenterItem = selectItem;

        // calculate the direction of moving
        float centerXValue = 0;
        bool isRight = false;
        if (selectItem.transform.localPosition.x > centerXValue)
            isRight = true;

        // calculate the offset * dFactor
        int moveIndexCount = GetMoveCurveFactorCount(preCenterItem, selectItem);
        float dvalue = 0.0f;
        if (isRight)
        {
            dvalue = -dFactor * moveIndexCount;
        }
        else
        {
            dvalue = dFactor * moveIndexCount;
        }
        float originValue = curHorizontalValue;
        LerpTweenToTarget(originValue, curHorizontalValue + dvalue, true);
    }

    // Click the right button to select the next item.
    public void OnBtnRightClick()
    {
        if (!IsDemandMode)
            return;
        if (!canChangeItem)
            return;
        int targetIndex = curCenterItem.CurveOffSetIndex + 1;
        if (targetIndex > listEnhanceItems.Count - 1)
            targetIndex = 0;
        SetHorizontalTargetItemIndex(listEnhanceItems[targetIndex]);
    }

    // Click the left button the select next next item.
    public void OnBtnLeftClick()
    {
        if (!IsDemandMode)
            return;
        if (!canChangeItem)
            return;
        int targetIndex = curCenterItem.CurveOffSetIndex - 1;
        if (targetIndex < 0)
            targetIndex = listEnhanceItems.Count - 1;
        SetHorizontalTargetItemIndex(listEnhanceItems[targetIndex]);
    }

    public float factor = 0.001f;
    // On Drag Move
    public void OnDragEnhanceViewMove(Vector2 delta)
    {
        setFishActivefalse();
        if (!IsDemandMode)
            return;
        // In developing
        if (Mathf.Abs(delta.x) > 0.0f)
        {
            curHorizontalValue += delta.x * factor;
            LerpTweenToTarget(0.0f, curHorizontalValue, false);
        }
    }

    // On Drag End
    public void OnDragEnhanceViewEnd()
    {

        // find closed item to be centered
        int closestIndex = 0;
        float value = (curHorizontalValue - (int)curHorizontalValue);
        float min = float.MaxValue;
        float tmp = 0.5f * (curHorizontalValue < 0 ? -1 : 1);
        for (int i = 0; i < listEnhanceItems.Count; i++)
        {
            float dis = Mathf.Abs(Mathf.Abs(value) - Mathf.Abs((tmp - listEnhanceItems[i].CenterOffSet)));
            if (dis < min)
            {
                closestIndex = i;
                min = dis;
            }
        }
        originHorizontalValue = curHorizontalValue;
        float target = ((int)curHorizontalValue + (tmp - listEnhanceItems[closestIndex].CenterOffSet));
        preCenterItem = curCenterItem;
        curCenterItem = listEnhanceItems[closestIndex];
        LerpTweenToTarget(originHorizontalValue, target, true);
        canChangeItem = false;

    }



    public void MethodCall(int i)
    {

        switch (i)
        {
            case 1:

                break;
        }
    }

    public void RelieveMove()
    {
        IsDemandMode = true;
        OnDragEnhanceViewEnd();
    }
    public void SetIsDemandMoveFalse()
    {
        IsDemandMode = false;
    }
    public void OnClickLeftMove()
    {
        SetIsDemandMoveFalse();
        Vector2 vec2 = new Vector2(20, 0);
        EnhanceViewMove(vec2);
    }

    public void OnClickRightMove()
    {
        SetIsDemandMoveFalse();
        Vector2 vec2 = new Vector2(-20, 0);
        EnhanceViewMove(vec2);
    }
    public void EnhanceViewMove(Vector2 delta)
    {

        // In developing
        if (Mathf.Abs(delta.x) > 0.0f)
        {
            curHorizontalValue += delta.x * factor;
            LerpTweenToTarget(0.0f, curHorizontalValue, false);
        }
    }

    public void setAct(Action<int> _action)
    {
        act = _action;
    }
    public void setAct2(Action<int> _action)
    {
        act2 = _action;
    }

    private void InstantiateTextuer(String _teyp)
    {
        for (int i = 0; i < TextuerName.listTextuerProp.Count; i++)
        {
            if (TextuerName.listTextuerProp[i].TEYP == _teyp)
            {
                //获取对应纹理
                Texture tex = Resources.Load<Texture>("song/" + TextuerName.listTextuerProp[i].textureroll);
                MyNGUIEnhanceItem ME = Resources.Load<MyNGUIEnhanceItem>("Prefabs/fuzi3");

                MyNGUIEnhanceItem fish = Instantiate(ME, this.transform.position, this.transform.rotation) as MyNGUIEnhanceItem;
                //设置主纹理
                fish.GetComponent<UITexture>().mainTexture = tex;
                // fish.GetComponent<UITexture>().MakePixelPerfect();
                //设置层级
                fish.transform.parent = transform;
                fish.Set_mTrs();
                EnhanceItem go = fish.GetComponent<EnhanceItem>();

                go.Number = TextuerName.listTextuerProp[i].number -1 ;
                //添加到listEnhanceItems中
                listEnhanceItems.Add(go);
            }
        }


    }

    private void exchangeTextuer()
    {
        fish1.gameObject.SetActive(true);
        fish1.GetComponent<UITexture>().mainTexture = Resources.Load<Texture>("song/" + TextuerName.listTextuerProp[curCenterItem.Number].textureSong);

    }

    public void setFishActivefalse()
    {
        fish1.gameObject.SetActive(false);
    }

    public void initialize()
    {
       // Clear();
        listSortedItems.Clear();
        TextuerName = TextuerPropModel.GetInstance();
        InstantiateTextuer(TextuerName.TEYP);

        canChangeItem = true;
        int count = listEnhanceItems.Count;
        dFactor = (Mathf.RoundToInt((1f / count) * 10000f)) * 0.0001f;
        mCenterIndex = count / 2;
        if (count % 2 == 0)
            mCenterIndex = count / 2 - 1;
        int index = 0;
        for (int i = count - 1; i >= 0; i--)
        {
            listEnhanceItems[i].CurveOffSetIndex = i;
            listEnhanceItems[i].CenterOffSet = dFactor * (mCenterIndex - index);
            //listEnhanceItems[i].SetSelectState(false);
            GameObject obj = listEnhanceItems[i].gameObject;

            if (inputType == InputSystemType.NGUIAndWorldInput)
            {
                DragEnhanceView script = obj.GetComponent<DragEnhanceView>();
                if (script != null)
                    script.SetScrollView(this);
            }
            else
            {
                UDragEnhanceView script = obj.GetComponent<UDragEnhanceView>();
                if (script != null)
                    script.SetScrollView(this);
            }
            index++;
        }

        // set the center item with startCenterIndex
        if (startCenterIndex < 0 || startCenterIndex >= count)
        {
            Debug.LogError("## startCenterIndex < 0 || startCenterIndex >= listEnhanceItems.Count  out of index ##");
            startCenterIndex = mCenterIndex;
        }

        // sorted items
        listSortedItems = new List<EnhanceItem>(listEnhanceItems.ToArray());
        totalHorizontalWidth = cellWidth * count * cellWidthDeviant;
        curCenterItem = listEnhanceItems[startCenterIndex];
        curHorizontalValue = 0.5f - curCenterItem.CenterOffSet;
        LerpTweenToTarget(0f, curHorizontalValue, false);

        // 
        // enable the drag actions
        // 
        EnableDrag(true);

        UITexture ME1 = Resources.Load<UITexture>("Prefabs/fuzi1");
        //curCenterItem.GetComponent<UITexture>().mainTexture = Resources.Load<Texture>("Texture/centre" + curCenterItem.CurveOffSetIndex);

        fish1 = Instantiate(ME1, new Vector3(0, -1, 0), this.transform.rotation) as UITexture;
        fish1.mainTexture = Resources.Load<Texture>("song/" + TextuerName.listTextuerProp[curCenterItem.Number].textureSong);
        fish1.depth = 99;
        fish1.transform.parent = transform;
        fish1.transform.localScale = new Vector3(1, 1, 1);
        fish1.transform.localPosition = new Vector3(0, -1, 0);
        setAct(RS.getCurrentTextuer);
        setAct2(SO.GetComponent<StopOperation>().StopCallbackFunction);
        depthFactor = listEnhanceItems.Count;

        if (act != null)
        {

            act(curCenterItem.Number);
        }
        if (act2 != null)
        {
            act2(curCenterItem.Number);
        }
        SO.GetComponent<glrc>().chushihua();
    }
    public void Clear()
    {
        if (listEnhanceItems.Count != 0)
        {
            for (int i = 0; i < listEnhanceItems.Count; i++)
            {
                Destroy(listEnhanceItems[i].gameObject);
            }

            Destroy(fish1.gameObject);

            fish1 = null;
            listEnhanceItems.Clear();
        }
    }
}
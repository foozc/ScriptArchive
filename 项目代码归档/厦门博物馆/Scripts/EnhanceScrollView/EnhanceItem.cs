using UnityEngine;
using System.Collections;

public class EnhanceItem : MonoBehaviour
{
    // Start index
    private int curveOffSetIndex = 0;
    private int number = 0;
    public int CurveOffSetIndex
    {
        get { return this.curveOffSetIndex; }
        set { this.curveOffSetIndex = value; }
    }

    // Runtime real index(Be calculated in runtime)
    private int curRealIndex = 0;
    public int RealIndex
    {
        get { return this.curRealIndex; }
        set { this.curRealIndex = value; }
    }

    // Curve center offset 
    private float dCurveCenterOffset = 0.0f;
    public float CenterOffSet
    {
        get { return this.dCurveCenterOffset; }
        set { dCurveCenterOffset = value; }
    }

    public int Number
    {
        get
        {
            return number;
        }

        set
        {
            number = value;
        }
    }

    private Transform mTrs;

    void Awake()
    {
        mTrs = this.transform;
        OnAwake();
    }

    void Start()
    {
        OnStart();
    }

    // Update Item's status
    // 1. position
    // 2. scale
    // 3. "depth" is 2D or z Position in 3D to set the front and back item
    public void UpdateScrollViewItems(
        float xValue,
        float depthCurveValue,
        int depthFactor,
        float itemCount,
        float yValue,
        float scaleValue)
    {
        Vector3 targetPos = Vector3.one;
        Vector3 targetScale = Vector3.one;
        // position
        targetPos.x = xValue;
        targetPos.y = yValue;
        mTrs.localPosition = targetPos;

        // Set the "depth" of item
        // targetPos.z = depthValue;
        SetItemDepth(depthCurveValue, depthFactor, itemCount);
        // scale
        targetScale.x = targetScale.y = scaleValue;
        mTrs.localScale = targetScale;
    }

    protected virtual void OnClickEnhanceItem(MyNGUIEnhanceItem _nItem = null, MyUGUIEnhanceItem _Item = null)        
    {
        EnhanceScrollView ESV = _nItem.GetComponentInParent<EnhanceScrollView>();
        ESV.setFishActivefalse();
        ESV.SetHorizontalTargetItemIndex(this);
    }

    protected virtual void OnStart()
    {
    }

    protected virtual void OnAwake()
    {
    }

    protected virtual void SetItemDepth(float depthCurveValue, int depthFactor, float itemCount)
    {
    }

    // Set the item center state
    public virtual void SetSelectState(bool isCenter)
    {
    }

    public void Set_mTrs()
    {
        mTrs = this.transform;
        Vector3 vec = new Vector3(1,1,1);
        this.transform.localScale = vec;
    }
}

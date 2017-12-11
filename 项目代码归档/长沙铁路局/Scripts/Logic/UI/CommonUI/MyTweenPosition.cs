//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's position.
/// </summary>
/*
*版本：1.0
*Copyright 2013-2016 ，武汉飞戈数码科技有限公司
*Encoding:UTF-8
*Version:1.0
*CreateDate:yyyy-MM-dd
*Desciption:功能说明:NGUI,TweenPosition改写的类
*Author:作者
*
*/
[AddComponentMenu("NGUI/Tween/Tween Position")]
public class MyTweenPosition : UITweener
{
	private bool TextChange=false;
	private Vector3 from;
	private Vector3 to;

	[HideInInspector]
	public bool worldSpace = false;

	Transform mTrans;
	UIRect mRect;

	public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }

	[System.Obsolete("Use 'value' instead")]
	public Vector3 position { get { return this.value; } set { this.value = value; } }

	/// <summary>
	/// Tween's current value.
	/// </summary>
	public void setTextChange(bool tc)
	{
		this.TextChange = tc;
	}
	public Vector3 value
	{
		get
		{
			return worldSpace ? cachedTransform.position : cachedTransform.localPosition;
		}
		set
		{
			if (mRect == null || !mRect.isAnchored || worldSpace)
			{
				if (worldSpace) cachedTransform.position = value;
				else cachedTransform.localPosition = value;
			}
			else
			{
				value -= cachedTransform.localPosition;
				NGUIMath.MoveRect(mRect, value.x, value.y);
			}
		}
	}

	void Awake ()
	{
		mRect = GetComponent<UIRect>();
	}
	private void MathFromAndTo()
	{
		UIRoot root = GameObject.FindObjectOfType<UIRoot>();
		if (root != null)
		{
			float s = (float)root.activeHeight / Screen.height;
			int width = Mathf.CeilToInt(transform.parent.GetComponent<UIPanel>().GetViewSize().x * s);
			UILabel ub = gameObject.GetComponent<UILabel>();
            float textsize = ub.width;
			from = new Vector3(width / 2 + textsize / 2, 0, gameObject.transform.position.z);
			to = new Vector3(-width / 2 - textsize / 2, 0, gameObject.transform.position.z);
            duration = (from.x - to.x) / 50;
			TextChange = false;
		}
	}
	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished)
	{
		if (TextChange)
		{
			MathFromAndTo();
		}
		value = from * (1f - factor) + to * factor;
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenPosition Begin (GameObject go, float duration, Vector3 pos)
	{
		TweenPosition comp = UITweener.Begin<TweenPosition>(go, duration);
		comp.from = comp.value;
		comp.to = pos;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenPosition Begin (GameObject go, float duration, Vector3 pos, bool worldSpace)
	{
		TweenPosition comp = UITweener.Begin<TweenPosition>(go, duration);
		comp.worldSpace = worldSpace;
		comp.from = comp.value;
		comp.to = pos;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue () { from = value; }

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue () { to = value; }

	[ContextMenu("Assume value of 'From'")]
	void SetCurrentValueToStart () { value = from; }

	[ContextMenu("Assume value of 'To'")]
	void SetCurrentValueToEnd () { value = to; }
}

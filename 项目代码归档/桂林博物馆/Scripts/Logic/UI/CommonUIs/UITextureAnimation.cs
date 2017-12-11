using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Logic.UI.CommonUIs
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(UITexture))]
    [AddComponentMenu("NGUI/UI/Texture Animation")]
    public class UITextureAnimation : MonoBehaviour
    {
        public int startFrameIndex = 0;
        public int endFrameIndex = 0;

        [HideInInspector]
        [SerializeField]
        protected int mFPS = 30;
        [HideInInspector]
        [SerializeField]
        protected string mPrefix = "";
        [HideInInspector]
        [SerializeField]
        protected bool mLoop = true;
        [HideInInspector]
        [SerializeField]
        protected bool mSnap = true;
        [HideInInspector]
        [SerializeField]
        protected bool mActive = true;

        protected UITexture mTexture;
        protected float mDelta = 0f;
        protected List<string> mTextureNames = new List<string>();
        private int frameIndex = 0;
        private Action animationAction;

        /// <summary>
        /// Number of frames in the animation.
        /// </summary>

        public int frames { get { return mTextureNames.Count; } }

        /// <summary>
        /// Animation framerate.
        /// </summary>

        public int framesPerSecond { get { return mFPS; } set { mFPS = value; } }

        /// <summary>
        /// Set the name prefix used to filter sprites from the atlas.
        /// </summary>

        public string namePrefix { get { return mPrefix; } set { if (mPrefix != value) { mPrefix = value; RebuildSpriteList(); } } }

        /// <summary>
        /// Set the animation to be looping or not
        /// </summary>

        public bool loop { get { return mLoop; } set { mLoop = value; } }

        /// <summary>
        /// Returns is the animation is still playing or not
        /// </summary>

        public bool isPlaying { get { return mActive; } }

        /// <summary>
        /// 设置动画播放结束回调
        /// </summary>
        /// <param name="action"></param>
        public void setEndAction(Action action)
        {
            animationAction = action;
        }

        /// <summary>
        /// Rebuild the sprite list first thing.
        /// </summary>

        protected virtual void Start() { RebuildSpriteList(); }

        /// <summary>
        /// Advance the sprite animation process.
        /// </summary>

        protected virtual void Update()
        {
            if (mActive && mTextureNames.Count > 1 && Application.isPlaying && mFPS > 0)
            {
                mDelta += Mathf.Min(1f, RealTime.deltaTime);
                float rate = 1f / mFPS;

                while (rate < mDelta)
                {
                    mDelta = (rate > 0f) ? mDelta - rate : 0f;

                    if (++frameIndex >= mTextureNames.Count)
                    {
                        frameIndex = 0;
                        mActive = mLoop;
                        if (animationAction != null)
                            animationAction();
                    }

                    if (mActive)
                    {
                        Texture temp = mTexture.mainTexture;
                        mTexture.mainTexture = Resources.Load(mTextureNames[frameIndex]) as Texture;
                        if (mSnap) mTexture.MakePixelPerfect();
                        Resources.UnloadAsset(temp);
                    }
                }
            }
        }
        
        /// <summary>
        /// Rebuild the sprite list after changing the sprite name.
        /// </summary>

        public void RebuildSpriteList()
        {
            if (mTexture == null) mTexture = GetComponent<UITexture>();
            mTextureNames.Clear();

            if (mTexture != null)
            {
                for (int i = startFrameIndex; i <= endFrameIndex; ++i)
                {
                    mTextureNames.Add(mPrefix + i.ToString().PadLeft(3, '0'));
                }
                mTextureNames.Sort();
            }
        }

        /// <summary>
        /// Reset the animation to the beginning.
        /// </summary>

        public void Play() { mActive = true; }

        /// <summary>
        /// Pause the animation.
        /// </summary>

        public void Pause() { mActive = false; }

        /// <summary>
        /// Reset the animation to frame 0 and activate it.
        /// </summary>

        public void ResetToBeginning()
        {
            mActive = true;
            frameIndex = 0;

            if (mTexture != null && mTextureNames.Count > 0)
            {
                Texture temp = mTexture.mainTexture;
                mTexture.mainTexture = Resources.Load(mTextureNames[frameIndex]) as Texture;
                if (mSnap) mTexture.MakePixelPerfect();
                Resources.UnloadAsset(temp);
            }
        }
    }
}

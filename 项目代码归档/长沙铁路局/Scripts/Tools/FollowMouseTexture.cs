using UnityEngine;

/// <summary>
/// Selectable sprite that follows the mouse.
/// </summary>
namespace Assets.Scripts.Tools
{
    [RequireComponent(typeof(UITexture))]
    public class FollowMouseTexture : MonoBehaviour
    {
        static public FollowMouseTexture instance;

        // Camera used to draw this cursor
        public Camera uiCamera;

        Transform mTrans;
        //UISprite mSprite;

        //UIAtlas mAtlas;
        //string mSpriteName;
        UITexture mUITexture;
        Texture mTexture;

        /// <summary>
        /// Keep an instance reference so this class can be easily found.
        /// </summary>

        void Awake() { instance = this; }
        void OnDestroy() { instance = null; }

        /// <summary>
        /// Cache the expected components and starting values.
        /// </summary>

        void Start()
        {
            mTrans = transform;
            mUITexture = GetComponentInChildren<UITexture>();

            if (uiCamera == null)
                uiCamera = NGUITools.FindCameraForLayer(gameObject.layer);

            if (mUITexture != null)
            {
                mTexture = mUITexture.mainTexture;
                if (mUITexture.depth < 100)
                    mUITexture.depth = 100;
            }

        }

        /// <summary>
        /// Reposition the widget.
        /// </summary>

        void Update()
        {
            Vector3 pos = Input.mousePosition;

            if (uiCamera != null)
            {
                // Since the screen can be of different than expected size, we want to convert
                // mouse coordinates to view space, then convert that to world position.
                pos.x = Mathf.Clamp01(pos.x / Screen.width);
                pos.y = Mathf.Clamp01(pos.y / Screen.height);
                mTrans.position = uiCamera.ViewportToWorldPoint(pos);

                // For pixel-perfect results
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7
			if (uiCamera.isOrthoGraphic)
#else
                if (uiCamera.orthographic)
#endif
                {
                    Vector3 lp = mTrans.localPosition;
                    lp.x = Mathf.Round(lp.x);
                    lp.y = Mathf.Round(lp.y);
                    mTrans.localPosition = lp;
                }
            }
            else
            {
                // Simple calculation that assumes that the camera is of fixed size
                pos.x -= Screen.width * 0.5f;
                pos.y -= Screen.height * 0.5f;
                pos.x = Mathf.Round(pos.x);
                pos.y = Mathf.Round(pos.y);
                mTrans.localPosition = pos;
            }
        }

        /// <summary>
        /// Clear the cursor back to its original value.
        /// </summary>

        static public void Clear()
        {
            if (instance != null && instance.mUITexture != null)
                Set(instance.mTexture);
        }


        static public void Set(Texture texture)
        {
            if (instance != null && instance.mUITexture)
            {
                instance.mUITexture.mainTexture = texture;
                instance.mUITexture.MakePixelPerfect();
                instance.Update();
            }
        }
    }
}
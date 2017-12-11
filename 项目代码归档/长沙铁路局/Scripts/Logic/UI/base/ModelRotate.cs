using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Logic.UI
{
    public class ModelRotate:UIbase
    {
        public bool autoRotate = true;
        private bool autoRotateFirstValue;  //初始化时autorotate的值/
        private bool mouseOnModel = false;  //鼠标在模型上/
        private GameObject modelParentGO;
        private GameObject turnLeftBtn;
        private GameObject turnRightBtn;
        private float anglePerSecond = 3f;
        private Bounds mBounds;   //模型的边界框/
        private UIWidget widget; 


        public override int Depth
        {
            get
            {
                Debug.Log("highestDepth=" + (this.widget.depth + 1) + ";  lowestDepth=" + this.widget.depth);
                return widget.depth;
            }
            set
            {
                adjustDepth(value);  //这个方法会把父物体上的widget本身的depth一起设置了/
            }
        }

        private void adjustDepth(int lowestDepthValue)
        {
            this.widget.depth = lowestDepthValue;
            this.turnLeftBtn.GetComponent<UIWidget>().depth = ++lowestDepthValue;
            this.turnRightBtn.GetComponent<UIWidget>().depth = lowestDepthValue;
        }


        void Awake()
        {
            autoRotateFirstValue = autoRotate;
            this.widget = this.GetComponent<UIWidget>();
            this.modelParentGO = this.transform.Find("Model").gameObject;
            this.turnLeftBtn = this.transform.Find("TurnLeft").gameObject;
            this.turnRightBtn = this.transform.Find("TurnRight").gameObject;
            UIEventListener.Get(modelParentGO).onDrag = modelOnDrag;
            UIEventListener.Get(modelParentGO).onHover = modelOnHover;
            UIEventListener.Get(turnLeftBtn).onPress = leftBtnPressed;
            UIEventListener.Get(turnRightBtn).onPress = rightBtnPressed;
        }


        private void modelOnDrag(GameObject go, Vector2 delta)
        {
            if (mouseOnModel)
                modelParentGO.transform.Rotate(Vector3.up, -delta.x, Space.World);
        }

        private void modelOnHover(GameObject go, bool state)
        {
            if(state)
            {
                if (autoRotate) autoRotate = false;
                mouseOnModel = true;
            }
            else
            {
                if (autoRotateFirstValue) autoRotate = true;
                mouseOnModel = false;
            }
        }

        private void leftBtnPressed(GameObject go, bool state)
        {
            if(state)
            {
                if (autoRotate) autoRotate = false;
                StartCoroutine(rotate(-anglePerSecond));
            }
            else
            {
                StopAllCoroutines();
                if (autoRotateFirstValue) autoRotate = true;
            }
        }

        private void rightBtnPressed(GameObject go, bool state)
        {
            if (state)
            {
                if (autoRotate) autoRotate = false;
                StartCoroutine(rotate(anglePerSecond));
            }
            else
            {
                StopAllCoroutines();
                if (autoRotateFirstValue) autoRotate = true;
            }
        }


        private IEnumerator rotate(float angle)
        {
            while(true)
            {
                modelParentGO.transform.Rotate(Vector3.up, angle, Space.World);
                yield return null;
            }
        }


        void Update()
        {
            if(autoRotate)
            {
                modelParentGO.transform.Rotate(Vector3.up, anglePerSecond, Space.World);
            }
        }



        public void addModel(GameObject model)
        {

            if (modelParentGO.transform.childCount > 0)
                clearChildren(modelParentGO.transform);

            /************ 调尺寸缩放 *************
            this.mBounds = getModelBound(model);
            if (string.IsNullOrEmpty(mBounds.ToString()))
            {
                Debug.LogError("Could not find any bounds in model", this);
                return;
            }

            Vector3 modelContainerSize = this.modelParentGO.collider.bounds.size;
            float scaleX = modelContainerSize.x/mBounds.size.x;
            float scaleY = modelContainerSize.y/mBounds.size.y;
            float scaleZ = modelContainerSize.z/mBounds.size.z;
            float scale = Mathf.Max(scaleX, scaleY, scaleZ);
            model.transform.localScale = new Vector3(scale,scale,scale);
            //************ 调尺寸缩放 *************/

            model.transform.parent = this.modelParentGO.transform;
            model.transform.localPosition = Vector3.zero;
            model.transform.localEulerAngles = Vector3.zero;
        }

        
        /// <summary>
        /// 获取模型的边界
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Bounds getModelBound(GameObject model)
        {
            Bounds aLocalBounds = new Bounds();
            //Renderer[] aListRenderers = UnityEngine.Object.FindObjectsOfType(typeof(Renderer)) as Renderer[];
            Renderer[] aListRenderers = model.GetComponentsInChildren<Renderer>();
            if (aListRenderers != null)
            {
                foreach (Renderer aRenderer in aListRenderers)
                {
                    if (string.IsNullOrEmpty(aLocalBounds.ToString()))
                        aLocalBounds = aRenderer.bounds;
                    else
                    {
                        Bounds encapBounds = aLocalBounds;
                        encapBounds.Encapsulate(aRenderer.bounds);
                        aLocalBounds = encapBounds;
                    }
                }
            }

            return aLocalBounds;
        }

    }
}

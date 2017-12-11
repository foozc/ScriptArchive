using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Logic.UI
{
    public class PosFollow:MonoBehaviour
    {
        public Transform traceTarget;           //要跟踪的目标/
        public bool tracePosition = true;       //是否跟踪position/
        public bool traceRotation = true;       //是否跟踪rotation/
        public bool traceScale = true;          //是否跟踪scale/

        private Vector3 targetPreviousPos = Vector3.zero;      //目标的前一个位置/
        private bool matchPosition = false;     //是否开始匹配position/
        private Vector3 posDifference = Vector3.zero;   //当前对象与目标对象position的初始差值，之后都用这个值去修正位置/

        private Vector3 targetPreviousRot = Vector3.zero;  //目标的前一个旋转角度/
        private bool matchRotation = false;    //是否开始匹配rotation/
        private Vector3 rotDifference = Vector3.zero;   //当前对象与目标对象rotation的初始差值，之后都用这个值去修正旋转/

        private Vector3 targetPreviousScl = Vector3.zero;      //目标的前一个缩放/
        private bool matchScale = false;     //是否开始匹配缩放/
        private Vector3 sclDifference = Vector3.zero;   //当前对象与目标对象scale的初始差值，之后都用这个值去修正缩放/
        



        void Awake()
        {
            if (tracePosition)
            {
                targetPreviousPos = traceTarget.transform.position;
                posDifference = traceTarget.transform.position - this.transform.position;
            }
            if (traceRotation)
            {
                targetPreviousRot = traceTarget.transform.localEulerAngles;
                rotDifference = traceTarget.transform.localEulerAngles - this.transform.localEulerAngles;
            }
            if (traceScale)
            {
                targetPreviousScl = traceTarget.transform.localScale;
                Vector3 targetScale = targetPreviousScl;
                Vector3 selfScale = this.transform.localScale;
                sclDifference = new Vector3(targetScale.x/selfScale.x,targetScale.y/selfScale.y,targetScale.z/selfScale.z);
            }
        }


        void OnEnable()
        {
            if (tracePosition)
            {
                matchPosition = true;
                targetPreviousPos = traceTarget.transform.position;
                this.transform.position = traceTarget.transform.position - posDifference;
            }
            if (traceRotation)
            {
                matchRotation = true;
                targetPreviousRot = traceTarget.transform.localEulerAngles;
                this.transform.localEulerAngles = traceTarget.transform.localEulerAngles - rotDifference;
            }
            if (traceScale)
            {
                matchScale = true;
                targetPreviousScl = traceTarget.transform.localScale;
                Vector3 targetScale = targetPreviousScl;
                this.transform.localScale = new Vector3(targetScale.x/sclDifference.x,targetScale.y/sclDifference.y,targetScale.z/sclDifference.z);
            }
        }


        void OnDisable()
        {
            if (tracePosition)
                matchPosition = false;
            if (traceRotation)
                matchRotation = false;
            if (traceScale)
                matchScale = false;
        }



        void Update()
        {
            if (tracePosition && matchPosition && Vector3.Distance(traceTarget.transform.localPosition, targetPreviousPos) > 0)
            {
                Vector3 deltaPos = traceTarget.transform.position - targetPreviousPos;
                targetPreviousPos = traceTarget.transform.position;
                this.transform.position += deltaPos;
            }
            if (traceRotation && matchRotation && Vector3.Distance(traceTarget.transform.localEulerAngles, targetPreviousRot) > 0)
            {
                Vector3 deltaRot = traceTarget.transform.localEulerAngles - targetPreviousRot;
                targetPreviousRot = traceTarget.transform.localEulerAngles;
                this.transform.localEulerAngles += deltaRot;
            }
            if (traceScale && matchScale && Vector3.Distance(traceTarget.transform.localScale, targetPreviousScl) > 0)   //distance只是用来判断两个值是否相等/
            {
                Vector3 targetScale = traceTarget.transform.localScale;
                this.transform.localScale = new Vector3(targetScale.x / sclDifference.x, targetScale.y / sclDifference.y, targetScale.z / sclDifference.z);
                targetPreviousScl = targetScale;
            }
        }
    }
}

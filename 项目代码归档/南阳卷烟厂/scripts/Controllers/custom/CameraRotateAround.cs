/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	CameraRotateAround 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年11月3日14:17:33# 
 *Description: 		   	相机围绕物体旋转聚焦功能
 *History: 				修改版本记录
*/

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraRotateAround : MonoBehaviour
{

    public Transform target;                //主相机要围绕其旋转的物体  

    public float distance = 7.0f;           //主相机与目标物体之间的距离  

    private float eulerAngles_x;            //主相机的欧拉角x

    private float eulerAngles_y;            //主相机的欧拉角y

    //水平滚动相关  
    public int distanceMax = 10;            //主相机与目标物体之间的最大距离  

    public int distanceMin = 3;             //主相机与目标物体之间的最小距离  

    public float xSpeed = 30.0f;            //主相机水平方向旋转速度  

    //垂直滚动相关  
    public int yMaxLimit = 90;              //最大y（单位是角度）  

    public int yMinLimit = 45;             //最小y（单位是角度）  

    public float ySpeed = 30.0f;            //主相机纵向旋转速度  

    public float defVerticalAngle = 70f;    //默认垂直角度

    //滚轮相关  
    public float scrollSensitivity = 1.0f;  //鼠标滚轮灵敏度（备注：鼠标滚轮滚动后将调整相机与目标物体之间的间隔）  

    public LayerMask CollisionLayerMask;

    // Use this for initialization  
    void Start()
    {

    }

    /// <summary>
    /// 镜头聚焦物体
    /// </summary>
    /// <param name="target">目标物体</param>
    /// <param name="directionAngle">水平方向的视角角度</param>
    public void FocusTarget(Transform target, float directionAngle = 0)
    {
        this.target = target;
        //根据设置，初始化镜头角度
        this.transform.eulerAngles = new Vector3(defVerticalAngle, directionAngle, this.transform.eulerAngles.z);
        Vector3 eulerAngles = this.transform.eulerAngles;//当前物体的欧拉角  
        this.eulerAngles_x = eulerAngles.y;
        this.eulerAngles_y = eulerAngles.x;

        //根据距离和角度，初始化镜头位置
        this.distance = Mathf.Clamp(this.distance, (float)this.distanceMin, (float)this.distanceMax);
        Apply();
    }

    // Update is called once per frame  
    void LateUpdate()
    {
        if (target)
        {
            //右键控制角度
            if (Input.GetMouseButton(1))
            {
                this.eulerAngles_x += ((Input.GetAxis("Mouse X") * this.xSpeed) * this.distance) * 0.02f;

                this.eulerAngles_y -= (Input.GetAxis("Mouse Y") * this.ySpeed) * 0.02f;

                this.eulerAngles_y = ClampAngle(this.eulerAngles_y, (float)this.yMinLimit, (float)this.yMaxLimit);
            }
            //滚轮控制距离
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                this.distance = Mathf.Clamp(this.distance - (Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity),
                           (float)this.distanceMin, (float)this.distanceMax);
            }
            Apply();
        }
    }

    private void Apply()
    {
        Quaternion quaternion = Quaternion.Euler(this.eulerAngles_y, this.eulerAngles_x, (float)0);

        //从目标物体处，到当前脚本所依附的对象（主相机）发射一个射线，如果中间有物体阻隔，则更改this.distance（这样做的目的是为了不被挡住）  
        RaycastHit hitInfo = new RaycastHit();

        if (Physics.Linecast(this.target.position, this.transform.position, out hitInfo, this.CollisionLayerMask))
        {
            this.distance = hitInfo.distance - 0.05f;
        }
        Vector3 vector = quaternion * new Vector3((float)0, (float)0, -this.distance) + this.target.position;

        //更改主相机的旋转角度和位置  
        this.transform.rotation = quaternion;

        this.transform.position = vector;
    }

    //把角度限制到给定范围内  

    public float ClampAngle(float angle, float min, float max)
    {
        while (angle < -360)
        {
            angle += 360;
        }

        while (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }
}

/** 
 *Copyright(C) 2017 by 	Orient Information Technology Co.,Ltd
 *All rights reserved. 
 *FileName:     	   	FPSController 
 *Author:       	   	#yulong# 
 *Date:         	   	#2017年10月10日18:07:33# 
 *Description: 		   	第一人称控制器：控制人物和视角的移动、处理移动时和UI的相互关系
 *History: 				修改版本记录
*/

using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class FPSController : MonoBehaviour
{

    private CharacterMotor motor;

    // Use this for initialization
    void Start()
    {
        motor = GetComponent<CharacterMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the input vector from keyboard or analog stick
        var directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (directionVector != Vector3.zero)
        {
            if (Dialog._instance.ObjectNameUI) { Dialog._instance.ObjectNameUI.SetActive(false); }
            // Get the length of the directon vector and then normalize it
            // Dividing by the length is cheaper than normalizing when we already have the length anyway
            var directionLength = directionVector.magnitude;
            directionVector = directionVector / directionLength;

            // Make sure the length is no bigger than 1
            directionLength = Mathf.Min(1, directionLength);

            // Make the input vector more sensitive towards the extremes and less sensitive in the middle
            // This makes it easier to control slow speeds when using analog sticks
            directionLength = directionLength * directionLength;

            // Multiply the normalized direction vector by the modified length
            directionVector = directionVector * directionLength;
        }

        // Apply the direction to the CharacterMotor
        motor.inputMoveDirection = transform.rotation * directionVector;
        motor.inputJump = Input.GetButton("Jump");
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

public class NewBehaviourScript : MonoBehaviour 
{

    void Start () 
	{
        int[] arr = new int[] {1,2,23,3,3 };
        for (int i = 0; i < arr.Length; i++)
        {

        }
	}
	
	void OnGUI()
    {
        if (GUILayout.Button("fawefawef"))
        {
            ProcessStartInfo Info = new ProcessStartInfo("E:/ͼƬ/499625221354837257.jpg");
            Process Pro = Process.Start(Info);
            
        }
    }

}

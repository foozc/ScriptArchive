using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class CameraPlayback : MonoBehaviour
{
		public GameObject player;
		public ArrayList list = new ArrayList ();
		GameObject originObj = new GameObject();
		public string fileName = "a.txt";
		string txtInfo = "";
		// Use this for initialization
		void Start ()
		{
			originObj = null;
		}

		bool isStart = false;
		bool isPlay = false;
		int step = 0;

		void OnGUI ()
		{
				if (GUI.Button (new Rect (200, 200, 200, 50), "Record")) {
//						ClearObj ();
//						list.Clear ();
						isStart = true;
						isPlay = false;
				}
				if (GUI.Button (new Rect (200, 400, 200, 50), "Stop")) {
					step = 0;
					isStart = false;
					if(originObj != null)
					{
						player.transform.position = originObj.transform.position;
						player.transform.rotation = originObj.transform.rotation;
						originObj = null;
						CreateOrOPenFile(Application.streamingAssetsPath+"/Records/",fileName,txtInfo);
						ClearObj ();
						list.Clear ();
					}
					
				}
				if (GUI.Button (new Rect (200, 300, 200, 50), "RePlay")) {
						step = 0;
						isStart = false;
						isPlay = true;
				}
		}


		void ClearObj ()
		{
				foreach (GameObject obj in list) {
						Destroy (obj);
				}
		}
		// Update is called once per frame
		void Update ()
		{
				if (isStart) {
						
						GameObject obj = new GameObject ();
						obj.transform.position = player.transform.position;
						obj.transform.rotation = player.transform.rotation;
						if(originObj == null)
						{
							originObj  = obj;
						}	
						txtInfo += obj.transform.position.x + "," +obj.transform.position.y + "," + obj.transform.position.z + "," +obj.transform.rotation.x + "," +obj.transform.rotation.y + "," + obj.transform.rotation.z + ","+ "," + obj.transform.rotation.w + ";";
						list.Add (obj);
				}
				if (isPlay) {
						if (list != null && list.Count > step) {
								player.transform.position = (list [step] as GameObject).transform.position;    
								player.transform.rotation = (list [step] as GameObject).transform.rotation;  
						}else{
						
							isPlay = false;
						}
				}
				step++;
		}
			void CreateOrOPenFile(string path, string name, string info)
			{
				
				StreamWriter sw;
				
				if (!File.Exists(name))
				{
					sw = File.CreateText(path + name);
					sw.WriteLine(info);
					sw.Close();
					sw.Dispose();
				}
				else
				{
					//sw = fi.AppendText();
				}
				
			}

}

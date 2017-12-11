using UnityEngine;
using System.Collections;

public class Idle : MonoBehaviour {

	public bool idling = true;
	public int rand = 0;
	
	public void Awake () 
	{
	
	}
		
	public void Update()
	{
		if (idling)
		{
			if (!GetComponent<Animation>().isPlaying)
			{
				rand  = Random.Range(0,10);
				if (0 <= rand && rand < 1)  
				{
					GetComponent<Animation>().Play("scratch");	
				}
				else if (1 <= rand && rand < 2)  
				{
					GetComponent<Animation>().Play("neckcrack");	
				}
				else if (2 <= rand && rand < 3)  
				{
					GetComponent<Animation>().Play("crossarms");	
				}
				else 
				{
					GetComponent<Animation>().Play("idle1");
				} 
			}
		}
	}
}

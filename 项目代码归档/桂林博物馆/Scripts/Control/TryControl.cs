using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryControl : MonoBehaviour {

    
    public TableControl tableControl;
    private float start, end;
    private bool IAnimation = false;

    private int a = 0;
    

    public void test1()
    {
        start = Input.mousePosition.x;
    }
    public void test2()
    {
        
        end = Input.mousePosition.x;
        
        
        IAnimation = true;
        
    }

    //public void Update()
    //{
       // tableControl.animationControlMouse(tableControl.dressCube, start, end);
       //xRotation = 30 *(in - out);
     //   if (IAnimation && (a <100))
      //  {
      //      tableControl.animationlast(tableControl.dressCube, -30 );
      //      a++;
//}
     //   else if(a>=100)
      //  {
      //      tableControl.animationlast(tableControl.dressCube,0);
      //      IAnimation = false;
       //     a = 0;

        //}
    //}
}

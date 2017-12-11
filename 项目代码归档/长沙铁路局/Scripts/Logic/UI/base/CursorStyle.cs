using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Logic.UI
{
    public enum StyleType
    { 
        Def = 0,    //默认/
        Drag        //拖拽/
    }

    public class CursorStyle
    {
        //指针纹理的Texture Type需要设置为Cursor。/
        //如果CursorMode为CursorMode.Auto的话，指针纹理的尺寸需控制在32*32。/

        public static void setStyle(StyleType cursorType)
        {
            Texture2D cursorTexture = getCursorTexture(cursorType);
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }

        public static void setStyle(StyleType cursorType,Vector2 hotPoint, CursorMode mode)
        {
            Texture2D cursorTexture = getCursorTexture(cursorType);
            Cursor.SetCursor(cursorTexture, hotPoint, mode);  
        }

        private static Texture2D getCursorTexture(StyleType cursorType)
        {
            Texture2D t2d = null;
            switch(cursorType)
            {
                case StyleType.Def:
                    t2d = null;
                    break;
                case StyleType.Drag:
                    t2d = Resources.Load("test/UItest/cursorIconTest", typeof(Texture2D)) as Texture2D;
                    break;
            }           
            return t2d;
        }
    }
}

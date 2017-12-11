using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Assets.Scripts.Logic.UI
{
    public class NumericUpDown : MonoBehaviour
    {
        public UIInput numInput;

        private int minNum = 1;
        private int maxNum = 999;

        public void Init(int minNum, int maxNum, int defaultValue)
        {
            this.minNum = minNum;
            this.maxNum = maxNum;
            numInput.value = defaultValue.ToString();
            EventDelegate.Add(numInput.onChange, onChange);
        }
        public void Init(int minNum, int maxNum)
        {
            Init(minNum, maxNum, 1);
        }

        public void leftClick()
        {
            int defaultVlaue = Int32.Parse(numInput.value);
            if(defaultVlaue > minNum)
                numInput.value = (Int32.Parse(numInput.value) - 1).ToString();
            else numInput.value = minNum.ToString();
        }

        public void rightClick()
        {
            int defaultVlaue = Int32.Parse(numInput.value);
            if (defaultVlaue < maxNum)
                numInput.value = (Int32.Parse(numInput.value) + 1).ToString();
            else numInput.value = maxNum.ToString();
        }

        public void numericMax()
        {
            numInput.value = maxNum.ToString();
        }

        public int getValue()
        {
            return Int32.Parse(numInput.value);
        }

        public void onChange()
        {
            if(numInput.value.Equals(""))
            {
                numInput.value = "1";
                return;
            }
            if (Int32.Parse(numInput.value) > maxNum)
                numInput.value = maxNum.ToString();
            if (Int32.Parse(numInput.value) < minNum)
                numInput.value = minNum.ToString();
        }

    }
}

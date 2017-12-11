using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Event
{
    public class TaskIdentity
    {
        private int id;


        /// <summary>
        /// 任务id
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private bool isValid = true;


        /// <summary>
        /// 是否是非法任务
        /// </summary>
        public bool IsValid
        {
            get { return isValid; }
            set { isValid = value; }
        }
    }
}

using UnityEngine;
using System.Collections;
namespace Assets.Scripts.VO
{
	public class StuDate
	{
		private int sumNumber;
		private float sumExamTime;
		private float sumGrade;
		private float sumUseTime;
		private int term;
		public int SumNumber
		{
			get
			{
				return sumNumber;
			}

			set
			{
				sumNumber = value;
			}
		}
		public float SumExamTime
		{
			get
			{
				return sumExamTime;
			}

			set
			{
				sumExamTime = value;
			}
		}
		public int Term
		{
			get
			{
				return term;
			}

			set
			{
				term = value;
			}
		}
		public float SumGrade
		{
			get
			{
				return sumGrade;
			}

			set
			{
				sumGrade = value;
			}
		}
		public float SumUseTime
		{
			get
			{
				return sumUseTime;
			}

			set
			{
				sumUseTime = value;
			}
		}
	}
}

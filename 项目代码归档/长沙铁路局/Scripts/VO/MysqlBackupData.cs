using UnityEngine;
using System.Collections;
namespace Assets.Scripts.VO
{
	public class MysqlBackupData
	{
		private int id;
		private string url;
		private string backupTime;
		public int Id
		{
			get { return id; }
			set { id = value; }
		}
		public string Url
		{
			get { return url; }
			set { url = value; }
		}
		public string BackupTime
		{
			get { return backupTime; }
			set { backupTime = value; }
		}
	}
}

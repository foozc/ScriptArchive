using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Tools
{

    /// <summary>
    /// 绘图辅助工具
    /// </summary>
    public class GizmosDraw
    {

        public static void drawCircle(Transform m_Transform, float m_Theta, Color m_Color, float m_Radius)
        {
            if (m_Transform == null) return;
            if (m_Theta < 0.0001f) m_Theta = 0.0001f;

            // 设置矩阵
            Matrix4x4 defaultMatrix = Gizmos.matrix;
            Gizmos.matrix = m_Transform.localToWorldMatrix;

            // 设置颜色
            Color defaultColor = Gizmos.color;
            Gizmos.color = m_Color;

            // 绘制圆环
            Vector3 beginPoint = Vector3.zero;
            Vector3 firstPoint = Vector3.zero;
            for (float theta = 0; theta < 2 * Mathf.PI; theta += m_Theta)
            {
                float x = m_Radius * Mathf.Cos(theta);
                float z = m_Radius * Mathf.Sin(theta);
                Vector3 endPoint = new Vector3(x, 0, z);
                if (theta == 0)
                {
                    firstPoint = endPoint;
                }
                else
                {
                    Gizmos.DrawLine(beginPoint, endPoint);
                }
                beginPoint = endPoint;
            }

            // 绘制最后一条线段
            Gizmos.DrawLine(firstPoint, beginPoint);

            // 恢复默认颜色
            Gizmos.color = defaultColor;

            // 恢复默认矩阵
            //Gizmos.matrix = defaultMatrix;


        }

		public static GameObject drawSector(float radius, float angle, Color color, float step = 1f)
		{
			GameObject sector = new GameObject("Sector");        
			MeshFilter meshFilter = sector.AddComponent<MeshFilter>();       
			meshFilter.mesh = CreateSectorMesh(radius, angle, step);       
			MeshRenderer renderer = sector.AddComponent<MeshRenderer>();        
			renderer.material.shader = Shader.Find ("Custom/SingleColor");        
			renderer.material.color = color;        
			return sector;
		}

		public static Mesh CreateSectorMesh(float radius, float angle, float step = 1f)        {                
			Mesh m = new Mesh();                
			m.name = "Sector Mesh";                 
			angle = Mathf.Abs(angle);                
			if(angle < step) step = angle;                                 
			List<Vector3> vertices = new List<Vector3>();                
			List<int> triangles = new List<int>();                 
			Vector3 center = Vector3.zero;                
			vertices.Add(center);                 
			float an = -angle/2;                
			Quaternion euler;                
			Vector3 p;                
			while(an <= angle/2)                
			{                       
				euler = Quaternion.Euler(0f, an, 0f);                        
				p = center + radius*(euler*Vector3.forward);                        
				vertices.Add(p);                        
				an += step;                
			}                
			m.vertices = vertices.ToArray();                 
			int i = 1;                
			while(i < vertices.Count - 1)                
			{                        
				triangles.Add(0);                        
				triangles.Add(i);                        
				triangles.Add(i + 1);                        
				i++;                
			}                /*                
			 m.uv = new Vector2[] 
			 {                        
			 new Vector2 (0, 0),                        
			 new Vector2 (0, 1),                        
			 new Vector2(1, 1),                        
			 new Vector2 (1, 0)                };
			 */                 
			m.triangles = triangles.ToArray();                
			m.RecalculateNormals();                
			return m;        
		}
    }

}

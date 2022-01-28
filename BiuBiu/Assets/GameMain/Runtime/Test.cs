//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BiuBiu
{
	public class Test : MonoBehaviour
	{
		private Material material;

		private void Awake()
		{
			
		}

		private void Update()
		{
			// DrawScreenRect(new Rect(Vector2.zero, Vector2.one), Color.blue);
		}

		private void OnPostRender()
		{
			DrawScreenRect(new Rect(Vector2.zero, Vector2.one), Color.blue);
		}

		public void DrawScreenRect(Rect rect, Color color)
		{
			Begin (color);
			Debug.Log("DrawScreenRect");

			// GL.Vertex3 (10f, rect.yMin / Screen.height, 0);
			// GL.Vertex3 (10f, rect.yMax / Screen.height, 0);
			//
			// GL.Vertex3 (10f, rect.yMax / Screen.height, 0);
			// GL.Vertex3 (20f, rect.yMax / Screen.height, 0);
			//
			// GL.Vertex3 (20f, rect.yMax / Screen.height, 0);
			// GL.Vertex3 (20f, rect.yMin / Screen.height, 0);
			//
			// GL.Vertex3 (20f, rect.yMin / Screen.height, 0);
			// GL.Vertex3 (10f, rect.yMin / Screen.height, 0);

			GL.Vertex3 (10f, 20f, 0);
			GL.Vertex3 (10f, rect.yMax / Screen.height, 0);

			GL.Vertex3 (10f, rect.yMax / Screen.height, 0);
			GL.Vertex3 (20f, rect.yMax / Screen.height, 0);

			GL.Vertex3 (20f, rect.yMax / Screen.height, 0);
			GL.Vertex3 (20f, rect.yMin / Screen.height, 0);

			GL.Vertex3 (20f, rect.yMin / Screen.height, 0);
			GL.Vertex3 (10f, rect.yMin / Screen.height, 0);
			
			GL.End();
		}
		
		private void Begin(Color color)
		{
			if (material == null)
			{
				material = new Material (Shader.Find ("Unlit/Color"));
			}
			material.SetPass(0);
			GL.LoadOrtho();
			GL.Begin (GL.LINES);
			material.SetColor ("_Color", color);
		}
	}
}
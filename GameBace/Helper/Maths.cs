using System;
using Sce.Pss.Core;
using Sce.Pss.Core.Input;
using System.Collections.Generic;

namespace PSVX.Base
{
	/// <summary>
	/// 各种 静态 数学方法
	/// </summary>
	public class Maths
	{
		/// <summary>
		/// 全局唯一随机数
		/// </summary>
		public static Random Rnd = new Random();
		
		/// <summary>
        ///  判断两向量是否约等
        /// </summary>
        public static bool VectorsEquals(Vector2 v1, Vector2 v2,float deviation)
        {
            bool ise = false;
            if (v1.X - v2.X < deviation && v1.X - v2.X > -deviation &&
            v1.Y - v2.Y < deviation && v1.Y - v2.Y > -deviation)
                ise = true;

            return ise;
        }
		
		/// <summary>
		/// 标准化
		/// </summary>
		public static void Normalize(ref Vector2 v2)
		{
			if(v2 != Vector2.Zero)
			{
				v2 = v2.Normalize();
			}
		}
		
		/// <summary>
		/// 求正
		/// </summary>
		public static float Abs( float value)
		{
			if(value < 0)
				value = -value;
			
			return value;
		}
		
		/// <summary>
		/// 随机方向向量
		/// </summary>
		public static Vector2 RndVector()
		{
			Vector2 v2 = new Vector2(Rnd.Next(-100,100),Rnd.Next(-100,100));
			Normalize(ref v2);
			return v2;
		}
		
		public static float RndFloat()
		{
			return (float)(Maths.Rnd.Next(-100,100)* 0.01f);
		}
		
		/// <summary>
		/// 触摸模拟摇杆，异常的耗费CPU..PSV尽量不用！
		/// </summary>
		public static Vector2 TouchToAnalog ()
		{
			float x = 0, y = 0;
			
			//List<TouchData> touchDataList = Touch.GetData (0); 
			List<TouchData> touchDataList = TouchManager.Instance.TouchDatas; 
			
			foreach (TouchData touch in touchDataList) {
				if (touch.Status == TouchStatus.Move) {
					x = touch.X;
					y = touch.Y;
				}
				else
				{return Vector2.Zero;}
			}
			Vector2 v2 = new Vector2 (x, y);
			Maths.Normalize(ref v2);
			return v2;
		}
	}
}


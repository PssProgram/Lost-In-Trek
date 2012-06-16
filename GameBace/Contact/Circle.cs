using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System.Collections.Generic;

namespace PSVX.Base
{
	/// <summary>
	/// 圆形碰撞检测
	/// </summary>
	public class Circle
	{
		Vector2 center = Vector2.Zero;
		public Vector2 Center {
			get {
				return this.center;
			}
		}

		float radius = 0;
		public float Radius {
			get {
				return this.radius;
			}
			set {
				radius = value;
			}
		} 	
		
		public Circle (Vector2 center,float r)
		{
			this.center = center;
			this.radius = r;
		}
		
		public static bool IsContacted(Circle c1, Circle c2)
		{
			bool isc = false;
			float dis = Vector2.DistanceSquared(c1.Center,c2.Center) ;
			float rs = (c1.Radius+c2.Radius)*(c1.Radius+c2.Radius);
			
			if(dis < rs)
				isc = true;
			 
			return isc;
		}
	}
}


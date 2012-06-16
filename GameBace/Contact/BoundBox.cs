using System;
using Sce.Pss.Core;

namespace PSVX.Base
{
	/// <summary>
	/// 包围盒
	/// 默认是AABB
	/// </summary>
	public class BoundBox
	{
		public float Height;
		public float Width;
		public float X;
		public float Y;
		
		public OBB Obb
		{get; private set;}
		
		public Circle  Circle
		{get; private set;}
		//是否使用圆形检测，当有一方使用时，双方就使用圆形检测。
		public bool IsUseCircle=false;
		
		public BoundBox(float x,float y,float width,float height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
			Obb = null;
			Circle = new Circle(new Vector2(X,y),((width+height)*0.5F)*0.5F);
		}
		
		public BoundBox(float x,float y,float width,float height,bool Isc)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
			Obb = null;
			this.IsUseCircle = Isc;
			Circle = new Circle(new Vector2(X,y),((width+height)*0.5F)*0.5F);
		}
		
        /// <summary>
        /// 构造OBB，OBB必须比AABB小！
        /// </summary>
        public void StructureOBB(Vector2 dir, float width, float height)
        {
            Obb = new OBB(new Vector2(X,Y), dir, width, height);
        }
		
		/// <summary>
		/// 修改自动生成的圆形框的半径
		/// </summary>
		public void StructureCircle(float radius)
		{
			Circle.Radius = radius;
		}
		
		/// <summary>
		/// AABB
		/// </summary>
		public static bool IsContacted(BoundBox aabb1,BoundBox aabb2)
		{
			float x1 = (aabb1.X*2 + aabb1.Width)/2;
			float x2 = (aabb2.X*2 + aabb2.Width)/2;
			float y1 = (aabb1.Y*2 + aabb1.Height)/2;
			float y2 = (aabb2.Y*2 + aabb2.Height)/2;
			
			if(Math.Abs(x2-x1) <= (aabb1.Width + aabb2.Width)/2 && Math.Abs(y2-y1) 
			   <= (aabb1.Height + aabb2.Height)/2)
				return true;
			else
				return false;
		}
			   
		private float Multiply(Vector2 p1,Vector2 p2,Vector2 p0)
		{
			return ((p1.X - p0.X) * (p2.Y - p0.Y) - (p2.X - p0.X) * (p1.Y - p0.Y));
		}
	}
}


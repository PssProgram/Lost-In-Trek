using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PSVX.Base
{
	/// <summary>
	/// 使一个精灵成为追踪者
	/// </summary>
	public class BTracker
	{
		SpriteX  sprite;
		
	 	public	static readonly	Vector2 NoTrackPos = new Vector2(-9999,-9999);
		
		Vector2 currentTarget = NoTrackPos;
		public Vector2 CurrentTarget {
			get {
				return this.currentTarget;
			}
		}

		Vector2 newTarget = NoTrackPos;
		
		public float Speed=0;
		//旋转
		//psv 0.02f
		public float MaxRotSpeed = 0.02f;
		public float RotSpeed = 0.02f;
		//psv 0.001f
		public float RotSpeedInc = 0.0002f;
		
		//是否在追踪中。
		public bool IsTracking
		{get;private set;}
		
		//是否追到
		public bool IsReached
		{get;private set;}
		
		public BTracker (SpriteX  sprite,ref float moveSpeed,float RotSpeed,
		               float RotSpeedInc )
		{
			currentTarget = sprite.GetPosition();
			newTarget = sprite.GetPosition();
			
			Speed = moveSpeed ;
			this.sprite =sprite;
			IsTracking = false;
			IsReached  = false;
			this.MaxRotSpeed = RotSpeed;
			this.RotSpeed = RotSpeed;
			this.RotSpeedInc = RotSpeedInc;
		}
		
		public void Update()
		{
			
			 //追踪完成，是否有新的目标
			if (IsTracking == false) {
				if (Maths.VectorsEquals (currentTarget, newTarget, 20f)) {//新目标和原来的重复就不追踪了
				} else {
					IsTracking = true;
					currentTarget = newTarget;
					IsReached = false;
				}
			}
			if (currentTarget != NoTrackPos) {//追踪中
				if (IsTracking == true)
					TrackNow (currentTarget);
			}
		}
		
		public void SetTarger(Vector2 pos)
		{
			newTarget = pos;
		}
		
		public void ChangeCurrentTarget(Vector2 pos)
		{
			currentTarget = pos;
		}
		
		void TrackNow (Vector2 newPos) 
		{
			Vector2 nowPos = sprite.GetPosition();
			
			if (Maths.VectorsEquals(nowPos, currentTarget,10f)) 
			{//到目标位置 就停止
				IsTracking = false;
				IsReached = true;
			}
			
			Vector2 targetDir = currentTarget - nowPos;
			Maths.Normalize(ref targetDir);
			
			//用叉乘来确定两个向量的位置关系
            float cross = - sprite .NowDirection.X * targetDir.Y -
               -sprite.NowDirection.Y * targetDir.X;
			
			//选择旋转方向
			if(Maths.VectorsEquals(targetDir,sprite.NowDirection,0.16f))
			{
				cross =0;
				RotSpeed =MaxRotSpeed;
			}
			else
			{
				#region 旋转……
				if (cross >= 0)
                {
					RotSpeed = Maths.Abs(RotSpeed);
					RotSpeed +=RotSpeedInc;
				}
                if (cross < 0)
                {
					 RotSpeed = -Maths.Abs( RotSpeed);
					 RotSpeed -= RotSpeedInc;
                }
				#endregion
				sprite.Rotate(RotSpeed);
			}
			
			sprite.SetPosition(sprite.GetPosition() +
			                sprite.NowDirection*Speed);
		}
	}
}


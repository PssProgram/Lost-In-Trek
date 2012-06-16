using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System.Collections.Generic;

namespace PSVX.Battles
{
	/// <summary>
	/// 宇宙舰基类
	/// </summary>
	public class BWarship :LiveObject
	{
		static  Vector2 allWarshipScale = new Vector2 (0.65f, 0.65f);
		
		SpriteX body;
		public SpriteX Body {
			get {
				return this.body;
			}
			set {
				body = value;
			}
		}

		public float Speed =1f;
		//是否在移动中
		public bool IsMoveNow = false;
		
		//旋转
		public float MaxRotSpeed = 0.01f;
		public float RotSpeed = 0.01f;
		public float RotSpeedInc = 0.000001f;
		
		public Vector2 NowDirection 
		{ get{ return body.NowDirection; } }
		
		/// <summary>
		/// 当前的角度
		/// </summary>
		public float NowAngle 
		{ 
			get
			{
				return Vector2.Angle(NowDirection,new Vector2(-1,0));
			}
		}
		
		//追踪行为
		public BTracker TrackAction 
		{ get; set; }		
		
		//武器
		BWeaponSystem weapon;
		public BWeaponSystem Weapon {
			get {
				return this.weapon;
			}
			set {
				weapon = value;
			}
		}	
		//瞄准方向
		public Vector2 AimDir;
		//当前锁定的目标
		public BWarship LockShip;
		
		//可锁定的敌方列表
		public List<BWarship> CanLockEnemys=new List<BWarship>();
		
		//颜色表示，测试用
		public static Vector4 ColorID = new Vector4(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
		
		/// <summary>
		/// 构造一个宇宙舰基对象,不会碰撞的
		/// </summary>
		public BWarship (Node node,Vector2 pos,string path):
			base(node,1,1)
		{
			body = new SpriteX(path,pos);
			body.ChangeFather(this);
			body.CenterSprite(TRS.Local.Center);
			
				Speed=8f;
				MaxRotSpeed = 0.02f;
				RotSpeed =MaxRotSpeed;
				RotSpeedInc = 0.002f;
			this.Body.Scale = allWarshipScale;
		}
		
		/// <summary>
		/// 构造一个宇宙舰基对象，会碰撞的
		/// </summary>
		public BWarship (Node node,Vector2 pos,float width,float height,
		                 ContactType ctype,ContactDeal td,ContactDeal bd,string path):
			base(node,0,0,pos,width,height,ctype,td,bd)
		{
			body = new SpriteX(this,path,pos);
			body.CenterSprite(TRS.Local.Center);	
			HP = 100;
			
				Speed=8f;
				MaxRotSpeed = 0.02f;
				RotSpeed =MaxRotSpeed;
				RotSpeedInc = 0.002f;
			this.Body.Scale = allWarshipScale;
		}
		
		public override void UpdateFrame (float dt)
		{
			if(TrackAction != null)
			{
				TrackAction.Update();
			}
			IsMoveNow = false;
			base.UpdateFrame (dt);
		}
		
		public Vector2 GetPosition()
		{
			return body.GetPosition();
		}
		
		public void SetPosition(Vector2 pos)
		{
			body.SetPosition(pos);
		}
		
		//战舰只能按自己的面对方向前进。
		public void MoveShip()
		{
			IsMoveNow = true;
			this.SetPosition(this.GetPosition() +
			                 NowDirection*Speed);
		}
		
		public void RotateShip()
		{
			body.Rotate(RotSpeed);
		}
		
		//碰撞
		public override BoundBox GetBoundBox ()
		{
			base.Center = this.GetPosition();
			//BoundBox bb = new BoundBox(Center.X,Center.Y,Width,Heigth);
			//
			//bb.StructureOBB(body.NowDirection,25,40);
			//bb.StructureCircle(40f);
			//
			//base.drawBoundBox.SetBoundBoxInfor(bb);
			
			return base.GetBoundBox();
		}
		
		public override void ToContact (LiveObject ContactObj)
		{
			DebugScene.Instance.WriteLine("a12s","Spaceship Contact!");
			base.ToContact (ContactObj);
		}
		
		public override void BeContact (LiveObject ContactObj)
		{
			DebugScene.Instance.WriteLine("a12s","Spaceship Contact!");
			if(ContactObj is BBullet)
			{
				this.HP -=((BBullet)ContactObj).Att;
			}
			if(ContactObj is BMissile)
			{
				this.HP -=((BMissile)ContactObj).Att;
			}
			if(ContactObj is BBomb)
			{
				this.HP -=((BBomb)ContactObj).Att;
			}
			
			base.BeContact (ContactObj);
		}
	}
}


using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PSVX.Battles
{
	/// <summary>
	/// 不分敌我的炸弹，接触就爆炸
	/// </summary>
	public class BBomb:LiveObject
	{
		//攻击力
		public float Att = 1f;
		//发射方向
		public Vector2 Dir;
		public float Speed;
		//速度阻尼，用于递减速度；
		float speedDamp = 0.06f; 
		//爆炸时间
		float startBomb = 0;
		//是否爆炸
		bool isBomb=false;
		//爆炸持续时间
		float bombKeepTime = 1.5f;
		//自转速度
		float rotation = 0.06f;
		//刚抛出时的 不会检测时间
		public float NoContactTime = 0f;
		
		//本体
		SpriteX body;

		public SpriteX Body {
			get {
				return this.body;
			}
			set {
				body = value;
			}
		}
		
		//60fps!
		Ticker ticker60Fps = new Ticker(0.01599f);
		
		public BBomb (Node node, Vector2 pos, Vector2 Dir, float Speed,
		                float w, float h, WeapRanType wrt,float startBomb,  float NoContactTime,ContactType ctype, float att, string path):
			base(node,0,0,pos,w,h,ctype,null,null)
		{
			this.Att =att;
	 
			this.NoContactTime = NoContactTime;
			this. Speed = Speed;
			this.Dir = Dir;
			body =new SpriteX(path,pos);
			body.ChangeFather(this);
			body.CenterSprite (TRS.Local.Center);
			
			//
			body.Scale = new Vector2 (12f, 12f);
			body.Color = new Vector4(220/255f,82/255f,98/255f,255f/255f);
			//base.drawBoundBox = new DrawBoundBox(this);
		}
		
		public override void UpdateFrame (float dt)
		{
			if (ticker60Fps.isTime (dt)) 
			{
				if (Speed > 0)
				{
					Speed -= speedDamp;
					if (Speed < 0)
						Speed = 0;
				}
				//自转
				Body.Rotate (rotation);
				body.SetPosition (body.GetPosition () + Dir * Speed);
			}
			
			startBomb-=dt;
			if(startBomb <0)
			{
				startBomb =0;
				isBomb = true;
			}
			//
			if(isBomb == true)
			{
				bombKeepTime -= dt;
				//持续时间结束就死亡
				if(bombKeepTime <0)
					HP = 0;
			}
			//
			if(NoContactTime > 0)
				NoContactTime -=dt;
			
			
			base.UpdateFrame (dt);
		}
		
		public override void ToContact (LiveObject ContactObj)
		{
			Speed = 0;
			isBomb = true;
			rotation =0.44f;
			base.ToContact (ContactObj);
		}
		
		public override void BeContact (LiveObject ContactObj)
		{
			Speed = 0;
			rotation =0.44f;
			isBomb = true;
			base.BeContact (ContactObj);
		}
		
		public override BoundBox GetBoundBox ()
		{
			if(NoContactTime >0)
				return null;
			
			BoundBox bb = null;
			base.Center = body.GetPosition ();
			//爆炸后检测范围扩大
			if (isBomb == true) 
			{
				bb = new BoundBox(Center.X,Center.Y,Width*6f,Heigth*6f);
			}
			else
			{
				bb = base.GetBoundBox ();
			}
			bb.IsUseCircle = true;
			//base.drawBoundBox.SetBoundBoxInfor(bb);
			
			return bb;
		}
	}
}


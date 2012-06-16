using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PSVX.Battles
{
	/// <summary>
	/// 各种带追踪的导弹的基类
	/// </summary>
	public class BMissile :LiveObject
	{
		//攻击力
	 	public float Att = 1f;
		public float Speed;
		BWarship target;
		
		//射程
		float maxShootDis = 0;
		float nowShootDis = 0;
		
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
		
		BTracker  tracker;
		
		//60fps!
		Ticker ticker60Fps = new Ticker(0.01599f);
		
		public BMissile (Node node,Vector2 pos,BWarship target,float Speed,float rots,float rotsInc,
		                float w, float h, WeapRanType wrt, ContactType ctype,float att, string path):
			base(node,0,0,pos,w,h,ctype,null,null)
		{
			this.Att =att;
			this. Speed = Speed;
			this.target = target;
	 
			body = new SpriteX(path,pos);
			body.ChangeFather(this);
			body.CenterSprite(TRS.Local.Center);
			body.Scale = new Vector2(0.4f,0.4f);
			
			tracker =new BTracker(body,ref this.Speed,rots,rotsInc);
			tracker.SetTarger(target.GetPosition());
			
			body.Color = BWarship.ColorID;
			maxShootDis = BWeapon.GetRangeByType(wrt);
		}
		
		public override void UpdateFrame (float dt)
		{
			if(ticker60Fps.isTime(dt))
			{
				tracker.Update();
				tracker.ChangeCurrentTarget(target.GetPosition());
				
				nowShootDis +=tracker.Speed;
				if(nowShootDis > maxShootDis)
					HP=0;
			}	
			
			base.UpdateFrame (dt);
		}
		
		public override void ToContact (LiveObject ContactObj)
		{
			this.HP =0;
			base.ToContact (ContactObj);
		}
		
		public override void BeContact (LiveObject ContactObj)
		{
			this.HP =0;
			base.BeContact (ContactObj);
		}
		
		public override BoundBox GetBoundBox ()
		{
			base.Center = body.GetPosition();
			BoundBox bb =base.GetBoundBox ();
			//base.drawBoundBox.SetBoundBoxInfor(bb);
			
			return bb;
		}
	}
}


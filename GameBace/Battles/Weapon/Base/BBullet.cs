using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PSVX.Battles
{	
	/// <summary>
	/// 各种子弹，炮弹的基类,用于显示,和检测 
	/// </summary>
	public class BBullet : LiveObject
	{
		//攻击力
	 	public float Att = 1f;
		//发射方向
		public Vector2 Dir;
		public float Speed;
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
		
		//60fps!
		Ticker ticker60Fps = new Ticker(0.01599f);
		
		//一次发生是否完成
		public bool IsComplete = true;
		
		public BBullet (Node node,Vector2 pos,Vector2 Dir,float Speed,
		                float w, float h,WeapRanType wrt, ContactType ctype,float att, string path):
			base(node,0,0,pos,w,h,ctype,null,null)
		{
			this.Att =att;
			this. Speed = Speed;
			this.Dir = Dir;
			body =new SpriteX(path,pos);
			body.ChangeFather(this);
			body.CenterSprite(TRS.Local.Center);
			body.Color = new Vector4(12/255f,255/255f,98/255f,255f/255f);
			maxShootDis = BWeapon.GetRangeByType(wrt);
		}
		
		public override void UpdateFrame (float dt)
		{
			if (ticker60Fps.isTime (dt))
			{
				body.SetPosition (body.GetPosition () + Dir * Speed);
				
				nowShootDis +=Speed;
				if(nowShootDis > maxShootDis)
					HP=0;
			}
			
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


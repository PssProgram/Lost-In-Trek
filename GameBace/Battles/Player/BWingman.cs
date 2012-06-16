using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using PSVX.Battles;

namespace PSVX.Battles
{
	/// <summary>
	/// 僚机
	/// </summary>
	public class BWingman : BWarship
	{
		//行动模式,根据角色技能决定
		public enum ActionMode
		{
			Follow,//跟随在主人两侧 
		}
		
		ActionMode nowMode = ActionMode.Follow;
		//僚机主人
		BPlayer master;
		//编号
		int number = 0;
		
		//Follow模式
		//僚机要去位置
		Vector2 followPos = Vector2.Zero;
		float lastAngel =0;
		
		//60fps!
		Ticker ticker60Fps = new Ticker(0.01599f);
		
		public BWingman (Node node,Vector2 pos,int number,float w,float h)
			:base(node,pos,w,h,ContactType.None,null,null,"Battles//testwingman.png")
		{
			master = (BPlayer)node;
			this.number =number;
			Body.Scale = new Vector2(0.5f,0.5f);
		}
		
		public override void UpdateFrame (float dt)
		{
			if (ticker60Fps.isTime (dt)) 
			{	
				nowMode = master.WingmanCommand;
			
				if (nowMode == ActionMode.Follow)
				{
					Follow ();
				}
			
				base.UpdateFrame (dt);
			}
		}
			
		void Follow ()
		{
			//计算僚机要去位置
			Vector2 mdir = master.Mainship.NowDirection;
			Vector2 mpos = master.Mainship.GetPosition();
			Maths.Normalize(ref mdir);
			if (number == 0)
				followPos = new Vector2 (mdir.Y, -mdir.X) * 34f + mpos;
			else
				followPos = new Vector2 (-mdir.Y, mdir.X) * 34f + mpos;
			
			Body.Rotate (-lastAngel);
			Body.Rotate (master.Mainship.NowAngle);
			lastAngel = master.Mainship.NowAngle;
			
			Body.SetPosition (followPos);
		}
	}
}


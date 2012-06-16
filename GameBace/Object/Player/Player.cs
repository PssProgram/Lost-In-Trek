using System;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PSVX.Base
{
	public class Player:LiveObject
	{
		Ticker ticker = new Ticker(0.06f);
		SpriteX hero;
		
		public Player (Node node,float CreatTime,float DieTime):base(node,CreatTime,DieTime)
		{
			hero = new SpriteX(this,"hero2.png",new Vector2(200f,200f),new Vector2i(20,5),new Vector2i(1,1));
		}
		
		public override void UpdateFrame(float dt)
		{
			base.UpdateFrame(dt);
		}
		
		public override void HandleInput(float dt)
		{
			if(ticker.isTime(dt))
			{
				if(Input2.GamePad0.Down.Down)
				{
					hero.RunOneFrame(new Vector2i(2,1),new Vector2i(3,5));
					hero.RunAction(new MoveBy(new Vector2(0,-8),0.06f));
				}
				else if(Input2.GamePad0.Down.Release)
				{
					hero.RunOneFrame(new Vector2i(1,1),new Vector2i(1,1));
				}
				else if(Input2.GamePad0.Up.Down)
				{
					hero.RunOneFrame(new Vector2i(6,1),new Vector2i(7,5));
					hero.RunAction(new MoveBy(new Vector2(0,8),0.06f));
				}
				else if(Input2.GamePad0.Up.Release)
				{
					hero.RunOneFrame(new Vector2i(1,3),new Vector2i(1,3));
				}
				else if(Input2.GamePad0.Left.Down)
				{
					hero.RunOneFrame(new Vector2i(4,1),new Vector2i(5,5));
					hero.RunAction(new MoveBy(new Vector2(-8,0),0.06f));
				}
				else if(Input2.GamePad0.Left.Release)
				{
					hero.RunOneFrame(new Vector2i(1,2),new Vector2i(1,2));
				}
				else if(Input2.GamePad0.Right.Down)
				{
					hero.RunOneFrame(new Vector2i(4,1),new Vector2i(5,5));
					hero.RunAction(new MoveBy(new Vector2(8,0),0.06f));
				}
				else if(Input2.GamePad0.Right.Release)
				{
					hero.RunOneFrame(new Vector2i(1,2),new Vector2i(1,2));
				}
			}
		}
	}
}


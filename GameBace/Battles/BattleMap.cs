using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System.Collections.Generic;
using Sce.Pss.Core.Graphics;

namespace PSVX.Battles
{
	public enum BoundaryType
	{
		Left,
		Right,
		Up,
		Down
	}
	
	/// <summary>
	/// 显示的战斗地图
	/// </summary>
	public class BattleMap:GameObject
	{
		//会随摄像机移动而移动的动态星星数量
		int starCount = 30;
		//不会移动的
		SpriteX background;	
		SpriteX boundaryWarner;
		//不会显示的位置
		Vector2 NoDisplayPos =new Vector2(-100000,-10000);
		
		public BattleMap (Node node)
			:base(node)
		{
			background = new SpriteX("Battles//background.png",new Vector2(-100,-100));
			background.ChangeFather(this);
			boundaryWarner = new SpriteX("Battles//BoundaryWarning.png",NoDisplayPos);
			boundaryWarner.ChangeFather(this);
			boundaryWarner.CenterSprite(TRS.Local.Center);
			
			Vector2 pos;
			SpriteX x;
			for(int i=0;i<starCount;i++)
			{
				pos = new Vector2(Maths.Rnd.Next((int)(-BattleScene.Width*0.5f-100),(int)(BattleScene.Width*0.5f+100)),
				                  Maths.Rnd.Next((int)(-BattleScene.Heigth*0.5f-100),(int)(BattleScene.Heigth*0.5f+100)));
				 x = new SpriteX("Battles//star.png",pos);
				x.ChangeFather(this);
				x.CenterSprite(TRS.Local.Center);
			}
		}
		
		/// <summary>
		/// 使一些物体不随摄像机移动.
		/// </summary>
		public void KeepPos(Vector2 v2)
		{
			background.SetPosition(background.GetPosition() + v2);
		}
		
		//显示边界警告
		public void DisplayBoundaryWarn (Vector2 pos, BoundaryType type)
		{
			if (type == BoundaryType.Right) {
				boundaryWarner.CenterSprite (TRS.Local.MiddleLeft);
				if (boundaryWarner.Rotation != GameBase.Right) {
					boundaryWarner.Rotate (boundaryWarner.Rotation);
					boundaryWarner.Rotate (GameBase.Right);
				} else
					boundaryWarner.SetPosition (pos);
			}
			
			if (type == BoundaryType.Left) {
				boundaryWarner.CenterSprite (TRS.Local.MiddleLeft);
				if (boundaryWarner.Rotation != GameBase.Left) {
					boundaryWarner.Rotate (boundaryWarner.Rotation);
					boundaryWarner.Rotate (GameBase.Left);
				} else
					boundaryWarner.SetPosition (pos);
			}
			
			if (type == BoundaryType.Up) {
				boundaryWarner.CenterSprite (TRS.Local.MiddleLeft);
				if (boundaryWarner.Rotation != GameBase.Down) {
					boundaryWarner.Rotate (GameBase.Right);
					boundaryWarner.Rotate (GameBase.Down);
				} else
					boundaryWarner.SetPosition (pos);
			}
			
			if (type == BoundaryType.Down) {
				boundaryWarner.CenterSprite (TRS.Local.MiddleLeft);
				if (boundaryWarner.Rotation != GameBase.Up) {
					boundaryWarner.Rotate (GameBase.Right);
					boundaryWarner.Rotate (GameBase.Up);
				} else
					boundaryWarner.SetPosition (pos);
			}
		}
		
		//取消边界警告
		public void CancelBoundaryWarn()
		{
			boundaryWarner.SetPosition(NoDisplayPos);
		}
		                                
	}
}


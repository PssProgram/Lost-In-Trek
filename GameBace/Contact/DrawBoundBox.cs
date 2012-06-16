using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System.Collections.Generic;

namespace PSVX.Base
{
	/// <summary>
	/// 绘制一个包围盒
	/// </summary>
	public class DrawBoundBox :GameObject
	{
		//List<SpriteX> aabbPoints = new List<SpriteX>();
		//List<SpriteX> obbPoints = new List<SpriteX>();
		List<SpriteX> circlePoints = new List<SpriteX>();
		
		public DrawBoundBox (Node node)
			:base(node)
		{
			SpriteX p;
//			for(int i=0; i<4;i++)
//			{
//				p = new SpriteX(this,"Battles//primitive.png",
//				                new Vector2(-9999,-9999));
//				p.CenterSprite(TRS.Local.Center);
//				p.Color = new Vector4(37 / 255f, 255 / 255f, 28 / 255f, 255f / 255f);
//				p.Scale =new Vector2(0.7f,0.7f);
//				aabbPoints.Add(p);
//			}
			
//			for(int i=0; i<4;i++)
//			{
//				p = new SpriteX(this,"Battles//primitive.png",
//				                new Vector2(-9999,-9999));
//				p.CenterSprite(TRS.Local.Center);
//				p.Scale =new Vector2(0.7f,0.7f);
//				p.Color = new Vector4(227 / 255f, 64 / 255f, 255 / 255f, 255f / 255f);
//				obbPoints.Add(p);
//			}
//			
			for(int i=0; i<8;i++)
			{
				p = new SpriteX("Battles//primitive.png",
				                new Vector2(-9999,-9999));
				p.ChangeFather(this);
				p.CenterSprite(TRS.Local.Center);
				p.Scale =new Vector2(0.7f,0.7f);
				p.Color = new Vector4(52 / 255f, 150 / 255f, 255 / 255f, 255f / 255f);
				circlePoints.Add(p);
			}
				
		}
		
		//必须调用！
		public void SetBoundBoxInfor( BoundBox bb)
		{
			float w = bb.Width*0.5f;
			float h = bb.Height*0.5f;
			Vector2 pos = new Vector2(bb.X,bb.Y);
//			//AABB
//			aabbPoints[0].SetPosition(new Vector2(-w, -h) + pos);
//            aabbPoints[1].SetPosition(new Vector2(w, -h) + pos);
//            aabbPoints[2].SetPosition(new Vector2(w, h) + pos);
//            aabbPoints[3].SetPosition(new Vector2(-w, h) + pos);
//			//OBB
//			if(bb.Obb !=null)
//			{
//			  List<Vector2> ps = bb.Obb.GetPoints();
//			  obbPoints[0].SetPosition(ps[0]);
//			  obbPoints[1].SetPosition(ps[1]);
//			  obbPoints[2].SetPosition(ps[2]);
//			  obbPoints[3].SetPosition(ps[3]);
//			}
			//Circle
			if (bb.Circle != null)
			{
				circlePoints [0].SetPosition (GameBase.Up * bb.Circle.Radius + pos);
				circlePoints [1].SetPosition (GameBase.Down * bb.Circle.Radius + pos);
				circlePoints [2].SetPosition (GameBase.Left * bb.Circle.Radius + pos);
				circlePoints [3].SetPosition (GameBase.Right * bb.Circle.Radius + pos);
			
				circlePoints [4].SetPosition ((GameBase.Up + GameBase.Left).Normalize () * bb.Circle.Radius + pos);
				circlePoints [5].SetPosition ((GameBase.Down + GameBase.Left).Normalize () * bb.Circle.Radius + pos);
				circlePoints [6].SetPosition ((GameBase.Up + GameBase.Right).Normalize () * bb.Circle.Radius + pos);
				circlePoints [7].SetPosition ((GameBase.Down + GameBase.Right).Normalize () * bb.Circle.Radius + pos);
			}
			
		}
	}
}


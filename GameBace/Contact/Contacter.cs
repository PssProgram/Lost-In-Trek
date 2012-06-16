using System;
using System.Collections;
using System.Collections.Generic;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PSVX.Base
{
	public enum ContactType
	{
		Player, //会与，Item，SceneObject 检测
		Enemy,  //会与，Player，SceneObject 检测
		Bomb,//会与，Enemy，Player，SceneObject 检测
		Item,//会被检测
		SceneObject,//会被检测
		PlayerBullet,//会与，Enemy，Bomb，SceneObject 检测
		EnemyBullet,//会与，Player，Bomb，SceneObject 检测
		None // 不检测
	}

	public class Contacter
	{
		
		List<List<LiveObject>> ContactObjectList;
		private static Contacter instance;
		
		private Contacter ()
		{
			ContactObjectList = new List<List<LiveObject>> ();
			ContactObjectList.Add (new List<LiveObject> ());//Player
			ContactObjectList.Add (new List<LiveObject> ());//Enemy
			ContactObjectList.Add (new List<LiveObject> ());//Bomb
			ContactObjectList.Add (new List<LiveObject> ());//Item
			ContactObjectList.Add (new List<LiveObject> ());//SceneObject
			ContactObjectList.Add (new List<LiveObject> ());//PlayerBullet
			ContactObjectList.Add (new List<LiveObject> ());//EnemyBullet
		}
		
		public static Contacter Instance { 
			get { 
				if (instance == null) 
					instance = new Contacter (); 
				return instance; 
			} 
		}
		
		public void Add (LiveObject liveObject)
		{
			ContactObjectList [(int)liveObject.CType].Add (liveObject);
		}
		
		public void Clear ()
		{
			foreach (List<LiveObject> c in ContactObjectList) {
				c.Clear ();
			}
		}
		
		public void OnUpdate ()
		{
			//Player
			Contacting (ContactObjectList [(int)ContactType.Player],
			           ContactObjectList [(int)ContactType.Item]);
			Contacting (ContactObjectList [(int)ContactType.Player],
			           ContactObjectList [(int)ContactType.SceneObject]);
			 
			//Enemy
			Contacting (ContactObjectList [(int)ContactType.Enemy],
			           ContactObjectList [(int)ContactType.Player]);
			Contacting (ContactObjectList [(int)ContactType.Enemy],
			           ContactObjectList [(int)ContactType.SceneObject]);
		 
			//Bomb
			Contacting (ContactObjectList [(int)ContactType.Bomb],
			           ContactObjectList [(int)ContactType.Enemy]);
			Contacting (ContactObjectList [(int)ContactType.Bomb],
			           ContactObjectList [(int)ContactType.Player]);
			Contacting (ContactObjectList [(int)ContactType.Bomb],
			           ContactObjectList [(int)ContactType.SceneObject]);
			 
			//PlayerBullet 
			Contacting (ContactObjectList [(int)ContactType.PlayerBullet],
			           ContactObjectList [(int)ContactType.Bomb]);
			Contacting (ContactObjectList [(int)ContactType.PlayerBullet],
			           ContactObjectList [(int)ContactType.Enemy]);
			Contacting (ContactObjectList [(int)ContactType.PlayerBullet],
			           ContactObjectList [(int)ContactType.SceneObject]);
			
			//EnemyBullet
			Contacting (ContactObjectList [(int)ContactType.EnemyBullet],
			           ContactObjectList [(int)ContactType.Bomb]);
			Contacting (ContactObjectList [(int)ContactType.EnemyBullet],
			           ContactObjectList [(int)ContactType.Player]);
			Contacting (ContactObjectList [(int)ContactType.EnemyBullet],
			           ContactObjectList [(int)ContactType.SceneObject]);
				
			
			Clear ();
		}
 
		//碰见检测，主动init--被动pass
		private void Contacting (List<LiveObject> init, List<LiveObject> pass)
		{
			foreach (LiveObject oinit in init) {
				foreach (LiveObject opass in pass) 
				{
					if(oinit.status == ObjectStatus.Die||
					   opass.status == ObjectStatus.Die)
						return;
					
					BoundBox bbInit = oinit.GetBoundBox();
					BoundBox bbPass = opass.GetBoundBox();
					bool isc = false;
					
					if(bbInit == null || bbPass == null)
						return;
					
					//用什么检测
					if(bbInit.IsUseCircle == true || bbPass.IsUseCircle==true)
					{//用圆形
						isc = Circle.IsContacted(bbInit.Circle,bbPass.Circle);
					}
					else
					{//用AABB
						isc = BoundBox.IsContacted(bbInit,bbPass);
					}
					if (isc== true)
					{
						oinit.ToContact (opass);
						opass.BeContact (oinit);
					}
//					3中检测
//					AABB
//					bool iscaabb = BoundBox.IsContacted (oinit.GetBoundBox (), opass.GetBoundBox ());
//					  
//					bool iscobb = false;
//					if (iscaabb == true && oinit.GetBoundBox ().Obb != null && 
//					   opass.GetBoundBox ().Obb != null) 
//					{//有OBB
//						iscobb = OBB.IsContacted (oinit.GetBoundBox ().Obb, opass.GetBoundBox ().Obb);
//					}
//					
//					bool iscc = Circle.IsContacted (oinit.GetBoundBox ().Circle, opass.GetBoundBox ().Circle);
//					
//					if (iscaabb == true || iscobb == true || iscc == true)
//					{
//						oinit.ToContact (opass);
//						opass.BeContact (oinit);
//					}
				}
			}
		}
		
		/*
		public bool isInpoint(Vector2 P0)
		{
			Vector2 P1 = new Vector2(X,Y);
			Vector2 P2 = new Vector2(X+Width,Y);
			Vector2 P3 = new Vector2(X,Y+Height);
			Vector2 P4 = new Vector2(X+Width,Y+Height);
			 if (Multiply(P0, P1, P2) * Multiply(P0,P4, P3) <= 0
                && Multiply(P0, P4, P1) * Multiply(P0, P3, P2) <= 0)
                return true;
            return false;
		}
		*/
	}
}


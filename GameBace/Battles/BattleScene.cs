using System;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using PSVX.Base;

namespace PSVX.Battles
{
	public enum RunIn
	{
		PSV,
		PC,
		Universal  
	}
	
	/// <summary>
	/// 战斗场景
	/// 从大地图上进入，进行战斗处理，战斗完毕又返回大地图
	/// </summary>
	public class BattleScene:PSVX.Base.GameScene
	{
		//在什么上运行
		public static RunIn Runin = RunIn.PSV;
		
		Layer bacgroundBack;
		Layer bacground;
		Layer middle;
		Layer front;
		//显示战斗背景
		BattleMap map;
		public BattleMap Map {
			get {
				return this.map;
			}
		}	 

		//
		SpriteX otherSprites;
		BEnemyManager enemy;
		BPlayer player;
		//地图尺寸
		static float width =1700;
		public static float Width {
			get {
				return width;
			}
		}

	    static float heigth = 1000;
		public static float Heigth {
			get {
				return heigth;
			}
		}
		
		internal static	float LeftLimit;
		internal static	float RightLimit;
		internal static	float UpLimit;
		internal static	float DownLimit;
		
		//60fps! 只加在顶层！
		Ticker ticker60Fps = new Ticker(0.01599f);
		
		public override void Start ()
		{
			//先加载资源
			GameData.LoadTextureInfo("Battles//background.png");
			GameData.LoadTextureInfo("Battles//BoundaryWarning.png");
			GameData.LoadTextureInfo("Battles//primitive.png");
			GameData.LoadTextureInfo("Battles//star.png");
			GameData.LoadTextureInfo("Battles//test.png");
			GameData.LoadTextureInfo("Battles//testship.png");
			GameData.LoadTextureInfo("Battles//testwingman.png");
			
			bacgroundBack = new Layer ();
			bacground = new Layer ();
			middle = new Layer ();
			front = new Layer ();
			
			this.AddChild (bacgroundBack);//最下层
			this.AddChild (bacground); 
			this.AddChild (middle); 
			this.AddChild (front); 
			
			LeftLimit = - Width * 0.5f;
			RightLimit = Width * 0.5f; 
			UpLimit = - Heigth * 0.5f;
			DownLimit = Heigth * 0.5f;
			
			map = new BattleMap(bacgroundBack);
			enemy = new BEnemyManager(middle,this);
			player = new BPlayer(middle,this);
			otherSprites = new SpriteX (this, "Battles//test.png", new Vector2 (0, 0));
			otherSprites.CenterSprite (TRS.Local.Center);
			
			//锁定
			player.SetCanLockEnemys(enemy.GetAllWarships());
			enemy.SetCanLockEnemys(player.GetAllWarships());
			
			base.Start ();
		}
		
		public override void UpdateFrame (float dt)
		{
			if(!ticker60Fps.isTime(dt))
				return;
			
			//背景不动
			if(player.NowStatus == BPlayer.Status.Live)
			{
				map.KeepPos(player.CameraMoveDir * player.CameraMoveSpeed);
			}
		}
		
		//是否超出地图
		public static bool SceneSizeLimit(ref Vector2 pos)
		{
			if(pos.X < LeftLimit)
				pos.X =LeftLimit;
			if(pos.X > RightLimit)
				pos.X =RightLimit;
			if(pos.Y < UpLimit)
				pos.Y =UpLimit;
			if(pos.Y > DownLimit)
				pos.Y =DownLimit;
			
			return false;
		}
		
		//是否超出地图,给敌方AI用的
		public static bool SceneSizeLimit(Vector2 pos)
		{
			if (pos.X < LeftLimit)
				return true;
			if (pos.X > RightLimit)
				return true;
			if (pos.Y < UpLimit)
				return true;
			if (pos.Y > DownLimit)
				return true;
			
			return false;
		}
	}
}


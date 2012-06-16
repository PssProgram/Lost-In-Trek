using System.Collections;
using System.Collections.Generic;

using Sce.Pss.Core;
using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;

using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PSVX.Base
{
	public class GameBase:Node
	{
		public bool GameReStart = false;
		
		public static Vector2 Left = new Vector2(-1,0);
		public static Vector2 Right = new Vector2(1,0);
		public static Vector2 Up = new Vector2(0,-1);
		public static Vector2 Down = new Vector2(0,1);
		
		public GameBase()
		{
			Scheduler.Instance.Schedule(this, BaceUpdate, 0.0f, false);
		}
		
		public virtual void Main(){}
		
		public virtual void BaceUpdate(float dt)
		{
			
		}
		
		public GameScene ChangeScene(string sceneName)
		{
			DebugScene.Instance.WriteLine("載入場景:" + sceneName);
			var scene = SwitchScene(sceneName);
			if(scene != null)
			{
				//var transition = new TransitionSolidFade(scene) { PreviousScene = Director.Instance.CurrentScene, Duration = 1.5f, Tween = Sce.Pss.HighLevel.GameEngine2D.Base.Math.Linear	 };
				Director.Instance.ReplaceScene(scene);
				Scheduler.Instance.Schedule(scene, scene.UpdateFrame, 0.0f, false);
				scene.Start();
			}
			else
				DebugScene.Instance.WriteLine("未找到场景:" + sceneName);
			return scene;
		}
		
		public virtual GameScene SwitchScene(string sceneName)
		{
			return null;
		}
	}
}


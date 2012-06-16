using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO;

using Sce.Pss.Core;
using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Graphics;

using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

using PSVX.Base;
using PSVX.Battles;

namespace LostInTrek
{
	public class Game:GameBase
	{
		public static Game Instance;
		
		public override void Main()
		{
			var mainMenu = new MainMenu();
			GameData.LoadFont();//读取字体
			GameData.LoadPlanetConfig();//读取星球配置文件
			GameData.LoadEventConfig();//读取事件配置文件
			GameData.LoadMissionConfig();//读取任务配置文件
			GameData.AVGText = GameData.LoadText("AVGText.csv");//读取AVG文字
			Scheduler.Instance.Schedule(mainMenu, mainMenu.UpdateFrame, 0.0f, false);
			mainMenu.Start();
			Director.Instance.RunWithScene(mainMenu,true);
		}
		
		public override void BaceUpdate(float dt)
		{
			Contacter.Instance.OnUpdate();
			DebugScene.Instance.FpsUpdate(dt);
			TouchManager.Instance.OnUpdate();
				
			//新开始游戏操作
			if(GameReStart)
			{
				Contacter.Instance.Clear();
				GameReStart = false;
			}
		}
		
		public override GameScene SwitchScene (string sceneName)
		{
			switch (sceneName) {
			case "MainMenu":
				return new MainMenu ();
			case "TestScene":
				return new TestScene ();
			case "BattleScene":
				return new BattleScene ();
			case "PlanetScene":
				return new PlanetScene ();
			}
			return base.SwitchScene (sceneName);
		}
	}
}
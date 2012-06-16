using System;
using System.Collections.Generic;
using Sce.Pss.Core;
using PSVX.Base;

namespace LostInTrek
{
	public class TestScene:GameScene
	{
		
		public TestScene ()
		{
		}
		
		public override void Start()
		{
			this.Exit();
			Game.Instance.ChangeScene("MainMenu");
			//GameUI.Instance.avgManager.Setup(this);
			//GameUI.Instance.avgManager.LoadScript("TestScript.txt");
		}
		
		public override void UpdateFrame (float dt)
		{
			
		}
	}
}


using System;
using System.Collections.Generic;
using Sce.Pss.HighLevel.UI;

namespace PSVX.Base
{
	public class GameUI:Scene
	{
		private static GameUI instance;
		
		private GameUI ()
		{
		}
		
		public static GameUI Instance 
   		{ 
      		get
      		{ 
         		if (instance == null) 
            	instance = new GameUI(); 
         		return instance; 
      		}
   		}
		
		protected override void OnUpdate (float elapsedTime)
		{
			base.OnUpdate (elapsedTime);
			DebugScene.Instance.Update(elapsedTime);
		}
	}
}


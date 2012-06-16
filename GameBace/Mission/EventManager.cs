using System;

namespace PSVX.Base
{	
	public class EventManager
	{
		private static EventManager instance; 
		private EventManager ()
		{
			AVG.AVGManager.Instance.AVGFinished += new AVG.AVGFinishDelegate(this.EndEvent);
		}
		public static EventManager Instance
		{
			get  
		      { 
		         if (instance == null) 
		            instance = new EventManager(); 
		         return instance; 
		      } 
		}
		
		public bool isFinish = true;
		
		public void StartEvent(int EventID)
		{
			isFinish = false;
			AVG.AVGManager.Instance.LoadScript(GameData.EventDatas[EventID].ScriptName);
		}
		
		public void EndEvent()
		{
			isFinish = true;
		}
	}
}


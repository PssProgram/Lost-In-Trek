using System;
using System.Collections.Generic;

namespace PSVX.Base
{
	public class MissionManager
	{
		private static MissionManager instance; 
		private MissionManager ()
		{
		}
		
		public static MissionManager Instance
		{
			get  
		      { 
		         if (instance == null) 
		            instance = new MissionManager(); 
		         return instance; 
		      } 
		}
		
		public void Accept(int id)
		{
			//初始任务
			GameData.mainData.AcceptMission.Add(new AcceptMissionData(){ID = id,Setp = 0,Status = MissionStatus.Accept});
			foreach(KeyValuePair<int,EventData> item in GameData.EventDatas)
			{
				if(item.Value.MissionID == id && item.Value.MissionSetp == 0)
					GameData.mainData.TodoEvent.Add(item.Key);
			}
		}
	}
}


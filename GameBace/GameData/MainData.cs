using System;
using System.Collections.Generic;

namespace PSVX.Base
{
	public class MainData
	{
		public List<AcceptMissionData> AcceptMission;
		public List<int> TodoEvent;
		
		public MainData ()
		{
			
		}
		
		public static void NewGame()
		{
			GameData.mainData = new MainData();
			GameData.mainData.AcceptMission = new List<AcceptMissionData>();
			GameData.mainData.TodoEvent = new List<int>();
			MissionManager.Instance.Accept(10001);
		}
	}
}


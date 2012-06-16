using System;

namespace PSVX.Base
{
	public enum EventType
	{
		Space,
		Battle,
		Planet,
		Port,
		Shipyard,
		Shop,
		Market,
		Bar,
	}
	
	public class EventData
	{
		public EventType Type;
		public string Name;//名称
		public int Planet;//所在星球
		public int MissionID;//所属任务ID
		public int MissionSetp;//所属任务第几步
		public string ScriptName;//执行脚本名称
		public int GoToMissionSetp;//完成后转移到任务第几步
	}
}


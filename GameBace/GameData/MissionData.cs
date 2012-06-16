using System;

namespace PSVX.Base
{
	public enum MissionType
	{
		Main,
		Story,
		Other,
	}
	
	public enum MissionStatus
	{
		Accept,
		ToBeCompleted,
		Finish,
	}
	
	public class MissionData
	{
		public MissionType Type;//类型
		
		public string Name;//名称
		public string Describe;//任务描述
		public bool isLoop;//是否循环
		public int FollowMission;//后续任务
		
		public PrestigeType PrestigeType;//声望类型
		public int PrestigeLevel;//声望等级
		public int Member;//必须成员
		public int Item;//必须物品
		public int Money;//必须金钱
		
		public int AddPrestige;//奖励声望
		public int AddMoney;//奖励金钱
		public int AddItem1;//道具1
		public int AddItem1Num;//道具1数量
		public int AddItem2;//道具2
		public int AddItem2Num;//道具2数量
		public int SpecialAwards;//特殊奖励
		
		public MissionData ()
		{
			
		}
	}
	
	public class AcceptMissionData
	{
		public int ID{get;set;}//任务ID
		public int Setp{get;set;}//第几步
		public MissionStatus Status{get;set;}//状态
	}
}


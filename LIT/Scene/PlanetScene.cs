using System;
using System.Collections.Generic;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace LostInTrek
{
	public class PlanetScene:GameScene
	{
		int ID;
		public PlanetData Data;
		
		Layer BackGround;
		public SpriteX backGroundPic;
		
		bool isEventDone = false;
		Queue<int> TodoEvent;
		
		PlanetMenu planetMenu;
		
		public PlanetScene ()
		{
			
		}
		
		public override void Start ()
		{
			//添加BackGround层
			BackGround = new Layer();
			this.AddChild(BackGround);
		}
		
		public override void SetUp (int StartID)
		{
			ID = StartID;
			Data = GameData.PlanetDatas[StartID];
			DebugScene.Instance.WriteLine("讀取星球場景配置:ID" + StartID);
			//添加背景精灵
			AddBackground(Data.BgName);
			EventSelect(EventType.Planet);
		}
		
		private void AddBackground(string Name)
		{
			if(Name != "")
				backGroundPic = new SpriteX(BackGround,"BG/" + Name,Vector2.Zero);
			else
				backGroundPic = new SpriteX(BackGround,"BG/Default.png",Vector2.Zero);
		}
		
		public override void UpdateFrame (float dt)
		{
			if(Data != null)
			{
				if(isEventDone == false)
					DoingEvent();
				else if(planetMenu == null ||planetMenu.isMenuShow == false)
				{
					planetMenu = new PlanetMenu();
					planetMenu.ChangeFather(this);
					planetMenu.ShowMenu();
				}
				else if(planetMenu.isMenuShow && planetMenu.isPause == true)
				{
					planetMenu.Contiund();
				}
			}
		}
		
		public void ChangeMenu(MenuType type)
		{
			switch(type)
			{
			case MenuType.Planet:
				EventSelect(EventType.Planet);
				break;
			case MenuType.Port:
				EventSelect(EventType.Port);
				break;
			case MenuType.Market:
				EventSelect(EventType.Market);
				break;
			case MenuType.Government:
				EventSelect(EventType.Market);
				break;
			case MenuType.Bar:
				EventSelect(EventType.Bar);
				break;
			}
			isEventDone = false;
		}
		
		public void EventSelect(EventType eventType)
		{
			foreach(int i in GameData.mainData.TodoEvent)
			{
				if(GameData.EventDatas[i].Type == eventType && GameData.EventDatas[i].Planet == ID)
				{
					if(TodoEvent == null)
						TodoEvent = new Queue<int>();
					TodoEvent.Enqueue(i);
				}
			}
			foreach(int i in TodoEvent)
			{
				GameData.mainData.TodoEvent.Remove(i);
			}
		}
		
		public void DoingEvent()
		{
			if(TodoEvent.Count == 0)
			{
				if(EventManager.Instance.isFinish)
					isEventDone = true;
			}
			else
				EventManager.Instance.StartEvent(TodoEvent.Dequeue());
		}
	}
}


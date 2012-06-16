using System;
using System.Collections.Generic;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace LostInTrek
{
	public enum MenuType
	{
		None,
		Planet,
		Port,
		Government,
		Market,
		Bar,
	}
	
	public class PlanetMenu:Layer
	{
		PlanetScene fatherNode;
		public bool isMenuShow = false;
		public List<MenuButton> ButtonList;
		public MenuBGObject menuBG;
		bool CanBeControl = false;
		public bool isPause = false;
		MenuType ChangeMenu = MenuType.None;
		Ticker ticker;
		int SelectButton = -1;
		
		public PlanetMenu()
		{
		}
		
		public void ShowMenu()
		{
			menuBG = new MenuBGObject(this);
			ButtonList = new List<MenuButton>();
			ticker = new Ticker(1.5f);
			Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(this, UpdateFrame, 0.0f, false);
			
			ShowMenuButton(MenuType.Planet);
			
			isMenuShow = true;
		}
		
		public void Contiund()
		{
			for(int i = 0;i < ButtonList.Count;i++)
			{
				ButtonList[i].Remove();
			}
			ButtonList = new List<MenuButton>();
			ticker = new Ticker(1.5f);
			ShowMenuButton(ChangeMenu);
			ChangeMenu = MenuType.None;
			Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(this, UpdateFrame, 0.0f, false);
			isPause = false;
		}
		
		private void ShowMenuButton(MenuType Type)
		{
			switch(Type)
			{
			case MenuType.Planet:
				ButtonList.Add(new MenuButton(this,"PlanetMenu/Port_Button.png",new SelectTodo(GotoPort),ButtonList.Count));
				if(fatherNode.Data.Shipyard >= 0)
					ButtonList.Add(new MenuButton(this,"PlanetMenu/Government_Button.png",new SelectTodo(GotoPort),ButtonList.Count));
				if(fatherNode.Data.Government >= 0)
					ButtonList.Add(new MenuButton(this,"PlanetMenu/Shipyard_Button.png",new SelectTodo(GotoPort),ButtonList.Count));
				if(fatherNode.Data.Market >= 0)
					ButtonList.Add(new MenuButton(this,"PlanetMenu/Market_Button.png",new SelectTodo(GotoPort),ButtonList.Count));
				if(fatherNode.Data.Shipyard >= 0)
					ButtonList.Add(new MenuButton(this,"PlanetMenu/Bar_Button.png",new SelectTodo(GotoPort),ButtonList.Count));
				break;
			}
		}
		
		public void UpdateFrame(float dt)
		{
			if(ChangeMenu != MenuType.None)
			{
				if(ticker.isTime(dt))
				{
					fatherNode.ChangeMenu(ChangeMenu);
					Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(this, UpdateFrame);
					isPause = true;
				}
			}
			else if(CanBeControl == false)
			{
				if(ticker.isTime(dt))
					CanBeControl = true;
			}
			else if(SelectButton > -1)
			{
				if(Input2.GamePad0.Down.Press)
				{
					SelectButton += 1;
					if(SelectButton > ButtonList.Count - 1)
						SelectButton = 0;
					ChangeSelectButton();
				}
				else if(Input2.GamePad0.Up.Press)
				{
					SelectButton -= 1;
					if(SelectButton < 0)
						SelectButton = ButtonList.Count - 1;
					ChangeSelectButton();
				}
				else if(Input2.GamePad0.Circle.Press)
				{
					ButtonList[SelectButton].TouchInput();
				}
			}
			else if(SelectButton == -1)
			{
				if(Input2.GamePad0.Down.Press || Input2.GamePad0.Up.Press)
				{
					SelectButton = 0;
					ChangeSelectButton();
				}
			}
		}
		
		private void ChangeSelectButton()
		{
			for(int i= 0;i<ButtonList.Count;i++)
			{
				if(i == SelectButton)
					ButtonList[i].Select();
				else
					ButtonList[i].UnSelect();
			}
		}
		
		private void GotoPort()
		{
			UnShowMenuButton();
			ChangeMenu = MenuType.Port;
		}
		
		private void UnShowMenuButton()
		{
			ticker = new Ticker(0.7f);
			SelectButton = -1;
			CanBeControl = false;
			foreach(MenuButton mb in ButtonList)
				mb.UnShow();
		}
		
		public void ChangeFather(PlanetScene node)
		{
			if(fatherNode!= null)
				fatherNode.RemoveChild(this,false);
			node.AddChild(this);
			fatherNode = node;
		}
	}
	
	public class MenuButton:GameObject
	{
		SpriteX Image;
		public int Num;
		bool isShow = false;
		bool isEnd = false;
		private SelectTodo selectTodo;
		Ticker ShowTime;
		
		public MenuButton(Node node,string path,SelectTodo selectTodo,int num):base(node)
		{
			Num = num;
			this.selectTodo = selectTodo;
			var vector = new Vector2(0,90 + Num * 60);
			Image = new SpriteX(this,path,vector,new Vector2i(1,2),new Vector2i(1,1));
			Image.Color.A = 0;
			ShowTime = new Ticker(0.65f + Num*0.12f);
		}
		
		public override void UpdateFrame (float dt)
		{
			if(!isShow)
			{
				if(ShowTime.isTime(dt))
				{
					Image.Moveby(new Vector2(60,0),1f);
					Image.RunAction(new TintBy(new Vector4(0f,0f,0f,1f),0.5f));
					isShow = true;
				}
			}
			else if(!isEnd)
			{
				//base.UpdateFrame(dt);
				this.bounds = Image.GetBounds();
				TouchManager.Instance.AddTouchList(this);
			}
			else
			{
				if(ShowTime.isTime(dt))
				{
					Image.Moveby(new Vector2(-60,0),1f);
					Image.RunAction(new TintBy(new Vector4(0f,0f,0f,-1f),0.5f));
					Scheduler.Instance.Unschedule (this, UpdateFrame);
				}
			}
		}
		
		public override void TouchInput()
		{
			if (selectTodo != null )
            {
				Select();
                selectTodo();
            }
		}
		
		public void Select()
		{
			Image.TileIndex1D = 1;
		}
		
		public void UnSelect()
		{
			Image.TileIndex1D = 0;
		}
		
		public void UnShow()
		{
			ShowTime = new Ticker(0.5f - Num*0.08f);
			isEnd = true;
		}
	}
	
	public class MenuBGObject:GameObject
	{
		SpriteX Image;
		Ticker ShowTime;
		bool isEnd = false;
		
		public MenuBGObject(Node node):base(node)
		{
			var vector = new Vector2(30,-300);
			Image = new SpriteX(this,"PlanetMenu/Menu_BG.png",vector);
			Image.Color.A = 0;
			Image.RunAction(new TintBy(new Vector4(0f,0f,0f,1f),1f));
			Image.Moveby(new Vector2(0,320),1f);
		}
		
		public override void UpdateFrame (float dt)
		{
			if(isEnd)
			{
				if(ShowTime.isTime(dt))
				{
					Image.RunAction(new TintBy(new Vector4(0f,0f,0f,-1f),1f));
					Image.Moveby(new Vector2(0,-450),1f);
					Scheduler.Instance.Unschedule (this, UpdateFrame);
				}
			}
		}
		
		public void UnShow()
		{
			isEnd = true;
			ShowTime = new Ticker(0.65f);
		}
	}
}


using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System.Collections.Generic;

namespace LostInTrek
{
	public delegate void SelectTodo ();
	
	public class MainMenu:PSVX.Base.GameScene
	{
		Layer Bacground;
		Layer MenuScene;
		List<MainMenuButton> ButtonList;
		int SelectButton = -1;
		
		public MainMenu ()
		{
			
		}
		
		public override void Start ()
		{
			Bacground = new Layer();
			MenuScene = new Layer();
			this.AddChild(Bacground);//最下层
			this.AddChild(MenuScene);//菜单层
			DrawImage(Bacground,"Background","MainMenu/Menu_BG.png");
			TitleButton Title = new TitleButton(MenuScene);
			ButtonList = new List<MainMenuButton>();
			
			ButtonList.Add(new MainMenuButton(MenuScene,"MainMenu/Menu_Button1.png",new Vector2(389,310),new SelectTodo(LoadGame)));
			ButtonList.Add(new MainMenuButton(MenuScene,"MainMenu/Menu_Button2.png",new Vector2(391,375),new SelectTodo(NewGame)));
			ButtonList.Add(new MainMenuButton(MenuScene,"MainMenu/Menu_Button3.png",new Vector2(394,445),new SelectTodo(EnterStaff)));
		}
		
		public void NewGame()
		{
			//单次输出测试,两秒后消失
			DebugScene.Instance.WriteLine("NewGame");
			this.Exit();
			MainData.NewGame();
			Game.Instance.ChangeScene("PlanetScene").SetUp(0);
		}
		
		public void LoadGame()
		{
			DebugScene.Instance.WriteLine("Continue");
		}
		
		public void EnterStaff()
		{
			this.Exit();
			Game.Instance.ChangeScene("BattleScene");
		}
		
		public override void UpdateFrame(float dt)
		{	
			if(SelectButton > -1)
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
		
		public void ChangeSelectButton()
		{
			for(int i= 0;i<ButtonList.Count;i++)
			{
				if(i == SelectButton)
					ButtonList[i].Select();
				else
					ButtonList[i].UnSelect();
			}
		}
	}
	
	public class MainMenuButton:GameObject
	{
		SpriteX Image;
		private SelectTodo selectTodo;
		
		public MainMenuButton(Node node,string path,Vector2 vector2,SelectTodo selectTodo):base(node)
		{
			this.selectTodo = selectTodo;
			Image = new SpriteX(this,path,vector2,new Vector2i(1,2),new Vector2i(1,1));
		}
		
		public override void UpdateFrame(float dt)
		{
			//base.UpdateFrame(dt);
			this.bounds = Image.GetBounds();
			TouchManager.Instance.AddTouchList(this);
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
	}
	
	public class TitleButton:LiveObject
	{
		SpriteX Image;
		public TitleButton(Node node):base(node)
		{
			Image = new SpriteX(this,"MainMenu/Menu_Title.png",new Vector2(247,96));
		}
	}
}


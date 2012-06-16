using System;
using System.Collections.Generic;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PSVX.Base.AVG
{
	public delegate void AVGFinishDelegate();
	
	public class AVGManager:Sce.Pss.HighLevel.GameEngine2D.Node
	{
		private static AVGManager instance; 
		public static AVGManager Instance
		{
			get  
		      { 
		         if (instance == null) 
		            instance = new AVGManager(); 
		         return instance; 
		      } 
		}
		
		TextBox textBox;
		Scene Scene;
		ScriptManager scriptManager;
		bool isDone = true;
		
		Queue<ScriptComannd> ComanndQueue;
		
		public Layer AVGlayer;
		public Layer TextLayer;
		public SpriteX ld;
		public SpriteX rd;
		
		private AVGManager ()
		{
			scriptManager = new ScriptManager();
		}
		
		public void Setup()
		{
			Scene = Director.Instance.CurrentScene;
			textBox = new TextBox();
			TextLayer = new Layer();
			AVGlayer = new Layer();
			Scene.AddChild(AVGlayer);
			Scene.AddChild(TextLayer);
			textBox.NewTextBox();
			textBox.textBG.ChangeFather(TextLayer);
			textBox.textBG.ObjectTouched += new TouchInputDelegate(Input);
		}
		
		public void UnSetup()
		{
			Scene.RemoveChild(TextLayer,true);
			Scene.RemoveChild(AVGlayer,true);
			textBox.Clean();
			textBox = null;
		}
		
		public event AVGFinishDelegate AVGFinished;
		
		public void LoadScript(string ScriptName)
		{
			Setup();
			ComanndQueue = scriptManager.LoadScript(ScriptName);
			Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(this, ExecutionComannd, 0.0f, false);
		}
		
		//执行脚本
		public void ExecutionComannd(float dt)
		{
			textBox.OnUpdate(dt);
			
			if(isDone)
			{
				if(ComanndQueue.Count == 0)
				{
					Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(this,ExecutionComannd);
					UnSetup();
					AVGFinished();
				}
				else
				{
					var Comannd = ComanndQueue.Dequeue();
					switch(Comannd.comanndType)
					{
					case ComanndType.say:
						try{
						Say(GameData.AVGText[Comannd.text]);
						}
						catch
						{
							Say ("错误！找不到文字:" + Comannd.text);
						}
						isDone = false;
						break;
					}
				}
			}
			else if(Input2.GamePad0.Circle.Press)
			{
				Input ();
			}
		}

		void Input ()
		{
			if(textBox.isDoen == false)
				textBox.Finish();
			else
				isDone = true;
		}
		
		public void Say(string text)
		{
			textBox.StartPrint(text);
		}
	}
}


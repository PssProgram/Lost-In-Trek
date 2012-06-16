using System;
using System.Diagnostics;
using System.Collections.Generic;
using Sce.Pss.Core.Imaging;
using Sce.Pss.HighLevel.UI;

namespace PSVX.Base
{
	public enum FontColor
	{
		Red,
		Pink,
		Blue,
		Green,
		Yellow,
		Black,
		White,
	}
	
	
	
	public class DebugScene
	{
		public class InfoLab:Label
		{
			public float UpdateTime;
			public InfoLab(Scene scene,Font infoFont,float UpdateTime,FontColor color)
			{
				this.UpdateTime = UpdateTime;
				this.Font = infoFont;
				this.X = 5;
				switch(color)
				{
				case FontColor.Yellow:
					this.TextColor = new UIColor(1.0f,0.843f,0.0f,1.0f);
					break;
				case FontColor.Red:
					this.TextColor = new UIColor(1.0f,0f,0f,1.0f);
					break;
				case FontColor.Black:
					this.TextColor = new UIColor(0f,0f,0f,1.0f);
					break;
				case FontColor.Blue:
					this.TextColor = new UIColor(0.392f,0.584f,0.929f,1.0f);
					break;
				case FontColor.White:
					this.TextColor = new UIColor(1f,1f,1f,1.0f);
					break;
				case FontColor.Green:
					this.TextColor = new UIColor(0.196f,0.803f,0.196f,1.0f);
					break;
				case FontColor.Pink:
					this.TextColor = new UIColor(1.0f,0.682f,0.725f,1.0f);
					break;
				}
				scene.RootWidget.AddChildLast(this);
			}
		}
		
		Label fps;
		Label cpu;
		Font font;
		int randomNum = 0;
		
		public float TotleTime = 0;
		float elapsedTime = 0;
		int frameRate = 0;
		int frameCounter = 0;
		public List<KeyValuePair<string,InfoLab>> infoList;
		
		private static DebugScene instance;
		
		private DebugScene ()
		{
			//if   (System.Environment.StackTrace.ToLower().IndexOf( ":line ") <0)
			//	this.Visible = false;
			fps = new Label();
			cpu = new Label();
			infoList = new List<KeyValuePair<string,InfoLab>>();
			font = new Font(FontAlias.System,14,FontStyle.Regular);
			SetUp();
		}
		
		public static DebugScene Instance 
   		{ 
      		get  
      		{ 
         		if (instance == null) 
            	instance = new DebugScene(); 
         		return instance; 
      		} 
   		}
		
		private void SetUp()
		{
			fps.X = 5;
			fps.Y = 0;
			fps.Font = font;
			fps.TextColor = new UIColor(0.941f,0.913f,0.196f,1.0f);
			GameUI.Instance.RootWidget.AddChildLast(fps);
			cpu.X = 5;
			cpu.Y = 15;
			cpu.Font = font;
			cpu.TextColor = new UIColor(0.941f,0.913f,0.196f,1.0f);
			GameUI.Instance.RootWidget.AddChildLast(cpu);
		}
		
		public void FpsUpdate(float dt)
		{
			elapsedTime += dt;
            if (elapsedTime > 1)
            {
                elapsedTime -= 1;
                frameRate = frameCounter;
                frameCounter = 0;
            }
			frameCounter++;
			fps.Text = string.Format("Fps: {0}", frameRate);
		}
		
		/// <summary>
		/// 撰写一行屏幕信息
		/// </summary>
		/// <param name='Info'>
		/// 信息内容
		/// </param>
		public void WriteLine(string Info)
		{
			string LabelName = "Random" + randomNum;
			randomNum ++;
			WriteLine(LabelName,Info,FontColor.Yellow);
		}
		
		/// <summary>
		/// 撰写一行屏幕信息
		/// </summary>
		/// <param name='lableName'>
		/// 信息名称
		/// </param>
		/// <param name='Info'>
		/// 信息内容
		/// </param>
		public void WriteLine(string LableName,string Info)
		{
			WriteLine(LableName,Info,FontColor.Yellow);
		}
		
		/// <summary>
		/// 撰写一行屏幕信息
		/// </summary>
		/// <param name='lableName'>
		/// 信息名称
		/// </param>
		/// <param name='Info'>
		/// 信息内容
		/// </param>
		/// <param name='color'>
		/// 字体颜色
		/// </param>
		public void WriteLine(string lableName,string Info,FontColor color)
		{
			bool isNew = true;
			foreach(KeyValuePair<string,InfoLab> item in infoList)
			{
				if(item.Key == lableName)
				{
					isNew = false;
					item.Value.Text = Info;
					item.Value.UpdateTime = TotleTime;
					break;
				}
			}
			if(isNew)
			{
				infoList.Add(new KeyValuePair<string,InfoLab>(lableName,new InfoLab(GameUI.Instance,font,TotleTime,color)));
				infoList[infoList.Count - 1].Value.Text = Info;
				infoList[infoList.Count - 1].Value.UpdateTime = TotleTime;
			}
		}
		
		public void CPUUpdate(float cpuInfo)
		{
			cpu.Text = string.Format("CPU:{0:f2}%",cpuInfo);
		}
		
		public void Update(float elapsedTime)
		{
			List<int> deleList = new List<int>();
			TotleTime += elapsedTime;
			for(int i = 0;i<infoList.Count;i++)
			{
				infoList[i].Value.Y = 30 + (i)*15;
				if(infoList[i].Value.UpdateTime < TotleTime - 1000)
				{
					deleList.Add(i);
				}
			}
			
			foreach(int index in deleList)
			{
				if (index < infoList.Count) {
					GameUI.Instance.RootWidget.RemoveChild (infoList [index].Value);
					infoList.RemoveAt (index);
				}
			}
		}
	}
}


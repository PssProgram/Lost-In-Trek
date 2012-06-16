using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;

using Sce.Pss.Core;
using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Input;


using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace LostInTrek
{
	static public class EntryPoint
    {
		static Stopwatch stopwatch;
		static int frameCount;
        static int cpuTicks;
		
		public static void Run(string[] args)
		{
			stopwatch = Stopwatch.StartNew();
			
			uint sprites_capacity = 500;
			uint draw_helpers_capacity = 400;
			GraphicsContext context = new GraphicsContext(960,544,PixelFormat.Rgba, PixelFormat.Depth24, MultiSampleMode.Msaa4x);//建立Context
			Director.Initialize(sprites_capacity, draw_helpers_capacity, context);//调用基础构造
			Director.Instance.GL.Context.SetClearColor( 1,98,165,255 );
			Game.Instance = new Game();//主游戏类
			var game = Game.Instance;
			Sce.Pss.HighLevel.UI.UISystem.Initialize (context);// 初始化UI类
			Sce.Pss.HighLevel.UI.UISystem.SetScene (PSVX.Base.GameUI.Instance, null);
			
			//开启音乐线程
			PSVX.Base.SoundManager soundManager = new PSVX.Base.SoundManager
				(@"/Application/Content/Sound/",@"/Application/Content/Music/");//修改为你自己的目录
			Thread SoundManager = new Thread(new ThreadStart(soundManager.update));
			SoundManager.IsBackground = true;//设置为后台线程
			SoundManager.Start();//线程启动
			
			//开启游戏方法
			game.Main();//游戏主方法
			
			while (true)
            {
				int start = (int)stopwatch.ElapsedTicks;
				
				SystemEvents.CheckEvents();
				Director.Instance.GL.SetBlendMode(BlendMode.Normal);
                Director.Instance.Update();
                Director.Instance.Render();
                
				var gamePadData = GamePad.GetData (0);
				List<TouchData> touchDataList = Touch.GetData (0);//UI类的按键检测
				Sce.Pss.HighLevel.UI.UISystem.Update(touchDataList);
				PSVX.Base.TouchManager.Instance.BaceUpdate(touchDataList);
				Sce.Pss.HighLevel.UI.UISystem.Render ();
				
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap();
				
				cpuTicks += (int)stopwatch.ElapsedTicks - start;
				
				if ((++frameCount) % 100 == 0)
                {
					float freq = (float)Stopwatch.Frequency;
					float cpu = (float)cpuTicks * 60.0f / freq;
					PSVX.Base.DebugScene.Instance.CPUUpdate(cpu);
					cpuTicks = 0;
				}
			}
		}
	}
}

//游戏主运行方法
static class AppMain
{
	static void Main( string[] args )
	{
		Log.SetToConsole();
		LostInTrek.EntryPoint.Run(args);
	}
}
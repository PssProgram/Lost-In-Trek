using System;
using System.Text;
using System.IO;
using Sce.Pss.Core;
using System.Collections.Generic;

namespace PSVX.Base.AVG
{
	public enum ComanndType
	{
		ld,
		rd,
		say,
	}
		
	public class ScriptManager
	{
		
		public ScriptManager ()
		{
		}
		
		public Queue<ScriptComannd> LoadScript(string ScriptName)
		{
			DebugScene.Instance.WriteLine("載入腳本文件:" + ScriptName);
			
			Queue<ScriptComannd> ComanndQueue = new Queue<ScriptComannd>();;
			
			if(File.Exists(@"/Application/Content/Script/" + ScriptName + ".txt"))
			{
				StreamReader stream = new StreamReader(@"/Application/Content/Script/" + ScriptName + ".txt",Encoding.UTF8);

				string[] scriptText = stream.ReadToEnd().Replace("\n","").Split('\r');
				foreach(string s in scriptText)
				{
					if(s!= null)
					{
						ComanndQueue.Enqueue(SwitchComannd(s));
					}
				}
				stream.Close();
				DebugScene.Instance.WriteLine("腳本文件載入成功");
			}
			else
				DebugScene.Instance.WriteLine("腳本文件載入失敗");
			return ComanndQueue;
		}
		
		
		public ScriptComannd SwitchComannd(string Command)
		{
			if(Command != null)
			{
				string[] command = Command.Split(' ');
				string[] dir;
				ScriptComannd sc;
				switch(command[0])
				{
					//左立绘
					case "ld":
					sc = new ScriptComannd(){comanndType = ComanndType.ld};
					dir = command[1].Split(',');
					if(dir.Length == 1)
					{
						sc.Name = dir[0];
					}
					else if(dir.Length == 3)
					{
						sc.Position = new Vector2(float.Parse(dir[0]),float.Parse(dir[1]));
						sc.Name = dir[2];
					}
					return sc;
					
					//右立绘
					case "rd":
					sc = new ScriptComannd(){comanndType = ComanndType.rd};
					dir = command[1].Split(',');
					if(dir.Length == 1)
					{
						sc.Name = dir[0];
					}
					else if(dir.Length == 3)
					{
						sc.Position = new Vector2(float.Parse(dir[0]),float.Parse(dir[1]));
						sc.Name = dir[2];
					}
					return sc;
					
					//对话
					case "say":
					sc = new ScriptComannd(){comanndType = ComanndType.say};
					sc.text = int.Parse(command[1]);
					return sc;
				}
			}
			return null;
		}
	}
	
	public class ScriptComannd
	{
		public ComanndType comanndType{get;set;}
		public int text;
		public Vector2 Position;
		public string Name;
	}
}


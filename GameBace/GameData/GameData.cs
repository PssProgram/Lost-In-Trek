using System;
using System.IO;
using System.Collections.Generic;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PSVX.Base
{
	public static class GameData
	{
		public static Font font;
		public static Dictionary<string,TextureInfo> TextureInfoDictionary;
		
		public static List<SoundComannd> soundComannd = new List<SoundComannd>();
		public static List<MusicComannd> musicComannd = new List<MusicComannd>();
		
		public static List<string> AVGText;//AVG文字
		public static SortedList<int,PlanetData> PlanetDatas;
		public static SortedList<int,EventData> EventDatas;
		public static SortedList<int,MissionData> MissionDatas;
		
		//玩家信息
		public static MainData mainData;
		
		public static void LoadFont()
		{
			font = new Font(@"/Application/Content/Font/GameFont.ttf",30,FontStyle.Regular);
		}
		
		public static List<string> LoadText(string path)
		{
			DebugScene.Instance.WriteLine("開始讀取文本:" + path);
			List<string> List = null;
			string[] loadText = LoadFile ("Text/" + path);
			List = new List<string>();
			for(int i = 0;i<loadText.Length;i++)
			{
				List.Add(loadText[i].Split(',')[1]);
			}
			DebugScene.Instance.WriteLine("文本讀取成功");
			return List;
		}
		
		public static void LoadEventConfig()
		{
			SortedList<int,EventData> List = new SortedList<int, EventData>();
			string[] loadText = LoadFile("Config/Event.csv");
			if(loadText != null)
			{
				for(int i=1;i<loadText.Length;i++)
				{
					var eventData = new EventData();
					string[] Config = loadText[i].Split(',');
					if(Config.Length > 7)
					{
						eventData.Type = (EventType)int.Parse(Config[1]);
						eventData.Name = Config[2];
						eventData.MissionID = int.Parse(Config[3]);
						eventData.MissionSetp = int.Parse(Config[4]);
						eventData.Planet = int.Parse(Config[5]);
						eventData.ScriptName = Config[6];
						eventData.GoToMissionSetp = int.Parse(Config[7]);
						List.Add(int.Parse(Config[0]),eventData);
					}
				}
			}
			EventDatas = List;
			DebugScene.Instance.WriteLine("事件列表讀取成功");
		}
		
		public static void LoadMissionConfig()
		{
			SortedList<int,MissionData> List = new SortedList<int, MissionData>();
			string[] loadText = LoadFile("Config/Mission.csv");
			if(loadText != null)
			{
				for(int i=1;i<loadText.Length;i++)
				{
					var missionData = new MissionData();
					string[] Config = loadText[i].Split(',');
					if(Config.Length > 7)
					{
						missionData.Type = (MissionType)int.Parse(Config[1]);
						missionData.Name = Config[2];
						missionData.Describe = Config[3];
						if(Config[4] == "1")
							missionData.isLoop = true;
						else
							missionData.isLoop = false;
						int.TryParse(Config[5],out missionData.FollowMission);
						if(Config[6]!= "")
							missionData.PrestigeType = (PrestigeType)int.Parse(Config[6]);
						int.TryParse(Config[7],out missionData.PrestigeLevel);
						int.TryParse(Config[8],out missionData.Member);
						int.TryParse(Config[9],out missionData.Item);
						int.TryParse(Config[10],out missionData.Money);
						int.TryParse(Config[11],out missionData.AddPrestige);
						int.TryParse(Config[12],out missionData.AddMoney);
						int.TryParse(Config[13],out missionData.AddItem1);
						int.TryParse(Config[15],out missionData.AddItem2);
						int.TryParse(Config[14],out missionData.AddItem1Num);
						int.TryParse(Config[16],out missionData.AddItem2Num);
						int.TryParse(Config[17],out missionData.SpecialAwards);
						List.Add(int.Parse(Config[0]),missionData);
					}
				}
				MissionDatas = List;
			}
		}
		
		public static void LoadPlanetConfig()
		{
			SortedList<int,PlanetData> List = new SortedList<int, PlanetData>();
			string[] loadText = LoadFile ("Config/Planet.csv");
			for(int i=1;i<loadText.Length;i++)
			{
				var PlanetData = new PlanetData();
				string[] Config = loadText[i].Split(',');
				if(Config.Length > 6)
				{
					PlanetData.Name = Config[1];
					PlanetData.BgName = Config[2];
					PlanetData.Shipyard = int.Parse(Config[3]);
					PlanetData.Government = int.Parse(Config[4]);
					PlanetData.Market = int.Parse(Config[5]);
					PlanetData.Bar = int.Parse(Config[6]);
					List.Add(int.Parse(Config[0]),PlanetData);
				}
			}
			PlanetDatas = List;
			DebugScene.Instance.WriteLine("星球列表讀取成功:共" + List.Count + "个");
		}

		static string[] LoadFile (string path)
		{
			if(File.Exists(@"/Application/Content/" + path))
			{
				StreamReader stream = new StreamReader(@"/Application/Content/" + path,System.Text.Encoding.UTF8);
				string[] loadText = stream.ReadToEnd().Replace("\n","").Split('\r');
				stream.Close();
				return loadText;
			}
			return null;
		}
		
		/// <summary>
		/// 载入纹理信息
		/// </summary>
		/// <param name='path'>
		/// 纹理地址
		/// </param>
		public static void LoadTextureInfo(string path)
		{
			Texture2D texture;
			if(System.IO.File.Exists(@"/Application/Content/Pic/" + path))
			{
				texture = new Texture2D (@"/Application/Content/Pic/" + path, false);
				if(TextureInfoDictionary == null)
					TextureInfoDictionary = new Dictionary<string, TextureInfo>();
				TextureInfoDictionary.Add(path,new TextureInfo(texture));
			}
			else
				DebugScene.Instance.WriteLine("未找到文件:Pic/" + path);
		}
		
		/// <summary>
		/// 载入纹理信息
		/// </summary>
		/// <param name='path'>
		/// 纹理地址
		/// </param>
		/// <param name='cutImage'>
		/// 切分的块数
		/// </param>
		public static void LoadTextureInfo(string path,Vector2i cutImage)
		{
			Texture2D texture;
			if(System.IO.File.Exists(@"/Application/Content/Pic/" + path))
			{
				texture = new Texture2D (@"/Application/Content/Pic/" + path, false);
				if(TextureInfoDictionary == null)
					TextureInfoDictionary = new Dictionary<string, TextureInfo>();
				TextureInfoDictionary.Add(path,new TextureInfo (texture, new Vector2i (cutImage.Y, cutImage.X), TRS.Quad0_1));
			}
			else
				DebugScene.Instance.WriteLine("未找到文件:Pic/" + path);
		}
	}
}


using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

using Sce.Pss.Core;
using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;
using Sce.Pss.Core.Audio;

using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PSVX.Base
{
	public enum SoundComanndStatus
	{
		Play,
		Stop,
		Pause,
		Resume,
		ChangeVolume,
	}
	
	public class SoundManager
	{
		string SoundPath;//Sound目录
		string MusicPath;//Music目录
		Dictionary<string, SoundPlayer> SoundDictionary;
		BgmPlayer MusicPlayer;
		List<Bgm> MusicList;
		SoundComanndStatus musicStatus = SoundComanndStatus.Stop;
		bool musicLoop;
		float musicVolume = 100;//音乐音量
		int musicSoundIn = 5000;//音乐淡入毫秒
		int musicSoundOut = 5000;//音乐淡出毫秒
		
		public SoundManager (string SoundPath,string MusicPath)
		{
			this.SoundPath = SoundPath;
			this.MusicPath = MusicPath;
			SoundDictionary = new Dictionary<string, SoundPlayer>();
			MusicList = new List<Bgm>(2);
		}
		
		/// <summary>
		/// 判断音乐是否加装
		/// </summary>
		/// <param name='name'>
		/// 音效名称
		/// </param>
		private void SoundCheckCache(string name)
		{
			if (SoundDictionary.ContainsKey(name))
				return;
			
			var sound = new Sound(SoundPath + name);
			var player = sound.CreatePlayer();
			SoundDictionary[name] = player;
		}
		
		/// <summary>
		/// 播放音效
		/// </summary>
		/// <param name='name'>
		/// 音效名称
		/// </param>
		public void SoundPlay(string name,float volume,bool loop)
		{
			SoundStop(name);
			SoundDictionary[name].Volume = volume;
			SoundDictionary[name].Loop = loop;
			SoundDictionary[name].Play();
		}
		
		/// <summary>
		/// 停止播放音乐音效
		/// </summary>
		/// <param name='name'>
		/// 音效名称
		/// </param>
		public void SoundStop(string name)
		{
			SoundCheckCache(name);
			SoundDictionary[name].Stop();
		}
		
		/// <summary>
		/// 音效静音
		/// </summary>
		/// <param name='name'>
		/// 音效名称
		/// </param>
		public void MuteSound(string name)
		{
			if(SoundDictionary.ContainsKey(name))
				SoundDictionary[name].Volume = 0;
		}
		
		/// <summary>
		/// 音效恢复
		/// </summary>
		/// <param name='name'>
		/// 音效名称
		/// </param>
		public void ResumeSound(string name)
		{
			if(SoundDictionary.ContainsKey(name))
				SoundDictionary[name].Volume = 100;
		}
		
		/// <summary>
		/// 停止播放音乐
		/// </summary>
		public void MusicStop()
		{
			musicStatus = SoundComanndStatus.Stop;
		}
		
		/// <summary>
		/// 播放音乐
		/// </summary>
		/// <param name='name'>
		/// 音乐名称
		/// </param>
		public void MusicPlay(string name,float volume,bool loop)
		{
			var music = new Bgm(MusicPath + name);
			musicLoop = loop;
			musicVolume = volume;
			MusicList.Add(music);
			musicStatus = SoundComanndStatus.Play;
		}
		
		/// <summary>
		/// 建立音乐播放器
		/// </summary>
		public void CreatMusicPlayer(Bgm music)
		{
			MusicPlayer = music.CreatePlayer();
			MusicPlayer.Volume = 0;
			MusicPlayer.Loop = musicLoop;
			MusicPlayer.Play();
		}
		
		/// <summary>
		/// 暂停音乐
		/// </summary>
		public void MusicPause()
		{
			if(MusicPlayer != null && MusicPlayer.Status == BgmStatus.Playing)
				MusicPlayer.Pause();
			musicStatus = SoundComanndStatus.Pause;
		}
		
		/// <summary>
		/// 恢复音乐
		/// </summary>
		public void MusicResume()
		{
			if(MusicPlayer != null && MusicPlayer.Status == BgmStatus.Paused)
				MusicPlayer.Resume();
			musicStatus = SoundComanndStatus.Play;
		}
		
		/// <summary>
		/// 更改音乐音量
		/// </summary>
		/// <param name='volume'>
		/// 音乐音量
		/// </param>
		public void ChangeMusicVolume(float volume)
		{
			if(MusicPlayer.Status == BgmStatus.Playing)
				MusicPlayer.Volume = volume;
			musicVolume = volume;
		}
		
		/// <summary>
		/// 音乐淡入淡出管理器
		/// </summary>
		public void MusicManager()
		{
			if(musicStatus == SoundComanndStatus.Play)
			{
				if(MusicList.Count == 1)
				{
					if(MusicPlayer == null)
					{
						CreatMusicPlayer(MusicList[0]);
					}
					else if(MusicPlayer.Status == BgmStatus.Playing && MusicPlayer.Volume < musicVolume)
					{
						if((MusicPlayer.Volume += musicVolume/(float)(musicSoundIn / 200)) <= musicVolume)
						{
							MusicPlayer.Volume += musicVolume/(float)(musicSoundIn / 200);
						}
						else
							MusicPlayer.Volume = musicVolume;
					}
				}
				else if(MusicList.Count >= 2)
				{
					musicStatus = SoundComanndStatus.Stop;
				}
			}
			else if(musicStatus == SoundComanndStatus.Stop)
			{
				if(MusicPlayer == null)
				{
					MusicList.RemoveAt(0);
					if(MusicList.Count == 1)
					{
						musicStatus = SoundComanndStatus.Play;
					}
				}
				else if(MusicPlayer.Status == BgmStatus.Playing && MusicPlayer.Volume>0)
				{
					if((MusicPlayer.Volume -= musicVolume/(float)(musicSoundOut / 200)) >= 0)
					{
						MusicPlayer.Volume -= musicVolume/(float)(musicSoundOut / 200);
					}
					else
						MusicPlayer.Volume = 0;
				}
				else if(MusicPlayer.Volume <= 0)
				{
					MusicPlayer.Stop();
					MusicPlayer.Dispose();
					MusicPlayer = null;
					MusicList.RemoveAt(0);
					if(MusicList.Count == 1)
					{
						musicStatus = SoundComanndStatus.Play;
					}
				}
			}
		}
		
		/// <summary>
		/// 音效回收
		/// </summary>
		public void SoundKiller()
		{
			List<string> keyList = new List<string>();
			foreach(string key in SoundDictionary.Keys)
			{
				if(SoundDictionary[key].Status == SoundStatus.Stopped)
				{
					SoundDictionary[key].Dispose();
					keyList.Add(key);
				}
			}
			foreach(string key in keyList)
			{
				SoundDictionary.Remove(key);
			}
		}
		
		/// <summary>
		/// 循环方法
		/// </summary>
		public void update()
		{
			bool run = true;
			while(run)
			{
				while(GameData.soundComannd.Count>0)
				{
					SwitchSoundComannd(GameData.soundComannd[0]);
					GameData.soundComannd.RemoveAt(0);
				}
				while(GameData.musicComannd.Count>0)
				{
					SwitchMusicComannd(GameData.musicComannd[0]);
					GameData.musicComannd.RemoveAt(0);
				}
				if(MusicList.Count>0)
				{
					MusicManager();
				}
				if(SoundDictionary.Count>0)
				{
					SoundKiller();
				}
				Thread.Sleep(200);
			}
		}
		
		private void SwitchSoundComannd(SoundComannd soundComannd)
		{
			if(soundComannd.comannd == SoundComanndStatus.Play)
			{
				float volume;
				bool loop = soundComannd.loop;
				string name = soundComannd.soundName;
				//音量
				if(soundComannd.soundVolume<=100 && soundComannd.soundVolume>=0)
					volume = (float)soundComannd.soundVolume/100;
				else volume = 1.0f;
				SoundPlay(name,volume,loop);
			}
			else if(soundComannd.comannd == SoundComanndStatus.Stop)
				SoundStop(soundComannd.soundName);
			else if(soundComannd.comannd == SoundComanndStatus.Pause)
				MuteSound(soundComannd.soundName);
			else if(soundComannd.comannd == SoundComanndStatus.Resume)
				ResumeSound(soundComannd.soundName);
		}
		
		private void SwitchMusicComannd(MusicComannd musicComannd)
		{
			if(musicComannd.comannd == SoundComanndStatus.Play)
			{
				float volume;
				bool loop = musicComannd.loop;
				if(musicComannd.musicVolume<=100 && musicComannd.musicVolume>=0)
					volume = (float)musicComannd.musicVolume/100;
				else volume = 1.0f;
				MusicPlay(musicComannd.musicName,volume,loop);
			}
			else if(musicComannd.comannd == SoundComanndStatus.Stop)
				MusicStop();
			else if(musicComannd.comannd == SoundComanndStatus.Pause)
				MusicPause();
			else if(musicComannd.comannd == SoundComanndStatus.Resume)
				MusicResume();
			else if(musicComannd.comannd == SoundComanndStatus.ChangeVolume)
			{
				if(musicComannd.musicVolume<=100 && musicComannd.musicVolume>=0)
				{
					float volume;
					volume = (float)musicComannd.musicVolume/100;
					ChangeMusicVolume(volume);
				}
			}
		}
	}
	
	
	public class SoundComannd
	{
		public string soundName;
		public int soundVolume;
		public bool loop;
		public SoundComanndStatus comannd;
		public SoundComannd(string name,SoundComanndStatus comannd,int soundVolume,bool loop)
		{
			soundName = name;
			this.comannd = comannd;
			this.soundVolume = soundVolume;
			this.loop = loop;
		}
	}
	
	public class MusicComannd
	{
		public string musicName;
		public SoundComanndStatus comannd;
		public int musicVolume;
		public bool loop;
		public MusicComannd(string name,SoundComanndStatus comannd,int musicVolume,bool loop)
		{
			musicName = name;
			this.comannd = comannd;
			this.musicVolume = musicVolume;
			this.loop = loop;
		}
	}
}


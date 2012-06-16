using System.Collections;
using System.Collections.Generic;

using Sce.Pss.Core;
using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;

using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PSVX.Base
{
	public class GameScene:Scene
	{
		public Dictionary<string,SpriteX> spriteDictionary;
		
		public GameScene ()
		{
			this.Camera.SetViewFromViewport();
			this.spriteDictionary = new Dictionary<string, SpriteX>();
		}
		
		public virtual void Start(){}
		
		public virtual void SetUp(int StartID){}
		
		public virtual void Exit()
		{
			this.Cleanup();
			if(GameData.TextureInfoDictionary != null)
				GameData.TextureInfoDictionary.Clear();
			Scheduler.Instance.Unschedule(this,this.UpdateFrame);
		}
		
		public virtual void UpdateFrame(float dt){}
		
		/// <summary>
		/// 同Draw，但精灵不显示
		/// </summary>
		/// <returns>
		/// 创建的精灵
		/// </returns>
		/// <param name='name'>
		/// 精灵名称
		/// </param>
		/// <param name='path'>
		/// 纹理地址
		/// </param>
		public SpriteX LoadImage(Layer layer,string name,string path)
		{
			SpriteX sprite = DrawImage(layer,name,path);
			sprite.Visible = false;
			return sprite;
		}
		
		/// <summary>
		/// 同Draw，但精灵不显示
		/// </summary>
		/// <returns>
		/// 创建的精灵
		/// </returns>
		/// <param name='name'>
		/// 精灵名称
		/// </param>
		/// <param name='path'>
		/// 纹理地址
		/// </param>
		/// <param name='vector'>
		/// 精灵坐标
		/// </param>
		public SpriteX LoadImage(Layer layer,string name,string path,Vector2 vector)
		{
			SpriteX sprite = DrawImage(layer,name,path,vector);
			sprite.Visible = false;
			return sprite;
		}
		
		/// <summary>
		/// 同Draw，但精灵不显示
		/// </summary>
		/// <returns>
		/// 创建的精灵
		/// </returns>
		/// <param name='name'>
		/// 精灵名称
		/// </param>
		/// <param name='path'>
		/// 纹理地址
		/// </param>
		/// <param name='vector'>
		/// 精灵坐标
		/// </param>
		/// <param name='cutImage'>
		/// 切分的块数
		/// </param>
		/// <param name='showPiece'>
		/// 显示的图片部分坐标
		/// </param>
		public SpriteX LoadImage(Layer layer,string name,string path,Vector2 vector,Vector2i cutImage,Vector2i showPiece)
		{
			SpriteX sprite = DrawImage(layer,name,path,vector,cutImage,showPiece);
			sprite.Visible = false;
			return sprite;
		}
		
		/// <summary>
		/// 同Draw，但精灵不显示
		/// </summary>
		/// <returns>
		/// 创建的精灵
		/// </returns>
		/// <param name='name'>
		/// 精灵名称
		/// </param>
		/// <param name='path'>
		/// 纹理地址
		/// </param>
		/// <param name='vector'>
		/// 精灵坐标
		/// </param>
		/// <param name='rectangle'>
		/// 表示显示起始坐标及结束坐标的矩形
		/// </param>
		public SpriteX LoadImage(Layer layer,string name,string path,Vector2 vector,BoundBox rectangle)
		{
			SpriteX sprite = DrawImage(layer,name,path,vector,rectangle);
			sprite.Visible = false;
			return sprite;
		}
		
		//Draw指令
		
		/// <summary>
		/// 绘制一个新的精灵，默认在左上角
		/// </summary>
		/// <returns>
		/// 创建的精灵
		/// </returns>
		/// <param name='name'>
		/// 精灵名称
		/// </param>
		/// <param name='path'>
		/// 纹理地址
		/// </param>
		public SpriteX DrawImage(Layer layer,string name,string path)
		{
			SpriteX sprite = DrawImage (layer,name,path,Vector2.Zero);
			return sprite;
		}
		
		/// <summary>
		/// 绘制一个新的精灵，可设置精灵坐标
		/// </summary>
		/// <returns>
		/// 创建的精灵
		/// </returns>
		/// <param name='name'>
		/// 精灵名称
		/// </param>
		/// <param name='path'>
		/// 纹理地址
		/// </param>
		/// <param name='vector'>
		/// 精灵坐标
		/// </param>
		public SpriteX DrawImage(Layer layer,string name,string path,Vector2 vector)
		{
			SpriteX sprite = DrawImage (layer,name,path,vector,new Vector2i(1,1),new Vector2i(1,1));
			return sprite;
		}
		
		/// <summary>
		/// 绘制一个新的精灵，可设置精灵坐标以及进行切分
		/// </summary>
		/// <returns>
		/// 创建的精灵
		/// </returns>
		/// <param name='name'>
		/// 精灵名称
		/// </param>
		/// <param name='path'>
		/// 纹理地址
		/// </param>
		/// <param name='vector'>
		/// 精灵坐标
		/// </param>
		/// <param name='cutImage'>
		/// 切分的块数
		/// </param>
		/// <param name='showPiece'>
		/// 显示的图片部分坐标
		/// </param>
		public SpriteX DrawImage(Layer layer,string name,string path,Vector2 vector,Vector2i cutImage,Vector2i showPiece)
		{
			var sprite = new SpriteX(layer,path,vector,cutImage,showPiece);
			sprite.Name = name;
			spriteDictionary.Add(name,sprite);
			return sprite;
		}
		
		/// <summary>
		/// 绘制一个新的精灵，可设置坐标及显示部分
		/// </summary>
		/// <returns>
		/// 创建的精灵
		/// </returns>
		/// <param name='name'>
		/// 精灵名称
		/// </param>
		/// <param name='path'>
		/// 纹理地址
		/// </param>
		/// <param name='vector'>
		/// 精灵坐标
		/// </param>
		/// <param name='rectangle'>
		/// 表示显示起始坐标及结束坐标的矩形
		/// </param>
		public SpriteX DrawImage(Layer layer,string name,string path,Vector2 vector,BoundBox rectangle)
		{
			var sprite = new SpriteX(layer,path,vector,rectangle);
			sprite.Name = name;
			spriteDictionary.Add(name,sprite);
			return sprite;
		}
		
		//ScreenClear指令
		
		public void ScreenClear()
		{
			foreach(string s in spriteDictionary.Keys)
			{
			   	spriteDictionary[s].Visible=false;
			}
		}
		
		//Remove指令
		
		/// <summary>
		/// 移除对应名称的精灵
		/// </summary>
		/// <param name='name'>
		/// 精灵名称
		/// </param>
		public void RemoveImage(string name)
		{
			if(spriteDictionary.ContainsKey(name))
			{
				spriteDictionary[name].Remove();
				spriteDictionary.Remove(name);
			}
		}
		
		/// <summary>
		/// 移除所有的精灵
		/// </summary>
		public void RemoveAllImage()
		{
			foreach(string s in spriteDictionary.Keys)
			{
				spriteDictionary[s].Remove();
			}
			spriteDictionary.Clear();
		}
		
		//Hidden指令
		
		/// <summary>
		/// 隐藏指定精灵
		/// </summary>
		/// <param name='name'>
		/// 精灵名称
		/// </param>
		public void HiddenImage(string name)
		{
			GetSprite(name).Visible = false;
		}
		
		//Show指令
		
		/// <summary>
		/// 显示精灵
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		public void ShowImage(string name)
		{
			if(spriteDictionary.ContainsKey(name))
			{
				spriteDictionary[name].Visible = true;
			}
		}
		
		/// <summary>
		/// 显示所有精灵
		/// </summary>
		public void ShowAllImage()
		{
			foreach(string s in spriteDictionary.Keys)
			{
				if(!spriteDictionary[s].Visible)
					spriteDictionary[s].Visible = true;
			}
		}
		
		//Get指令
		
		/// <summary>
		/// 获取对应名称的精灵
		/// </summary>
		/// <returns>
		/// 指定名称的精灵
		/// </returns>
		/// <param name='name'>
		/// 精灵名称
		/// </param>
		public SpriteX GetSprite(string name)
		{
			if(spriteDictionary.ContainsKey(name))
			{
				return spriteDictionary[name];
			}
			else
				return null;
		}
		
		//音乐指令
		
		/// <summary>
		/// 播放音乐（默认循环播放）
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		public void playMusic(string name)
		{
			playMusic(name,100);
		}
		
		/// <summary>
		/// 播放音乐，可设置音量（默认循环播放）
		/// </summary>
		/// <param name='name'>
		/// 音乐名称
		/// </param>
		/// <param name='volume'>
		/// 音乐音量（0~100）
		/// </param>
		public void playMusic(string name,int volume)
		{
			PlayMusic(name,volume,true);
		}
		
		/// <summary>
		/// 播放音乐，可设置音量和是否循环
		/// </summary>
		/// <param name='name'>
		/// 音乐名称
		/// </param>
		/// <param name='volume'>
		/// 音乐音量（0~100）
		/// </param>
		/// <param name='loop'>
		/// 是否循环
		/// </param>
		public void PlayMusic(string name,int volume,bool loop)
		{
			GameData.musicComannd.Add(new MusicComannd(name,SoundComanndStatus.Play,volume,loop));
		}
		
		/// <summary>
		/// 暂停音乐
		/// </summary>
		public void PauseMusic()
		{
			GameData.musicComannd.Add(new MusicComannd(null,SoundComanndStatus.Pause,0,false));
		}
		
		/// <summary>
		/// 恢复音乐
		/// </summary>
		public void ResumeMusic()
		{
			GameData.musicComannd.Add(new MusicComannd(null,SoundComanndStatus.Resume,0,false));
		}
		
		/// <summary>
		/// 停止音乐（删除音乐）
		/// </summary>
		public void StopMusic()
		{
			GameData.musicComannd.Add(new MusicComannd(null,SoundComanndStatus.Stop,0,false));
		}
		
		/// <summary>
		/// 改变音乐音量
		/// </summary>
		/// <param name='volume'>
		/// 音乐音量.(0~100)
		/// </param>
		public void MusicVolume(int volume)
		{
			GameData.musicComannd.Add(new MusicComannd(null,SoundComanndStatus.ChangeVolume,volume,false));
		}
		
		//音效指令
		
		/// <summary>
		/// 播放音效
		/// </summary>
		/// <param name='name'>
		/// 音效名称
		/// </param>
		public void PlaySound(string name)
		{
			PlaySound(name,100);
		}
		
		/// <summary>
		/// 播放音效，可设置音量（默认不循环）
		/// </summary>
		/// <param name='name'>
		/// 音效名称
		/// </param>
		/// <param name='volume'>
		/// 音效音量(0~100)
		/// </param>
		public void PlaySound(string name,int volume)
		{
			PlaySound(name,volume,false);
		}
		
		/// <summary>
		/// 播放音效，可设置音量及是否循环
		/// </summary>
		/// <param name='name'>
		/// 音效名称
		/// </param>
		/// <param name='volume'>
		/// 音效音量(0~100)
		/// </param>
		/// <param name='loop'>
		/// 是否循环
		/// </param>
		public void PlaySound(string name,int volume,bool loop)
		{
			GameData.soundComannd.Add(new SoundComannd(name,SoundComanndStatus.Play,volume,loop));
		}
		
		/// <summary>
		/// 停止播放音效
		/// </summary>
		/// <param name='name'>
		/// 音效名称
		/// </param>
		public void StopSound(string name)
		{
			GameData.soundComannd.Add(new SoundComannd(name,SoundComanndStatus.Stop,0,false));
		}
		
		/// <summary>
		/// 音效静音
		/// </summary>
		/// <param name='name'>
		/// 音效名称
		/// </param>
		public void MuteSound(string name)
		{
			GameData.soundComannd.Add(new SoundComannd(name,SoundComanndStatus.Pause,0,false));
		}
		
		/// <summary>
		/// 恢复音效
		/// </summary>
		/// <param name='name'>
		/// 音效名称
		/// </param>
		public void ResumeSound(string name)
		{
			GameData.soundComannd.Add(new SoundComannd(name,SoundComanndStatus.Resume,0,false));
		}
		
		/// <summary>
		/// 移动摄像机.
		/// </summary>
		public void MoveCamera(Vector2 v2)
		{
			if(v2 == Vector2.Zero)
				return;
			
			this.Camera2D.Center += new Vector2(v2.X,-v2.Y);
		}
	}
}


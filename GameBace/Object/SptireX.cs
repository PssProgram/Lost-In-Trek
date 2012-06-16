using System;

using Sce.Pss.Core;
using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;

using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PSVX.Base
{
	public class SpriteX:SpriteTile
	{
		public Texture2D texture;
		public Node fatherNode;
		public Animation animation;
		public bool isPause = false;
		
		/// <summary>
		/// 当前方向，XNA坐标系, 调用旋转(Rotate())后，此方向也会更新， 
		/// 注意，旋转的初始方向是 水平向左
		/// </summary>
		public Vector2 NowDirection {
			get {
				float x = -Rotation.X;
				float y = Rotation.Y;
				return new Vector2 (x, y);
			}
		}
		
		/// <summary>
		/// PSVX精灵类
		/// </summary>
		/// <param name='scene'>
		/// 精灵所在的场景
		/// </param>
		/// <param name='path'>
		/// 精灵纹理
		/// </param>
		/// <param name='vector'>
		/// 精灵坐标
		/// </param>
		public SpriteX (Node node, string path, Vector2 vector)
			:this(node,path,vector,new Vector2i(1,1),new Vector2i(1,1))
		{
		}
		
		/// <summary>
		/// PSVX精灵类
		/// </summary>
		/// <param name='scene'>
		/// 精灵所在场景
		/// </param>
		/// <param name='path'>
		/// 精灵纹理
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
		public SpriteX (Node node, string path, Vector2 vector, Vector2i cutImage, Vector2i showPiece)
		{
			this.fatherNode = node;
			node.AddChild (this);
			Scheduler.Instance.Schedule (this, UpdateFrame, 0.0f, false);
			if(System.IO.File.Exists(@"/Application/Content/Pic/" + path))
				texture = new Texture2D (@"/Application/Content/Pic/" + path, false);
			else
				DebugScene.Instance.WriteLine("未找到文件:Pic/" + path);
			this.TextureInfo = new TextureInfo (texture, new Vector2i (cutImage.Y, cutImage.X), TRS.Quad0_1);
			this.TileIndex2D = new Vector2i (showPiece.Y - 1, cutImage.X - showPiece.X);
			this.Quad.S = new Vector2 (texture.Width / cutImage.Y, texture.Height / cutImage.X);
			this.CenterSprite (TRS.Local.TopLeft);
			this.Position = new Vector2 (vector.X, 544.0f - vector.Y);
		}
		
		/// <summary>
		/// PSVX精灵类
		/// </summary>
		/// <param name='node'>
		/// 精灵所在层
		/// </param>
		/// <param name='path'>
		/// 精灵名称
		/// </param>
		/// <param name='vector'>
		/// 精灵坐标
		/// </param>
		public SpriteX (string path, Vector2 vector)
		{
			Scheduler.Instance.Schedule (this, UpdateFrame, 0.0f, false);
			if(GameData.TextureInfoDictionary.ContainsKey(path))
			{
				this.TextureInfo = GameData.TextureInfoDictionary[path];
				this.Quad.S = TextureInfo.TileSizeInPixelsf;
				this.CenterSprite (TRS.Local.TopLeft);
				this.Position = new Vector2 (vector.X, 544.0f - vector.Y);
			}
			else
				DebugScene.Instance.WriteLine("未载入图片:" + path);
		}
		 
		/// <summary>
		/// PSVX精灵类
		/// </summary>
		/// <param name='node'>
		/// 精灵所在层
		/// </param>
		/// <param name='scene'>
		/// 精灵所在场景
		/// </param>
		/// <param name='path'>
		/// 精灵名称
		/// </param>
		/// <param name='vector'>
		/// 精灵坐标
		/// </param>
		/// <param name='showPiece'>
		/// 显示的图片部分坐标
		/// </param>
		public SpriteX (string path, Vector2 vector,Vector2i showPiece)
			:this(path,vector)
		{
			this.TileIndex2D = new Vector2i (showPiece.Y - 1, this.TextureInfo.NumTiles.Y - showPiece.X);
		}
		
		/// <summary>
		/// PSVX精灵类
		/// </summary>
		/// <param name='scene'>
		/// 精灵所在场景
		/// </param>
		/// <param name='path'>
		/// 精灵纹理
		/// </param>
		/// <param name='vector'>
		/// 精灵坐标
		/// </param>
		/// <param name='rectangle'>
		/// 切分的矩形.
		/// </param>
		public SpriteX (Node node, string path, Vector2 vector, BoundBox rectangle)
		{
			if(System.IO.File.Exists(@"/Application/Content/Pic/" + path))
				this.TextureInfo = new TextureInfo (CutTexture (@"/Application/Content/Pic/" + path, rectangle));
			else
				DebugScene.Instance.WriteLine("未找到文件:Pic/" + path);
			this.Quad.S = TextureInfo.TextureSizef;
			this.CenterSprite (TRS.Local.TopLeft);
			this.Position = new Vector2 (vector.X, 544.0f - vector.Y);
			this.fatherNode = node;
			node.AddChild (this);
		}
		
		private Texture2D CutTexture (string path, BoundBox rectangle)
		{
			ImageRect imageRect = new ImageRect ((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);
			if(System.IO.File.Exists(@"/Application/Content/Pic/" + path))
			{
				Image image = new Image (@"/Application/Content/Pic/" + path);
				image.Decode ();
			
			texture = new Texture2D ((int)rectangle.Width, (int)rectangle.Height, false, PixelFormat.Rgba);
			texture.SetPixels (0, image.Crop (imageRect).ToBuffer ());
			image.Dispose ();
			}
			else
				DebugScene.Instance.WriteLine("未找到文件:Pic/" + path);
			return texture;
		}
		
		internal virtual void UpdateFrame (float dt)
		{
			
		}
		
		/// <summary>
		/// 设置精灵坐标
		/// </summary>
		/// <param name='vector'>
		/// 精灵坐标
		/// </param>
		public void SetPosition (Vector2 vector)
		{
			this.Position = new Vector2 (vector.X, 544.0f - vector.Y);
		}
		
		/// <summary>
		/// 获取精灵坐标
		/// </summary>
		/// <param name='vector'>
		/// 精灵坐标
		/// </param>
		public Vector2 GetPosition ()
		{
			return  new Vector2 (Position.X, 544f - Position.Y);
		}
		
		/// <summary>
		/// 播放动画
		/// </summary>
		/// <param name='speed'>
		/// 动画速度
		/// </param>
		public void RunAnimation (float speed)
		{
			animation = new Animation (this, speed);
			this.RunAction (animation);
		}
		
		public void RunAnimation (int a, int b, float speed, bool loop)
		{
			animation = new Animation (this, ReIndex1D (a), ReIndex1D (b), speed, loop);
			this.RunAction (animation);
		}
		
		/// <summary>
		/// 精灵增量移动
		/// </summary>
		/// <param name='vector'>
		/// 增量坐标
		/// </param>
		/// <param name='speed'>
		/// 速度
		/// </param>
		public void Moveby (Vector2 vector, float speed)
		{
			var moveBy = new MoveBy (new Vector2 (vector.X, -vector.Y), speed);
			this.RunAction (moveBy);
		}
		
		/// <summary>
		/// 删除精灵
		/// </summary>
		public void Remove ()
		{
			texture.Dispose ();
			Scheduler.Instance.Unschedule(this,UpdateFrame);
			fatherNode.RemoveChild (this, true);
		}
		
		/// <summary>
		/// 暂停精灵
		/// </summary>
		public void Pause()
		{
			if(!isPause)
			{
				Scheduler.Instance.Unschedule(this,UpdateFrame);
				fatherNode.RemoveChild(this,false);
				isPause = true;
			}
		}
		
		public void Continue()
		{
			if(isPause)
			{
				Scheduler.Instance.Schedule (this, UpdateFrame, 0.0f, false);
				fatherNode.AddChild(this);
				isPause = false;
			}
		}
		
		/// <summary>
		/// 播放一帧动画
		/// </summary>
		/// <param name='start'>
		/// 起始Index
		/// </param>
		/// <param name='end'>
		/// 结束Index
		/// </param>
		public void RunOneFrame (Vector2i start, Vector2i end)
		{
			start = ReIndex2D (start);
			end = ReIndex2D (end);
			if (TileIndex2D.Y > end.Y && TileIndex2D.Y < start.Y) {
				if (TileIndex2D.X < TextureInfo.NumTiles.X - 1)
					TileIndex2D.X += 1;
				else {
					TileIndex2D.X = 0;
					TileIndex2D.Y -= 1;
				}
			} else if (TileIndex2D.Y == end.Y) {
				if (TileIndex2D.X < end.X) {
					TileIndex2D.X += 1;
				} else
					TileIndex2D = start;
			} else if (TileIndex2D.Y == start.Y && TileIndex2D.X >= start.X) {
				if (TileIndex2D.X < TextureInfo.NumTiles.X - 1)
					TileIndex2D.X += 1;
				else {
					TileIndex2D.X = 0;
					TileIndex2D.Y -= 1;
				}
			} else {
				TileIndex2D = start;
			}
		}
		
		/// <summary>
		/// 调转2D索引
		/// </summary>
		/// <returns>
		/// 反向索引
		/// </returns>
		/// <param name='vector'>
		/// 正向索引
		/// </param>
		private Vector2i ReIndex2D (Vector2i vector)
		{
			return new Vector2i (vector.Y - 1, TextureInfo.NumTiles.Y - vector.X);
		}
		
		private int ReIndex1D (int index1D)
		{
			int c = TextureInfo.NumTiles.X;
			int r = TextureInfo.NumTiles.Y;
			int x = index1D;
			int D1 = x + (r * c - c - 1) - ((x - 1) / c) * 2 * c;
			return D1;
		}
		
		/// <summary>
		/// 将自身加入Child
		/// </summary>
		/// <param name='scene'>
		/// 场景名称
		/// </param>
		public void ChangeFather (Node node)
		{
			if(fatherNode != null)
				fatherNode.RemoveChild (this, false);
			fatherNode = node;
			node.AddChild (this);
		}
		
		public Bounds2 GetBounds ()
		{
			Bounds2 bounds = new Bounds2 (new Vector2 (this.Position.X, 544 - this.Position.Y), new Vector2 (this.Position.X + this.Quad.S.X, this.Quad.S.Y + 544 - this.Position.Y));
			return bounds;
		}
	}
}


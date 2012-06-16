using System;

using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PSVX.Base
{
	/// <summary>
	/// 动画演出
	/// </summary>
	public class Animation:ActionBase
	{
		int start;
		int end;
		SpriteTile actionSprite;
		float counter;
		float frame_time;//帧时间
		float speed;
		bool loop;
		
		public Animation(SpriteTile sprite,float seconds)
				: this(sprite, 0, sprite.TextureInfo.NumTiles.X * sprite.TextureInfo.NumTiles.Y - 1, seconds,true)
		{
		}
		
		public Animation(SpriteTile sprite, int a, int b, float seconds, bool loop)
		{
			this.loop = loop;
			speed = 1.0f;
			
			actionSprite = sprite;
			
			int min = System.Math.Min(a,b);//最大值
			int max = System.Math.Max(a,b);//最小值
			int frames = System.Math.Max(1, max - min);//帧数
			
			frame_time = seconds;
			start = min;
			end = max;
		}
		
		public override void Run()
		{
			base.Run();
			Reset();
		}
		
		public override void Update(float dt)
		{
			dt *= speed;
			base.Update(dt);
			
			counter += dt;
			
			int frames = 0;
			while (frame_time > 0.0f && counter > frame_time)
			{
				counter -= frame_time;
				frames += 1;
			}
				
			int tile_index = IncrementTile(actionSprite, frames, start, end, loop);
				
			if (!loop && tile_index == end)
			{
				Stop();
			}
		}
		
		public void SetSpeed(float speed)
		{
			this.speed = speed;
		}
		
		public void Reset()
		{
			counter = 0.0f;
			SetTile(actionSprite, start);
		}
		
		public static void SetTile(SpriteTile sprite, int n)
		{
            sprite.TileIndex1D = n;
		}
		
		public static int IncrementTile(SpriteTile sprite, int steps, int min, int max, bool loop)
		{
			int x = sprite.TextureInfo.NumTiles.X;
			int y = sprite.TextureInfo.NumTiles.Y;
			
			int current = sprite.TileIndex2D.X + sprite.TileIndex2D.Y * x;
			
			if (loop)
			{
				current -= min;
				current += steps;
				current %= max - min;
				current += min;
			}
			else
			{
				current += steps;
				current = System.Math.Min(current, max);
			}
			
            sprite.TileIndex1D = current;
			
			return current;
		}
	}
}




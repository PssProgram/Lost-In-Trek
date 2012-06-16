using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System.Collections.Generic;

namespace PSVX.Battles
{
	public enum WeapType
	{
		Major,    //靠锁定
		Secondary,//右摇杆
		Special,  //方框
	}
	
	//AI根据这个决定与玩家的距离,还可以通过补充弹药来时间，AI时远时近的攻击
	public enum WeapRanType
	{
		ULongRange, 	//超远程, 1200 ~ 1800
		LongRange,      //远程 720 ~ 1200
		MediumRange,    //中程 500 ~720
		ShortRange,     //近程 320~500
		UShortRange,	//超近程 320以下  
		None,			//没有武器。
	}
	
	/// <summary>
	/// 武器基类,用来BWeaponSystem中,装武器.
	/// </summary>
	public class BWeapon : GameObject
	{
		//攻击力
		public float Att = 1f;
		//移动速度
		protected	float speed = 4f;
		protected	ContactType ctype;
		//发射速率
		protected   Countdown shootCd;
		//检测范围
		protected  float width =0;
		protected  float height =0;
		
		//武器的主人
		protected	BWarship master;
		
		//弹数
		public int Amount=1;
		//每次射击的数量
		public int OnceAmount=1;
		//类型
		public WeapType WeaponType;
		//射程类型
		public WeapRanType WeaponRangeType;
		
		public BWeapon (int amount, WeapType wt,WeapRanType wrt, ContactType ctype):
			base()
		{
			//this.master = (BWarship)node;
			this.ctype = ctype;
			this.Amount =amount;
			this.WeaponRangeType = wrt;
			this.WeaponType = wt;
		}
		
		public override void UpdateFrame (float dt)
		{
			if(master == null)
				return;
				
			shootCd.Update(dt);
			base.UpdateFrame (dt);
		}
		
		//必须调用！
		public void ChangeMaster(BWarship m)
		{
			this.master = m;
			this.fatherNode = m;
		}
		
		public virtual void Shoot(Layer weaponLayer)
		{
			if(master == null)
				return;
			
			if(Amount >0)
				Amount -= OnceAmount;
			else if (Amount <0)
				Amount =0;
		}
		
		//获取对应射程的float值 
		public static float GetRangeByType (WeapRanType wrt)
		{
			float range = 0;
			switch (wrt) 
			{
			case WeapRanType.ULongRange :
				range = 1800;
				break;
			case WeapRanType.LongRange :
				range = 1200;
				break;
			case WeapRanType.MediumRange :
				range = 720;
				break;
			case WeapRanType.ShortRange :
				range = 500;
				break;
			case WeapRanType.UShortRange :
				range = 330;
				break;
			}
			return range;
		}
	
		//获取对应射程的float值 的 平方
		public static float GetRangeSquaredByType (WeapRanType wrt)
		{
			float range = 0;
			switch (wrt) 
			{
			case WeapRanType.ULongRange :
				range = 3240000;
				break;
			case WeapRanType.LongRange :
				range = 1440000;
				break;
			case WeapRanType.MediumRange :
				range = 518400;
				break;
			case WeapRanType.ShortRange :
				range = 250000;
				break;
			case WeapRanType.UShortRange :
				range = 108900;
				break;
			}
			return range;
		}
		
	}
}


using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System.Collections.Generic;

namespace PSVX.Battles
{
	public class BGeneralBomb : BWeapon
	{
		//刚抛出时的 不会检测时间
	 	float noContactTime = 0.20f;
		//起爆时间
		float startBomb;
		
		public BGeneralBomb(int amount, WeapType wt,WeapRanType wrt,ContactType ctype, float shootcd):
			base(amount,wt,wrt,ctype)
		{
			//数据设置
			width =12;
			height = 12;
			speed =4.6f;
			startBomb = 0.1f;
			Att = 0.5f;
			shootCd = new Countdown(shootcd);
		}
		
		public override void Shoot (Layer weaponLayer)
		{
			if(shootCd.IsOver == true && Amount >0)
			{
				if(master.IsMoveNow == true)
				{
					speed =2.6f;
				}
				else
				{
					speed =6f;
				}
				new BBomb(weaponLayer,master.GetPosition(),-master.NowDirection,speed,width,height,this.WeaponRangeType,startBomb,
				         noContactTime,ctype,Att,"Battles//primitive.png");
				shootCd.Start();
				base.Shoot(weaponLayer);
			}
			
		}
	}
}


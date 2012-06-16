using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System.Collections.Generic;

namespace PSVX.Battles
{
	/// <summary>
	/// 防御机关炮，用于击落靠近的导弹。
	/// 射程短，威力低，连射数多。
	/// </summary>
	public class BDefenseCannon : BWeapon
	{
		//射击方向
		Vector2 shootDir = new Vector2(0,-1);
		//预先建好子弹对象！
		List<BBullet> bullets = new List<BBullet>();
			
		public BDefenseCannon (int amount, WeapType wt,WeapRanType wrt, ContactType ctype, float shootcd):
			base( amount,wt,wrt ,ctype)
		{
			//数据设置
			width =5;
			height = 5;
			speed = 8f;
			Att = 2f;
			shootCd =new Countdown(shootcd);
		}
		
		public override void Shoot (Layer weaponLayer)
		{
			if (shootCd.IsOver == true && Amount >0)
			{
				if (master.IsMoveNow == false)
				{
					new BBullet (weaponLayer, master.GetPosition (), master.AimDir, speed, width, height,  
					             this.WeaponRangeType, ctype,Att, "Battles//primitive.png");
				}
				else 
				{
					new BBullet (weaponLayer, master.GetPosition (), master.AimDir, speed+master.Speed, width, height,  
					           this.WeaponRangeType,ctype, Att, "Battles//primitive.png");
				}
				
				shootCd.Start();
				base.Shoot(weaponLayer);
			}
		}
	}
}


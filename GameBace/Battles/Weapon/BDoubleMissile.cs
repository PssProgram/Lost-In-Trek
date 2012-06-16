using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PSVX.Battles
{
	/// <summary>
	/// 普通导弹，一次2发
	/// </summary>
	public class BDoubleMissile:BWeapon
	{
		float rots = 0.02f;
		float rotsInc = 0.001f;
		
		public BDoubleMissile(int amount, WeapType wt,WeapRanType wrt,ContactType ctype, float shootcd):
			base(amount,wt,wrt,ctype)
		{
			//数据设置
			width =8;
			height = 8;
			speed = 3.8f;
			Att = 5f;
			
			rots=0.08f;
			rotsInc = 0.0003f;
			speed =6f;
			
			shootCd = new Countdown(shootcd);
			//一次的发射数量
			OnceAmount = 2;
		}
		
		public override void Shoot (Layer weaponLayer)
		{
			if(master.LockShip ==null)
			{
				DebugScene.Instance.WriteLine("No Enemy","No Enemy is Locked!");
				return;
			}
			
			if (shootCd.IsOver == true && Amount >0)
			{	
				if (OnceAmount == 2)
				{
					BMissile b1 = new BMissile (weaponLayer, master.GetPosition (), master.LockShip, speed, rots, rotsInc, width, height, this.WeaponRangeType, ctype,
				            Att, "Battles//testwingman.png");
					b1.Body.Rotate (master.NowAngle - 1.57f);
					BMissile b2 = new BMissile (weaponLayer, master.GetPosition (), master.LockShip, speed, rots, rotsInc, 8, 8, this.WeaponRangeType, ctype,
				            Att, "Battles//testwingman.png");
					b2.Body.Rotate (master.NowAngle + 1.57f);
					shootCd.Start ();
				}
				else
				{
					DebugScene.Instance.WriteLine("Noasdmy","发射导弹失败…………");
				}
				base.Shoot(weaponLayer);
			}
		}
	}
}


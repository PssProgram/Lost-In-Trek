using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System.Collections.Generic;

namespace PSVX.Battles
{
	/// <summary>
	/// 一艘战舰的所有武器
	/// </summary>
	public class BWeaponSystem : GameObject
	{
		// 切换武器的这版先不做,所以以下每个列表其实就只装一个元素.
		
		//主武器
		List<BWeapon> majorWeapons=new List<BWeapon>();
		//副武器
		List<BWeapon> secondaryWeapons=new List<BWeapon>();
		//特殊武器
		List<BWeapon> specialWeapons=new List<BWeapon>();
		
		//武器在的层
		Layer weaponLayer;
		
		/// <summary>
		/// 构造一个武器系统
		/// </summary>
		public BWeaponSystem (Node node,List<BWeapon> weapons,Layer weaponLayer):
			base(node)
		{
			this.weaponLayer = weaponLayer;
			for(int i= 0;i< weapons.Count;i++)
			{
				if(weapons[i].WeaponType == WeapType.Major)
					majorWeapons.Add(weapons[i]);
				if(weapons[i].WeaponType == WeapType.Secondary)
					secondaryWeapons.Add(weapons[i]);
				if(weapons[i].WeaponType == WeapType.Special)
					specialWeapons.Add(weapons[i]);
			}
		}
		
		public void ShootMajorWeapon(int index)
		{
			if(index < secondaryWeapons.Count && index >=0)
			{
				majorWeapons[index].Shoot(weaponLayer);
			}
		}
		
		public void ShootSecondaryWeapon(int index)
		{
			if(index < secondaryWeapons.Count && index >=0)
			{
				secondaryWeapons[index].Shoot(weaponLayer);
			}
		}
		
		public void ShootSpecialWeapons(int index)
		{
			if(index < specialWeapons.Count && index >=0)
			{
				specialWeapons[index].Shoot(weaponLayer);
			}
		}
		
		//判断有没有武器
		public bool IsHaveMajorWeapon()
		{
			if(majorWeapons.Count > 0 && majorWeapons[0].Amount >0 )
				return true;
			else
				return false;
		}
		
		public bool IsHaveSecondaryWeapon()
		{
			if(secondaryWeapons.Count > 0&& secondaryWeapons[0].Amount >0 )
				return true;
			else
				return false;
		}
		
		public bool IsHaveSpecialWeapon()
		{
			if(specialWeapons.Count > 0&& specialWeapons[0].Amount >0 )
				return true;
			else
				return false;
		}
		
		//获取当前射程最远的武器的射程
		public WeapRanType GetMostFarRange()
		{
			List<int> most = new List<int> ();
			if (IsHaveMajorWeapon ())
				most.Add ((int)majorWeapons [0].WeaponRangeType);
			if (IsHaveSecondaryWeapon ())
				most.Add ((int)secondaryWeapons [0].WeaponRangeType);
			if (IsHaveSpecialWeapon ())
				most.Add ((int)specialWeapons [0].WeaponRangeType);
			most.Sort ();
			
			if(most.Count == 0)
				return WeapRanType.None;
				
			return (WeapRanType)most[0];
		}
 
		//随机发射射程够的武器
		public void RndShoot(float dis)
		{
			//有武器，且射程够就发射
			if(IsHaveMajorWeapon() && 
			   BWeapon.GetRangeSquaredByType(majorWeapons[0].WeaponRangeType) > dis)
			{
				if(Maths.Rnd.Next(1,3) == 2)
					ShootMajorWeapon(0);
			}
			
			if(IsHaveSecondaryWeapon()&& 
			   BWeapon.GetRangeSquaredByType(secondaryWeapons[0].WeaponRangeType) > dis)
			{
				//if(Maths.Rnd.Next(1,3) == 2)
				ShootSecondaryWeapon(0);
			}
			
			if(IsHaveSpecialWeapon()&& 
			   BWeapon.GetRangeSquaredByType(specialWeapons[0].WeaponRangeType) > dis)
			{
				if(Maths.Rnd.Next(1,4) == 2)
					ShootSpecialWeapons(0);
			}
		}
	}
}


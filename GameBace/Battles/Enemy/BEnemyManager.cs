using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System.Collections.Generic;

namespace PSVX.Battles
{
	/// <summary>
	/// 敌方,一个舰队，包括一艘主舰，和多艘副舰
	/// </summary>
	public class BEnemyManager:GameObject
	{
		List<BEnemyship> ships = new List<BEnemyship>();
		
		//随机位置追踪测试
		Vector2 center = new Vector2 (480, 272);
		//Vector2 currentTarget;
		//float dis = 180;
		//战斗发生的场景
		BattleScene bScene;
		//武器在的层
		Layer weaponLayer;
		
		//60fps!
		Ticker ticker60Fps = new Ticker(0.01599f);
		
		public BEnemyManager  (Node node,BattleScene bScene):
			base(node)
		{
			this.bScene = bScene;
			weaponLayer = new Layer();
			this.AddChild (weaponLayer);  //最下层 
			
			List<List<BWeapon>> ws = new List<List<BWeapon>>();
			
			for(int i=0;i<10;i++)
			{
				List<BWeapon> weapons = new List<BWeapon>();
				weapons.Add(new BDefenseCannon(140,WeapType.Secondary,WeapRanType.UShortRange, ContactType.EnemyBullet,0.6f));
				weapons.Add(new BGeneralBomb(0,WeapType.Special,WeapRanType.UShortRange, ContactType.Bomb,1f));
				weapons.Add(new BDoubleMissile(20, WeapType.Major,WeapRanType.MediumRange, ContactType.EnemyBullet,3f));
				ws.Add(weapons);
			}
			
			for(int i=0;i<10;i++)
			{
				ships.Add(new BEnemyship(this,new Vector2(-200+i*20,0),ws[i],weaponLayer ,"Battles//testship.png"));
			}
		}
		
		public override void UpdateFrame (float dt)
		{ 
			if(!ticker60Fps.isTime(dt))
				return;
		}
		
		//锁定于被锁定
		//返回所有战舰
		public List<BWarship> GetAllWarships()
		{
			List<BWarship> bwsp = new List<BWarship>();
			
			for(int i=0;i<10;i++)
			{
				bwsp.Add(ships[i]);
			}
			
			return bwsp;
		}
		
		//设置可锁定的敌人
		public void SetCanLockEnemys(List<BWarship> es)
		{
			for(int i=0;i<10;i++)
			{
				ships[i].CanLockEnemys = es;
			}
		}
	}
	
}


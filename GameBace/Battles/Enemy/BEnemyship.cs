using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using System.Collections.Generic;

namespace PSVX.Battles
{
	/// <summary>
	/// 敌方主舰，包括本体和AI
	/// 
	/// //AI规则
	/// 1. 锁定目标，在自己的锁敌范围内随机选择一个目标进行锁定，目标超出锁敌范围就解除锁定。
	/// 2. 选择与目标的维持距离，根据自己射程最远的武器的射程距离和自己的锁敌范围，选择一个最远距离。
	/// 3. 移动，选择一个位置，这个位置是锁定的敌人加上自己与目标的维持距离。选定后就追踪过去。
	/// 4. 攻击，根据当前与锁定的目标的距离来选择，在射程内的武器就随机射击。射击几率 副武器>主武器>特殊武器。
	/// 5. 停止攻击，武器子弹全部打完就停止攻击，此时就投降或者逃跑(这两种行为还没做)。
	/// </summary>
	public class BEnemyship :BWarship
	{
		//惯性移动的速度
		float inertiaSpeed;
		//要移动到坐标
		Vector2 targetPos = Vector2.Zero;
		//锁敌半径
		float lockRadius = 650;
		//维持与锁定敌人的距离 , 是动态变化的,根据自己的武器来变化
		float keepDisFromEnemy = 150;
		//追踪间隔
		Countdown trackCd  = new Countdown(3f);
		//当前与敌人的距离
		float disFromEnemy = 0;
		
		//AI计算间隔
		Ticker aiticker = new Ticker(0.2f);
		//60fps!
		Ticker ticker60Fps = new Ticker(0.016f);
		
		//武器在的层
		Layer weaponLayer;
		//是否持有武器
		public bool IsHaveWeapon = true;
		
		public BEnemyship (Node node, Vector2 pos,List<BWeapon> weapons,Layer weaponLayer, string path)
			:base(node,pos,25,25, ContactType.Enemy,null,null,path)
		{
			this.weaponLayer =weaponLayer;
			
			//this.ToContact  
			//this.beContact 
			//随机AI间隔
			float rnd = Maths.Rnd.Next(1,5)*0.1f;
			aiticker = new Ticker(rnd);
			//随机锁敌半径
			lockRadius =Maths.Rnd.Next(500,900);
			//随机追踪间隔
			trackCd = new Countdown(Maths.Rnd.Next(10,50)*0.1f);
				
			this.Speed = 3.2f;
			inertiaSpeed = 1.83f;
			TrackAction = new BTracker (Body, ref  this.Speed, 0.025f, 0.0025f);
			lockRadius *= lockRadius;
			HP = 220;
			
			for(int i= 0;i < weapons.Count;i++)
			{
				weapons[i].ChangeMaster(this);
			}
			
			this.Weapon = new BWeaponSystem(this,weapons,weaponLayer);
		}
		
		public override void UpdateFrame (float dt)
		{
			if (ticker60Fps.isTime (dt)) 
			{
				if (IsHaveWeapon == true) 
				{
					InertiaMove ();
					if (aiticker.isTime (dt) == true) 
					{
						SelectTarget ();
						SelectTargetPos ();
						ChooseDisFormEnemy ();
					
						AttckNow ();
					}
				}
				else
				{
					StopAttck();
				}
				
				if (this.LockShip == null)
					DebugScene.Instance.WriteLine ("E2", "Enemy lock : False");
				else
					DebugScene.Instance.WriteLine ("E2", "Enemy lock : True !");
						
				BWarship.ColorID = new Vector4(255f / 255f, 255 / 255f, 255 / 255f, 255f / 255f);
				base.UpdateFrame (dt);
			}
				
			trackCd.Update(dt);
		}
		
		void SetTargetPos(Vector2 pos)
		{
			TrackAction.SetTarger(pos);
		}
		
		public Vector2 GetTargrtPos()
		{
			return TrackAction.CurrentTarget;
		}
		
		public override void BeContact (LiveObject ContactObj)
		{
			if(ContactObj is BBullet)
			{
				this.HP -=((BBullet)ContactObj).Att;
			}
			if(ContactObj is BMissile)
			{
				this.HP -=((BMissile)ContactObj).Att;
			}
			if(ContactObj is BBomb)
			{
				this.HP -=((BBomb)ContactObj).Att;
			}
		}
		
		//-----------------
		
		//惯性移动
		void InertiaMove()
		{
			if(TrackAction.IsTracking == false)
			{ 
				this.SetPosition(this.GetPosition()+NowDirection*
				                 inertiaSpeed);
				if (BattleScene.SceneSizeLimit (this.GetPosition ()))
				{//出界就自动回来。
					if (LockShip == null)
					{
						Vector2 returnDir = Vector2.Zero - this.GetPosition ();
						Maths.Normalize(ref returnDir);
						TrackPos(this.GetPosition () + returnDir*180f);
					} 
				}
			}
			
		}
		
		//选择/更新锁定目标
		void SelectTarget ()
		{
			//随机锁定半径内的敌人
			if (this.LockShip == null)
			{
				for (int i=0; i<CanLockEnemys.Count; i++)
				{
					if (Vector2.DistanceSquared (CanLockEnemys [i].GetPosition (),
					                           this.GetPosition ()) < lockRadius &&
					    CanLockEnemys [i].status != ObjectStatus.Die)
					{
						if (Maths.Rnd.Next (1, 4) == 2)
						{//1/3的几率锁定此敌人
							this.LockShip = CanLockEnemys [i];
						}
					}
					
			    }
		    }
			else
			{//锁定更新 
				disFromEnemy = Vector2.DistanceSquared (LockShip.GetPosition (),
					                           this.GetPosition ());
				if (disFromEnemy > lockRadius)
				{  //超出范围就解锁
					this.LockShip = null;
				}
			}
			
			//清除死亡的敌人 
			if( this.LockShip != null && this.LockShip.status
			   == ObjectStatus.Die)
			{
				this.LockShip = null;
			}
			for(int i=0;i<this.CanLockEnemys.Count;i++)
			{
				if(this.CanLockEnemys[i].status == ObjectStatus.Die)
					this.CanLockEnemys.RemoveAt(i);
			}
			 
	    }
		
		//选择要去的目标位置.
		void SelectTargetPos()
		{
			if(LockShip != null && TrackAction.IsTracking == false)
			{
				Vector2 targetDir = LockShip.GetPosition() - this.GetPosition();
				targetDir = new Vector2(targetDir.Y,-targetDir.X);
				Maths.Normalize(ref targetDir);
				
				//根据0～180的随机角度旋转targetDir
				targetDir.TurnTo(targetDir,Maths.Rnd.Next(314)*0.01f);
				
				targetDir=targetDir*keepDisFromEnemy + LockShip.GetPosition();
				BattleScene.SceneSizeLimit(ref targetDir);
				TrackPos(targetDir);
			}
		}
		
		//选择与敌人的距离
		void ChooseDisFormEnemy()
		{
			WeapRanType mostfarW=this.Weapon.GetMostFarRange();
			//武器用完了^……
			if(mostfarW == WeapRanType.None)
			{
				IsHaveWeapon = false;
				return;
			}
				
			float maxWRange = BWeapon.GetRangeByType(mostfarW);
			//随机抖动值
			float rnd = Maths.Rnd.Next(70,150);
			if(maxWRange < lockRadius)
			{
				keepDisFromEnemy = maxWRange - rnd;
			}
			else
			{
				keepDisFromEnemy = lockRadius - rnd;
			}
			
			if(keepDisFromEnemy < 40)
				keepDisFromEnemy =40;
		}
		
		//---------------------
		
		//选择地图内的 随机位置
		Vector2 SelectFreeTargetPos()
		{
			Vector2 pos = Maths.RndVector()*BattleScene.Heigth;
			BattleScene.SceneSizeLimit(ref pos);
			
			return pos;
		}
		
		//开始追踪
		void TrackPos( Vector2 pos)
		{
			if(TrackAction.IsTracking == false && 
			   trackCd.IsOver == true)
			{
				TrackAction.SetTarger(pos);
				trackCd.Start();
			}
		}
		
		//=================
		
		//TODO: AI攻击！
		void AttckNow()
		{
			//更新手动瞄准方向
			if(LockShip != null)
			{
				this.AimDir = LockShip.GetPosition() - this.GetPosition();
				Maths.Normalize(ref this.AimDir);
			}
			else
			{ this.AimDir = Vector2.Zero; }
			
			//根据射程随机选择武器
			if(LockShip != null)
			{
				this.Weapon.RndShoot(disFromEnemy);
			}
		}
		
		//不攻击了，没有武器 或者 投降了
		void StopAttck ()
		{
			//缓慢惯性移动
			this.SetPosition (this.GetPosition () + NowDirection *
				                 inertiaSpeed*0.15f);
			
			if (BattleScene.SceneSizeLimit (this.GetPosition ()))
			{//出界就自动回来。
				Vector2 returnDir = Vector2.Zero - this.GetPosition ();
				Maths.Normalize (ref returnDir);
				TrackPos (this.GetPosition () + returnDir * 180f);
			}
		}
		
    }
}


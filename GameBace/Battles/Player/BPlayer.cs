using System;
using PSVX.Base;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using Sce.Pss.Core.Input;
using System.Collections.Generic;

namespace PSVX.Battles
{
	/// <summary>
	/// 战斗时用的主角
	/// 主角的构成
	/// 2架僚机
	/// 
	/// 2艘副舰,移动模式:2.自由战斗。
	/// </summary>
	public class BPlayer:GameObject
	{
		public enum Status
		{
			Live,//生存
			God,//无敌
			Die,//死亡
		}
	 	public	Status NowStatus = Status.Live;
		
		//主舰
		BWarship mainship;
		public BWarship Mainship {
			get {
				return this.mainship;
			}
		}

		//左摇杆控制主舰移动
		Vector2 analogLeftDir = Vector2.Zero;
		//右
		Vector2 analogRightDir = Vector2.Zero;
		//摇杆无效值
		float analogInvalid = 0.15f;
		//武器在的层
		Layer weaponLayer;
		
		float lastAngle=-99;
		
		//控制Camera
		public float CameraMoveSpeed = 1f;
		public Vector2 CameraMoveDir =Vector2.Zero;
		
		//战斗发生的场景
		BattleScene bScene;
		
		//僚机
		BWingman wingman01;
		BWingman wingman02;
		//对僚机的命令
		public BWingman.ActionMode WingmanCommand = BWingman.ActionMode.Follow;
		
		//60fps!
		Ticker ticker60Fps = new Ticker(0.01599f);
		
		public BPlayer (Node node,BattleScene battleScene):
			base(node)
		{
			weaponLayer = new Layer();
			this.AddChild (weaponLayer);  //最下层
			
			this.bScene = battleScene;
			mainship = new BWarship(this,new Vector2(480,272),25,25,
			                         ContactType.Player, null,null,"Battles//testship.png");
			wingman01 = new BWingman(this,new Vector2(480,272),0,15,15);
			wingman02 = new BWingman(this,new Vector2(480,272),1,15,15);
			
			CameraMoveSpeed = 3.5f;
			mainship.Speed=CameraMoveSpeed;
			mainship.HP = 7000;
			
			//设置武器
			List<BWeapon> weapons = new List<BWeapon>();
			
			weapons.Add(new BDefenseCannon(200,WeapType.Secondary, WeapRanType.UShortRange, ContactType.PlayerBullet,0.25f));
			weapons.Add(new BGeneralBomb(120,WeapType.Special,WeapRanType.UShortRange, ContactType.Bomb,1f));
			weapons.Add(new BDoubleMissile(220, WeapType.Major,WeapRanType.MediumRange, ContactType.PlayerBullet,2f));
			
			for(int i= 0;i < weapons.Count;i++)
			{
				weapons[i].ChangeMaster(mainship);
			}
			
			mainship.Weapon = new BWeaponSystem(this,weapons,weaponLayer);
		}
		
		public override void UpdateFrame (float dt)
		{
			if (ticker60Fps.isTime (dt))
			{	
				if (NowStatus == Status.Live) 
				{
					PlayerControl ();
					MovePlayer ();
					ControlWingman ();
					AttackNow ();
					LockEnemy ();
				
					if (mainship.status == ObjectStatus.Die)
						NowStatus = Status.Die;
				}
			
				if (NowStatus == Status.Die)
				{
					DebugScene.Instance.WriteLine ("w3", "Player is Death!!", FontColor.Red);
					wingman01.status = ObjectStatus.Die;
					wingman02.status = ObjectStatus.Die;
				}
			
				DebugScene.Instance.WriteLine ("2xs3", "Player HP :" + mainship.HP.ToString (), FontColor.Green);
				DebugScene.Instance.WriteLine ("2g3", mainship.Weapon.GetMostFarRange().ToString());
				
				BWarship.ColorID = new Vector4 (255f / 255f, 80 / 255f, 22 / 255f, 255f / 255f);
			}
		}
		
		public Vector2 GetPosition()
		{
			return mainship.GetPosition();
		}
		
		//锁定于被锁定
		//返回所有战舰
		public List<BWarship> GetAllWarships()
		{
			List<BWarship> bwsp = new List<BWarship>();
			bwsp.Add(mainship);
			
			return bwsp;
		}
		//设置可锁定的敌人
		public void SetCanLockEnemys(List<BWarship> es)
		{
			mainship.CanLockEnemys = es;
			mainship.LockShip = mainship.CanLockEnemys[0];
		}
		
		//控制僚机
		void ControlWingman ()
		{
			WingmanCommand = BWingman.ActionMode.Follow;
		}
		
		void PlayerControl()
		{
			//左摇杆控制主舰移动
			#region 
			float x = Input2.GamePad0.AnalogLeft.X;
			float y = Input2.GamePad0.AnalogLeft.Y;
			if (x > -analogInvalid && x < analogInvalid)
				x = 0;
			if (y > -analogInvalid && y < analogInvalid)
				y = 0;
			
			//触摸屏模拟摇杆
			//if(!Input2.GamePad0.Circle.Down)
			//{
				Vector2 touchV2 = Maths.TouchToAnalog ();
				if (touchV2 != Vector2.Zero) {
					x = touchV2.X;
					y = touchV2.Y;
					if (x > -analogInvalid && x < analogInvalid)
						x = 0;
					if (y > -analogInvalid && y < analogInvalid)
						y = 0;
				}
			//}
			analogLeftDir = new Vector2 (x, y);
				
			#endregion        
			
			//右摇杆控制主舰移动方向
			#region 
			float xr = Input2.GamePad0.AnalogRight.X;
			float yr = Input2.GamePad0.AnalogRight.Y;
			if (xr > -analogInvalid && xr < analogInvalid)
				xr = 0;
			if (yr > -analogInvalid && yr < analogInvalid)
				yr = 0;
			
			//触摸屏模拟摇杆 右
			if(Input2.GamePad0.Circle.Down)
			{
				Vector2 touchV2r = Maths.TouchToAnalog ();
				if (touchV2r != Vector2.Zero) {
					xr = touchV2r.X;
					yr = touchV2r.Y;
					if (xr > -analogInvalid && xr < analogInvalid)
						xr = 0;
					if (yr > -analogInvalid && yr < analogInvalid)
						yr = 0;
				}
			}
			analogRightDir = new Vector2 (xr, yr);
			mainship.AimDir = analogRightDir;
			Maths.Normalize(ref mainship.AimDir);
			#endregion        
		}
		
		//实际上是摄像机的移动!
		void MovePlayer()
		{
			if(analogLeftDir == Vector2.Zero)
			{
				CameraMoveDir = Vector2.Zero;
				return;
			}
			
			#region 旋转主角
			//角度
			float angle = Vector2.Angle(analogLeftDir,new Vector2(-1,0));
			if(lastAngle == -99)
			{
				lastAngle = angle =0;
			}
			if(angle != lastAngle)
			{
				mainship.RotSpeed = -lastAngle;
				mainship.RotateShip();
				
				mainship.RotSpeed = angle;
				mainship.RotateShip();
				lastAngle = angle;
			}
			#endregion
			
			
			MapLimit ();
		}
		
		//移动和地图限制
		void MapLimit ()
		{
			Vector2 nowPos = mainship.GetPosition ()+mainship.NowDirection*5f;
			
			if (nowPos.X < BattleScene.LeftLimit) {
				CameraMoveDir = Vector2.Zero;
				bScene.Map.DisplayBoundaryWarn(mainship.GetPosition(),
				                                    BoundaryType.Left);
				
			} else if (nowPos.X > BattleScene.RightLimit) {
				CameraMoveDir = Vector2.Zero;
				bScene.Map.DisplayBoundaryWarn (mainship.GetPosition (),
				                                    BoundaryType.Right);
				
			} else if (nowPos.Y < BattleScene.UpLimit) {
				CameraMoveDir = Vector2.Zero;
				bScene.Map.DisplayBoundaryWarn (mainship.GetPosition (),
				                                    BoundaryType.Up);
				
			} else if (nowPos.Y > BattleScene.DownLimit) {
				CameraMoveDir = Vector2.Zero;
				bScene.Map.DisplayBoundaryWarn (mainship.GetPosition (),
				                                    BoundaryType.Down);
				
			} else {
				CameraMoveDir = mainship.NowDirection;
				mainship.SetPosition (mainship.GetPosition () + 
			                    CameraMoveDir * CameraMoveSpeed);
			
				bScene.MoveCamera (CameraMoveDir * CameraMoveSpeed);
				bScene.Map.CancelBoundaryWarn ();
				mainship.IsMoveNow = true;
				
			}
		}
		
		//战斗
		void AttackNow()
		{
			if(analogRightDir != Vector2.Zero)
			{
				mainship.Weapon.ShootSecondaryWeapon(0);
			}
			if(Input2.GamePad0.Square.Down)
			{
				mainship.Weapon.ShootSpecialWeapons(0);
			}
			if(Input2.GamePad0.R.Down)
			{
				mainship.Weapon.ShootMajorWeapon(0);
			}
		}
		
		//锁定有关
		void LockEnemy()
		{
			//清除死亡的敌人
			if( mainship.LockShip != null && mainship.LockShip.status
			   == ObjectStatus.Die)
			{
				mainship.LockShip = null;
			}
			for(int i=0;i<mainship.CanLockEnemys.Count;i++)
			{
				if(mainship.CanLockEnemys[i].status == ObjectStatus.Die)
					mainship.CanLockEnemys.RemoveAt(i);
			}
		}
		
		//锁定
		void LockAction()
		{
			//按下L锁定敌人，锁定中或锁定完成的敌人，离开屏幕时，会有个小箭头指示敌人的位置方向！
			//这样就可以省掉小地图了。
		}
	}
}


using System;
using System.Collections.Generic;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;
using Sce.Pss.Core;

namespace PSVX.Base
{
	public delegate void  ContactDeal(LiveObject ContactObj);
	
	public enum ObjectStatus
	{
		Live,//生存
		God,//无敌
		Die,//死亡
	}
	public class LiveObject:GameObject
	{
		public float HP = 1;//生命
		public ObjectStatus status;//物体状态
		public float GodTime;//无敌时间
		private float GodTimer;//无敌时间计数器
		public float DieTime;//死亡时间
		//碰撞有关
		public ContactType CType= ContactType.None; //碰撞类型
		public Vector2 Center = Vector2.Zero;
		public float Width =0;
		public float Heigth =0;
		
		//碰撞后的处理
		ContactDeal  toContactDeal;
	    ContactDeal beContactDeal;
		
		//绘制碰撞盒，测试用
		public	DrawBoundBox drawBoundBox;
		
		/// <summary>
		/// 创建一个活体
		/// </summary>
		public LiveObject (Node node)
			:base(node) {}
		
		/// <summary>
		/// 创建一个活体
		/// </summary>
		public LiveObject (Node node,float CreatTime,float DieTime):base(node)
		{ 
			Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(this, Tick, 0.0f, false);
			this.GodTimer = CreatTime;
			this.DieTime = DieTime;
			this.Born();
			//this.ContactDate = new List<Contacter.Contact>();//碰撞资料
			this.status = ObjectStatus.God;
		}
		
		/// <summary>
		/// 创建一个活体,带有碰撞检测
		/// </summary>
		public LiveObject (Node node,float CreatTime,float DieTime,
		                  Vector2 center, float width,float height,ContactType ctype,
		                   ContactDeal td,ContactDeal bd)
			:base(node)
		{
			//drawBoundBox = new DrawBoundBox(this);
			
			this.toContactDeal = td;
			this.beContactDeal = bd;
			
			this.Width = width;
			this.Heigth = height;
			this.Center = new Vector2(center.X,-center.Y);
			this.CType = ctype;
			
			Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(this, Tick, 0.0f, false);
			this.GodTimer = CreatTime;
			this.DieTime = DieTime;
			this.Born();
			//this.ContactDate = new List<Contacter.Contact>();//碰撞资料
			this.status = ObjectStatus.God;
		}
		
		public virtual void Born(){}
		
		public virtual void Die(){}
		
		public override void UpdateFrame(float dt)
		{
			if(status == ObjectStatus.God)
			{
				GodTimer -= dt;
				if(GodTimer <= 0.0f)
				{
					status = ObjectStatus.Live;
				}
			}
			if(status == ObjectStatus.Live)
			{
				if(CType != ContactType.None)
				{
					Contacter.Instance.Add(this);
				}
			}
			if(status == ObjectStatus.Die)
			{
				DieTime -= dt;
				if(DieTime <= 0.0f)
				{
					Die();
					Remove();
				}
			}
			if(status != ObjectStatus.Die && HP <=0)
			{
				status = ObjectStatus.Die;
			}
		}
		
		private void Tick(float dt)
		{
			if(status == ObjectStatus.Live)
			{
				HandleInput(dt);
			}
		}
		
		public virtual void HandleInput(float dt){}
		
		public virtual void TakeDamage(float damage)
		{
			if(status == ObjectStatus.Live)
			{
				HP -= damage;
				GodTimer += GodTime;
				status = ObjectStatus.God;
			}
		}
		
		//碰撞检测
		//获取包围盒
		public virtual BoundBox GetBoundBox()
		{
			BoundBox bb =new BoundBox(Center.X,Center.Y,Width,Heigth);
			//drawBoundBox.SetBoundBoxInfor(bb);
			
			return bb;
		}
		//主动碰撞到
		public virtual void ToContact(LiveObject ContactObj)
		{
			if (toContactDeal != null)
				toContactDeal (ContactObj);
		}
		//被碰撞到
		public virtual void BeContact (LiveObject ContactObj)
		{
			if (beContactDeal != null)
				beContactDeal (ContactObj);
		}
		
	}
}


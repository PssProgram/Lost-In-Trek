using System;
using System.Collections.Generic;
using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PSVX.Base
{
	public delegate void TouchInputDelegate();
	
	public class GameObject:Node
	{
		public Node fatherNode;//父类
		public float FrameSpeed = 60;//帧率
		public Bounds2 bounds;
		public event TouchInputDelegate ObjectTouched;
		
		public GameObject()
		{
			Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(this, UpdateFrame, 0.0f, false);
		}
		
		public GameObject (Node node)
		{
			fatherNode = node;
			node.AddChild(this);
			Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Schedule(this, UpdateFrame, 0.0f, false);
		}
		
		public virtual void UpdateFrame(float dt)
		{
		}
		
		//触摸输入
		public virtual void TouchInput()
		{
			ObjectTouched();
		}
		
		public void ChangeFather(Node node)
		{
			if(fatherNode!= null)
				fatherNode.RemoveChild(this,false);
			node.AddChild(this);
			fatherNode = node;
		}
		
		public void Remove()
		{
			this.RemoveAllChildren(true);
			fatherNode.RemoveChild(this,true);
			Sce.Pss.HighLevel.GameEngine2D.Scheduler.Instance.Unschedule(this,UpdateFrame);
		}
	}
}


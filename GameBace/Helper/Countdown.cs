using System;

namespace PSVX.Base
{
	/// <summary>
	/// 倒计时，,倒计时完成，到下次Start()之间是true
	/// </summary>
	public class Countdown
	{
		public bool IsOver
		{get; private set;}
		
		bool  isStart = false;
		float maxTime;
		float nowTime;
		
		public Countdown (float maxTime)
		{
			this.maxTime = maxTime;
			this.nowTime = maxTime;
			IsOver = true;
		}
		
		public void Update(float dt)
		{
			if(isStart == true)
			{
				nowTime -= dt;
				if(nowTime <0 )
				{
					nowTime = maxTime;
					isStart = false;
					IsOver = true;
				}
				else
					IsOver=false;
			}
		}
		
		public void Start()
		{
			isStart = true;
			nowTime = maxTime;
		}
	}
}


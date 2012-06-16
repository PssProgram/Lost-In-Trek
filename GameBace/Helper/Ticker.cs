using System;

namespace PSVX.Base
{
	public class Ticker
	{
		float tickTime;
		float addTime;
		
		public Ticker (float tickTime)
		{
			this.tickTime = tickTime;
			this.addTime = 0.0f;
		}
		
		public bool isTime(float dt)
		{
			addTime += dt;
			if(addTime >= tickTime)
			{
				addTime = 0;
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}


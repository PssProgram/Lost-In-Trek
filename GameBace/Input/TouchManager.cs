using System;
using System.Collections.Generic;

using Sce.Pss.Core;
using Sce.Pss.Core.Input;
using Sce.Pss.Core.Environment;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;

using Sce.Pss.HighLevel.GameEngine2D;
using Sce.Pss.HighLevel.GameEngine2D.Base;

namespace PSVX.Base
{
	public class TouchManager
	{
		private static TouchManager instance;
		public List<GameObject> TouchList;
		public List<TouchData> TouchDatas = new List<TouchData>();
			
		private TouchManager ()
		{
			TouchList = new List<GameObject>();
		}
		
		public static TouchManager Instance 
   		{ 
      		get  
      		{ 
         		if (instance == null) 
            	instance = new TouchManager(); 
         		return instance;
      		} 
   		} 
		
		public void BaceUpdate(List<TouchData> touchDataList)
		{
			TouchDatas = touchDataList;
		}
		
		public void OnUpdate()
		{
			foreach (TouchData touchData in TouchDatas) 
			{
				if (TouchList.Count > 0 && touchData.Status == TouchStatus.Down )
				{
					Vector2 TouchPoint = new Vector2((touchData.X + 0.5f) * 960,(touchData.Y + 0.5f) * 544);
					foreach(GameObject go in TouchList)
					{
						if(go.bounds.IsInside(TouchPoint))
							go.TouchInput();
					}
				}
			}
			
			TouchList.Clear();
		}
		
		public void AddTouchList(GameObject gameobject)
		{
			this.TouchList.Add(gameobject);
		}
	}
}


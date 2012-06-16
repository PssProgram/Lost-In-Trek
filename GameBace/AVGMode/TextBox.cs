using System;
using Sce.Pss.Core;
using Sce.Pss.HighLevel.UI;
using Sce.Pss.Core.Imaging;
using PSVX.Base;

namespace PSVX.Base.AVG
{
	
	public class TextBox
	{
		public TextBG textBG;
		Label Textlable;
		
		public bool isDoen = true;
		string Text;
		int pos = 0;
		float totleTime = 0;
		float intervalTime = 0.1f;
		
		
		public TextBox ()
		{
			
		}
		
		public TextBG NewTextBox()
		{
			textBG = new TextBG();
			Textlable = new Label();
			
			Textlable.Font = GameData.font;
			Textlable.SetPosition(207f,389f);
			Textlable.TextColor = new UIColor(1f,1f,1f,1.0f);
			Textlable.Width = 560f;
			GameUI.Instance.RootWidget.AddChildLast(Textlable);
			return textBG;
		}
		
		public void Show()
		{
			textBG.Visible = true;
		}
		
		public void Hide()
		{
			textBG.Visible = false;
		}
		
		public void StartPrint(string text)
		{
			Text = text;
			totleTime = 0;
			pos = 0;
			isDoen = false;
		}
		
		public void OnUpdate(float dt)
		{
			if(!isDoen)
			{
				if(Textlable.Text == Text)
					isDoen = true;
				else
				{
					totleTime += dt;
					if (totleTime > pos * intervalTime && pos <= Text.Length)
					{
						Textlable.Text = Text.Substring(0, pos);
						pos ++ ;
					}
				}
			}
		}
		
		public void Finish()
		{
			isDoen = true;
			Textlable.Text = Text;
		}
		
		public void Clean()
		{
			GameUI.Instance.RootWidget.RemoveChild(Textlable);
			textBG.Remove();
		}
	}
	
	public class TextBG:GameObject
	{
		SpriteX image;
		public TextBG()
		{
			image = new SpriteX(this,"AVG/TextBox.png",new Vector2(179,364));
		}
		
		public override void UpdateFrame (float dt)
		{
			this.bounds = image.GetBounds();
			TouchManager.Instance.AddTouchList(this);
		}
		
		public override void TouchInput ()
		{
			base.TouchInput();
		}
	}
}


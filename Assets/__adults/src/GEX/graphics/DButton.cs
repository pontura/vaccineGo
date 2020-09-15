using System;
using System.Collections.Generic;
using GEX.fonts;

namespace GEX.graphics
{
	public class DButton : XButton
	{
        public DButton(int width= 40, int height = 30, String title = "", Fontb font = null, Spriteg img= null) 
		{
			//super();
            
            Spriteg sp = new Spriteg();
			sp.createFrame(width, height);
			ImageFX.makeSpriteHDegrade(sp, 0x333333, 0x666666);
			ImageFX.addSpriteStroke(sp, 0xFF444444, 2);
			ImageFX.addSpriteStroke(sp, 0xFF111111, 1);
			if (img != null)
			{
				if (img.x == 0 && img.y == 0)//auto center
				{
					sp.draw(img, (width >> 1) - ((int)img.width >> 1), (height >> 1) - ((int)img.height >> 1));
				}
				else
					sp.draw(img, (int)img.x, (int)img.y);
			}
			if(title != "")
				sp.drawCenterString(font, title);
			
			this.setFrameUp(sp);
			this.setFrameDown(sp);
			ImageFX.setSpriteColor(sp, 0xFFFFFF, 0x44);
			this.setFrameOver(sp);

            //ImageFX.setSpriteColor(sp, 0xFF0000, 0x44);
            //this.setFrameDisable(sp);
			this.makeDisable();
            
		}
	}
}

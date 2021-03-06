﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace SunBurn
{
	public class MenuListData :List<MenuItem>
	{
		public MenuListData ()
		{
			this.Add(new MenuItem{
				Title = "Settings",
				IconSource = "settings.png",
				TargetType = typeof(SettingsPage)
			});

			this.Add (new MenuItem {
				Title = "Frontpage",
				IconSource = "",
				TargetType = typeof(HomePage)
			});
		}
	}
}


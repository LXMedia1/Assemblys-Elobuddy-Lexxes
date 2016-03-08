using System;
using System.Linq;
using System.Reflection;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace LX.LX_Activator.Items
{
	class Banner_of_Command : Item
	{
		public override void CreateMenu()
		{
			Activator.Menu_Items_Situation.AddLabel("Banner of Command");
			Activator.Menu_Items_Situation.Add("useInCombo", new CheckBox("Use In Combo-Mode"));
			Activator.Menu_Items_Situation.Add("useInHarras", new CheckBox("Use In Harras-Mode"));
		}

		public override void Use()
		{
			if (Activator.Menu_Items_Situation["useInCombo"].Cast<CheckBox>().CurrentValue &&
				Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
				EloBuddy.SDK.Item.HasItem(ItemId.Banner_of_Command) &&
				EloBuddy.SDK.Item.CanUseItem(ItemId.Banner_of_Command))
			{
				var SiegeMinion = ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault(u => !u.IsDead && u.IsValidTarget(1100) && u.BaseSkinName.Contains("MinionSiege") && u.Team == ObjectManager.Player.Team);
				if (SiegeMinion != null)
					EloBuddy.SDK.Item.UseItem(ItemId.Banner_of_Command, SiegeMinion);
			}
			if (Activator.Menu_Items_Situation["useInHarras"].Cast<CheckBox>().CurrentValue &&
				Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) &&
				EloBuddy.SDK.Item.HasItem(ItemId.Banner_of_Command) &&
				EloBuddy.SDK.Item.CanUseItem(ItemId.Banner_of_Command))
			{
				var SiegeMinion = ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault(u => !u.IsDead && u.IsValidTarget(1100) && u.BaseSkinName.Contains("MinionSiege") && u.Team == ObjectManager.Player.Team);
				if (SiegeMinion != null)
					EloBuddy.SDK.Item.UseItem(ItemId.Banner_of_Command, SiegeMinion);
			}

					
		}
	}
}

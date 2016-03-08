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
				var Minions = ObjectManager.Get<Obj_AI_Minion>().Where(u => !u.IsDead && u.IsValidTarget(1100));
				foreach (var minion in Minions)
				{
					Console.WriteLine(minion.Name);
					Console.WriteLine(minion.AttackRange);
					Console.WriteLine(minion.BaseSkinName);
					Console.WriteLine(minion.Type);
					Console.WriteLine(minion.Health);
					Console.WriteLine(minion.MinionLevel);
				}
			}
			

					//EloBuddy.SDK.Item.UseItem(ItemId.Health_Potion);
			
		}
	}
}

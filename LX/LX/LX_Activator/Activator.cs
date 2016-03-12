using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using LX.LX_Activator.Items;
using LX.LX_Activator.Spells_Passive;
using LX.LX_Activator.Summoners;

namespace LX.LX_Activator
{
	class Activator
	{
		public static Menu Menu;
		public static Menu Menu_Potions;
		public static Menu Menu_Summoners;
		public static Menu Menu_Items_Situation;

		public static List<Item> ItemList;
		public static List<Summoner> SummonerList;
		public static List<Spell_Passive> SpellList;
		internal static void Initiate()
		{
			Menu = MainMenu.AddMenu("LX", "lx", "LX - Ultimate");
			
			Menu_Potions = Menu.AddSubMenu("Potion Manager", "lx_potionmanager", "LX - Ultimate PotionManager");
			Menu_Potions.AddLabel("Hint: He will never use Potions for Mana for non-Mana Champions!");
			Menu_Potions.AddLabel("If you activate > Disable Mana < he will not use Potions for Mana at all.");
			Menu_Potions.Add("disable_Mana", new CheckBox("Disable Mana"));

			Menu_Summoners = Menu.AddSubMenu("Summoner Manager", "lx_summonermanager", "LX - Ultimate SummonerManager");

			Menu_Items_Situation = Menu.AddSubMenu("Items Situational", "lx_items_situations", "LX - Ultimate Items - Situational");
			
			SummonerList = new List<Summoner>
			{
				new Exhoust(),
				new Heal(),
				new Ignite(),
				new Smite(),
			};

			ItemList = new List<Item>
			{
				new Banner_of_Command(),
				new Corrupting_Potion(),
				new Health_Potion(),
				new Hunters_Potion(),
				new Refillable_Potion(),
				new Total_Biscuit_of_Rejuvenation(),

			};

			SpellList = new List<Spell_Passive>
			{
				new Morgana_Q(),
				new Morgana_W(),
				new Morgana_E(),
			};

			foreach (var item in ItemList)
				item.CreateMenu();
			foreach (var summoner in SummonerList)
				summoner.CreateMenu();
			foreach (var spell in SpellList)
				spell.CreateMenu();

			Game.OnUpdate += OnUpdate;

		}

		private static void OnUpdate(EventArgs args)
		{
			if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
			{
				foreach (var buff in ObjectManager.Player.Buffs)
				{
					Console.WriteLine(buff.Name + " Count: " + buff.Count);
				}
			}
			foreach (var summoner in SummonerList.Where(o => o.active()))
				summoner.Use(); 
			foreach (var item in ItemList)
				item.Use();
			foreach (var spell in SpellList.Where(o => o.active()))
				spell.Use();
		}
	}
}

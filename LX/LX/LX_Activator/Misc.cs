using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace LX.LX_Activator
{
	static class Misc
	{
		public static void AddPotionMenu(this Menu menu, string name, int Health)
		{
			menu.AddGroupLabel(name.Replace("_", " "));
			var slider = menu.Add(name + "_useAtHealth", new Slider("", Health));
			menu[name + "_useAtHealth"].Cast<Slider>().DisplayName = "Use [" + name.Replace("_", " ") + "] at " + menu[name + "_useAtHealth"].Cast<Slider>().CurrentValue + " % Health";
			slider.OnValueChange += OnValueChangedPotionHealth;
		}
		public static void AddPotionMenu(this Menu menu, string name, int Health, int Mana)
		{
			menu.AddGroupLabel(name.Replace("_", " "));
			
			var slider = menu.Add(name + "_useAtHealth", new Slider("", Health));
			menu[name + "_useAtHealth"].Cast<Slider>().DisplayName = "Use [" + name.Replace("_", " ") + "] at " + menu[name + "_useAtHealth"].Cast<Slider>().CurrentValue + " % Health";
			slider.OnValueChange += OnValueChangedPotionHealth;

			if (ObjectManager.Player.IsNoManaChamp())
				return;

			slider = menu.Add(name + "_useAtMana", new Slider("", Mana));
			menu[name + "_useAtMana"].Cast<Slider>().DisplayName = "Use [" + name.Replace("_", " ") + "] at " + menu[name + "_useAtMana"].Cast<Slider>().CurrentValue + " % Mana";
			slider.OnValueChange += OnValueChangedPotionMana;
		}

		private static void OnValueChangedPotionHealth(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
		{
			var menuname = sender.DisplayName.Split('[', ']')[1].Replace(" ", "_");
			Activator.Menu_Potions[menuname + "_useAtHealth"].Cast<Slider>().DisplayName = "Use [" + menuname.Replace("_", " ") + "] at " + Activator.Menu_Potions[menuname + "_useAtHealth"].Cast<Slider>().CurrentValue + " % Health";
		}
		private static void OnValueChangedPotionMana(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
		{
			var menuname = sender.DisplayName.Split('[', ']')[1].Replace(" ", "_");
			Activator.Menu_Potions[menuname + "_useAtMana"].Cast<Slider>().DisplayName = "Use [" + menuname.Replace("_", " ") + "] at " + Activator.Menu_Potions[menuname + "_useAtMana"].Cast<Slider>().CurrentValue + " % Mana";
		}
		
		public static bool PotionConditionMatch(this Menu menu, string name)
		{
			try
			{
				if (menu[name + "_useAtHealth"].Cast<Slider>().CurrentValue >= ObjectManager.Player.HealthPercent)
					return true;
				if (ObjectManager.Player.IsNoManaChamp() || menu[name + "disable_Mana"].Cast<CheckBox>().CurrentValue)
					return false;
				if (menu[name + "_useAtMana"].Cast<Slider>().CurrentValue >= ObjectManager.Player.Mana)
					return true;
			}
			catch (Exception)
			{
				return false; // if no mana menu Exist
			}
			return false;
		}

		public static bool IsPotionUseReady(this AIHeroClient hero)
		{
			return !(ObjectManager.Player.IsDead ||
					 ObjectManager.Player.IsInShopRange() ||
					 ObjectManager.Player.IsRecalling() || 
					 ObjectManager.Player.HasBuff("RegenerationPotion") ||
					 ObjectManager.Player.HasBuff("ItemMiniRegenPotion") || 
					 ObjectManager.Player.HasBuff("ItemCrystalFlask") ||
					 ObjectManager.Player.HasBuff("ItemCrystalFlaskJungle") ||
					 ObjectManager.Player.HasBuff("ItemDarkCrystalFlask"));
		}
		internal static bool IsNoManaChamp(this AIHeroClient hero)
		{
			return NoManaChamp.Contains(hero.ChampionName);
		}
		public static bool IsChanneling(this Obj_AI_Base unit)
		{
			// todo AddChannelSpellcheck
			return false;
		}

		public static Vector3 ModifyPrediction(this PredictionResult prediction, int modificator)
		{
			var distance = prediction.UnitPosition.Distance(prediction.CastPosition);
			var newpos = prediction.CastPosition.FromVtoV(prediction.UnitPosition, distance * modificator / 100);
			return newpos;
		}

		public static IEnumerable<Obj_AI_Base> CollisionObjects(this Vector3 position,int width)
		{
			return (from obj in ObjectManager.Get<Obj_AI_Base>() 
					let rec = new Geometry.Polygon.Rectangle(ObjectManager.Player.Position.To2D(), position.To2D(), width + obj.BoundingRadius) 
					where rec.IsInside(obj) 
					select obj).ToList();
		}

		public static Vector3 FromVtoV(this Vector3 from, Vector3 to, float range)
		{
			return from + Vector3.Normalize(to - from) * range;
		}
		public static List<string> NoManaChamp = new List<string>
		{
			"Aatrox",
			"Akali",
			"Garen","Gnar","Katarina","RekSai","Renektion","Rengar","Riven","Rumble","Shyvana","Tryndamere","Yasuo"
			,"DrMundo","Mordekaiser","Vladimir","LeeSin","Shen","Kennen",
			"Zac",
			"Zed"
		};

		public static bool IsStunnedHotFix(this Obj_AI_Base unit)
		{
			if (unit.HasBuff("DarkBindingMissile")) // Morgana Q
				return true;
			if (unit.HasBuff("zyragraspingrootshold")) // Zyra E
				return true;
			if (unit.HasBuff("sorakaesnare")) // Soraka E
				return true;
			return false;
		}

		// Some Buffy i may need later
		// kagesluckypickdisplay stack 3 ( frostfang get money for dmg )
	}
}

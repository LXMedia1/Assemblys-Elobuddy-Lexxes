using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace LX.LX_Activator.Spells_Passive
{
	class Morgana_Q : Spell_Passive
	{
		public static bool _active;
		public static Spell.Skillshot Spell;
		public static Menu Menu;

		public override bool active()
		{
			return _active;
		}

		public override void CreateMenu()
		{
			if (ObjectManager.Player.ChampionName != "Morgana")
				return;
			_active = true;
			Spell = new Spell.Skillshot(SpellSlot.Q, 1175, SkillShotType.Linear, 150, 1200, 70); // 1175,150,1200,70
			
			Menu = Activator.Menu.AddSubMenu("Morgana Q");
			
			Menu.AddSeparator();
			Menu.AddGroupLabel("Using:");

			Menu.Add("active_combo", new CheckBox("Active in Combo Mode"));			
			var slider = Menu.Add("targetingMode_combo", new Slider("", 0, 0, 3));
			Menu["targetingMode_combo"].Cast<Slider>().DisplayName = "Targeting Modus: " + GetTargetModeName(Menu["targetingMode_combo"].Cast<Slider>().CurrentValue);
			slider.OnValueChange += OnValueChamgedCombo;

			Menu.Add("active_harras", new CheckBox("Active in Harras Mode"));
			slider = Menu.Add("targetingMode_harras", new Slider("", 0, 0, 3));
			Menu["targetingMode_harras"].Cast<Slider>().DisplayName = "Targeting Modus: " + GetTargetModeName(Menu["targetingMode_harras"].Cast<Slider>().CurrentValue);
			slider.OnValueChange += OnValueChamgedHarras;
			
			Menu.AddGroupLabel("Drawing:");
			Menu.Add("drawing_range", new CheckBox("Draw SpellRange"));
			
			
			Drawing.OnDraw += OnDraw;			

		}

		private void OnValueChamgedHarras(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
		{
			Menu["targetingMode_harras"].Cast<Slider>().DisplayName = "Targeting Modus: " + GetTargetModeName(args.NewValue);
		}

		private void OnValueChamgedCombo(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
		{
			Menu["targetingMode_combo"].Cast<Slider>().DisplayName = "Targeting Modus: " + GetTargetModeName(args.NewValue);
		}

		private static string GetTargetModeName(int id)
		{
			switch (id)
			{
				case 0:
					return "Target Selector ( lesser casting but better focus )";
				case 1:
					return "Easy Killable ( Focus the easyst Killable Target )";
				case 2:
					return "Most Dangerous ( Focus on Feedlvl )";
				case 3:
					return "Any ( Will Focus what can get Hit Easy )";
			}
			return "Error";
		}

		private static void OnDraw(EventArgs args)
		{
			if (Menu["drawing_range"].Cast<CheckBox>().CurrentValue && Spell.IsLearned)
				Circle.Draw(Spell.IsReady() ? SharpDX.Color.Green : SharpDX.Color.Red, Spell.Range, ObjectManager.Player);

			var target = TargetSelector.GetTarget(Spell.Range, DamageType.Magical);
			if (target == null)
				return;
			var pred = Spell.GetPrediction(target);
			var rec = new Geometry.Polygon.Rectangle(ObjectManager.Player.Position.To2D(),
				pred.CastPosition.To2D(), Spell.Width);
			Drawing.DrawText(pred.CastPosition.WorldToScreen(),Color.White,pred.HitChance.ToString( ),2);
			rec.Draw((pred.Collision) ? Color.Red : Color.Green);
		}

		public override void Use()
		{
			if (Spell.IsReady() && Menu["active_combo"].Cast<CheckBox>().CurrentValue &&
			    Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
			{
				switch (Menu["targetingMode_harras"].Cast<Slider>().CurrentValue)
				{
					case 0:
						Mode_TargetSelector();
						break;
					case 1:
						Mode_EasyKillable();
						break;
					case 2:
						Mode_MostDangerous();
						break;
					case 3:
						Mode_Any();
						break;
				}
			}
		}

		private void Mode_TargetSelector()
		{
			var target = TargetSelector.GetTarget(Spell.Range, DamageType.Magical);
			if (target == null)
				return;
			var pred = Spell.GetPrediction(target);
			var mod = pred.ModifyPrediction(10);
			if ( pred.HitChance >= HitChance.High )
				Spell.Cast(pred.CastPosition);


		}

		private void Mode_EasyKillable()
		{
			
		}

		private void Mode_MostDangerous()
		{
			
		}

		private void Mode_Any()
		{
			
		}

		private static Vector3 GetBestCastPosition(Obj_AI_Base target)
		{
			Vector3 pos;
			var spellmissingRange = target.Distance(ObjectManager.Player) - Spell.Range + Spell.Radius * 2;
			if (spellmissingRange > 0)
			{
				pos = target.Position.FromVtoV(ObjectManager.Player.Position, spellmissingRange -Spell.Width);
				return pos;
			}
			var unitsArround = EntityManager.Heroes.Enemies.Where(u => u != target &&
				u.IsInRange(target, Spell.Width * 2) && u.IsValidTarget());
			AIHeroClient bestSecondTarget = null;
			foreach (var unit in unitsArround)
			{
				if (bestSecondTarget == null)
					bestSecondTarget = unit;
				if (bestSecondTarget.BaseAbilityDamage + bestSecondTarget.BaseAttackDamage < unit.BaseAbilityDamage + unit.BaseAttackDamage)
					bestSecondTarget = unit;
			}
			if (bestSecondTarget != null)
			{
				var bestpos = target.Position.FromVtoV(bestSecondTarget.Position, target.Distance(bestSecondTarget) / 2 - new Random().Next(1, 25));
				if (bestpos.Distance(ObjectManager.Player.Position) <= Spell.Range)
					return bestpos;
			}
			else
			{
				if (target.HealthPercent <= 20)
				{
					pos = target.Position.FromVtoV(ObjectManager.Player.Position, -Spell.Width + new Random().Next(1, 25));
					return pos;
				}
				if (target.HealthPercent >= 80)
				{
					pos = target.Position.FromVtoV(ObjectManager.Player.Position, Spell.Width - new Random().Next(1, 25));
					return pos;
				}
				pos = target.Position.FromVtoV(ObjectManager.Player.Position, new Random().Next(1, 100) - 50);
				return pos;
			}
			return Vector3.Zero;
		}
	}
}

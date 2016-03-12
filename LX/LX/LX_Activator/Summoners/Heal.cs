using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Reflection;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using LX.LX_Evade;
using Spell = EloBuddy.SDK.Spell;

namespace LX.LX_Activator.Summoners
{
	class Heal : Summoner
	{
		public static bool _active;
		public static Spell.Active Hal;
		public override void CreateMenu()
		{
			if (ObjectManager.Player.GetSpellSlotFromName("summonerheal") == SpellSlot.Unknown)
				return;
			_active = true;

			Hal = new Spell.Active(ObjectManager.Player.GetSpellSlotFromName("summonerheal"), 850);

			Activator.Menu_Summoners.AddGroupLabel("Heal");
			Activator.Menu_Summoners.Add("deactivateHeal", new CheckBox("Disable Heal", false));
			var slider = Activator.Menu_Summoners.Add("active_heal_onPercent", new Slider("", 20));
			Activator.Menu_Summoners["active_heal_onPercent"].Cast<Slider>().DisplayName = "Use on " + Activator.Menu_Summoners["active_heal_onPercent"].Cast<Slider>().CurrentValue + " % Health.";
			slider.OnValueChange += OnSliderHealpercentChange;
			Activator.Menu_Summoners.Add("HealforBuddy", new CheckBox("Use Heal for Buddy", true));
			slider = Activator.Menu_Summoners.Add("active_heal_onPercent_buddy", new Slider("", 10));
			Activator.Menu_Summoners["active_heal_onPercent_buddy"].Cast<Slider>().DisplayName = "Use on " + Activator.Menu_Summoners["active_heal_onPercent_buddy"].Cast<Slider>().CurrentValue + " % Mate Health.";
			slider.OnValueChange += OnSliderHealpercentBuddyChange;
			Activator.Menu_Summoners.AddLabel("Use Conditions");
			Activator.Menu_Summoners.Add("UseIfEnemyNear", new CheckBox("Use just If Enemy Near", true));
			Activator.Menu_Summoners.Add("UseforIncomeDmg", new CheckBox("Use also for Income Damage", true));

			SpellDedector.OnSpellWillHit += SpellWillHit;
		}

		private void SpellWillHit(LX_Evade.Spell spell, Obj_AI_Base target)
		{
			try
			{
				var caster = (AIHeroClient) spell.Caster;
				if (caster.GetSpellDamage(target, spell.Slot) >= target.Health && target.IsValidTarget(Hal.Range) &&
				    target.IsVisible && !target.IsDead)
				{
					if (Activator.Menu_Summoners["UseforIncomeDmg"].Cast<CheckBox>().CurrentValue)
						Hal.Cast();
				}
			}
			catch (Exception)
			{
				//IGGNORED
			}


		}

		private void OnSliderHealpercentBuddyChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
		{
			Activator.Menu_Summoners["active_heal_onPercent_buddy"].Cast<Slider>().DisplayName = "Use on " + args.NewValue + " % Health.";
		}

		private void OnSliderHealpercentChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
		{
			Activator.Menu_Summoners["active_heal_onPercent"].Cast<Slider>().DisplayName = "Use on " + args.NewValue + " % Health.";
		}

		public override bool active()
		{
			return _active;
		}

		public override void Use()
		{
			if (!Hal.IsReady() || Activator.Menu_Summoners["deactivateHeal"].Cast<CheckBox>().CurrentValue)
				return;
			var EnemyCount = EntityManager.Heroes.Enemies.Count(u => u.IsValidTarget(1500) && u.IsVisible && !u.IsDead);
			var Mates = EntityManager.Heroes.Allies.Where(u => u.IsValidTarget(Hal.Range - 10) && !u.IsDead && u.IsVisible);
			var Me = ObjectManager.Player;

			if (!Activator.Menu_Summoners["HealforBuddy"].Cast<CheckBox>().CurrentValue)
				Mates = null;
			if (Mates != null)
				if (Mates.Any(Mate => (Prediction.Health.GetPrediction(Mate, 300) / Mate.MaxHealth * 100) <=
									  Activator.Menu_Summoners["active_heal_onPercent_buddy"].Cast<Slider>().CurrentValue &&
									  Mate.MaxHealth - Mate.Health > GetHealPower() / 2 && (Activator.Menu_Summoners["UseIfEnemyNear"].Cast<CheckBox>().CurrentValue && EnemyCount > 0 || Activator.Menu_Summoners["UseforIncomeDmg"].Cast<CheckBox>().CurrentValue && Prediction.Health.GetPrediction(Mate, 300) <= 10)))
				{
					Hal.Cast();
					return;
				}
			if ((Prediction.Health.GetPrediction(Me, 300) / Me.MaxHealth * 100) <=
			    Activator.Menu_Summoners["active_heal_onPercent"].Cast<Slider>().CurrentValue &&
			    Me.MaxHealth - Me.Health > GetHealPower() && (EnemyCount > 0 || Prediction.Health.GetPrediction(Me, 300) <= 10))
			{
				Hal.Cast();
				return;
			}
		}

		private float GetHealPower()
		{
			return (75 + (15*ObjectManager.Player.Level));
		}
	}
}

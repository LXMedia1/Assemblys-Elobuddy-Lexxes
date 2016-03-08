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

			Menu.AddLabel("The Elobuddy Prediction is not so good right now, if i find a Solution ill add a own.");
			Menu.Add("active_combo", new CheckBox("Active in Combo Mode"));
			var slider = Menu.Add("active_combo_hitpercent", new Slider("",70));
			Menu["active_combo_hitpercent"].Cast<Slider>().DisplayName = "Combo Hitchance : " + Menu["active_combo_hitpercent"].Cast<Slider>().CurrentValue + " %";
			slider.OnValueChange += OnSliderComboHitpercent;
			Menu.Add("active_harras", new CheckBox("Active in Harras Mode"));
			slider = Menu.Add("active_harras_hitpercent", new Slider("",90));
			Menu["active_harras_hitpercent"].Cast<Slider>().DisplayName = "Harras Hitchance : " + Menu["active_harras_hitpercent"].Cast<Slider>().CurrentValue + " %";
			slider.OnValueChange += OnSliderHarrasHitpercent;

			Menu.AddLabel("Modified Prediction: Its Simple, it takes the Ememy Position and the Predicted");
			Menu.AddLabel("Position From Elobuddy Prediction, Calculate the Selected Percent Amoount Back to");
			Menu.AddLabel("his Current Position and Checks there for Collision´s.");
			Menu.AddLabel("If active he not use the HitchancePercent Above!");
			Menu.Add("use_Modified_Prodiction", new CheckBox("Use Modified Prediction"));
			slider = Menu.Add("modifie_prediction_by", new Slider("",20));
			Menu["modifie_prediction_by"].Cast<Slider>().DisplayName = "Modifie Position to Current Position by " + Menu["modifie_prediction_by"].Cast<Slider>().CurrentValue + " %";
			slider.OnValueChange += OnSliderModifiepercent;

			Menu.AddGroupLabel("Drawing:");
			Menu.Add("drawing_range", new CheckBox("Draw SpellRange"));

			
			Drawing.OnDraw += OnDraw;			

		}

		private void OnSliderModifiepercent(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
		{
			Menu["modifie_prediction_by"].Cast<Slider>().DisplayName = "Modifie Position to Current Position by " + Menu["modifie_prediction_by"].Cast<Slider>().CurrentValue + " %";
		}

		private void OnSliderHarrasHitpercent(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
		{
			Menu["active_harras_hitpercent"].Cast<Slider>().DisplayName = "Harras Hitchance : " + Menu["active_harras_hitpercent"].Cast<Slider>().CurrentValue + " %";
		}

		private void OnSliderComboHitpercent(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
		{
			Menu["active_combo_hitpercent"].Cast<Slider>().DisplayName = "Combo Hitchance : " + Menu["active_combo_hitpercent"].Cast<Slider>().CurrentValue + " %";
		}

		private static void OnDraw(EventArgs args)
		{
			if (Menu["drawing_range"].Cast<CheckBox>().CurrentValue && Spell.IsLearned)
				Circle.Draw(Spell.IsReady() ? SharpDX.Color.Green : SharpDX.Color.Red, Spell.Range, ObjectManager.Player);
		}

		public override void Use()
		{
			if (Spell.IsReady() && Menu["active_combo"].Cast<CheckBox>().CurrentValue &&
			    Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
			{
				var target = TargetSelector.GetTarget(Spell.Range, DamageType.Magical);
				if (target == null)
					return;
				var pred = Spell.GetPrediction(target);
				if (Menu["use_Modified_Prodiction"].Cast<CheckBox>().CurrentValue)
				{
					var modifiedPosition = Spell.GetPrediction(target).ModifyPrediction(Menu["modifie_prediction_by"].Cast<Slider>().CurrentValue);
					if (modifiedPosition.CollisionObjects(Spell.Radius + 5).All(u => u.Team == ObjectManager.Player.Team || u.NetworkId == target.NetworkId))
					{
						Spell.Cast(modifiedPosition);
					}
				}
				else
				{
					if (pred.HitChancePercent >= Menu["active_combo_hitpercent"].Cast<Slider>().CurrentValue && pred.Collision)
						Spell.Cast(pred.CastPosition);
				}
			}
			if (Spell.IsReady() && Menu["active_harras"].Cast<CheckBox>().CurrentValue &&
			    Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
			{
				var target = TargetSelector.GetTarget(Spell.Range, DamageType.Magical);
				if (target == null)
					return;
				var pred = Spell.GetPrediction(target);
				if (Menu["use_Modified_Prodiction"].Cast<CheckBox>().CurrentValue)
				{
					var modifiedPosition = Spell.GetPrediction(target).ModifyPrediction(Menu["modifie_prediction_by"].Cast<Slider>().CurrentValue);
					if (modifiedPosition.CollisionObjects(Spell.Radius + 5).All(u => u.Team == ObjectManager.Player.Team || u.NetworkId == target.NetworkId))
					{
						Spell.Cast(modifiedPosition);
					}
				}
				else
				{
					if (pred.HitChancePercent >= Menu["active_harras_hitpercent"].Cast<Slider>().CurrentValue && pred.Collision)
						Spell.Cast(pred.CastPosition);
				}
			}
		}

		private void Mode_TargetSelector()
		{
			


		}
	}
}

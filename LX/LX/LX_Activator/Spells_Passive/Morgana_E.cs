using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using LX.LX_Evade;
using SharpDX;
using Color = System.Drawing.Color;
using Spell = EloBuddy.SDK.Spell;

namespace LX.LX_Activator.Spells_Passive
{
	class Morgana_E : Spell_Passive
	{
		public static bool _active;
		public static Spell.Targeted Spell;
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
			Spell = new Spell.Targeted(SpellSlot.E, 750);

			Menu = Activator.Menu.AddSubMenu("Morgana E");
			Menu.AddLabel("* Beta: Some enemyCC-Spells are added, its just for Testing");
			Menu.AddSeparator();
			Menu.AddGroupLabel("Using:");
			Menu.Add("active_passive", new CheckBox("Active in Passive Mode"));
			Menu.AddGroupLabel("Drawing:");
			Menu.Add("drawing_range", new CheckBox("Draw SpellRange"));

			
			Drawing.OnDraw += OnDraw;
			SpellDedector.OnSpellWillHit += SpellWillHit;

		}

		private void SpellWillHit(LX_Evade.Spell spell, Obj_AI_Base target)
		{
			if (Spell.IsReady() && Menu["active_passive"].Cast<CheckBox>().CurrentValue && target.IsValidTarget(Spell.Range))
				Spell.Cast(target);
		}

		private static void OnDraw(EventArgs args)
		{
			if (Menu["drawing_range"].Cast<CheckBox>().CurrentValue && Spell.IsLearned)
				Circle.Draw(Spell.IsReady() ? SharpDX.Color.Green : SharpDX.Color.Red, Spell.Range, ObjectManager.Player);
		}

		
	}
}

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
	class Morgana_W : Spell_Passive
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
			Spell = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 250, int.MaxValue, 210); // normaly width 215 but -5 for safeHit

			Menu = Activator.Menu.AddSubMenu("Morgana W");
			Menu.AddLabel("* Awesome Spell-Positioning by Lexxes");
			Menu.AddLabel("* Smartest W useage Vaticano");
			Menu.AddLabel("* Can Cast on MaxRange ( not just Center like most other Assemblys ).");
			Menu.AddLabel("* If in Range it also Cast so that also hit other Enemys.");
			Menu.AddLabel("* Casts to Zone or to fleeway.");
			Menu.AddLabel("* All CastSpots light Randomized will still hit but look humanlike.");
			Menu.AddSeparator();
			Menu.AddGroupLabel("Using:");
			Menu.Add("active_passive", new CheckBox("Active in Passive Mode"));
			Menu.AddGroupLabel("Drawing:");
			Menu.Add("drawing_range", new CheckBox("Draw SpellRange"));

			
			Drawing.OnDraw += OnDraw;
			

		}

		private static void OnDraw(EventArgs args)
		{
			if (Menu["drawing_range"].Cast<CheckBox>().CurrentValue && Spell.IsLearned)
				Circle.Draw(Spell.IsReady() ? SharpDX.Color.Green : SharpDX.Color.Red, Spell.Range + Spell.Radius* 2, ObjectManager.Player);
		}

		public override void Use()
		{
			if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
			{
				foreach (var buff in ObjectManager.Player.Buffs)
				{
					Console.WriteLine(buff.Name + " Count: "+buff.Count);
				}
			}
			if (Spell.IsReady() && Menu["active_passive"].Cast<CheckBox>().CurrentValue)
			{
				var ImmobileUnits = EntityManager.Heroes.Enemies.Where(u => u.IsValidTarget(Spell.Range + Spell.Radius * 2) &&
					(u.HasBuffOfType(BuffType.Stun) || u.HasBuffOfType(BuffType.Snare) || u.HasBuffOfType(BuffType.Suppression) ||
					u.IsRooted || u.IsStunned  || u.IsStunnedHotFix())); // todo add Channeling for specific Spells
				AIHeroClient bestTarget = null;
				foreach (var unit in ImmobileUnits)
				{
					if (bestTarget == null)
						bestTarget = unit;
					if (bestTarget.BaseAbilityDamage + bestTarget.BaseAttackDamage < unit.BaseAbilityDamage + unit.BaseAttackDamage)
						bestTarget = unit;
				}
				if (bestTarget == null)
					return;
				var bestPosition = GetBestCastPosition(bestTarget);
				if (bestPosition != Vector3.Zero)
				{
					if (bestPosition.Distance(ObjectManager.Player.Position) <= Spell.Range)
						Spell.Cast(bestPosition);
				}
			}
		}

		private static Vector3 GetBestCastPosition(Obj_AI_Base target)
		{
			Vector3 pos;
			var spellmissingRange = target.Distance(ObjectManager.Player) - Spell.Range + Spell.Radius * 2 - target.BoundingRadius;
			if (spellmissingRange > 0)
			{
				pos = target.Position.FromVtoV(ObjectManager.Player.Position, spellmissingRange -Spell.Width);
				return pos;
			}
			var unitsArround = EntityManager.Heroes.Enemies.Where(u => u != target &&
				u.IsInRange(target, Spell.Width * 2 - target.BoundingRadius) && u.IsValidTarget());
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
				if (bestpos.Distance(ObjectManager.Player.Position - target.BoundingRadius) <= Spell.Range)
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

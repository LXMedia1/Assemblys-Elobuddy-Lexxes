using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
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
			Spell = new Spell.Skillshot(SpellSlot.W,900,SkillShotType.Circular,250,int.MaxValue,210);

			Menu = Activator.Menu.AddSubMenu("Morgana W");
			Menu.AddLabel("* Awesome Spell-Positioning by Lexxes");
			Menu.AddLabel("* Smartest W useage Vaticano");
			Menu.AddLabel("* Can Cast on MaxRange ( not just Center like most other Assemblys ).");
			Menu.AddLabel("* If in Range it also Cast so that also hit other Enemys.");
			Menu.AddLabel("* Casts to Zone or to fleeway.");
			Menu.AddLabel("* All CastSpots light Randomized will still hit but look humanlike.");
			Menu.AddSeparator();
			Menu.Add("active_passive", new CheckBox("Active in Passive Mode"));

		}

		public override void Use()
		{
			if (Spell.IsReady() && Menu["active_passive"].Cast<CheckBox>().CurrentValue)
			{
				var ImmobileUnits = EntityManager.Heroes.Enemies.Where(u => u.IsValidTarget(Spell.Range + Spell.Radius) && 
					(!u.CanMove || u.IsRooted || u.IsStunned || u.IsTaunted )); // todo add Channeling for specific Spells
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

				Drawing.DrawCircle(bestPosition,Spell.Radius,Color.Red);
				if (bestPosition != Vector3.Zero)
				{
					Spell.Cast(bestPosition);
				}
			}
		}

		private static Vector3 GetBestCastPosition(Obj_AI_Base target)
		{
			Vector3 pos;
			var spellmissingRange = Spell.Range - target.Distance(ObjectManager.Player);
			if (spellmissingRange > 0)
			{
				pos = target.Position.FromVtoV(ObjectManager.Player.Position, spellmissingRange);
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
				var rangeLeft = Math.Abs(spellmissingRange);
				if (target.HealthPercent <= 30)
				{
					pos = target.Position.FromVtoV(ObjectManager.Player.Position, spellmissingRange - new Random().Next(1, 25));
					return pos;
				}
				if (target.HealthPercent >= 30)
				{
					pos = target.Position.FromVtoV(ObjectManager.Player.Position, rangeLeft - new Random().Next(1, 25));
					return pos;
				}
				pos = target.Position.FromVtoV(ObjectManager.Player.Position, new Random().Next(1, 100));
				return pos;
			}
			return Vector3.Zero;
		}
	}
}

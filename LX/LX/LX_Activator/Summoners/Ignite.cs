using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace LX.LX_Activator.Summoners
{
	class Ignite : Summoner
	{
		public static bool _active;
		public static Spell.Targeted Dot;
		public override void CreateMenu()
		{
			if (ObjectManager.Player.GetSpellSlotFromName("summonerdot") == SpellSlot.Unknown)
				return;
			_active = true;
			Dot = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"),600);

			Activator.Menu_Summoners.AddGroupLabel("Ignite");
			Activator.Menu_Summoners.AddLabel("Basic");
			Activator.Menu_Summoners.Add("ignite_killable", new CheckBox("Use Ignite on Killable"));
			//Activator.Menu_Summoners.AddLabel("Anti-Behavier");
			//Activator.Menu_Summoners.AddLabel("Hint: Antibehavier checks if this Champ is Near and if he could use this Spell, if YES he wont use Ignote for Killables exept himself.");
			//Activator.Menu_Summoners.Add("ignite_anti_Mundo", new CheckBox("Anti Mundo R"));
		}
		public override bool active()
		{
			return _active;
		}
		public override void Use()
		{
			if (AntiChampBehavier() || !Dot.IsReady())
				return;
			if (Activator.Menu_Summoners["ignite_killable"].Cast<CheckBox>().CurrentValue)
			{
				var ignitetarget =
					EntityManager.Heroes.Enemies.FirstOrDefault(
						u =>
							u.IsValidTarget(Dot.Range) &&
							u.Health < ObjectManager.Player.GetSummonerSpellDamage(u, DamageLibrary.SummonerSpells.Ignite));
				if (ignitetarget != null)
					Dot.Cast(ignitetarget);
			}
		}

		public bool AntiChampBehavier()
		{
			return false;
			// Dr Mundo R ( cant do currently couse not have the Name of Mundo R Buff) not own the Champion
			// Nasus R
			if (Activator.Menu_Summoners["ignite_anti_Mundo"].Cast<CheckBox>().CurrentValue)
			{
				var mundoOnSteroids = EntityManager.Heroes.Enemies.FirstOrDefault(u => u.HasBuff("MISSING") && u.IsValidTarget(Dot.Range));
				if (mundoOnSteroids != null)
				{
					Dot.Cast(mundoOnSteroids);
					return true;
				}
				if (EntityManager.Heroes.Enemies.Any(u => u.ChampionName == "DrMundo"
													  && u.IsValidTarget(Dot.Range + 200) &&
													  (u.Spellbook.GetSpell(SpellSlot.R).Cooldown <= 5) || u.HasBuff("MISSING")))
				{
					// wait couse moron is near
					return true;
				}
			}

			return false;

		}
	}
}

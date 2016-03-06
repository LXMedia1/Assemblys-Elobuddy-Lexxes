using System;
using System.Linq;
using System.Reflection;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace LX.LX_Activator.Summoners
{
	class Exhoust : Summoner
	{
		public static bool _active;
		public static Spell.Targeted Exh;
		public override void CreateMenu()
		{
			if (ObjectManager.Player.GetSpellSlotFromName("summonerexhaust") == SpellSlot.Unknown)
				return;
			_active = true;

			Exh = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerexhaust"), 650);

			Activator.Menu_Summoners.AddGroupLabel("Exhoust");
			Activator.Menu_Summoners.AddLabel("Basic");
			Activator.Menu_Summoners.Add("exhoust_safe", new CheckBox("Use to Safe Me/Buddy"));
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
			if (AntiChampBehavier() || !Exh.IsReady())
				return;
			if (Activator.Menu_Summoners["exhoust_safe"].Cast<CheckBox>().CurrentValue)
			{
				var Enemys = EntityManager.Heroes.Enemies.Where(u => u.IsValidTarget(Exh.Range));
				var Mates = EntityManager.Heroes.Allies.Where(u => Enemys.Any(en => en.IsInAutoAttackRange(u)));
				AIHeroClient mostDangerousTarget = null;
				foreach (var mate in Mates)
				{
					foreach (var enemy in Enemys)
					{
						if (enemy.GetAutoAttackDamage(mate) * 3 >= mate.Health && mostDangerousTarget == null)
							mostDangerousTarget = enemy;
						if (enemy.GetAutoAttackDamage(mate) * 3 >= mate.Health && mostDangerousTarget.GetAutoAttackDamage(mate) < enemy.GetAutoAttackDamage(mate))
							mostDangerousTarget = enemy;
					}
				}
				if (mostDangerousTarget != null)
					Exh.Cast(mostDangerousTarget);
			}
		}

		public bool AntiChampBehavier()
		{
			return false;
			// Vayne R, Yi R
			

			return false;

		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK.Enumerations;

namespace LX.LX_Evade
{
	class SpellDB
	{
		public static List<SpellData> Spells = new List<SpellData>();

		static SpellDB()
		{
			Spells.Add(
				   new SpellData
				   {
					   ChampionName = "Morgana",
					   SpellName = "DarkBindingMissile",
					   Slot = SpellSlot.Q,
					   Type = SkillShotType.Linear,
					   Delay = 250,
					   Range = 1300,
					   Radius = 80,
					   MissileSpeed = 1200,
					   FixedRange = true,
					   AddHitbox = true,
					   DangerValue = 3,
					   IsDangerous = true,
					   MissileSpellName = "DarkBindingMissile",
					   EarlyEvade = new[] { EarlyObjects.Allies, EarlyObjects.Minions, EarlyObjects.AllyObjects },
					   CanBeRemoved = true,
					   CollisionObjects =
						   new[] { Collision.CollisionObjectTypes.Champions, Collision.CollisionObjectTypes.Minion, Collision.CollisionObjectTypes.YasuoWall },
				   });
		}


		public static SpellData GetByName(string spellName)
		{
			spellName = spellName.ToLower();
			foreach (var spellData in Spells)
			{
				if (spellData.SpellName.ToLower() == spellName || spellData.ExtraSpellNames.Contains(spellName))
				{
					return spellData;
				}
			}

			return null;
		}
	}
}

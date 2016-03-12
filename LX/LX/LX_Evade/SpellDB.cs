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
		public static List<Spell> Spells = new List<Spell>();

		static SpellDB()
		{
			// Aatrox Q
			Spells.Add(
				   new Spell
				   {
					   ChampionName = "Aatrox",
					   SpellName = "AatroxQ",
					   Slot = SpellSlot.Q,
					   Type = SkillShotType.Circular,
					   Delay = 600,
					   Range = 650,
					   Radius = 250,
					   MissileSpeed = 2000,
					   AddHitbox = true,
					   DamageLvl = Spell.DamageLevel.Medium,
					   CCLvl = Spell.CCLevel.Slow,
				   });

			// Ahri E
			Spells.Add(
			   new Spell
			   {
				   ChampionName = "Ahri",
				   SpellName = "AhriSeduce",
				   Slot = SpellSlot.E,
				   Type = SkillShotType.Linear,
				   Delay = 250,
				   Range = 1000,
				   Whidth = 60,
				   MissileSpeed = 1550,
				   AddHitbox = true,
				   DamageLvl = Spell.DamageLevel.Medium,
				   CCLvl = Spell.CCLevel.Taunt,
				   CollisionObj = Spell.CollisionObjects.BasicCollision,
				   MissileSpellName = "AhriSeduceMissile",
			   });

			#region Amumu

			Spells.Add(
				new Spell
				{
					ChampionName = "Amumu",
					SpellName = "BandageToss",
					Slot = SpellSlot.Q,
					Type = SkillShotType.Linear,
					Delay = 250,
					Range = 1100,
					Whidth = 90,
					MissileSpeed = 2000,
					AddHitbox = true,
					DamageLvl = Spell.DamageLevel.Medium,
					CCLvl = Spell.CCLevel.Stun,
					MissileSpellName = "SadMummyBandageToss",
					CollisionObj = Spell.CollisionObjects.BasicCollision,
				});

			Spells.Add(
				new Spell
				{
					ChampionName = "Amumu",
					SpellName = "CurseoftheSadMummy",
					Slot = SpellSlot.R,
					Type = SkillShotType.Circular,
					Delay = 250,
					Range = 0,
					Radius = 550,
					DamageLvl = Spell.DamageLevel.Deadly,
					CCLvl = Spell.CCLevel.Stun,
					MissileSpeed = int.MaxValue,
					AddHitbox = false,
					MissileSpellName = "",
				});

			#endregion

			#region Ashe

			Spells.Add(
				new Spell
				{
					ChampionName = "Ashe",
					SpellName = "EnchantedCrystalArrow",
					Slot = SpellSlot.R,
					Type = SkillShotType.Linear,
					Delay = 250,
					Range = 20000,
					Whidth = 130,
					MissileSpeed = 1600,
					AddHitbox = true,
					DamageLvl = Spell.DamageLevel.Deadly,
					CCLvl = Spell.CCLevel.Stun,
					MissileSpellName = "EnchantedCrystalArrow",
					CollisionObj = Spell.CollisionObjects.Enemy_Champions,
				});
			#endregion

			#region Bard

			Spells.Add(
				new Spell
				{
					ChampionName = "Bard",
					SpellName = "BardQ",
					Slot = SpellSlot.Q,
					Type = SkillShotType.Linear,
					Delay = 250,
					Range = 950,
					Whidth = 60,
					MissileSpeed = 1600,
					AddHitbox = true,
					MissileSpellName = "BardQMissile",
					CollisionObj = Spell.CollisionObjects.BasicCollision_allow1,
					DamageLvl = Spell.DamageLevel.Low,
					CCLvl = Spell.CCLevel.Stun,
				});
			
			Spells.Add(
				new Spell
				{
					ChampionName = "Bard",
					SpellName = "BardR",
					Slot = SpellSlot.R,
					Type = SkillShotType.Circular,
					Delay = 500,
					Range = 3400,
					Radius = 350,
					MissileSpeed = 2100,
					AddHitbox = true,
					MissileSpellName = "BardR",
					DamageLvl = Spell.DamageLevel.None,
					CCLvl = Spell.CCLevel.Stun,
				});
			#endregion

			#region Blitzcrank

			Spells.Add(
				new Spell
				{
					ChampionName = "Blitzcrank",
					SpellName = "RocketGrab",
					Slot = SpellSlot.Q,
					Type = SkillShotType.Linear,
					Delay = 250,
					Range = 1050,
					Whidth = 70,
					MissileSpeed = 1800,
					AddHitbox = true,
					DamageLvl = Spell.DamageLevel.High,
					CCLvl = Spell.CCLevel.Stun,
					MissileSpellName = "RocketGrabMissile",
					CollisionObj = Spell.CollisionObjects.BasicCollision,
				});

			Spells.Add(
				new Spell
				{
					ChampionName = "Blitzcrank",
					SpellName = "StaticField",
					Slot = SpellSlot.R,
					Type = SkillShotType.Circular,
					Delay = 250,
					Range = 0,
					Radius = 600,
					MissileSpeed = int.MaxValue,
					AddHitbox = false,
					DamageLvl = Spell.DamageLevel.High,
					EffectLvl = Spell.EffectLevel.Silence,
					MissileSpellName = "",
				});
			#endregion

			#region Brand

			Spells.Add(
				new Spell
				{
					ChampionName = "Brand",
					SpellName = "BrandBlaze",
					Slot = SpellSlot.Q,
					Type = SkillShotType.Linear,
					Delay = 250,
					Range = 1100,
					Whidth = 60,
					MissileSpeed = 1600,
					AddHitbox = true,
					MissileSpellName = "BrandBlazeMissile",
					DamageLvl = Spell.DamageLevel.High,
					CCLvl = Spell.CCLevel.Stun,
					CollisionObj = Spell.CollisionObjects.BasicCollision,
				});

			Spells.Add(
				new Spell
				{
					ChampionName = "Brand",
					SpellName = "BrandFissure",
					Slot = SpellSlot.W,
					Type = SkillShotType.Circular,
					Delay = 850,
					Range = 900,
					Radius = 240,
					MissileSpeed = int.MaxValue,
					AddHitbox = true,
					DamageLvl = Spell.DamageLevel.High,
					MissileSpellName = "",
				});

			#endregion

			#region Caitlyn

			Spells.Add(
				new Spell
				{
					ChampionName = "Caitlyn",
					SpellName = "CaitlynPiltoverPeacemaker",
					Slot = SpellSlot.Q,
					Type = SkillShotType.Linear,
					Delay = 625,
					Range = 1300,
					Whidth = 90,
					MissileSpeed = 2200,
					AddHitbox = true,
					DamageLvl = Spell.DamageLevel.Medium,
					CCLvl = Spell.CCLevel.Stun,				
				});

			Spells.Add(
				new Spell
				{
					ChampionName = "Caitlyn",
					SpellName = "CaitlynEntrapment",
					Slot = SpellSlot.E,
					Type = SkillShotType.Linear,
					Delay = 125,
					Range = 1000,
					Whidth = 70,
					MissileSpeed = 1600,
					AddHitbox = true,
					MissileSpellName = "CaitlynEntrapmentMissile",
					DamageLvl = Spell.DamageLevel.Low,
					CCLvl = Spell.CCLevel.Slow,
					CollisionObj = Spell.CollisionObjects.BasicCollision,
				});

			#endregion Caitlyn

			#region Draven

			Spells.Add(
				new Spell
				{
					ChampionName = "Draven",
					SpellName = "DravenDoubleShot",
					Slot = SpellSlot.E,
					Type = SkillShotType.Linear,
					Delay = 250,
					Range = 1100,
					Whidth = 130,
					MissileSpeed = 1400,
					AddHitbox = true,
					MissileSpellName = "DravenDoubleShotMissile",
					DamageLvl = Spell.DamageLevel.Low,
					CCLvl = Spell.CCLevel.KnockSideway,
					CollisionObj = Spell.CollisionObjects.YasuoWall,
				});

			Spells.Add(
				new Spell
				{
					ChampionName = "Draven",
					SpellName = "DravenRCast",
					Slot = SpellSlot.R,
					Type = SkillShotType.Linear,
					Delay = 400,
					Range = 20000,
					Whidth = 160,
					MissileSpeed = 2000,
					AddHitbox = true,
					MissileSpellName = "DravenR",
					DamageLvl = Spell.DamageLevel.Deadly,
					CollisionObj = Spell.CollisionObjects.YasuoWall,
				});

			#endregion Draven
			
			#region Ekko

			Spells.Add(
				new Spell
				{
					ChampionName = "Ekko",
					SpellName = "EkkoQ",
					Slot = SpellSlot.Q,
					Type = SkillShotType.Linear,
					Delay = 250,
					Range = 950,
					Whidth = 60,
					MissileSpeed = 1650,
					AddHitbox = true,
					MissileSpellName = "ekkoqmis",
					DamageLvl = Spell.DamageLevel.High,
					CollisionObj = Spell.CollisionObjects.BasicChampionCollision,
				});

			Spells.Add(
				new Spell
				{
					ChampionName = "Ekko",
					SpellName = "EkkoW",
					Slot = SpellSlot.W,
					Type = SkillShotType.Circular,
					Delay = 3750,
					Range = 1600,
					Radius = 375,
					MissileSpeed = 1650,
					AddHitbox = false,
					DamageLvl = Spell.DamageLevel.Low,
					CCLvl = Spell.CCLevel.Stun,
					MissileSpellName = "EkkoW",
				});

			#endregion Ekko

			// Morgana Q
			Spells.Add(
				   new Spell
				   {
					   ChampionName = "Morgana",
					   SpellName = "DarkBindingMissile",
					   MissileSpellName = "DarkBindingMissile",
					   HitName = "Morgana_Base_Q_Tar.troy",
					   Slot = SpellSlot.Q,
					   Type = SkillShotType.Linear,
					   Delay = 250,
					   Range = 1300,
					   Whidth = 80,
					   MissileSpeed = 1200,
					   DamageLvl = Spell.DamageLevel.Medium,
					   CCLvl = Spell.CCLevel.Snare,
					   CollisionObj = Spell.CollisionObjects.BasicCollision,
				   });

			

		}


		public static Spell GetSpellByName(string spellName)
		{
			return Spells.FirstOrDefault(spell => spell.SpellName == spellName);
		}
	}
}

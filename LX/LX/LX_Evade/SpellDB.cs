﻿using System;
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
					Radius = 90,
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
					Radius = 60,
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

			#region Blatzcrank

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

			// Ekko W
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

using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace LX.LX_Activator.Summoners
{
	class Smite : Summoner
	{
		public static bool _active;
		public static float SmiteDamage;
		public static List<Monster> MonsterList = new List<Monster>();
 
		public enum SmiteType
		{
			Basic,
			Duel,
			Quick,
			Ganker,
			AOE,
		}
		public override void CreateMenu()
		{
			if (ObjectManager.Player.GetSpellSlotFromName("summonersmite") == SpellSlot.Unknown &&
				ObjectManager.Player.GetSpellSlotFromName("s5_summonersmiteduel") == SpellSlot.Unknown &&
				ObjectManager.Player.GetSpellSlotFromName("s5_summonersmitequick") == SpellSlot.Unknown &&
				ObjectManager.Player.GetSpellSlotFromName("s5_summonersmiteplayerganker") == SpellSlot.Unknown &&
				ObjectManager.Player.GetSpellSlotFromName("itemsmiteaoe") == SpellSlot.Unknown)
				return;
			_active = true;

			SetMonsterList();

			Activator.Menu_Summoners.AddGroupLabel("Smite");
			Activator.Menu_Summoners.AddLabel("Basic Monster Smiting");
			foreach (var monster in MonsterList.Where(m => m.IsOnMap()))
				monster.AddCheckbox();
			
		}

		public override bool active()
		{
			return _active;
		}
		public override void Use()
		{
			UpdateSmite();
			foreach (var monster in MonsterList.Where(m => m.IsActive()))
			{
				monster.SmiteIt();
			}
		}

		private void UpdateSmite()
		{
			var level = ObjectManager.Player.Level;
			int[] smitedamage = {20*level + 370, 30*level + 330, 40*level + 240, 50*level + 100};
			SmiteDamage = smitedamage.Max();
		}

		private void SetMonsterList()
		{
			MonsterList.Add(new Monster("TT_Spiderboss","SpiderBoss",GameMapId.TwistedTreeline,true));
			MonsterList.Add(new Monster("TTNGolem","Golem TT", GameMapId.TwistedTreeline, true));
			MonsterList.Add(new Monster("TTNWolf","Wolf TT", GameMapId.TwistedTreeline, true));
			MonsterList.Add(new Monster("TTNWraith","Wraith TT", GameMapId.TwistedTreeline, true));
			MonsterList.Add(new Monster("SRU_Dragon", "Dragon", GameMapId.SummonersRift, true));
			MonsterList.Add(new Monster("SRU_Baron", "Baron", GameMapId.SummonersRift, true));
			MonsterList.Add(new Monster("SRU_Blue", "Blue Buff", GameMapId.SummonersRift, true));
			MonsterList.Add(new Monster("SRU_Red", "Red Buff", GameMapId.SummonersRift, true));
			MonsterList.Add(new Monster("SRU_Krug", "Krug", GameMapId.SummonersRift, true));
			MonsterList.Add(new Monster("SRU_Gromp", "Gromp", GameMapId.SummonersRift, true));
			MonsterList.Add(new Monster("SRU_Murkwolf", "Wolf", GameMapId.SummonersRift, true));
			MonsterList.Add(new Monster("SRU_Razorbeak", "Razorbeak", GameMapId.SummonersRift, true));
			MonsterList.Add(new Monster("Sru_Crab", "Crap", GameMapId.SummonersRift, true));
		}

		public class Monster
		{
			public string Name;
			public string RealName;
			public GameMapId MapID;
			public bool DefaultValue;

			public Monster(string rname, string name, GameMapId mapid, bool defaultvalue)
			{
				RealName = rname;
				Name = name;
				MapID = mapid;
				DefaultValue = defaultvalue;
			}

			public bool IsOnMap()
			{
				return MapID == Game.MapId;
			}

			public bool IsActive()
			{
				return IsOnMap() && Activator.Menu_Summoners["smite_" + RealName].Cast<CheckBox>().CurrentValue;
			}
			public void AddCheckbox()
			{
				Activator.Menu_Summoners.Add("smite_" + RealName, new CheckBox(Name, DefaultValue));
			}

			internal void SmiteIt()
			{
				var Monster =
					ObjectManager.Get<Obj_AI_Minion>()
						.FirstOrDefault(
							m =>
								m.IsValidTarget(570) &&
								m.BaseSkinName.StartsWith(RealName) &&
								!m.BaseSkinName.Contains("Mini") &&
								!m.BaseSkinName.Contains("Spawn"));
				if (Monster == null)
					return;
				if (!(Monster.Health <= SmiteDamage)) 
					return;
				var smite = GetActiveSmite();
				if (smite == null)
					return;
				smite.Cast(Monster);
			}

			private Spell.Targeted GetActiveSmite()
			{
				if (ObjectManager.Player.GetSpellSlotFromName("summonersmite") != SpellSlot.Unknown)
					return new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonersmite"), 570);
				if (ObjectManager.Player.GetSpellSlotFromName("s5_summonersmiteduel") != SpellSlot.Unknown)
					return new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("s5_summonersmiteduel"), 570);
				if (ObjectManager.Player.GetSpellSlotFromName("s5_summonersmitequick") != SpellSlot.Unknown)
					return new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("s5_summonersmitequick"), 570);
				if (ObjectManager.Player.GetSpellSlotFromName("s5_summonersmiteplayerganker") != SpellSlot.Unknown)
					return new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("s5_summonersmiteplayerganker"), 570);
				if (ObjectManager.Player.GetSpellSlotFromName("itemsmiteaoe") != SpellSlot.Unknown)
					return new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("itemsmiteaoe"), 570);
				return null;

			}
		}
	}
}

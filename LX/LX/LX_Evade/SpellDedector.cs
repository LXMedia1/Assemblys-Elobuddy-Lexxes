using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using Newtonsoft.Json.Serialization;

namespace LX.LX_Evade
{
	public static class SpellDedector
	{
		public delegate void OnSpellWillHitH(Spell spell, Obj_AI_Base target);
		public static event OnSpellWillHitH OnSpellWillHit;

		public static List<Spell> ActiveSpells = new List<Spell>();

		internal static void Initiate()
		{
			Game.OnUpdate += Game_OnUpdate;
			Drawing.OnDraw += Drawing_OnDraw;
			Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
			GameObject.OnCreate += OnCreateObject;
			GameObject.OnDelete += OnDeleteObject;
			
		}

		private static void Drawing_OnDraw(EventArgs args)
		{
			ActiveSpells.Draw();
		}

		private static void Game_OnUpdate(EventArgs args)
		{
			foreach (var spell in ActiveSpells.Where(s=> !s.IsFriendly))
				foreach (var hero in EntityManager.Heroes.Allies)
					if (spell.IsAboutToHit(500, hero))
						OnSpellWillHit(spell, hero);

			ActiveSpells.RemoveAll(spell => !spell.IsActive());
		}
		
		private static void OnCreateObject(GameObject sender, EventArgs args)
		{
			for (var i = ActiveSpells.Count - 1; i >= 0; i--)
			{
				var spell = ActiveSpells[i];
				if ((spell.HitName == "" || sender.Name != spell.HitName) &&
					(spell.HitNameRegex == "" || !new Regex(spell.HitNameRegex).IsMatch(sender.Name))) continue;
				Console.WriteLine("RemoveSpell => [" + sender.Name + "] Collide with anything.");
				ActiveSpells.RemoveAt(i);
				return;
			}

			if (IgnoreContains(sender.Name))
				return;
			
			//Console.WriteLine(Core.GameTickCount + " OnCreateObject_sender_Name: " + sender.Name);
		}

		private static void OnDeleteObject(GameObject sender, EventArgs args)
		{
			for (var i = ActiveSpells.Count - 1; i >= 0; i--)
			{
				var spell = ActiveSpells[i];
				if ((spell.HitName == "" || sender.Name != spell.HitName) &&
					(spell.HitNameRegex == "" || !new Regex(spell.HitNameRegex).IsMatch(sender.Name))) continue;
				Console.WriteLine("RemoveSpell => [" + sender.Name + "] Collide with anything.");
				ActiveSpells.RemoveAt(i);
				return;
			}

			if (IgnoreContains(sender.Name) && NotIgnoreContains(sender.Name))
				return;
			//Console.WriteLine(Core.GameTickCount + " OnDeleteObject_sender_Name: " + sender.Name);
		}

		private static bool NotIgnoreContains(string p)
		{
			return !(p.Contains("Morgana") ||
					p.Contains("Morgana"));
		}

		private static bool IgnoreContains(string p)
		{
			return (p.Contains("Order") ||
					p.Contains("order") ||
					p.Contains("Chaos") ||
					p.Contains("chaos") ||
					p.Contains("missile") ||
					p.Contains("empty.troy") || 
					p.Contains("Shopkeeper") ||
					p.Contains("Minion") ||
					p.Contains("DrawFX") ||
					p.Contains("LaserSight") ||
					p.Contains("SRU_") ||
					p.Contains("FeelNoPain") ||
					p.Contains("DrawFX"));
		}

		private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
		{
			if (sender == null || !sender.IsValid)
				return;
			var spell = SpellDB.GetSpellByName(args.SData.Name);
			if (spell != null)
				ActiveSpells.Add(new Spell(sender,args,spell));
			else
			{
			/*	Console.WriteLine(Core.GameTickCount + " ProcessSpellCast_SData_Name: " + args.SData.Name);
				Console.WriteLine(Core.GameTickCount + " ProcessSpellCast_SData_CastRadius: " + args.SData.CastRadius);
				Console.WriteLine(Core.GameTickCount + " ProcessSpellCast_SData_CastRange: " + args.SData.CastRange);
				Console.WriteLine(Core.GameTickCount + " ProcessSpellCast_SData_CastTime: " + args.SData.CastTime);
				Console.WriteLine(Core.GameTickCount + " ProcessSpellCast_SData_CastRadiusSecondary: " + args.SData.CastRadiusSecondary);			
			*/}
		}

	
	}
}

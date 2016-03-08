using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using Color = System.Drawing.Color;

namespace LX.LX_Evade
{
	static class Evade
	{
		public static List<RunningSpell> ActiveSpellList = new List<RunningSpell>();  
		internal static void Initiate()
		{
			Obj_AI_Base.OnProcessSpellCast += ObjAiHeroOnOnProcessSpellCast;
			Game.OnUpdate += OnUpdate;
			Drawing.OnDraw += OnDraw;
		}

		private static void OnUpdate(EventArgs args)
		{
			ActiveSpellList.RemoveAll(spells => !spells.IsActive());
		}

		private static void OnDraw(EventArgs args)
		{
			foreach (var spell in ActiveSpellList)
			{
				spell.Draw();
			}
		}

		private static void ObjAiHeroOnOnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
		{
			if (sender == null || !sender.IsValid)
			{
				return;
			}
			//Console.WriteLine(Core.GameTickCount + " ProcessSpellCast: " + args.SData.Name);

			if (!sender.IsValid || sender.Team == ObjectManager.Player.Team && false /* Test on Alliance*/)
			{
				return;
			}
			var spellData = SpellDB.GetByName(args.SData.Name);

			//Skillshot not added in the database.
			if (spellData == null)
			{
				return;
			}

			ActiveSpellList.Add(new RunningSpell(sender, args, spellData));
		}

		public static void Draw(this RunningSpell spell)
		{
			Geometry.Polygon DrawingPolygon = new Geometry.Polygon();
			switch (spell.SpellData.Type)
			{
				case SkillShotType.Linear:
					DrawingPolygon = new Geometry.Polygon.Rectangle(spell.GlobalGetMissilePosition(), spell.CastData.End.To2D(), spell.SpellData.Radius);
					break;
			}
			DrawingPolygon.Draw(Color.White,2);
		}
		public class RunningSpell
		{
			public Obj_AI_Base Caster;
			public GameObjectProcessSpellCastEventArgs CastData;
			public SpellData SpellData;
			public int StartTick;
			public Vector3 Direction;
			public RunningSpell(Obj_AI_Base from, GameObjectProcessSpellCastEventArgs args, SpellData spell)
			{
				StartTick = Core.GameTickCount;
				Direction = (args.End - args.Start).Normalized();
				Caster = from;
				CastData = args;
				SpellData = spell;
			}
			public bool IsActive()
			{
				if (SpellData.MissileAccel != 0)
				{
					return Core.GameTickCount <= StartTick + 5000;
				}

				return Core.GameTickCount <=
					   StartTick + SpellData.Delay + SpellData.ExtraDuration +
					   1000 * (CastData.Start.Distance(CastData.End) / SpellData.MissileSpeed);
			}
			public Vector2 GlobalGetMissilePosition(int time = 0)
			{
				var t = Math.Max(0, Core.GameTickCount + time - StartTick - SpellData.Delay);
				t = (int)Math.Max(0, Math.Min(CastData.End.Distance(CastData.Start), t * SpellData.MissileSpeed / 1000));
				return (CastData.Start + Direction * t).To2D();
			}
		}
	}
}

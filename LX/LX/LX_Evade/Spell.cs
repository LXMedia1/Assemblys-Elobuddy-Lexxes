using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using GamePath = System.Collections.Generic.List<SharpDX.Vector2>;

namespace LX.LX_Evade
{
	public class Spell
	{
		public enum DamageLevel
		{
			None = 0,
			Low = 1,
			Medium = 2,
			High = 3,
			Deadly = 4,
		}
		public enum CCLevel
		{
			None,
			Slow,
			Snare,
			Fear,
			KnockUp,
			Stun,
			Taunt,
			KnockSideway,
		}
		public enum EffectLevel
		{
			None ,
			Silence,	
		}
		public enum CollisionObjects
		{
			None,
			BasicCollision,
			Enemy_Minions,
			Friendly_Minions,
			Monsters,
			Enemy_Champions,
			Friendly_Champions,
			YasuoWall,
			Wall,
			BasicCollision_allow1,
			BasicChampionCollision,
		}


		public string ChampionName; // Champion who can Cast this Spell
		public string SpellName; // Spellname 
		public string MissileSpellName; // Missle / Projectilname
		public string HitName = ""; // Name of the obj who apears if Hit/Collide
		public string HitNameRegex = ""; // Regex if alli/enemy have diffrent names
		public SpellSlot Slot; // Spellslot
		public SkillShotType Type; // Type of the Spell ( linear circle cone ... )
		public int Delay; // Time after cast when Projectile starts
		public int Range; // MaxRange of Spell
		public int Radius; // Radius for CirleSpells
		public int Whidth; // Whide for LinearSpells
		public int MissileSpeed; // ProjectilSpeed
		public bool AddHitbox = true; // Adding Hitbox ( Morgana W no hitbox for exemple )
		public DamageLevel DamageLvl = DamageLevel.None;
		public CCLevel CCLvl = CCLevel.None;
		public EffectLevel EffectLvl = EffectLevel.None;
		public CollisionObjects CollisionObj = CollisionObjects.None;
		public int MissileAccel = 0;
		public int ExtraDuration = 0;
		public Obj_AI_Base Caster;
		public bool IsFriendly;
		public bool DontCross = true;
		public bool DontAddExtraDuration;

		public int StartTick;
		public Vector3 StartPosition;
		public Vector3 EndPosition;
		public Vector3 Direction;
		public float Distance;

		public static Geometry.Polygon Polygon;
		public Geometry.Polygon DrawingPolygon;

		public Spell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args, Spell spell)
		{
			StartTick = Core.GameTickCount;
			StartPosition = args.Start;
			EndPosition = args.End;
			Distance = StartPosition.Distance(EndPosition);
			Direction = (EndPosition - StartPosition).Normalized();

			Caster = sender;
			IsFriendly = sender.Team == ObjectManager.Player.Team;

			ChampionName = spell.ChampionName;
			SpellName = spell.SpellName;
			MissileSpellName = spell.MissileSpellName;
			HitName = spell.HitName;
			Slot = spell.Slot;
			Type = spell.Type;
			Delay = spell.Delay;
			Range = spell.Range;
			Radius = spell.Radius;
			Whidth = spell.Whidth;
			MissileSpeed = spell.MissileSpeed;
			AddHitbox = spell.AddHitbox;
			DamageLvl = spell.DamageLvl;
			CCLvl = spell.CCLvl;
			CollisionObj = spell.CollisionObj;

			DrawingPolygon = new Geometry.Polygon.Rectangle(spell.CurrentPosition(), spell.EndPosition, spell.Whidth);
			Polygon = new Geometry.Polygon.Rectangle(spell.CurrentPosition(), spell.EndPosition, spell.AddHitbox ? spell.Whidth + ObjectManager.Player.BoundingRadius * 2 : spell.Whidth);

		}

		public Spell()
		{

		}

		public Spell(Spell spell)
		{
			ChampionName = spell.ChampionName;
			SpellName = spell.SpellName;
			MissileSpellName = spell.MissileSpellName;
			HitName = spell.HitName;
			Slot = spell.Slot;
			Type = spell.Type;
			Delay = spell.Delay;
			Range = spell.Range;
			Radius = spell.Radius;
			Whidth = spell.Whidth;
			MissileSpeed = spell.MissileSpeed;
			AddHitbox = spell.AddHitbox;
			DamageLvl = spell.DamageLvl;
			CCLvl = spell.CCLvl;
			CollisionObj = spell.CollisionObj;

			StartTick = Core.GameTickCount;
			StartPosition = ObjectManager.Player.Position + Vector3.Normalize(ObjectManager.Player.Direction - ObjectManager.Player.Position) * Range / 2;
			EndPosition = StartPosition + Vector3.Normalize(ObjectManager.Player.Position - StartPosition) * Range;
			Direction = (ObjectManager.Player.Position - StartPosition).Normalized();

			Caster = ObjectManager.Player;
			IsFriendly = false;

		}

		public Vector3 CurrentPosition()
		{
			return PredictedPosition();
		}
		public Vector3 PredictedPosition(int time = 0)
		{
			var t = Math.Max(0, Core.GameTickCount + time - StartTick - Delay);
			t = (int)Math.Max(0, Math.Min(EndPosition.Distance(StartPosition), t * MissileSpeed / 1000));
			return (StartPosition + Direction * t);
		}

		public Geometry.Polygon EvadePolygon { get; set; }
		public Geometry.Polygon PathFindingPolygon { get; set; }
		public Geometry.Polygon PathFindingInnerPolygon { get; set; }
		public bool IsActive()
		{
			if (MissileAccel != 0)
			{
				return Core.GameTickCount <= StartTick + 5000;
			}

			return Core.GameTickCount <=
				   StartTick + Delay + ExtraDuration +
				   1000 * (StartPosition.Distance(EndPosition) / MissileSpeed);
		}

		public bool IsAboutToHit(int time, Obj_AI_Base unit)
		{
			if (Type == SkillShotType.Linear)
			{
				var missilePos = PredictedPosition();
				var missilePosAfterT = PredictedPosition(time);

				var projection = unit.ServerPosition.To2D().ProjectOn(missilePos.To2D(), missilePosAfterT.To2D());

				return projection.IsOnSegment && projection.SegmentPoint.Distance(unit.ServerPosition) < Whidth;
			}
			var timeToExplode = ExtraDuration + Delay +
								(int)((1000 * StartPosition.Distance(EndPosition)) / MissileSpeed) -
								(Core.GameTickCount - StartTick);
			return timeToExplode <= time;
		}

		public bool IsSafe(Vector2 point)
		{
			return Polygon.IsOutside(point);
		}
		
	}
}

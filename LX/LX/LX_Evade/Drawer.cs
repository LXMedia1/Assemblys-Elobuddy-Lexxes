using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using Color = System.Drawing.Color;


namespace LX.LX_Evade
{
	static class Drawer
	{
		public static void Draw(this List<Spell> spelllist)
		{
			foreach (var spell in spelllist)
			{
				spell.Draw();
			}
		}

		public static void Draw(this Spell spell)
		{
			if (spell.IsFriendly)
				return;
			Geometry.Polygon PolygonClear;
			//var PolygonReal = new Geometry.Polygon();
			switch (spell.Type)
			{
				case SkillShotType.Linear:
					PolygonClear = new Geometry.Polygon.Rectangle(spell.CurrentPosition(), spell.EndPosition, spell.Whidth);
					PolygonClear.Draw(Color.White, 2);
					/*PolygonReal = new Geometry.Polygon.Rectangle(spell.CurrentPosition(), spell.EndPosition, spell.AddHitbox ? spell.Whidth + ObjectManager.Player.BoundingRadius * 2 :spell.Whidth);
					PolygonReal.Draw(Color.Green, 2);
					var CandidateList = PolygonReal.GetAllEvadePositions();
					foreach (var candi in CandidateList)
					{
						var drawpoint = new Geometry.Polygon.Rectangle(ObjectManager.Player.Position, candi.To3D(), 1);
						drawpoint.Draw(Color.Black);
					}*/
					break;
				case SkillShotType.Circular:
					PolygonClear = new Geometry.Polygon.Circle(spell.EndPosition, spell.Radius);
					PolygonClear.Draw(Color.White, 2);
					//PolygonReal = new Geometry.Polygon.Circle(spell.EndPosition, spell.AddHitbox ? spell.Radius + ObjectManager.Player.BoundingRadius * 2 : spell.Radius);
					break;
			}
		}
		public static Geometry.ProjectionInfo ProjectOn(this Vector2 point, Vector2 segmentStart, Vector2 segmentEnd)
		{
			var cx = point.X;
			var cy = point.Y;
			var ax = segmentStart.X;
			var ay = segmentStart.Y;
			var bx = segmentEnd.X;
			var by = segmentEnd.Y;
			var rL = ((cx - ax) * (bx - ax) + (cy - ay) * (by - ay)) /
					 ((float)Math.Pow(bx - ax, 2) + (float)Math.Pow(by - ay, 2));
			var pointLine = new Vector2(ax + rL * (bx - ax), ay + rL * (by - ay));
			float rS;
			if (rL < 0)
			{
				rS = 0;
			}
			else if (rL > 1)
			{
				rS = 1;
			}
			else
			{
				rS = rL;
			}

			var isOnSegment = rS.CompareTo(rL) == 0;
			var pointSegment = isOnSegment ? pointLine : new Vector2(ax + rS * (bx - ax), ay + rS * (@by - ay));
			return new Geometry.ProjectionInfo(isOnSegment, pointSegment, pointLine);
		}

		public static List<Vector2> To2DList(this Vector3[] v)
		{
			return v.Select(point => point.To2D()).ToList();
		}
	}
}

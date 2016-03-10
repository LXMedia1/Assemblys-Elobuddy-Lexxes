using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy.SDK;
using SharpDX;
using EloBuddy;

namespace LX.LX_Evade
{
	static class EvadePositions
	{
		public static List<Vector2> GetAllEvadePositions(this Geometry.Polygon polygon)
		{
			var CandidateList = new List<Vector2>();
			var myPosition = ObjectManager.Player.Position.To2D();
			for (var i = 0; i <= polygon.Points.Count - 1; i++)
			{
				var sideStart = polygon.Points[i];
				var sideEnd = polygon.Points[(i == polygon.Points.Count - 1) ? 0 : i + 1];
				var originalCandidate = myPosition.ProjectOn(sideStart, sideEnd).SegmentPoint;
				var distanceToEvadePoint = Vector2.DistanceSquared(originalCandidate, myPosition);

				if (distanceToEvadePoint < 600 * 600)
				{
					var direction = (sideEnd - sideStart).Normalized();
					const int s = 10;
					for (var j = -s; j <= s; j++)
					{
						var candidate = originalCandidate + j * 20 * direction;
						CandidateList.Add(candidate);
					}

				}
			}
			return CandidateList;
		}
		
	}
}

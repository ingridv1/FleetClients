using FleetClients.FleetManagerServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FleetClients
{
	public static class PoseDataFactory
	{
		public static Regex PoseStringRegex { get; } = new Regex(@"(?:[^\d-]*)(?<x>-?\d*\.?\d*)(?:[^\d-]*)(?<y>-?\d*\.?\d*)(?:[^\d-]*)(?<heading>-?\d*\.?\d*$)", RegexOptions.Singleline);

		public static bool TryParseString(string poseString, out PoseData poseData)
		{
			try
			{
				poseData = ParseString(poseString);
			}
			catch (Exception ex)
			{
				poseData = null;
			}

			return poseData != null;
		}

		public static PoseData ParseString(string poseString)
		{
			if (string.IsNullOrEmpty(poseString)) throw new ArgumentNullException("poseString");

			Match match = PoseStringRegex.Match(poseString);

			if (match.Success)
			{
				double x = double.Parse(match.Groups[1].Value);
				double y = double.Parse(match.Groups[2].Value);
				double heading = double.Parse(match.Groups[3].Value);

				return new PoseData() { X = x, Y = y, Heading = heading };
			}

			throw new ArgumentOutOfRangeException("poseString");
		}

		public static PoseData NaNPose => new PoseData() { X = double.NaN, Y = double.NaN, Heading = double.NaN };
		
		public static PoseData ZeroPose => new PoseData() { X = 0, Y = 0, Heading = 0 };

		public static PoseData OriginEast => new PoseData() { X = 0, Y = 0, Heading = 0 };

		public static PoseData OriginWest=> new PoseData() { X = 0, Y = 0, Heading = Math.PI };

		public static PoseData OriginSouth => new PoseData() { X = 0, Y = 0, Heading = -Math.PI / 2 };

		public static PoseData OriginNorth => new PoseData() { X = 0, Y = 0, Heading = Math.PI / 2 };
	}
}

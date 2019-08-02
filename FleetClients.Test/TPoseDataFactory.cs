using FleetClients.FleetManagerServiceReference;
using NUnit.Framework;
using System.Collections.Generic;

namespace FleetClients.Test
{
	[TestFixture]
	[Category("PoseDataFactory")]
	public class TPoseDataFactory
	{
		[Test]
		[TestCase(0,1,2)]
		[TestCase(0, -1, -2)]
		public void ParseString(double x, double y, double heading)
		{
			foreach(string subString in FormattedStrings(x,y,heading))
			{
				PoseData poseData = PoseDataFactory.ParseString(subString);

				Assert.AreEqual(x, poseData.X);
				Assert.AreEqual(y, poseData.Y);
				Assert.AreEqual(heading, poseData.Heading);
			}	
		}

		private IEnumerable<string> FormattedStrings(double x, double y, double heading)
		{
			List<string> strings = new List<string>();

			strings.Add(string.Format("x{0},y{1},heading{2}", x, y, heading));
			strings.Add(string.Format("{0},{1},{2}", x, y, heading));
			strings.Add(string.Format("x:{0},y:{1},heading:{2}", x, y, heading));
			strings.Add(string.Format("x {0},y {1},heading {2}", x, y, heading));

			return strings;
		}
	}
}

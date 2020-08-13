using FleetClients.Core.FleetManagerServiceReference;
using System;
using System.Text.RegularExpressions;

namespace FleetClients.Core
{
    /// <summary>
    /// Factory class for parsing pose strings.
    /// </summary>
    public static class PoseDataFactory
    {
        private static Regex PoseStringRegex { get; } = new Regex(@"(?:[^\d-]*)(?<x>-?\d*\.?\d*)(?:[^\d-]*)(?<y>-?\d*\.?\d*)(?:[^\d-]*)(?<heading>-?\d*\.?\d*$)", RegexOptions.Singleline);

        /// <summary>
        /// Tries to parse the pose defined in poseString.
        /// </summary>
        /// <param name="poseString">E.g. 0.5,0.2,0.57 or x0.5,y0.2,h0.57</param>
        /// <param name="poseData">Parsed results</param>
        /// <returns>True on success</returns>
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

        /// <summary>
        /// Parses the pose defined in poseString
        /// </summary>
        /// <param name="poseString">E.g. 0.5,0.2,0.57 or x0.5,y0.2,h0.57</param>
        /// <returns>Parsed result</returns>
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

        /// <summary>
        /// A pose where all elements are set to NaN.
        /// </summary>
        public static PoseData NaNPose => new PoseData() { X = double.NaN, Y = double.NaN, Heading = double.NaN };

        /// <summary>
        /// A pose with position origin and heading zero.
        /// </summary>
        public static PoseData ZeroPose => new PoseData() { X = 0, Y = 0, Heading = 0 };

        /// <summary>
        /// A pose with position origin and heading zero.
        /// </summary>
        public static PoseData OriginEast => new PoseData() { X = 0, Y = 0, Heading = 0 };

        /// <summary>
        /// A pose with position origin and heading one hundred and eighty degrees.
        /// </summary>
        public static PoseData OriginWest => new PoseData() { X = 0, Y = 0, Heading = Math.PI };

        /// <summary>
        /// A pose with position origin and heading one hundred and two hundred and seventy degrees.
        /// </summary>
        public static PoseData OriginSouth => new PoseData() { X = 0, Y = 0, Heading = -Math.PI / 2 };

        /// <summary>
        /// A pose with position origin and heading ninety degrees.
        /// </summary>
        public static PoseData OriginNorth => new PoseData() { X = 0, Y = 0, Heading = Math.PI / 2 };
    }
}
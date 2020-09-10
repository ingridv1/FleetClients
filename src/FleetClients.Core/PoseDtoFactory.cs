using GAAPICommon.Core.Dtos;
using System;
using System.Text.RegularExpressions;

namespace FleetClients.Core
{
    /// <summary>
    /// Factory class for parsing pose strings.
    /// </summary>
    public static class PoseDtoFactory
    {
        private static Regex PoseStringRegex { get; } = new Regex(@"(?:[^\d-]*)(?<x>-?\d*\.?\d*)(?:[^\d-]*)(?<y>-?\d*\.?\d*)(?:[^\d-]*)(?<heading>-?\d*\.?\d*$)", RegexOptions.Singleline);

        /// <summary>
        /// Tries to parse the pose defined in poseString.
        /// </summary>
        /// <param name="poseString">E.g. 0.5,0.2,0.57 or x0.5,y0.2,h0.57</param>
        /// <param name="poseDto">Parsed results</param>
        /// <returns>True on success</returns>
        public static bool TryParseString(string poseString, out PoseDto poseDto)
        {
            try
            {
                poseDto = ParseString(poseString);
            }
            catch (Exception ex)
            {
                poseDto = null;
            }

            return poseDto != null;
        }

        /// <summary>
        /// Parses the pose defined in poseString
        /// </summary>
        /// <param name="poseString">E.g. 0.5,0.2,0.57 or x0.5,y0.2,h0.57</param>
        /// <returns>Parsed result</returns>
        public static PoseDto ParseString(string poseString)
        {
            if (string.IsNullOrEmpty(poseString))
                throw new ArgumentNullException("poseString");

            Match match = PoseStringRegex.Match(poseString);

            if (match.Success)
            {
                double x = double.Parse(match.Groups[1].Value);
                double y = double.Parse(match.Groups[2].Value);
                double heading = double.Parse(match.Groups[3].Value);

                return new PoseDto() { X = x, Y = y, Heading = heading };
            }

            throw new ArgumentOutOfRangeException("poseString");
        }

        /// <summary>
        /// A pose where all elements are set to NaN.
        /// </summary>
        public static PoseDto NaNPose => new PoseDto() { X = double.NaN, Y = double.NaN, Heading = double.NaN };

        /// <summary>
        /// A pose with position origin and heading zero.
        /// </summary>
        public static PoseDto ZeroPose => new PoseDto() { X = 0, Y = 0, Heading = 0 };

        /// <summary>
        /// A pose with position origin and heading zero.
        /// </summary>
        public static PoseDto OriginEast => new PoseDto() { X = 0, Y = 0, Heading = 0 };

        /// <summary>
        /// A pose with position origin and heading one hundred and eighty degrees.
        /// </summary>
        public static PoseDto OriginWest => new PoseDto() { X = 0, Y = 0, Heading = Math.PI };

        /// <summary>
        /// A pose with position origin and heading one hundred and two hundred and seventy degrees.
        /// </summary>
        public static PoseDto OriginSouth => new PoseDto() { X = 0, Y = 0, Heading = -Math.PI / 2 };

        /// <summary>
        /// A pose with position origin and heading ninety degrees.
        /// </summary>
        public static PoseDto OriginNorth => new PoseDto() { X = 0, Y = 0, Heading = Math.PI / 2 };
    }
}
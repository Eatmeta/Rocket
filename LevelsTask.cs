using System;
using System.Collections.Generic;

namespace func_rocket
{
    public class LevelsTask
    {
        private static readonly Physics StandardPhysics = new Physics();
        private static readonly Vector StartPosition = new Vector(200, 500);
        private static Vector TargetPosition { get; set; }
        private static readonly Rocket Rocket = new Rocket(StartPosition, Vector.Zero, -Math.PI / 2);

        private static readonly Dictionary<string, Gravity> Gravities = new Dictionary<string, Gravity>
        {
            {
                "Zero",
                (size, v) => (v - new Vector(size.Width, size.Height)).Normalize().BoundTo(size)
            },
            {
                "Heavy",
                (size, v) => new Vector(0, size.Height - v.Y).Normalize() * 0.9
            },
            {
                "Up",
                (size, v) =>
                {
                    var result = new Vector(0, v.Y - size.Height);
                    var ratio = 300 / ((size.Height - v.Y) + 300.0);
                    return result.Normalize() * ratio;
                }
            },
            {
                "WhiteHole",
                (size, v) =>
                {
                    var result = new Vector(v.X - 600, v.Y - 200);
                    var ratio = 140 * result.Length / (result.Length * result.Length + 1);
                    return result.Normalize() * ratio;
                }
            },
            {
                "BlackHole",
                (size, v) =>
                {
                    var anomaly = (Rocket.Location + TargetPosition) / 2.0;
                    var result = new Vector(anomaly.X - v.X, anomaly.Y - v.Y);
                    var ratio = 300 * result.Length / (result.Length * result.Length + 1);
                    return result.Normalize() * ratio;
                }
            },
            {
                "BlackAndWhite",
                (size, v) =>
                {
                    var whiteHoleVector = Gravities["WhiteHole"].Invoke(size, v);
                    var blackHoleVector = Gravities["BlackHole"].Invoke(size, v);
                    return (whiteHoleVector + blackHoleVector) / 2.0;
                }
            }
        };

        public static IEnumerable<Level> CreateLevels()
        {
            foreach (var gravity in Gravities)
            {
                TargetPosition = gravity.Key == "Up" ? new Vector(700, 500) : new Vector(600, 200);

                yield return new Level(gravity.Key,
                    Rocket,
                    TargetPosition,
                    gravity.Value,
                    StandardPhysics);
            }
        }
    }
}
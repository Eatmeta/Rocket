using System;
using System.Drawing;

namespace func_rocket
{
    public class ForcesTask
    {
        /// <summary>
        /// Создает делегат, возвращающий по ракете вектор силы тяги двигателей этой ракеты.
        /// Сила тяги направлена вдоль ракеты и равна по модулю forceValue.
        /// </summary>
        public static RocketForce GetThrustForce(double forceValue)
        {
            return rocket =>  new Vector(
                rocket.Location.X * Math.Cos(rocket.Direction) + rocket.Location.Y * Math.Cos(rocket.Direction),
                rocket.Location.X * Math.Sin(rocket.Direction) + rocket.Location.Y * Math.Sin(rocket.Direction)).Normalize() * forceValue;
        }

        /// <summary>
        /// Преобразует делегат силы гравитации, в делегат силы, действующей на ракету
        /// </summary>
        public static RocketForce ConvertGravityToForce(Gravity gravity, Size spaceSize)
        {
            return rocket => gravity.Invoke(spaceSize,rocket.Location);
        }

        /// <summary>
        /// Суммирует все переданные силы, действующие на ракету, и возвращает суммарную силу.
        /// </summary>
        public static RocketForce Sum(params RocketForce[] forces)
        {
            return force =>
            {
                var sum = new Vector(0,0);
                for (var i = 0; i < forces.Length; i++)
                {
                    if (i == 0)
                        sum = forces[i].Invoke(force);
                    else
                        sum += forces[i].Invoke(force);
                }
                return sum;
            };
        }
    }
}
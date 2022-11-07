namespace func_rocket
{
    public class ControlTask
    {
        public static Turn ControlRocket(Rocket rocket, Vector target)
        {
            var a = 1.551;
            var b = 2.45;
            var c = 0.982;
            
            if ((a * rocket.Velocity.Angle + b * rocket.Direction) / (a + b) < c * (target - rocket.Location).Angle)
                return Turn.Right;
            return (a * rocket.Velocity.Angle + b * rocket.Direction) / (a + b) > c * (target - rocket.Location).Angle
                ? Turn.Left
                : Turn.None;
        }
    }
}
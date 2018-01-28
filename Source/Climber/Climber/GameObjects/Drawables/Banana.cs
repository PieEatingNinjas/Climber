namespace Climber
{
    public class Banana : FixedDrawableEntity, IFruit
    {
        public override string Char => "🍌";

        public Banana(int row, int maxTimeToLive) : base(row, maxTimeToLive) { }
    }
}

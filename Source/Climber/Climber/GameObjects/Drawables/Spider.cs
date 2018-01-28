namespace Climber
{
    public class Spider : FixedDrawableEntity, IEnemy
    {
        public Spider(int row, int maxTimeToLive) : base(row, maxTimeToLive) { }

        public override string Char => "🕷";
    }
}

namespace Climber
{
    public interface IPlayer : IDrawableEntity
    {
        void Climb(ClimbingDirection direction);
    }
}

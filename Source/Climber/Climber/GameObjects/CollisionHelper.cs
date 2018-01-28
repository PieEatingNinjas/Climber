using System.Diagnostics;
using Windows.Foundation;

namespace Climber
{
    public static class CollisionHelper
    {
        public static CollisionType HandleCollision(IEnemy enemy, IDrawableEntity item, Rect itemRect)
        {
            if (!(enemy is Spider && item is Snake))
            {
                return CollisionType.EnemyGotItem;
            }
            return CollisionType.None;
        }
        public static CollisionType HandlePlayerCollision(Player player, IDrawableEntity entity2, Rect collisionRect)
        {
            if (entity2 is IFruit)
            {
                if (collisionRect.Height >= 23)
                {
                    Debug.WriteLine("YUMMIE");
                    return CollisionType.PlayerGotFruit;
                }
            }
            else if (entity2 is IEnemy e)
            {
                if (collisionRect.Height >= 23 && collisionRect.Width >= 15)
                {
                    return CollisionType.EnemyGotPlayer;
                }
                else if (collisionRect.Height >= 10 && collisionRect.Width >= 10)
                {
                    Debug.WriteLine("Near miss...");
                    return CollisionType.NearMiss;
                }
            }
            return CollisionType.None;
        }
    }
}

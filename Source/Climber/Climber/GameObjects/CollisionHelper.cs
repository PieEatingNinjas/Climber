﻿using System.Diagnostics;
using Windows.Foundation;

namespace Climber
{
    public static class CollisionHelper
    {
        public static CollisionType HandleCollision(IEnemy enemy, IDrawableEntity item, Rect collisionRect)
        {
            if (!(enemy is Spider && item is Snake) && collisionRect.Width >= 10)
            {
                return CollisionType.EnemyGotItem;
            }
            return CollisionType.None;
        }

        public static CollisionType HandlePlayerCollision(IPlayer player, IDrawableEntity entity, Rect collisionRect)
        {
            if (entity is IFruit)
            {
                if (collisionRect.Height >= 23)
                {
                    Debug.WriteLine("YUMMIE");
                    return CollisionType.PlayerGotFruit;
                }
            }
            else if (entity is IEnemy e)
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

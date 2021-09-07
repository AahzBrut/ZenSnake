using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class Utils
    {
        public static bool IsFartherToAllThen(this List<Vector2> list, Vector2 target, float distance)
        {
            foreach (var position in list)
            {
                if (Vector2.Distance(position, target) < distance) return false;
            }

            return true;
        }

        public static Vector2 ToVector2(this Vector2Int source) => new Vector2(source.x, source.y);
    }
}
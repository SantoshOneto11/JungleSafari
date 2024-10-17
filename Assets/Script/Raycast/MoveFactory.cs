using UnityEngine;

namespace Jungle
{
    public static class MoveFactory 
    {
        public static IMove CreateMovement(GameObject obj)
        {
            if (Utility.isProd)
            {
                return obj.AddComponent<RandomMovement>();
            }
            else
            {
                return obj.AddComponent<Movement>();
            }
        }
    }
}

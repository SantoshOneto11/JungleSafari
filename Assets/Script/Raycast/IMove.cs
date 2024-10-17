using UnityEngine;

namespace Jungle
{
    public interface IMove
    {
        public void MovePlayer(Transform playerTransform, Animal animal);
    }
}

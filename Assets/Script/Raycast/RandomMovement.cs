using UnityEngine;

namespace Jungle
{
    public class RandomMovement : MonoBehaviour, IMove
    {
        public int moveSpeed = 2;
        private float reachThreashold = 0.1f;
        public void MovePlayer(Transform playerTransform, Animal animal)
        {
            Vector2 pos = Vector2.MoveTowards(playerTransform.position, animal.RandomVector, moveSpeed * Time.deltaTime);
            transform.position = pos;
            if (Vector2.Distance(transform.position, animal.RandomVector) < reachThreashold)
            {
                animal.HasReached = true;
                StartCoroutine(animal.FindRandomPos());
            }
        }
    }
}

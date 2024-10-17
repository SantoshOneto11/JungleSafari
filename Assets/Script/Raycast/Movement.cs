using UnityEngine;

namespace Jungle
{
    public class Movement : MonoBehaviour, IMove
    {
        public int moveSpeed = 2;

        public void MovePlayer(Transform playerTransform, Animal animal)
        {
            float xMove = Input.GetAxis("Horizontal");
            float yMove = Input.GetAxis("Vertical");

            Vector3 move = new Vector3(xMove, yMove, 0f);
            playerTransform.Translate(move * moveSpeed * Time.deltaTime);
        }
    }
}

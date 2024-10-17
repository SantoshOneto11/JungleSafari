using UnityEngine;

namespace Jungle
{
    public class GameController : MonoBehaviour
    {
        public Food food;
        public Rabbit rabbit;
        public Wolf wolf;

        public float nextFoodSpawnTime = 5f;
        public float foodSpawnVarience = 2f;

        private void Update()
        {
            if (nextFoodSpawnTime <= Time.time)
            {
                SpawnFood();
                nextFoodSpawnTime = Time.time;
                nextFoodSpawnTime += Random.Range(0f, foodSpawnVarience);
            }

            if (Input.GetMouseButtonDown(0))
            {
                SpawnRabbit(rabbit);
            }
        }

        void SpawnRabbit(Rabbit rabbit)
        {
            Vector2 rabbitPos = rabbit.GetRandomVector();
            Rabbit obj = Instantiate(rabbit, rabbitPos, Quaternion.identity);

            obj.OnMateEvent.AddListener(HandleMate);
        }
        void SpawnFood()
        {
            Vector2 foodPosition = rabbit.GetRandomVector();
            Food obj = Instantiate(food, foodPosition, Quaternion.identity);
        }

        void HandleMate(bool status)
        {
            if (status)
            {
                SpawnRabbit(rabbit);
            }
        }
    }
}

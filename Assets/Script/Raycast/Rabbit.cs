using System.Collections;
using UnityEngine;
namespace Jungle
{
    public class Rabbit : Animal
    {
        [Header("Child")]
        [SerializeField] float lifeCycle = 10f;
        [SerializeField] string foodTag = "Food";
        public float foodEnergy = 10f;

        private IMove move;
        private void Start()
        {
            Initialize(); // Call to set up ID and status
            lifeCycle += Time.time;
            RandomVector = GetRandomVector();
            StartCoroutine(FindRandomPos());
            move = MoveFactory.CreateMovement(gameObject);

            // Listeners to Events
            OnAnimalReachingFood.AddListener(ManageFood);
            OnMateEvent.AddListener(ManageCollider);
        }

        private void Update()
        {
            DrawRays(foodTag);

            if (Time.time > lifeCycle)
            {
                GetComponent<SpriteRenderer>().color = deadColor;
                return;     //If PplayerDead then No need to Move
            }

            PlayerDeadEffect(lifeCycle);

            if (move != null && !IsMateFound)
            {
                move.MovePlayer(transform, this);
            }
        }

        void ManageFood(GameObject obj)
        {
            float food = obj.GetComponent<Food>().foodEnergy;
            ConsumeFood(food);
            Destroy(obj);
        }

        void ConsumeFood(float food)
        {
            lifeCycle += food;
        }

        void ManageCollider(bool status)
        {
            GetComponent<BoxCollider2D>().enabled = status;
        }
    }
}

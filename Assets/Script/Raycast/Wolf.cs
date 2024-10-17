using System.Collections;
using UnityEngine;

namespace Jungle
{
    public class Wolf : Animal
    {
        [Header("Child")]
        public float lifeCycle = 10f;
        [SerializeField] string foodTag = "Rabbit";

        private IMove move;


        private void Start()
        {
            //ChooseRandomStatus();
            RandomVector = GetRandomVector();
            StartCoroutine(FindRandomPos());
            move = MoveFactory.CreateMovement(gameObject);

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
            float food = obj.GetComponent<Rabbit>().foodEnergy;
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

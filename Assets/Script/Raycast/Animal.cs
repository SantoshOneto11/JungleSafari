using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Jungle
{
    public class Animal : MonoBehaviour
    {
        [Header("Movement & Life")]
        [SerializeField] protected float moveSpeed = 10f;
        public float nextMatchInterval { get; private set; } = 5f;
        public bool HasReached { get; set; }
        public bool IsMateFound { get; protected set; } = false;
        public bool CanMove { get; set; } = true;

        public string Id { get; protected set; }
        public string lastMateId { get; protected set; }
        public enum Gender { Male, Female };
        public Gender AnimalGender { get; private set; }

        public Vector2 RandomVector { get; protected set; }

        [Header("Raycast")]
        [SerializeField] protected float noOfRays = 6f;
        [SerializeField] protected float rayDistance = 50f;
        [SerializeField] protected LayerMask hitLayer;

        [Header("Color")]
        [SerializeField] protected Color defaultColor = Color.white;
        [SerializeField] protected Color deadColor = Color.grey;
        [SerializeField] protected Color statusColor0 = Color.magenta;
        [SerializeField] protected Color statusColor1 = Color.blue;

        [Header("Events")]
        public UnityEvent<GameObject> OnAnimalReachingFood;
        public UnityEvent<bool> OnMateEvent;

        public UnityEvent OnPlayerDied;


        public void Initialize()
        {
            nextMatchInterval += Time.time;
            Id = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            AssignGender();
        }

        protected void AssignGender()
        {
            AnimalGender = (Gender)UnityEngine.Random.Range(0, 2);
            if (AnimalGender == Gender.Male)
            {
                GetComponent<SpriteRenderer>().color = statusColor0;
            }
            else if (AnimalGender == Gender.Female)
            {
                GetComponent<SpriteRenderer>().color = statusColor1;
            }
        }
        public void DrawRays(string tagName)
        {
            float angleStep = 360f / noOfRays;
            float detectionRadius = rayDistance; // Use rayDistance as the radius for the detection
            Vector3 origin = transform.position;

            // Detect all nearby objects using OverlapCircleAll
            Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(origin, detectionRadius, hitLayer);

            // Draw a circle to show the detection area
            int segments = 50; // Number of segments to draw the circle
            float angle = 0f;
            for (int i = 0; i < segments; i++)
            {
                float x1 = Mathf.Cos(Mathf.Deg2Rad * angle) * detectionRadius;
                float y1 = Mathf.Sin(Mathf.Deg2Rad * angle) * detectionRadius;

                float x2 = Mathf.Cos(Mathf.Deg2Rad * (angle + 360f / segments)) * detectionRadius;
                float y2 = Mathf.Sin(Mathf.Deg2Rad * (angle + 360f / segments)) * detectionRadius;

                Vector3 point1 = origin + new Vector3(x1, y1, 0f);
                Vector3 point2 = origin + new Vector3(x2, y2, 0f);

                Debug.DrawLine(point1, point2, Color.green);

                angle += 360f / segments;
            }

            DetectCollidedObjects(nearbyObjects, tagName);
        }


        public void DetectCollidedObjects(Collider2D[] nearbyObjects, string tagName)
        {
            // Process detected objects
            foreach (var nearbyObject in nearbyObjects)
            {
                if (nearbyObject.gameObject.tag == tagName)
                {
                    RandomVector = nearbyObject.transform.position;
                    if (Vector2.Distance(transform.position, RandomVector) < 0.1f)
                    {
                        OnAnimalReachingFood.Invoke(nearbyObject.gameObject);
                    }
                }

                //if (nearbyObject.gameObject != gameObject && nearbyObject.CompareTag(gameObject.tag)) // Ignore self and check tag
                //{
                //    if (nearbyObject.TryGetComponent<Animal>(out Animal nearAnimal))
                //    {
                //        if (nearAnimal.AnimalGender != AnimalGender && !IsMateFound)
                //        {
                //            if (lastMateId != nearAnimal.Id && nearAnimal.gameObject != null)
                //            {
                //                Debug.Log("Nearby object Clone: " + nearbyObject.name);
                //                if (!IsMateFound && nextMatchInterval > Time.time && nearAnimal.nextMatchInterval > Time.time)
                //                {
                //                    lastMateId = nearAnimal.Id;
                //                    StartCoroutine(FollowTime(nearbyObject.gameObject));
                //                    IsMateFound = true;
                //                }

                //            }
                //        }
                //    }


                //}

                if (nearbyObject.gameObject != gameObject && nearbyObject.CompareTag(gameObject.tag))
                {
                    if (nearbyObject.TryGetComponent<Animal>(out Animal nearAnimal) && IsEligibleForMatching(nearAnimal))
                    {
                        Debug.Log("Nearby object Clone: " + nearbyObject.name);
                        lastMateId = nearAnimal.Id;
                        StartCoroutine(FollowTime(nearbyObject.gameObject));
                        IsMateFound = true;
                    }
                }
            }
        }

        private bool IsEligibleForMatching(Animal nearAnimal)
        {
            return nearAnimal.AnimalGender != AnimalGender &&
                   lastMateId != nearAnimal.Id &&
                   nextMatchInterval < Time.time &&
                   nearAnimal.nextMatchInterval < Time.time &&
                   !IsMateFound;
        }

        public IEnumerator FollowTime(GameObject obj)
        {
            var currentAnimal = GetComponent<Animal>();
            var targetAnimal = obj.GetComponent<Animal>();

            if (currentAnimal != null) currentAnimal.CanMove = false;
            if (targetAnimal != null) targetAnimal.CanMove = false;

            ManageCollider(obj, false);

            float journeyLength = Vector2.Distance(transform.position, obj.transform.position);
            float startTime = Time.time;

            // Move towards the other object until close enough
            while (Vector2.Distance(transform.position, obj.transform.position) > 0.1f)
            {
                float distCovered = (Time.time - startTime) * moveSpeed;
                float fractionOfJourney = distCovered / journeyLength;

                transform.position = Vector2.Lerp(transform.position, obj.transform.position, fractionOfJourney);
                yield return null;
            }

            yield return new WaitForSeconds(2f);

            OnMateEvent.Invoke(IsMateFound);
            IsMateFound = false;

            if (currentAnimal != null) currentAnimal.CanMove = true;
            if (targetAnimal != null) targetAnimal.CanMove = true;

            StartCoroutine(FindRandomPos());
            ManageCollider(obj, true);
        }


        public void ManageCollider(GameObject obj, bool status)
        {
            obj.GetComponent<BoxCollider2D>().enabled = status;
            GetComponent<BoxCollider2D>().enabled = status;
            if (status)
            {
                obj.GetComponent<Animal>().nextMatchInterval = Time.time + 5f;
                gameObject.GetComponent<Animal>().nextMatchInterval = Time.time + 5f;
            }
        }


        public void PlayerDeadEffect(float lifeCycle)
        {
            if (Time.time > lifeCycle)
            {
                GetComponent<SpriteRenderer>().color = deadColor;
                OnPlayerDied.Invoke();
                return;     //If PplayerDead then No need to Move
            }

            lifeCycle -= Time.deltaTime;
            if (lifeCycle <= 5f)
            {
                float time = Mathf.PingPong(Time.time * 5f, 1f);
                GetComponent<SpriteRenderer>().color = Color.Lerp(defaultColor, deadColor, time);
            }
        }

        #region Random Position OnScreen
        public IEnumerator FindRandomPos()
        {
            RandomVector = GetRandomVector();
            yield return new WaitUntil(() => HasReached);
            HasReached = false;
        }

        public Vector2 GetRandomVector()
        {
            float padding = 0.2f;

            // Get camera bounds
            float cameraHeight = 2f * Camera.main.orthographicSize;
            float cameraWidth = cameraHeight * Camera.main.aspect;

            // Get camera's position in world space (important to center the random range)
            Vector3 cameraPosition = Camera.main.transform.position;

            // Define min and max X and Y values based on camera position and size
            float minX = cameraPosition.x - (cameraWidth / 2f) + padding;
            float maxX = cameraPosition.x + (cameraWidth / 2f) - padding;
            float minY = cameraPosition.y - (cameraHeight / 2f) + padding;
            float maxY = cameraPosition.y + (cameraHeight / 2f) - padding;

            // Generate a random position within the camera bounds
            float randomX = UnityEngine.Random.Range(minX, maxX);
            float randomY = UnityEngine.Random.Range(minY, maxY);

            return new Vector2(randomX, randomY);
        }

        #endregion
    }
}

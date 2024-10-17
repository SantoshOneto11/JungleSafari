using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float deltaX, aspect;
    private Rigidbody2D rb;
    [SerializeField] private float screenHeight, screenWidth;


    void Start()
    {
        // Get the Rigidbody2D component attached to the object
        rb = GetComponent<Rigidbody2D>();

        // Ensure gravity is disabled so it doesn't affect X-axis movement
        rb.gravityScale = 0;

        aspect = (float)Screen.height / Screen.width;
        screenWidth = -0.13f + Camera.main.orthographicSize / aspect;
    }

    void Update()
    {
        HandleTouchInput();

    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    deltaX = touchPos.x - transform.position.x;
                    break;

                case TouchPhase.Moved:
                    float targetX = touchPos.x - deltaX;

                    // Clamp the target position so the object doesn't go off-screen
                    float clampedPosX = Mathf.Clamp(targetX, -screenWidth, screenWidth);

                    // Move the object to the clamped position
                    rb.MovePosition(new Vector2(clampedPosX, transform.position.y));

                    // Log the touch position
                    Utility.myLog(clampedPosX.ToString());
                    break;

                case TouchPhase.Ended:
                    rb.velocity = Vector2.zero;
                    break;
            }
        }
    }

}

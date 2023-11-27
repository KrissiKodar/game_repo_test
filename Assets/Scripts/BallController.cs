using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Camera cam;
    public Rigidbody2D body;
    public Vector2 direction;
    public float impulse;
    public float maxReflectAngle;
    public float diameter;
    Vector2 startPosition;
    
    //public GameObject Ball;

    // Start is called before the first frame update
    void Start()
    {
        float currentDiameter = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float scale = diameter / currentDiameter;
        transform.localScale = new Vector3(scale, scale, 1);
        startPosition = transform.position;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        float radius = diameter / 2f;
        float ballAngle = Vector2.Angle(transform.position, body.velocity);
        float x = transform.position.x;
        float y = transform.position.y;
        float yLevelBound = cam.orthographicSize;
        float xLevelBound = yLevelBound * cam.aspect;
        //Debug.Log(xLevelBound);
        //Debug.Log(yLevelBound);
        
        if (ballAngle < 90 && (x < -xLevelBound + radius || x > xLevelBound - radius || 
                               y < -yLevelBound + radius || y > yLevelBound - radius))
        {
            Reset();
        } 
    }

    public void Reset()
    {
        transform.position = startPosition;
        body.velocity = direction.normalized * impulse;

        // Reset score
        GameManager.instance.score = 0;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D paddle = other.attachedRigidbody;
        if (paddle != null)
        {
            Vector2 paddleNormal = other.transform.up;

            // Don't bounce balls that enter from behind
            float ballAngle = Vector2.Angle(paddleNormal, body.velocity);

            if (ballAngle > 90)
            {
                // Reflect the ball's velocity about the paddle normal to get the bounce velocity
                Vector2 reflectedVelocity = Vector2.Reflect(body.velocity, paddleNormal);

                // Now we clamp the reflection angle to maxReflectAngle
                // We want the signed angle so we know which direction to rotate
                float reflectAngle = Vector2.SignedAngle(paddleNormal, reflectedVelocity);

                // Check if the bounce is too shallow
                if (Mathf.Abs(reflectAngle) > maxReflectAngle)
                {
                    // figure out how far past the maximum angle we are
                    float deltaAngle = (Mathf.Sign(reflectAngle) * maxReflectAngle) - reflectAngle;

                    // A quaternion represents a rotation, in this case about the Z axis
                    Quaternion clampRotation = Quaternion.Euler(0, 0, deltaAngle);

                    // Multiplying a vector by a quaternion gives you that vector rotated by the quaternion
                    reflectedVelocity = clampRotation * reflectedVelocity;
                }

                // Update the ball's velocity to bounce it away
                body.velocity = reflectedVelocity;

                // Score points!
                GameManager.instance.score++;

                //Instantiate(Ball);
            }
        }
    }
}

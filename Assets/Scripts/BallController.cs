using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    public Rigidbody2D body;
    public Vector2 direction;
    public float impulse;

    // Start is called before the first frame update
    void Start()
    {
        body.velocity = direction.normalized * impulse;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float maxReflectAngle;
        //public GameObject Ball;
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
                    //GameManager.instance.score++;

                    //Instantiate(Ball);

                }


            }
        }












}

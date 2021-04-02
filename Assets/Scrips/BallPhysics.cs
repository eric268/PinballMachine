using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField]
    private float m_fSpeed;

    [SerializeField]
    private Rigidbody m_rigidbody;

    [SerializeField]
    private float m_fCollisionMultiplyer;

    [SerializeField]
    private Vector3 m_vDirection;

    private bool m_bCollidingWithBottomBumper;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        if (!m_rigidbody)
        {
            Debug.LogError("Could not find ball rigid body");
        }
        m_bCollidingWithBottomBumper = false;
    }

    // Update is called once per frame
    void Update()
    {
    }



    private void OnDrawGizmos()
    {
    }

    void ChangeBallSpeed(Collision collision, float collisionMultiplier, bool addForce, float forceMultiplier)
    {
        
        m_fSpeed = m_rigidbody.velocity.magnitude;

        //This detects collision between walls and bounces ball off the wall
        m_vDirection = Vector3.Reflect(m_rigidbody.velocity.normalized, collision.contacts[0].normal);

        //Collision multiplier acts to simulate bumpers exerting force on ball on collision
        m_rigidbody.velocity = (m_vDirection * Mathf.Max(m_fSpeed, 0f)) * collisionMultiplier;

        //Clamps velocity because if it hits mutliple ob
        float x = Mathf.Clamp(m_rigidbody.velocity.x, -25.0f, 25.0f);
        float y = Mathf.Clamp(m_rigidbody.velocity.y, -25.0f, 25.0f);
        float z = Mathf.Clamp(m_rigidbody.velocity.z, -25.0f, 25.0f);
        m_rigidbody.velocity = new Vector3(x, y, z);
        
        //Adds additonal force
        if (addForce)
        {
            m_rigidbody.AddForce(collision.contacts[0].normal * forceMultiplier, ForceMode.Impulse);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        for (int i = 0; i < 3; i++)
        {
            //Want the ball to bounce but not lose any velocity
            if (collision.transform.name == "CaseExterior" + i)
            {
                ChangeBallSpeed(collision, 1, false, 0.0f);
                break;
            }
            //Want the ball to reflect with a small increase in velocity
            else if (collision.transform.name == "top_bumper" + i)
            {
                ChangeBallSpeed(collision, 1.3f, true, 1.0f);
                ScoreDisplay.m_fScore += 100f * ScoreDisplay.m_fMultiplier;
                break;
            }
            //Want the ball to reflect with a large increase in velocity
            else if (collision.transform.name == "BottomWedge" + i)
            {
                Vector3 normal = collision.contacts[0].normal;
                Vector3 LBumperNormal = new Vector3(0.8f, 0.4f, 0.4f);
                Vector3 RBumperNormal = new Vector3(-0.8f, 0.4f, 0.4f);
                Vector3 LBumperTopNormal = new Vector3(0.2f, 0.7f, 0.7f);
                Vector3 RBumperTopNormal = new Vector3(-0.5f, 0.6f, 0.6f);
                ScoreDisplay.m_fScore += 50f * ScoreDisplay.m_fMultiplier;

                //Essentailly checking if the collision normal is equal to the normal of the hypothenus side of the bumper
                //Dont want to check collision for reflection with sides or bottom of wedge
                //Since floating point percision makes it difficult to check equality between vectors I essentailly check if the distance between
                //the vectors is extremely small aka they are the same
                if (Vector3.SqrMagnitude(normal - LBumperNormal) < 0.01f || Vector3.SqrMagnitude(normal - RBumperNormal) < 0.01f || 
                   Vector3.SqrMagnitude(normal - LBumperTopNormal) < 0.01f || Vector3.SqrMagnitude(normal - RBumperTopNormal) < 0.01f)
               {
                    ChangeBallSpeed(collision, 3.0f, true, 1.0f);
               }

                break;
            }
            else if (collision.transform.name == "TopWedge" + i)
            {
                
                ChangeBallSpeed(collision, 1, false, 0.0f);
                break;
            }
        }
    }
}
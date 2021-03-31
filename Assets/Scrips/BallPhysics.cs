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

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        if (!m_rigidbody)
        {
            Debug.LogError("Could not find ball rigid body");
        }

    }

    // Update is called once per frame
    void Update()
    {
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawLine(new Vector3(-1.092f, 0.58f, -0.48f), new Vector3(-1.092f, 0.58f, -0.48f) + temp * 2);
    }

    void ChangeBallSpeed(Collision collision, float collisionMultiplier, bool addForce, float forceMultiplier)
    {
        //Maintain 85% velocity if collision with case exterior
        m_fSpeed = m_rigidbody.velocity.magnitude;

        //This detects collision between walls and bounces ball off the wall losing 50% of its total speed
        m_vDirection = Vector3.Reflect(m_rigidbody.velocity.normalized, collision.contacts[0].normal);

        m_rigidbody.velocity = (m_vDirection * Mathf.Max(m_fSpeed, 0f)) * collisionMultiplier;

        if (addForce)
        {
            m_rigidbody.AddForce(collision.contacts[0].normal * forceMultiplier, ForceMode.Impulse);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        for (int i = 0; i < 3; i++)
        {

            if (collision.transform.name == "CaseExterior" + i)
            {
                ChangeBallSpeed(collision, 1, false, 0.0f);
                break;
            }
            else if (collision.transform.name == "top_bumper" + i)
            {
                ChangeBallSpeed(collision, 1, true, 4.0f);
                break;
            }
            else if (collision.transform.name == "BottomWedge" + i)
            {
                Vector3 normal =  collision.contacts[0].normal;
                Vector3 LBumperNormal = new Vector3(0.8f, 0.4f, 0.4f);
                Vector3 RBumperNormal = new Vector3(-0.8f, 0.4f, 0.4f);

                //Essentailly checking if the collision normal is equal to the normal of the hypothenus side of the bumper
                //Since floating point percision makes it difficult to check equality between vectors I essentailly check if the distance between
                //the vectors is extremely small aka they are the same
                if (Vector3.SqrMagnitude(normal - LBumperNormal) < 0.01f || Vector3.SqrMagnitude(normal - RBumperNormal) < 0.01f)
                {
                    ChangeBallSpeed(collision, 1, true, 3.0f);
                }

                break;
            }
            else if (collision.transform.name == "TopWedge" + i)
            {
                
                ChangeBallSpeed(collision, 1, false, 0.0f);
                break;
            }
        }
        if (collision.transform.name == "MoonBumper")
        {
            ChangeBallSpeed(collision, 1, true, 2.0f);
        }
    }
}
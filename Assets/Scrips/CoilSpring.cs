using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoilSpring : MonoBehaviour
{
    [SerializeField]
    private float m_fSpringConstant;
    [SerializeField]
    private float m_fDampingConstant;
    [SerializeField]
    private Vector3 m_vRestPos;
    [SerializeField]
    private float m_fMass;
    [SerializeField]
    private Rigidbody m_attachedBody = null;

    [SerializeField]
    private float m_fPullSpeed = 0.003f;

    private Vector3 m_vForce;
    private Vector3 m_vPrevVel;
    private bool m_bSpaceDown = false;

    private bool m_bCollidingWithSpring = false;

    private int m_iPullCounter = 0;

    private void Start()
    {
        m_attachedBody = GameObject.Find("PlungerTop").GetComponent<Rigidbody>();
        if (!m_attachedBody)
        {
            Debug.LogError("Could not find attached rigid body");
        }
        m_fMass = m_attachedBody.mass;
        m_bCollidingWithSpring = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            m_attachedBody.isKinematic = true;
            m_bSpaceDown = true;
        }
        if (Input.GetKeyUp("space"))
        {
            m_bSpaceDown = false;
        }
        //Want to stop plunger movement if it hs reached the stopper
        if (m_bCollidingWithSpring)
        {
            m_attachedBody.isKinematic = true;
        }
    }

    private void FixedUpdate()
    {
        if (m_bSpaceDown)
        {
            m_iPullCounter++;
            //This ensure the spring cannot be pulled past the pinball case
            if (m_iPullCounter < 145.0f)
            {
                //Only moving on then z,y planes
                float z = m_attachedBody.position.z - m_fPullSpeed;
                float y = m_attachedBody.position.y - m_fPullSpeed;
                m_attachedBody.MovePosition(new Vector3(m_attachedBody.position.x, y, z));
            }
        }
        //Shoot the ball
        if (!m_bSpaceDown)
        {
            m_attachedBody.isKinematic = false;
            m_iPullCounter = 0;
        }
        UpdateSpringForce();
    }

    private void UpdateSpringForce()
    {

        //The first line is Hooke's law
        m_vForce = -m_fSpringConstant * (m_vRestPos - m_attachedBody.transform.position) -
            //This allows for dampening otherwise spring would extend and contract forever
            m_fDampingConstant * (m_attachedBody.velocity - m_vPrevVel);

        m_attachedBody.AddForce(m_vForce, ForceMode.Acceleration);

        m_vPrevVel = m_attachedBody.velocity;
    }

    private void OnDrawGizmos()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "PlungerTop")
        {
            m_bCollidingWithSpring = true;   
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "PlungerTop")
        {
            m_bCollidingWithSpring = false;
        }
    }
}

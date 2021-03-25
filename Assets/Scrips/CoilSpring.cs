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
    private bool m_bIsBungee = false;

    [SerializeField]
    private float m_fPullSpeed = 0.003f;

    private Vector3 m_vForce;
    private Vector3 m_vPrevVel;
    private bool spaceDown = false;

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
        // If you want to test the CoilSpring, or Bridge scenes...
        // ...remember to remove the comment for CALC_SPRING_COEFF
#if CALC_SPRING_COEFF
        m_fSpringConstant = CalculateSpringConstant();
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            m_attachedBody.isKinematic = true;
            spaceDown = true;
        }

        if (m_bCollidingWithSpring)
        {
            m_attachedBody.isKinematic = true;
        }
    }

    private void FixedUpdate()
    {
        if (spaceDown)
        {
            float z = m_attachedBody.position.z -  m_fPullSpeed;
            float y = m_attachedBody.position.y - m_fPullSpeed;
            float xRot = transform.rotation.x;

            m_attachedBody.MovePosition(new Vector3(m_attachedBody.position.x, y, z));

            m_iPullCounter++;
            if (m_iPullCounter  >= 145)
            {
                m_attachedBody.isKinematic = false;
                spaceDown = false;
                m_iPullCounter = 0;
            }
        }

  
        UpdateSpringForce();
    }

    private float CalculateSpringConstant()
    {
        // k = F / dX
        // F = m * a
        // k = m * a / (xf - xi)

        float fDX = (m_vRestPos - m_attachedBody.transform.position).magnitude;

        if (fDX <= 0f)
        {
            return Mathf.Epsilon;
        }

        return (m_fMass * Physics.gravity.y) / (fDX);
    }

    private void UpdateSpringForce()
    {
        // F = -kx
        // F = -kx -bv

        m_vForce = -m_fSpringConstant * (m_vRestPos - m_attachedBody.transform.position) -
            m_fDampingConstant * (m_attachedBody.velocity - m_vPrevVel);

        m_attachedBody.AddForce(m_vForce, ForceMode.Acceleration);

        m_vPrevVel = m_attachedBody.velocity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(m_vRestPos, 1f);

        if (m_attachedBody)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_attachedBody.transform.position, 1f);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, m_attachedBody.transform.position);
        }
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

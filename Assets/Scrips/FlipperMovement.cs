using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_rigidbody;

    [SerializeField]
    private bool m_bAPressed;

    [SerializeField]
    private bool m_bDPressed;

    [SerializeField]
    private Quaternion m_leftInitialRotation;

    [SerializeField]
    private Vector3 m_vRotationAngle;



    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        if (!m_rigidbody)
        {
            Debug.LogError("Could not find rigid body");
        }
        m_bAPressed = false;
        m_bDPressed = false;

        m_leftInitialRotation = transform.rotation;

        m_vRotationAngle = transform.InverseTransformDirection(-Vector3.up);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            m_bAPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            m_bDPressed = true;
        }    
        if (Input.GetKeyUp(KeyCode.A))
        {
            m_bAPressed = false;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            m_bDPressed = false;
        }

        if (name == "Left_Flipper" && m_bAPressed)
        {
            Debug.LogError(m_leftInitialRotation);
            //m_rigidbody.MoveRotation(m_leftInitialRotation  * Quaternion.AngleAxis(10, m_vRotationAngle));

            transform.rotation *= Quaternion.AngleAxis(10, m_vRotationAngle);

        }
        else if (name == "Left_Flipper" && !m_bAPressed)
        {
            m_rigidbody.MoveRotation(m_leftInitialRotation);
        }
    }
}

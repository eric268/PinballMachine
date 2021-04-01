using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlocker : MonoBehaviour
{
    GameObject m_blocker;
    GameObject m_ball;

    [SerializeField]
    private bool m_bLifeLost;

    [SerializeField]
    private int m_iDeathTimer;

    [SerializeField]
    private bool m_bBeginRightShooter;

    [SerializeField]
    private int m_iRSDelayCounter;

    [SerializeField]
    private Vector3 m_vForceNormal;

    private GameObject m_RightBarrier;
    // Start is called before the first frame update
    void Start()
    {
        m_blocker = GameObject.Find("Blocker");
        if (!m_blocker)
        {
            Debug.LogError("Cannot find top blocker");
        }

        m_ball = GameObject.Find("Sphere");
        if (!m_ball)
        {
            Debug.LogError("Cannot find sphere");
        }
        m_RightBarrier = GameObject.Find("RightBlocker");

        m_bLifeLost = false;
        m_iDeathTimer = 0;
        m_bBeginRightShooter = false;
        m_iRSDelayCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (m_bLifeLost)
        {
            m_iDeathTimer++;
            if ( m_iDeathTimer > 120)
            {
                m_iDeathTimer = 0;
                m_bLifeLost = false;
                m_ball.GetComponent<Rigidbody>().transform.position = new Vector3(3.35f, -1.415f, -2.233f);
                GameVariables.m_iLivesRemaining--;
            }
        }
        if (m_bBeginRightShooter)
        {
            m_iRSDelayCounter++;
            if (m_iRSDelayCounter == 120)
            {
                m_ball.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 5f, 5f), ForceMode.Impulse);
            }
            if (m_iRSDelayCounter == 180)
            {
                m_iRSDelayCounter = 0;
                m_bBeginRightShooter = false;
                
                m_RightBarrier.transform.position = new Vector3(2.62f, -0.76f, -2.127f);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Sphere" && transform.name == "TopCollider")
        {
            m_blocker.transform.position = new Vector3(1.554f, 4.52f, 3.162f);
        }
        else if (other.transform.name == "Sphere" && transform.name == "BottomCollider")
        {
            m_blocker.transform.position = new Vector3(-100.0f, -100.0f, -100.0f);
            m_RightBarrier.transform.position = new Vector3(-100.0f, -100.0f, -100.0f);
            m_bLifeLost = true;
        }
        else if (other.transform.name == "Sphere" && transform.name == "RSTrigger")
        {
            m_bBeginRightShooter = true;
        }
    }
}

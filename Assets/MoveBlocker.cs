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

        m_bLifeLost = false;
        m_iDeathTimer = 0;
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
            m_bLifeLost = true;
        }
    }
}

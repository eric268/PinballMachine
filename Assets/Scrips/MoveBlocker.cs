using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlocker : MonoBehaviour
{
    //These are barreirs as seen at top and bottom of right side after ball passes

    static public GameObject m_blocker;
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

    static public GameObject m_RightBarrier;
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
        //Means ball has reaches bottom of case
        if (m_bLifeLost)
        {
            m_iDeathTimer++;
            if ( m_iDeathTimer > 120)
            {
                m_iDeathTimer = 0;
                m_bLifeLost = false;
                ScoreDisplay.m_iLives--;
                
                if (ScoreDisplay.m_iLives <= 0)
                {
                    //Want to freeze the ball out of sight until new game started
                    m_ball.GetComponent<Rigidbody>().MovePosition(new Vector3(-100.0f, -100.0f, -100.0f));
                    m_ball.GetComponent<Rigidbody>().isKinematic = true;

                }
                else
                {
                    //Moves ball to starting position because player has another life
                    m_ball.GetComponent<Rigidbody>().MovePosition(new Vector3(3.35f, -1.415f, -2.233f));
                }
            }
        }
        //This is the small tunnel right of the flipper that saves and shoots the ball once per life
        if (m_bBeginRightShooter)
        {
            m_iRSDelayCounter++;
            if (m_iRSDelayCounter == 120)
            {
                m_ball.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 5f, 5f), ForceMode.Impulse);
            }
            //Delayed further so ball has time to pass collider
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
        //This activates the top blocker stopping the ball from reetnering beginning funnel
        if (other.transform.name == "Sphere" && transform.name == "TopCollider")
        {
            m_blocker.transform.position = new Vector3(1.554f, 4.52f, 3.162f);
        }
        //This checks if the ball has fallen below the flippers
        else if (other.transform.name == "Sphere" && transform.name == "BottomCollider")
        {
            MoveBlockersToStartingPositon();
            m_bLifeLost = true;
        }
        //Stops the ball from reentering the right shooter 
        else if (other.transform.name == "Sphere" && transform.name == "RSTrigger")
        {
            m_bBeginRightShooter = true;
        }
    }
    static public void MoveBlockersToStartingPositon()
    {
        //I am using the transform because these do not have rigid bodies as they do not need them
        //Only used to stop ball entering off limits areas
        m_blocker.transform.position = new Vector3(-100.0f, -100.0f, -100.0f);
        m_RightBarrier.transform.position = new Vector3(-100.0f, -100.0f, -100.0f);
    }
}

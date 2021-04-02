using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierCollisions : MonoBehaviour
{
    //Information regarding the yellow cards in the semi circle that act as score multiplier if all hit
    [SerializeField]
    private GameObject [] m_boxMultipliers = new GameObject[3];
    private Vector3[] m_vboxMultiPosArray = new Vector3[3];

    //Place to send cards when they have been hit and waiting to return
    private Vector3 m_vInActivePosition = new Vector3(-100f, -100f, -100f);

    private int m_iBoxMultiplierCounter;

    private Rigidbody m_rigidbody;

    private int m_iBoxResetCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_iBoxMultiplierCounter = 0;
        
        m_rigidbody = GetComponent<Rigidbody>();
        if (!m_rigidbody)
        {
            Debug.LogError("Could not find sphere rigid body");
        }
        //Initalize the array value with the yellow cards (Targets)
        for (int i =0; i< 3; i++)
        {
            m_boxMultipliers[i] = GameObject.Find("Target" + i);
            m_vboxMultiPosArray[i] = m_boxMultipliers[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
       //If all cards have been hit begin
        if (m_iBoxMultiplierCounter >= 3)
        {
            m_iBoxResetCounter++;

            //wait a while so no double collision with ball
            if (m_iBoxResetCounter > 120)
            {
               //Reset all varibales and increment multiplier
                ResetBoxMultiplierPositions();
                m_iBoxResetCounter = 0; 
                ScoreDisplay.m_fMultiplier += 0.5f;
                m_iBoxMultiplierCounter = 0;
                for (int i = 0; i < m_boxMultipliers.Length; i++)
                {
                    m_boxMultipliers[i].GetComponent<MultiplierVariables>().m_bisHit = false;
                }
            }
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < 3; i++)
        {
            //Checks collision with all 3 targets, reflects ball and increments counter if true
            if (collision.transform.name == "Target" + i)
            {
                if (!collision.transform.GetComponent<MultiplierVariables>().m_bisHit)
                {
                    collision.transform.GetComponent<MultiplierVariables>().m_bisHit = true;
                    float speed = m_rigidbody.velocity.magnitude;

                    //This detects collision between walls and bounces ball off the wall
                    Vector3 dir = Vector3.Reflect(m_rigidbody.velocity.normalized, collision.contacts[0].normal);

                    //Collision multiplier acts to simulate bumpers exerting force on ball on collision
                    m_rigidbody.velocity = (dir * Mathf.Max(speed, 0f));


                    collision.transform.position = m_vInActivePosition;
                    m_iBoxMultiplierCounter++;
                }
            }
            
        }
    }
    //Resets the yellow cards to starting pos
    public void ResetBoxMultiplierPositions()
    {
        for (int i = 0; i < 3; i++)
        {
            m_boxMultipliers[i].transform.position  = m_vboxMultiPosArray[i];
        }
    }
}

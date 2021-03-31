using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVariables : MonoBehaviour
{
    static public int m_iLivesRemaining;
    static public int m_iScore;
    // Start is called before the first frame update
    void Start()
    {
        m_iLivesRemaining = 3;
        m_iScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

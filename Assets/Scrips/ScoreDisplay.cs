using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{


    public TextMeshProUGUI m_scoreText;
    public TextMeshProUGUI m_livesText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_scoreText.text = "Score: " + GameVariables.m_iScore;
        m_livesText.text = "Lives: " + GameVariables.m_iLivesRemaining;
    }
}

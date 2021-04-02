using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    static public float m_fScore = 0;
    static public int m_iLives = 3;
    static public float m_fMultiplier = 1f;
    static public bool m_bRestartGame;

    public TextMeshProUGUI m_scoreText;
    public TextMeshProUGUI m_livesText;
    public TextMeshProUGUI m_multiplierText;
    public TextMeshProUGUI m_gameOverText;
    public TextMeshProUGUI m_controlsText;
    private GameObject m_ball;

    private void Start()
    {
        m_ball = GameObject.Find("Sphere");
        if (!m_ball)
        {
            Debug.Log("Unable to find ball in UI elements");
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        m_scoreText.text = "Score: " + m_fScore;
        m_livesText.text = "Lives: " + m_iLives;
        m_multiplierText.text = "Multiplier: " + m_fMultiplier + "x";

        if (m_iLives == 0)
        {
            m_gameOverText.text = "Game Over\n Insert Coin";
        }
        else
        {
            m_gameOverText.text = "";
        }
    }

    public void RestartGame()
    {
        MoveBlocker.MoveBlockersToStartingPositon();
        m_fScore = 0;
        m_iLives = 3;
        m_fMultiplier = 1.0f;
        m_ball.GetComponent<Rigidbody>().position = new Vector3(3.381f, -1.276f, -2.298f);
        m_ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        m_ball.GetComponent<Rigidbody>().isKinematic = false;
        m_ball.GetComponent<MultiplierCollisions>().ResetBoxMultiplierPositions();
    }
    
}

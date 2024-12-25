using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;         // Displays the current score
    public Text HighScoreText;    // Reference to ScoreText(1)
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;
    private string bestPlayerName = "";
    private int bestScore = 0;

    void Start()
    {
        // Load the highest score and player name from PlayerPrefs
        if (PlayerPrefs.HasKey("HighScore"))
        {
            bestScore = PlayerPrefs.GetInt("HighScore");
            bestPlayerName = PlayerPrefs.GetString("HighScorePlayer");
        }
        string playerName = PlayerData.Instance.GetPlayerName();
        ScoreText.text = $"Score: {playerName} : {m_Points}";

        UpdateHighScoreText();

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        string playerName = PlayerData.Instance.GetPlayerName();
        m_Points += point;
        ScoreText.text = $"Score {playerName}: {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        // Check if the current score is the highest score
        if (m_Points > bestScore)
        {
            bestScore = m_Points;
            bestPlayerName = PlayerData.Instance.GetPlayerName();

            // Save the new high score and player name
            PlayerPrefs.SetInt("HighScore", bestScore);
            PlayerPrefs.SetString("HighScorePlayer", bestPlayerName);
            PlayerPrefs.Save();
        }

        UpdateHighScoreText();
    }

    private void UpdateHighScoreText()
    {
        HighScoreText.text = $"Best Score: {bestPlayerName} : {bestScore}";
    }
}

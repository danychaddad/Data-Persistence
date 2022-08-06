using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text HighScore;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {
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
        HighScoreText();
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
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        CheckHighScore();
    }

    public void HighScoreText()
    {
        if (File.Exists(Application.persistentDataPath + "/highscore.json"))
        {
            string text = File.ReadAllText(Application.persistentDataPath + "/highscore.json");
            PlayerData player = JsonUtility.FromJson<PlayerData>(text);
            HighScore.text = "Best Score : " + player.name + " : " + player.highscore;
        }
        else
        {
            HighScore.text = "No high score yet!";
        }
    }
    public void CheckHighScore()
    {
        if (File.Exists(Application.persistentDataPath + "/highscore.json"))
        {
            string text = File.ReadAllText(Application.persistentDataPath + "/highscore.json");
            PlayerData player = JsonUtility.FromJson<PlayerData>(text);
            if (m_Points > player.highscore)
            {
                player.highscore = m_Points;
                player.name = UIManager.instance.playerName;
                string json = JsonUtility.ToJson(player);
                File.WriteAllText(Application.persistentDataPath + "/highscore.json", json);
                HighScoreText();
            }
        }
        else
        {
            PlayerData newPlayer = new PlayerData();
            newPlayer.name = UIManager.instance.playerName;
            newPlayer.highscore = m_Points;
            string json = JsonUtility.ToJson(newPlayer);
            File.WriteAllText(Application.persistentDataPath + "/highscore.json", json);
        }
    }
    [System.Serializable]
    class PlayerData
    {
        public string name;
        public int highscore;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI InputField;
    public string playerName;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetName()
    {
        playerName = InputField.text;
    }

    public void StartGame() {
        SceneManager.LoadScene(1);
    }

    [System.Serializable]
    class SaveData
    {
        public int highscore;
    }
}

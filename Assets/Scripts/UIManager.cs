using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject MainPanel;
    // To be implement
    //public GameObject gameOverPanel; 
    //public Text score;
    //public Text highScore1;
    //public Text highScore2;

    // Start is called before the first frame update
    void Start()
    {
        // highScore1.text = "High Score: " + PlayerPrefs.GetInt("highScore");
        // Get the score / high score this way
        
    }

    private void Awake()
    {
        
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    public void GameStart()
    {
        // Load levels scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        // To be implementer using GameManager script - future.
        //score.text = PlayerPrefs.GetInt("score").ToString();
        //highScore2.text = PlayerPrefs.GetInt("highScore").ToString();
        //gameOverPanel.SetActive(true);

    }

    public void Reset() // To start game again on game over (in future)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

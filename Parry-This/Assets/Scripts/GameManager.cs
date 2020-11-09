using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameInstance;
    private int currentSceneIndex;
    public delegate void GameFlowEvent();
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (gameInstance == null)
            gameInstance = this;
        else
            Destroy(this.gameObject);
    }
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    void Update()
    {
        
    }
}

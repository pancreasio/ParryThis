using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static event GameManager.GameFlowEvent OnMenuButtonPressed;
    public static event GameManager.GameFlowEvent OnStartButtonPressed;

    public void GoToMenu()
    {
        if(OnMenuButtonPressed!=null)
            OnMenuButtonPressed.Invoke();
        GameManager.gameInstance.GoToMenu();
    }

    public void ExitGame()
    {
        Application.Quit();
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        if(OnStartButtonPressed!=null)
            OnStartButtonPressed.Invoke();
        GameManager.gameInstance.StartGame();
    }
}

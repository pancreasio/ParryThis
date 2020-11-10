using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    public static event GameManager.GameFlowEvent OnMenuButtonPressed;
    public static event GameManager.GameFlowEvent OnStartButtonPressed;
    public GameObject menuUI;
    public GameObject logUI;
    public Text logText;


    public void GoToMenu()
    {
        if(OnMenuButtonPressed!=null)
            OnMenuButtonPressed.Invoke();
        GameManager.gameInstance.GoToMenu();
    }

    public void GoToLogs()
    {
        menuUI.SetActive(false);
        logUI.SetActive(true);
        logText.text = PluginTest.loggerInstance.ShowLogs();
    }

    public void BackToMenu()
    {
        menuUI.SetActive(true);
        logUI.SetActive(false);
    }

    public void DeleteLogs()
    {
        PluginTest.loggerInstance.Noop();
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        if(OnStartButtonPressed!=null)
            OnStartButtonPressed.Invoke();
    }
}

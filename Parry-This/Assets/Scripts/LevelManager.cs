using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    
    public delegate void GameplayEvent();
    public delegate void GameplayDamageEvent(int damage);

    public static LevelManager LevelInstance;
    public PlayerController playerCharacter;
    public List<Combat> encounterList;
    private Combat nextEncounter = null;

    [System.Serializable]
    public struct LevelData
    {
        public int currentLevelIndex;
        public int nextLevelIndex;
        public GameplayEvent OnLevelCompleted;
        public GameplayEvent OnLevelFailed;
    }

    public LevelData currentLevelData;

    public GameObject pauseUI;
    public GameObject gameUI;

    private void Awake()
    {
        LevelInstance = this;    
    }

    private void OnDestroy() 
    {
        LevelInstance = null;    
    }

    void Start()
    {
        playerCharacter.OnAttack += PlayerAttacked;
        playerCharacter.OnDeath += LevelFailed;
        InputManager.OnAttackStart += playerCharacter.Attack;
        InputManager.OnDefendStart += playerCharacter.Defend;
        InputManager.OnDefendEnd += playerCharacter.EndDefend;
        pauseUI.SetActive(false);
        ProcessEncounter();
    }

    private void ProcessEncounter()
    {
        FindNextEncounter();
        GoToEncounter();
    }

    private void FindNextEncounter()
    {
        foreach(Combat combat in encounterList)
        {
            if(!combat.completed)
            {
                nextEncounter = combat;
                break;
            }
        }
    }

    private void BeginNextEncounter()
    {
        playerCharacter.OnDestinationReached -= BeginNextEncounter;
        nextEncounter.OnCombatEnded += ProcessEncounter;
        nextEncounter.enemyCharacter.OnAttack += EnemyAttacked;
        nextEncounter.BeginCombat();
    }
    private void GoToEncounter()
    {
        playerCharacter.ProceedToNextCombat(nextEncounter.playerExpectedPosition.transform.position.x);
        playerCharacter.OnDestinationReached += BeginNextEncounter;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayerAttacked(int damage)
    {
        nextEncounter.enemyCharacter.RecieveDamage(damage);
    }

    void EnemyAttacked(int damage)
    {
        playerCharacter.RecieveDamage(damage);
    }

    public static void InvokeIfNotNull(GameplayEvent eventToInvoke)
    {
        if(eventToInvoke !=null)
        eventToInvoke.Invoke();
    }

    private void LevelFailed()
    {
        nextEncounter.LostCombat();
        InvokeIfNotNull(currentLevelData.OnLevelFailed);
    }

    private void LevelCompleted()
    {
        InvokeIfNotNull(currentLevelData.OnLevelCompleted);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
        gameUI.SetActive(false);
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        gameUI.SetActive(true);
    }
}

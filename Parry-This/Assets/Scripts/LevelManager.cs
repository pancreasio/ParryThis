using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    
    public delegate void GameplayEvent();

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

    void PlayerAttacked()
    {

    }

    public static void InvokeIfNotNull(GameplayEvent eventToInvoke)
    {
        if(eventToInvoke !=null)
        eventToInvoke.Invoke();
    }

    private void LevelFailed()
    {
        InvokeIfNotNull(currentLevelData.OnLevelFailed);
    }

    private void LevelCompleted()
    {
        InvokeIfNotNull(currentLevelData.OnLevelCompleted);
    }
}

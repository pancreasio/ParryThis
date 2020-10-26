using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    
    public delegate void GameplayEvent();

    public Character playerCharacter;
    public Character enemyCharacter;

    // Start is called before the first frame update
    void Start()
    {
        playerCharacter.OnAttack += PlayerAttacked;
        InputManager.OnAttackStart += playerCharacter.Attack;
        InputManager.OnDefendStart += playerCharacter.Defend;
        InputManager.OnDefendEnd += playerCharacter.EndDefend;
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
}

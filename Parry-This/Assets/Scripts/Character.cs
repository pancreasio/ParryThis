using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum CharacterStates
    {
        Idle,
        Windup,
        Followthrough,
        Blocking,
        Parry,
        Armored,
        Attacking,
        Damaged,
        Invulnerable,
    }

    public LevelManager.GameplayEvent OnAttack;
    private delegate void ChangeStateFunction();

    private CharacterStates currentState;

    public float attackWindup;
    public float attackFollowthrough;
    public float blockWindup;
    public float blockFollowthrough;
    public float parryTime;

    private void Start()
    {
        currentState = CharacterStates.Idle;
    }

    private void Update()
    {
        
    }

    public void Attack()
    {
        if (currentState == CharacterStates.Idle)
        {
            BeginAttack();
        }
    }

    private void BeginAttack()
    {
        currentState = CharacterStates.Windup;
        StartCoroutine(ChangeState(CharacterStates.Attacking, attackWindup, PerformAttack()));
    }

    ChangeStateFunction PerformAttack()
    {
        OnAttack.Invoke();
        currentState = CharacterStates.Windup;
        StartCoroutine(ChangeState(CharacterStates.Idle, attackFollowthrough));
        return null;
    }

    private IEnumerator ChangeState(CharacterStates targetState,float targetTime, ChangeStateFunction stateFunction )
    {
        float time = 0f;

        while (time <= targetTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        currentState = targetState;

        if(stateFunction != null)
            stateFunction.Invoke();
    }

    private IEnumerator ChangeState(CharacterStates targetState, float targetTime)
    {
        float time = 0f;

        while (time <= targetTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        currentState = targetState;
    }
}

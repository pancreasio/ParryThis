using System;
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

    private Animator characterAnimator;

    private void Start()
    {
        currentState = CharacterStates.Idle;
        characterAnimator = GetComponentInChildren<Animator>();
        characterAnimator.SetTrigger("IDLE");
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

    public void Defend()
    {
        
    }

    private void BeginAttack()
    {
        currentState = CharacterStates.Windup;
        characterAnimator.SetTrigger("ATTACK");
        StartCoroutine(ChangeState(CharacterStates.Attacking, attackWindup, PerformAttack));
    }

    void PerformAttack()
    {
        OnAttack.Invoke();
        currentState = CharacterStates.Windup;
        StartCoroutine(ChangeState(CharacterStates.Idle, attackFollowthrough, ReturnToIdle));
    }

    void ReturnToIdle()
    {
        currentState = CharacterStates.Idle;
        characterAnimator.SetTrigger("IDLE");
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

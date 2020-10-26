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

    private bool returningToIdle;
    private ChangeStateFunction QueuedAction;
    private Animator characterAnimator;

    private void Start()
    {
        QueuedAction = null;
        returningToIdle = false;
        currentState = CharacterStates.Idle;
        characterAnimator = GetComponentInChildren<Animator>();
        characterAnimator.SetTrigger("IDLE");
    }

    private void Update()
    {
        Debug.Log(currentState);
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
        if (currentState == CharacterStates.Idle)
        {
            BeginBlock();
        }
    }

    public void EndDefend()
    {
        if (currentState == CharacterStates.Parry || currentState == CharacterStates.Blocking)
        {
            EndBlock();
        }
    }
    private void BeginBlock()
    {
        currentState = CharacterStates.Parry;
        characterAnimator.SetTrigger("BLOCK");
        StartCoroutine(ChangeState(CharacterStates.Blocking, parryTime));
    }

    private void EndBlock()
    {
        characterAnimator.SetTrigger("IDLE");
        StartCoroutine(ChangeState(CharacterStates.Idle, blockFollowthrough, ReturnToIdle));
    }

    private void BeginAttack()
    {
        currentState = CharacterStates.Windup;
        characterAnimator.SetTrigger("ATTACK");
        StartCoroutine(ChangeState(CharacterStates.Attacking, attackWindup, PerformAttack));
    }

    void PerformAttack()
    {
        if(OnAttack != null)
            OnAttack.Invoke();
        characterAnimator.SetTrigger("IDLE");
        StartCoroutine(ChangeState(CharacterStates.Idle, attackFollowthrough, ReturnToIdle));
    }

    void ReturnToIdle()
    {
        currentState = CharacterStates.Idle;
    }
    private IEnumerator ChangeState(CharacterStates targetState, float targetTime, ChangeStateFunction stateFunction)
    {
        float time = 0f;

        while (time <= targetTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        currentState = targetState;

        if (stateFunction != null)
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

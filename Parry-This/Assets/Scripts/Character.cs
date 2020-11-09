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
    public LevelManager.GameplayEvent OnDeath;
    private int hitpoints;
    public int maxHitpoints;
    protected delegate void ChangeStateFunction();

    private CharacterStates currentState;

    public float attackWindup;
    public float attackFollowthrough;
    public float blockWindup;
    public float blockFollowthrough;
    public float parryTime;

    public float deathTime = 1f;

    private bool returningToIdle;
    private bool armored;
    private ChangeStateFunction QueuedAction;
    protected Animator characterAnimator;

    protected void Start()
    {
        QueuedAction = null;
        returningToIdle = false;
        armored = false;
        hitpoints = maxHitpoints;
        currentState = CharacterStates.Idle;
        characterAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {

    }

    public virtual void BeginCombat()
    {

    }

    public void RecieveDamage(int incomingDamage)
    {
        if(currentState != CharacterStates.Blocking && currentState != CharacterStates.Parry && !armored)
        {
            hitpoints -= incomingDamage;
            if(hitpoints<= 0)
            {
                StartCoroutine(Die());
            }
        }
    }

    protected IEnumerator Die()
    {
        characterAnimator.SetTrigger("DIE");
        float timer = 0f;
        while(timer<deathTime)
        {
            timer+= Time.deltaTime;
            yield return null;
        }
        LevelManager.InvokeIfNotNull(OnDeath);
    }

    public void Attack()
    {
        if (currentState == CharacterStates.Idle && hitpoints > 0)
        {
            BeginAttack();
        }
    }

    public void Defend()
    {
        if (currentState == CharacterStates.Idle && hitpoints > 0)
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
    protected void BeginBlock()
    {
        currentState = CharacterStates.Parry;
        characterAnimator.SetTrigger("BLOCK");
        StartCoroutine(ChangeState(CharacterStates.Blocking, parryTime));
    }

    protected void EndBlock()
    {
        characterAnimator.SetTrigger("IDLE");
        StartCoroutine(ChangeState(CharacterStates.Idle, blockFollowthrough, ReturnToIdle));
    }

    protected void BeginAttack()
    {
        currentState = CharacterStates.Windup;
        characterAnimator.SetTrigger("ATTACK");
        StartCoroutine(ChangeState(CharacterStates.Attacking, attackWindup, PerformAttack));
    }

    protected void PerformAttack()
    {
        if(OnAttack != null)
            OnAttack.Invoke();
        characterAnimator.SetTrigger("IDLE");
        StartCoroutine(ChangeState(CharacterStates.Idle, attackFollowthrough, ReturnToIdle));
    }

    protected void ReturnToIdle()
    {
        armored = false;
        currentState = CharacterStates.Idle;
    }
    protected private IEnumerator ChangeState(CharacterStates targetState, float targetTime, ChangeStateFunction stateFunction)
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

    protected private IEnumerator ChangeState(CharacterStates targetState, float targetTime)
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

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
        Walking,
    }

    public LevelManager.GameplayDamageEvent OnAttack;
    public LevelManager.GameplayEvent OnDeath;
    private int hitpoints;
    public int maxHitpoints;

    public int attackDamage;
    protected delegate void ChangeStateFunction();

    protected CharacterStates currentState;

    public float attackWindup;
    public float attackFollowthrough;
    public float blockWindup;
    public float blockFollowthrough;
    public float parryTime;

    public float deathTime = 1f;
    public float damagedTime = 1f;

    private bool returningToIdle;
    private bool armored;
    private bool interrupted;
    private ChangeStateFunction QueuedAction;
    protected Animator characterAnimator;

    protected virtual void Start()
    {
        QueuedAction = null;
        returningToIdle = false;
        armored = false;
        interrupted = false;
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

    public virtual void EndCombat()
    {

    }

    public virtual void RecieveDamage(int incomingDamage)
    {
        if (!armored && (currentState == CharacterStates.Attacking || currentState == CharacterStates.Windup || currentState == CharacterStates.Idle))
        {
            if (currentState != CharacterStates.Idle)
                interrupted = true;
            hitpoints -= incomingDamage;
            if (hitpoints <= 0)
            {
                currentState = CharacterStates.Damaged;
                interrupted = false;
                StartCoroutine(Die());
            }
            else
            {
                characterAnimator.SetTrigger("DAMAGED");
                interrupted = false;
                Damaged();
            }
        }
    }

    protected IEnumerator Die()
    {
        characterAnimator.SetTrigger("DIE");
        float timer = 0f;
        while (timer < deathTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        LevelManager.InvokeIfNotNull(OnDeath);
    }

    protected void Damaged()
    {
        //characterAnimator.SetTrigger("IDLE");
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
    public void BeginBlock()
    {
        currentState = CharacterStates.Parry;
        characterAnimator.ResetTrigger("IDLE");
        characterAnimator.SetTrigger("BLOCK");
    }

    public void BeginWalk()
    {
        currentState = CharacterStates.Walking;
        characterAnimator.ResetTrigger("IDLE");
        characterAnimator.SetTrigger("WALK");
    }

    public void EndBlock()
    {
        characterAnimator.SetTrigger("IDLE");
    }

    public void ResetBlock()
    {
        currentState = CharacterStates.Idle;
        characterAnimator.ResetTrigger("BLOCK");
    }

    public void EndParry()
    {
        currentState = CharacterStates.Blocking;
    }

    protected void BeginAttack()
    {
        currentState = CharacterStates.Attacking;
        characterAnimator.ResetTrigger("IDLE");
        characterAnimator.SetTrigger("ATTACK");
    }

    public void PerformAttack()
    {
        if (OnAttack != null)
            OnAttack.Invoke(attackDamage);
    }

    public void ReturnToIdle()
    {
        characterAnimator.SetTrigger("IDLE");
        armored = false;
        interrupted = false;
        currentState = CharacterStates.Idle;
    }
}

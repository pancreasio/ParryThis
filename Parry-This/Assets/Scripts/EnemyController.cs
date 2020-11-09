using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{
    // Start is called before the first frame update
    public float minimumAttackInterval;
    public float maximumAttackInterval;
    private float attackTimer;
    private bool canAttack;
    protected override void Start()
    {
        base.Start();
        attackTimer = 0f;
        canAttack = false;
    }

    public override void BeginCombat()
    {
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack && currentState == CharacterStates.Idle)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= minimumAttackInterval)
            {
                if (attackTimer >= maximumAttackInterval)
                {
                    Attack();
                    attackTimer = 0f;
                }
                else
                {
                    if (Random.Range(minimumAttackInterval, maximumAttackInterval) < attackTimer)
                    {
                        Attack();
                        attackTimer = 0f;
                    }
                }
            }
        }
    }
}

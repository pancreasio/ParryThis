using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionNotifyer : MonoBehaviour
{
    public Character character;
    public void Attack()
    {
        character.PerformAttack();
    }
    public void BeginBlock()
    {
        character.BeginBlock();
    }

    public void EndParry()
    {
        character.EndParry();
    }
    public void EndBlock()
    {
        character.ResetBlock();
    }

    public void Idle()
    {
        character.ReturnToIdle();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static LevelManager.GameplayEvent OnAttackStart;
    public static LevelManager.GameplayEvent OnAttackEnd;
    public static LevelManager.GameplayEvent OnDefendStart;
    public static LevelManager.GameplayEvent OnDefendEnd;

    public void Attack()
    {
        LevelManager.InvokeIfNotNull(OnAttackStart);
    }

    public void BeginDefend()
    {
        LevelManager.InvokeIfNotNull(OnDefendStart);
    }

    public void EndDefend()
    {
        LevelManager.InvokeIfNotNull(OnDefendEnd);
    }
}

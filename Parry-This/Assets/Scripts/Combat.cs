using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    // Start is called before the first frame update
    public Character enemyCharacter;
    public GameObject playerExpectedPosition;
    public bool completed;

    public LevelManager.GameplayEvent OnCombatEnded;
    void Start()
    {
        completed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginCombat()
    {
        LevelManager.LevelInstance.playerCharacter.BeginCombat();
        enemyCharacter.BeginCombat();
        enemyCharacter.OnDeath += EndCombat;
    }

    private void EndCombat()
    {
        completed = true;
        enemyCharacter.gameObject.SetActive(false);
        LevelManager.InvokeIfNotNull(OnCombatEnded);
    }
}

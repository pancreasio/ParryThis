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
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayerAttacked()
    {

    }
}

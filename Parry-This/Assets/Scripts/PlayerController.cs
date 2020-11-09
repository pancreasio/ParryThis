using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    public float moveSpeed;
    public LevelManager.GameplayEvent OnDestinationReached;
    protected override void Start()
    {
        base.Start();
    }

    public void ProceedToNextCombat(float targetPosition)
    {
        characterAnimator.SetTrigger("WALK");
        StartCoroutine(WalkToTarget(targetPosition));
    }

    private void DestinationReached()
    {
        characterAnimator.SetTrigger("IDLE");
        ReturnToIdle();
        LevelManager.InvokeIfNotNull(OnDestinationReached);
    }

    private IEnumerator WalkToTarget(float targetPosition)
    {
        while(transform.position.x< targetPosition)
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(targetPosition, transform.position.y, 0f);
        DestinationReached();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

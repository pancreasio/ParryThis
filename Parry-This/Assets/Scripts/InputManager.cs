using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static LevelManager.GameplayEvent OnAttackStart;
    public static LevelManager.GameplayEvent OnAttackEnd;
    public static LevelManager.GameplayEvent OnDefendStart;
    public static LevelManager.GameplayEvent OnDefendEnd;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (Camera.main.ScreenToViewportPoint(touch.position).x < 0.5f)
                    LevelManager.InvokeIfNotNull(OnDefendStart);
                else
                    LevelManager.InvokeIfNotNull(OnAttackStart);
            }
            else
            if (touch.phase == TouchPhase.Ended)
            {
                if (Camera.main.ScreenToViewportPoint(touch.position).x < 0.5f)
                    LevelManager.InvokeIfNotNull(OnDefendEnd);
                else
                    LevelManager.InvokeIfNotNull(OnAttackStart);
            }
        }
#endif

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main.ScreenToViewportPoint(Input.mousePosition).x < 0.5f)
                LevelManager.InvokeIfNotNull(OnDefendStart);
            else
                LevelManager.InvokeIfNotNull(OnAttackStart);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (Camera.main.ScreenToViewportPoint(Input.mousePosition).x < 0.5f)
                LevelManager.InvokeIfNotNull(OnDefendEnd);
            else
                LevelManager.InvokeIfNotNull(OnAttackEnd);
        }
#endif
    }
}

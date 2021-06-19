using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class Weight : MonoBehaviour
{
    private LeanDragTranslateRigidbody lean;
    void Start()
    {
        lean = gameObject.GetComponent<LeanDragTranslateRigidbody>();
    }

    void Update()
    {
        if (GameManager.gameState == GameState.Over)
        {
            lean.enabled = false;
        }
    }
}

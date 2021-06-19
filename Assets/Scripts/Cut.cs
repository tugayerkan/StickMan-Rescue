using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class Cut : MonoBehaviour
{
    public static event Action OnSucces;

    public GameObject hangMan, rope;
    public Rigidbody rb;
    public Rigidbody sphereJoint;
    public LeanDragTranslate lean;

    private void Start()
    {

    }
    private void Update()
    {
        if (GameManager.gameState == GameState.StandBy)
        {
            if (lean != null)
            {
                lean.enabled = false;
            }
        }
        else if (GameManager.gameState == GameState.Playing)
        {
            if (lean != null)
            {
                lean.enabled = true;
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Cut"))
        {
            Debug.Log("Cut");
            if (sphereJoint != null)
            {
                sphereJoint.AddForce(Vector3.down * 5, ForceMode.Impulse);
            }
            rb.isKinematic = false;
            rope.gameObject.SetActive(false);
            OnSucces?.Invoke();
        }
    }

}

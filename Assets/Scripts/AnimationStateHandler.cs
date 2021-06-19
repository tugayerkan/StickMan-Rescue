using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SencanUtils;
public class AnimationStateHandler : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();


    }

    void Update()
    {

        if (GameManager.gameState == GameState.Over)
        {
            _animator.SetBool("IsDead", true);
        }
        else if (GameManager.gameState == GameState.Playing)
        {
            _animator.SetBool("IsDead", false);
        }
    }
    private void OnEnable()
    {
        Cut.OnSucces += OnSuccessfull;
        CharacterController.onLevel += OnLevelWon;
    }
    private void OnDisable()
    {
        Cut.OnSucces -= OnSuccessfull;
        CharacterController.onLevel -= OnLevelWon;

    }
    public void OnSuccessfull()
    {
        _animator.SetBool("IsFalling", true);
        _animator.SetBool("IsGrounded", true);
    }
    public void OnLevelWon()
    {
        _animator.SetBool("isWon", true);
    }
}

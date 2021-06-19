using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;


public class FlameThrower : MonoBehaviour
{
    [SerializeField] private Collider flameThrowerCollider;
    [SerializeField] private float interval = 1f;
    [SerializeField] private ParticleSystem flames;

    private bool _isStopped;
    private void Start()
    {
        FlameThrowerControl();
    }
    private async void FlameThrowerControl()
    {
        if (_isStopped) return;
        flameThrowerCollider.enabled = false;
        flames.Stop();
        await UniTask.Delay(TimeSpan.FromSeconds(interval));
        if (_isStopped) return;
        flameThrowerCollider.enabled = true;
        flames.Play();
        await UniTask.Delay(TimeSpan.FromSeconds(interval));
        FlameThrowerControl();
    }
    private void OnDisable()
    {
        _isStopped = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            other.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            GameManager.gameState = GameState.Over;
        }
    }
}

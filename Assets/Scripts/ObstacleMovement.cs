using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObstacleMovement : MonoBehaviour
{
    public GameObject knife;
    public float moveDuration;
    void Start()
    {
        //obstacle.transform.DOMoveY(moveDuration, 2f).SetEase(Ease.InSine).SetLoops(-1, LoopType.Yoyo);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weight"))
        {
            Debug.Log("onTrack");
            knife.transform.DOMoveX(-0.8f, 0.25f);
        }
    }
}

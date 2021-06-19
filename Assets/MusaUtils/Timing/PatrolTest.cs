using System;
using Cysharp.Threading.Tasks;
using MU;
using MusaUtils;
using UnityEngine;

public class PatrolTest : MonoBehaviour
{
    public Transform area;
    void Start()
    {
        Patrolling();
    }

    private async void Patrolling()
    {
        QuickPatrol.StartPatrol(gameObject, area, 3f, 1f);
        await UniTask.Delay(TimeSpan.FromSeconds(5));
        QuickPatrol.StopPatrol();
    }
}

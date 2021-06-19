using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CharacterController : MonoBehaviour
{
    public static event Action onLevel;

    private Renderer _headMesh;
    public GameObject head, obiRope, neckRope;
    public ObjectController hangMan;
    private Counter _count;

    [SerializeField]
    [Range(0f, 1f)] float lerpTime;
    [SerializeField]
    Color red, white;


    void Start()
    {
        _count = Counter.counter;
        _headMesh = head.GetComponent<Renderer>();

    }

    void Update()
    {
        if (GameManager.gameState == GameState.Playing)
        {

            _headMesh.material.color = Color.Lerp(_headMesh.material.color, red, lerpTime);
        }
        else
        {
            _headMesh.material.color = Color.Lerp(_headMesh.material.color, white, lerpTime);
        }


        if (GameManager.gameState == GameState.LevelWon)
        {
           // obiRope.gameObject.SetActive(false);
            neckRope.gameObject.SetActive(false);
            onLevel?.Invoke();

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Placement"))
        {
            _count.count++;
            hangMan.isWin = true;
            ObjectController.boxIsPlaced = true;

        }
    }
}

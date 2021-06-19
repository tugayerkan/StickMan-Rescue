using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

[System.Serializable]
public class Boxes
{
    public int BoxID;
    public GameObject box;
    public LeanDragTranslateRigidbody Lean;

}
public class ObjectContainer : MonoBehaviour
{

    public Boxes[] boxes;
    public ObjectController box;
    Counter counter;

    void Start()
    {
        counter = Counter.counter;


        for (int i = 0; i < boxes.Length; i++)
        {
            box = GameObject.FindWithTag("Object").GetComponent<ObjectController>();
            if (boxes[1] != null)
            {
                boxes[1].Lean.enabled = false;
            }
            else
            {
                return;
            }
        }

    }


    void Update()
    {
        if (GameManager.gameState == GameState.StandBy) 
        {

            boxes[0].Lean.enabled = false;
        }
        else if (GameManager.gameState == GameState.Playing)
        {
            boxes[0].Lean.enabled = true;
        }

        Debug.Log("Counter:" + counter.count);
        if (counter.count == 1)
        {
            Debug.Log("girdi");
            StartCoroutine(ActivateLean());


        }
        else if (counter.count == 2)
        {
            StartCoroutine(DeactivateLean());
        }
        else
        {
            return;
        }
    }
    private IEnumerator ActivateLean()
    {
        if (counter.count <= 1 && counter.count > 0 && GameManager.gameState == GameState.Playing)
        {
            yield return new WaitForSeconds(0.5f);
            if (counter.count < boxes.Length)
            {

                boxes[counter.count].Lean.enabled = true;
            }
        }
    }
    private IEnumerator DeactivateLean()
    {
        if (counter.count <= 1 && counter.count > 0)
        {
            yield return new WaitForSeconds(0.5f);
            boxes[counter.count].Lean.enabled = false;

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public static Counter counter;
    public int count;

    private void Awake()
    {
        counter = this;
    }
}

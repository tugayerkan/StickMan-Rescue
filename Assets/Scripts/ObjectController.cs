using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectController : MonoBehaviour
{
    public Rigidbody rb;
    public Transform center;
    private LeanDragTranslateRigidbody Lean;
    private Counter _count;
    private Vector3 _defaultPos;

    public bool isWin;
    private bool _check;
    public static bool boxIsPlaced;
    public float step = 1;
    private float _timer;

    private void Awake()
    {
        _defaultPos = transform.position;
    }
    void Start()
    {
        Lean = gameObject.GetComponent<LeanDragTranslateRigidbody>();
        _count = Counter.counter;
        // FindObjectOfType<RayFire.RayfireRigid>().Initialize();

    }

    void Update()
    {
        if (gameObject.transform.position.y < -1 || gameObject.transform.position.y > 4f)
        {
            ResetObject();
        }
        if (GameManager.gameState == GameState.Over)
        {
            Lean.enabled = false;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (_check)
        {
            return;
        }

        if (other.CompareTag("Placement"))
        {

            if (rb.velocity.sqrMagnitude < 5)
            {
                _timer = 0;
                //rb.transform.position = Vector3.MoveTowards(transform.position, center.transform.position, step);
                //transform.eulerAngles = Vector3.zero;
                // rb.constraints = RigidbodyConstraints.FreezeAll;
                //Lean.enabled = false;
                //count.count++;
                //boxIsPlaced = true;
                //isWin = true;
                //check = true;
                //objectInPlace = true;
                Debug.Log("Entered");
            }
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (_check)
        {
            return;
        }
        if (other.CompareTag("Placement") && rb.velocity.sqrMagnitude < 5)
        {
            if (_timer >= 2)
            {
                // transform.eulerAngles = Vector3.zero;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                Lean.enabled = false;
                _count.count++;
                boxIsPlaced = true;
                isWin = true;
                _check = true;
            }
            else
            {
                _timer += Time.fixedDeltaTime;
            }
        }
    }

    private void ResetObject()
    {
        transform.position = _defaultPos;
        transform.eulerAngles = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.constraints = RigidbodyConstraints.None;
    }
}

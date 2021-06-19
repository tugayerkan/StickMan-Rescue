using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayFire;

public class GlassShatter : MonoBehaviour
{
    public RayfireRigid ray;
    public GameObject normalGlass;
    public GameObject glassRoot;
    void Start()
    {
         RayfireRigid ray= FindObjectOfType<RayFire.RayfireRigid>();
    }

   
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Weight"))
        {
            normalGlass.gameObject.SetActive(false);
            ray.Initialize();
           
                        
        }
    }
    private IEnumerator DisableRoot()
    {
        yield return new WaitForSeconds(0.5f);
        glassRoot.gameObject.SetActive(false);
    }
}

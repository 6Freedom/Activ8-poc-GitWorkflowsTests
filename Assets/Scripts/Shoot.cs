using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject sphere;

    public GameObject Sphere
    {
        get => sphere;
        set => sphere = value;
    }

    [SerializeField] private float shootForce = 2;
    
    
    
    void Update()
    {
        if (Input.anyKeyDown)
        {
            GameObject shootedSphere = Instantiate(sphere, transform.position, transform.rotation);
            
            Vector3 force = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);

            force.Normalize();

            force *= shootForce;
            
            shootedSphere.GetComponent<Rigidbody>().AddForce(force);
        }
    }
}

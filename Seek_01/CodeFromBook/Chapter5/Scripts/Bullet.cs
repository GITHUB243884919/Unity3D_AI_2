using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{ 
    public float LifeTime = 3.0f;
    public int damage = 50;
	public float beamVelocity = 100;


	public void Go ()
	{
		rigidbody.AddForce(transform.forward * 10, ForceMode.VelocityChange);
	}
	
	void FixedUpdate()
	{
		rigidbody.AddForce(transform.forward * beamVelocity, ForceMode.Acceleration);
	}


    void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    void Update()
    {
        //transform.position += transform.forward * Speed * Time.deltaTime;       
    }

    void OnCollisionEnter(Collision collision)
    {        
        Destroy(gameObject);
    }
}
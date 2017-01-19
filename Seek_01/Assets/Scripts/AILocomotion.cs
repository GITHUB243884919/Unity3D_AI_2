using UnityEngine;
using System.Collections;

public class AILocomotion : Vehicle 
{
	private CharacterController controller;
	private Rigidbody theRigidbody;
	private Vector3 moveDistance;
	public bool displayTrack;
    private Animator m_animator = null;
	// Use this for initialization
	void Start () 
	{
		controller = GetComponent<CharacterController>();
		theRigidbody = GetComponent<Rigidbody>();
		moveDistance = new Vector3(0,0,0);
        m_animator = GetComponent<Animator>();
		base.Start();
	}
	

	void FixedUpdate()
	{
        //Debug.Log("acceleration :" + acceleration);
        //Debug.Log("velocity0 :" + velocity);
		velocity += acceleration * Time.fixedDeltaTime;

        //Debug.Log("velocity1 :" + velocity);
		if (velocity.sqrMagnitude > sqrMaxSpeed)
			velocity = velocity.normalized * maxSpeed;
        
        //if (velocity != Vector3.zero)
        //{
        //    m_animator.SetBool("isRun", true);
        //}

        //Debug.Log("velocity2 :" + velocity);
		moveDistance = velocity * Time.fixedDeltaTime;
		
		if (isPlanar)
		{
			velocity.y = 0;
			moveDistance.y = 0;
		}

		if (displayTrack)
			//Debug.DrawLine(transform.position, transform.position + moveDistance, Color.red,30.0f);
			Debug.DrawLine(transform.position, transform.position + moveDistance, Color.red, 30.0f);
		
		if (controller != null)
		{
			//if (displayTrack)
				//Debug.DrawLine(transform.position, transform.position + moveDistance, Color.blue,20.0f);
			controller.SimpleMove(velocity);

		}
		else if (theRigidbody == null || theRigidbody.isKinematic)
		{
            //Debug.Log("moveDistance " + moveDistance);
			transform.position += moveDistance;
		}
		else
		{
			theRigidbody.MovePosition(theRigidbody.position + moveDistance);		
		}
		
		//updata facing direction
		if (velocity.sqrMagnitude > 0.00001)
		{
			Vector3 newForward = Vector3.Slerp(transform.forward, velocity, damping * Time.deltaTime);
			if (isPlanar)
				newForward.y = 0;
			transform.forward = newForward;
		}

		//gameObject.animation.Play("walk");
        //速度为0时，动画的跑步动作停止
        m_animator.SetBool("isRun", true);
        if (velocity == Vector3.zero)
        {
            m_animator.SetBool("isRun", false);
        }
        
        //velocity = Vector3.zero;

	}
}

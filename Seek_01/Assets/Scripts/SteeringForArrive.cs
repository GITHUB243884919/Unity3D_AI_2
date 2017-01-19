using UnityEngine;
using System.Collections;

public class SteeringForArrive : Steering 
{
	//public enum Deceleration{slow = 3, normal = 2, fast = 1};
	//public Deceleration deceleration;
	public bool isPlanar = true;
	//public float DecelerationTweaker = 0.8f;
	public float arrivalDistance = 0.3f;
	public float characterRadius = 1.2f;
    
    //原书代码没有设置减速半径初始值
    //public float slowDownDistance;
	public float slowDownDistance = 2.0f;
    //增加一个距离值，表示已经到达
    public float nearDistance     = 0.5f;

	public GameObject target;
	private Vector3 desiredVelocity;
	private Vehicle m_vehicle;
	private float maxSpeed;


	void Start () {
		m_vehicle = GetComponent<Vehicle>();
		maxSpeed = m_vehicle.maxSpeed;
		isPlanar = m_vehicle.isPlanar;
	}
	

	public override Vector3 Force()
	{
		Vector3 toTarget = target.transform.position - transform.position;
		Vector3 desiredVelocity;
		Vector3 returnForce;
		if (isPlanar) toTarget.y = 0;
		float distance = toTarget.magnitude;

		if (distance > slowDownDistance)
		{
			desiredVelocity = toTarget.normalized * maxSpeed;
			returnForce = desiredVelocity - m_vehicle.velocity;
		}
        else if (distance <= nearDistance)
        {
            m_vehicle.velocity = Vector3.zero;
            returnForce = Vector3.zero;
        }
		else
		{
			desiredVelocity = toTarget - m_vehicle.velocity;
			returnForce = desiredVelocity - m_vehicle.velocity;
		}

		return returnForce;
	}

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(target.transform.position, slowDownDistance);
    }
}

using UnityEngine;
using System.Collections;

public class SteeringForPursuit : Steering {

	public GameObject target;
	private Vector3 desiredVelocity;
	private Vehicle m_vehicle;
	private float maxSpeed;


	void Start () {
		m_vehicle = GetComponent<Vehicle>();
		maxSpeed = m_vehicle.maxSpeed;
	}
	

	public override Vector3 Force()
	{
		Vector3 toTarget = target.transform.position - transform.position;

		float relativeDirection = Vector3.Dot(transform.forward, target.transform.forward);

		if ((Vector3.Dot(toTarget, transform.forward) > 0) && (relativeDirection < -0.95f))
		{
			desiredVelocity = (target.transform.position - transform.position).normalized * maxSpeed;
			return (desiredVelocity - m_vehicle.velocity);
		}
        
		float lookaheadTime = toTarget.magnitude / (maxSpeed + target.GetComponent<Vehicle>().velocity.magnitude);
        //如果加了以下的if语句，那么玩家永远的目标后面，追不上：）
        //if (lookaheadTime > 1.0f)
        //{
        //    lookaheadTime = 1.0f;
        //}
        desiredVelocity = (
            target.transform.position +
            target.GetComponent<Vehicle>().velocity * lookaheadTime -
            transform.position
        ).normalized * maxSpeed;
        //上面是原书计算预期速度的方法，需要先算出T(lookaheadTime)，以下是我的算法，去掉
        //了T的概念，而直接用当前目标的速度代表目标的移动。这样的效果会出现玩家接近目标时追
        //多了的情况，永远跑在了前面。
        //desiredVelocity = (
        //    target.transform.position +
        //    target.GetComponent<Vehicle>().velocity -
        //    transform.position
        //).normalized * maxSpeed;
        
        return (desiredVelocity - m_vehicle.velocity);

	}
}

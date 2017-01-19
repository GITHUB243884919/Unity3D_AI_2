using UnityEngine;
using System.Collections;

//pay attention that this function is quite related with frame rate

public class SteeringForWander2 : Steering
{

    public float wanderRadius;
    public float wanderDistance;
    public float wanderJitter;
    public bool isPlanar;
    //public GameObject targetIndicator;

    private Vector3 desiredVelocity;
    private Vehicle m_vehicle;
    private float maxSpeed;
    private Vector3 circleTarget;
    private Vector3 wanderTarget;


    void Start()
    {
        m_vehicle = GetComponent<Vehicle>();
        maxSpeed = m_vehicle.maxSpeed;
        isPlanar = m_vehicle.isPlanar;
        //目标点初始值为圆内的一个点
        circleTarget = new Vector3(wanderRadius * 0.707f, 0, wanderRadius * 0.707f);
    }


    public override Vector3 Force()
    {
        //随机位移
        Vector3 randomDisplacement = new Vector3(
            (Random.value - 0.5f) * 2 * wanderJitter,
            (Random.value - 0.5f) * 2 * wanderJitter,
            (Random.value - 0.5f) * 2 * wanderJitter);
        if (isPlanar)
            randomDisplacement.y = 0;

        //新的目标点 = （原目标点 + 随机位移）
        circleTarget += randomDisplacement;
        //circleTarget = wanderRadius * circleTarget.normalized;
        circleTarget = circleTarget.normalized;

        //真正的目标点，这个是相对玩家的。之前的circleTarget是相对圆心的
        //wanderTarget = m_vehicle.velocity.normalized * wanderDistance +
        //    circleTarget + transform.position;
        wanderTarget = circleTarget + transform.position;
        //预期速度
        desiredVelocity = (wanderTarget - transform.position).normalized * maxSpeed;
        return (desiredVelocity - m_vehicle.velocity);
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(
        //    transform.position + m_vehicle.velocity.normalized * wanderDistance,
        //    wanderRadius);

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(
        //    transform.position * wanderDistance,
        //    wanderRadius);

        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(
        //    transform.position + m_vehicle.velocity.normalized * wanderDistance + circleTarget,
        //    1.0f);

    }
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Radar))]
public class SteeringForSeparation : Steering {

	public float comfortDistance = 1;
	public float multiplierInsideComfortDistance = 2;

	void Start () {
	
	}

	public override Vector3 Force()
	{
		Vector3 steeringForce = new Vector3(0,0,0);
        //Debug.Log(GetComponent<Radar>().neighbors.Count + " neighbors");
		foreach (GameObject s in GetComponent<Radar>().neighbors)
		{
			if ((s!=null)&&(s != this.gameObject))
			{
                //Debug.Log("calc separate force");
				Vector3 toNeighbor = transform.position - s.transform.position;
				float length = toNeighbor.magnitude;
                //steeringForce += toNeighbor.normalized / length;
                //if (length < comfortDistance)
                //    steeringForce *= multiplierInsideComfortDistance;
                //原书存在bug，当两者距离为0时发生了除0操作。
                //修改为当距离为0的时候，产生一个[0.1,1]的随机位移，让下一帧根据新位置计算
                if (length == 0)
                {
                    transform.position += Vector3.one * Random.Range(0.1f, 1.0f);
                }
                else
                {
                    steeringForce += toNeighbor.normalized / length;
                    if (length < comfortDistance)
                        steeringForce *= multiplierInsideComfortDistance;
                }

			}
		}

		return steeringForce;
	}
}

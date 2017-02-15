using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SightSensor))]
public class DrawViewCone : Editor
{
	private float viewDistance;
	private float fieldOfView;
	
	void OnSceneGUI()
	{
		SightSensor myTarget = (SightSensor)target;
		viewDistance = myTarget.viewDistance;
		fieldOfView = myTarget.fieldOfView;

		float fieldOfViewinRadians = fieldOfView*3.14f/180.0f;
		Vector3 leftRayPoint = myTarget.transform.TransformPoint(new Vector3(-viewDistance * Mathf.Sin(fieldOfViewinRadians),0,viewDistance * Mathf.Cos(fieldOfViewinRadians)));
		Vector3 from = leftRayPoint - myTarget.transform.position;
		
		Handles.color = new Color(0f, 1f, 1f, 0.2f);
		
		Handles.DrawSolidArc(myTarget.transform.position,myTarget.transform.up,from,fieldOfView*2,viewDistance);
		
		Handles.color = new Color(0f,1f,1f,0.1f);
	}
	
}


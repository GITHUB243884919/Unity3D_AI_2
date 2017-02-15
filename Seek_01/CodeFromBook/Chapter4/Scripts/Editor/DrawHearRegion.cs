using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundSensor))]
public class DrawHearRegion : Editor
{
	private float radius;
	
	void OnSceneGUI()
	{
		SoundSensor myTarget = (SoundSensor)target;
		radius = myTarget.hearingDistance;
		
		Handles.color = new Color(0f, 0.8f, 0.4f, 0.2f);
		
		Handles.DrawSolidDisc(myTarget.transform.position,myTarget.transform.up,radius);
		
		Handles.color = new Color(0f,1f,1f,0.1f);
	}
	
}


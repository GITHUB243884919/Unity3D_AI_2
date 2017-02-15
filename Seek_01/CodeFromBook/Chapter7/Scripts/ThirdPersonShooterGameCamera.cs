using UnityEngine;
using System.Collections;

public class ThirdPersonShooterGameCamera : MonoBehaviour {
	
	public Transform player;
	public Texture crosshair; 

	[HideInInspector]
	public Transform aimTarget; 
	
	public float smoothingTime = 10.0f; // it should follow it faster by jumping (y-axis) (previous: 0.1 or so) ben0bi
	public Vector3 pivotOffset = new Vector3(0.2f, 0.7f,  0.0f); // offset of point from player transform (?) ben0bi
	public Vector3 camOffset   = new Vector3(0.0f, 0.7f, -3.4f); // offset of camera from pivotOffset (?) ben0bi
	public Vector3 closeOffset = new Vector3(0.35f, 1.7f, 0.0f); // close offset of camera from pivotOffset (?) ben0bi
	
	public float horizontalAimingSpeed = 800f; 
	public float verticalAimingSpeed = 800f;   
	public float maxVerticalAngle = 80f;
	public float minVerticalAngle = -80f;
	
	public float mouseSensitivity = 0.3f;
		
	private float angleH = 0;
	private float angleV = 0;
	private Transform cam;
	private float maxCamDist = 1;
	private LayerMask mask;
	private Vector3 smoothPlayerPos;	

	void Start () 
	{
		// [edit] no aimtarget gameobject needs to be placed anymore - ben0bi
		GameObject g=new GameObject();
		aimTarget=g.transform;
		// Add player's own layer to mask
		mask = 1 << player.gameObject.layer;
		// Add Igbore Raycast layer to mask
		mask |= 1 << LayerMask.NameToLayer("Ignore Raycast");
		// Invert mask
		mask = ~mask;
		
		cam = transform;
		smoothPlayerPos = player.position;
		
		maxCamDist = 3;
	}
	

	void LateUpdate () {
		if (Time.deltaTime == 0 || Time.timeScale == 0 || player == null) 
			return;

		angleH += Mathf.Clamp(Input.GetAxis("Mouse X") , -1, 1) * horizontalAimingSpeed * Time.deltaTime;
													 
		angleV += Mathf.Clamp(Input.GetAxis("Mouse Y") , -1, 1) * verticalAimingSpeed * Time.deltaTime;
		// limit vertical angle
		angleV = Mathf.Clamp(angleV, minVerticalAngle, maxVerticalAngle);
		
		// Before changing camera, store the prev aiming distance.
		// If we're aiming at nothing (the sky), we'll keep this distance.
		float prevDist = (aimTarget.position - cam.position).magnitude;
		
		// Set aim rotation
		Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
		Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
		cam.rotation = aimRotation;
		
		// Find far and close position for the camera
		smoothPlayerPos = Vector3.Lerp(smoothPlayerPos, player.position, smoothingTime * Time.deltaTime);
		smoothPlayerPos.x = player.position.x;
		smoothPlayerPos.z = player.position.z;
		Vector3 farCamPoint = smoothPlayerPos + camYRotation * pivotOffset + aimRotation * camOffset;
		Vector3 closeCamPoint = player.position + camYRotation * closeOffset;
		float farDist = Vector3.Distance(farCamPoint, closeCamPoint);
		
		// Smoothly increase maxCamDist up to the distance of farDist
		maxCamDist = Mathf.Lerp(maxCamDist, farDist, 5 * Time.deltaTime);
		
		// Make sure camera doesn't intersect geometry
		// Move camera towards closeOffset if ray back towards camera position intersects something 
		RaycastHit hit;
		Vector3 closeToFarDir = (farCamPoint - closeCamPoint) / farDist;
		float padding = 0.3f;
		if (Physics.Raycast(closeCamPoint, closeToFarDir, out hit, maxCamDist + padding, mask)) {
			maxCamDist = hit.distance - padding;
		}
		cam.position = closeCamPoint + closeToFarDir * maxCamDist;
		
		// Do a raycast from the camera to find the distance to the point we're aiming at.
		float aimTargetDist;
		if (Physics.Raycast(cam.position, cam.forward, out hit, 100, mask)) {
			aimTargetDist = hit.distance + 0.05f;
		}
		else {
			// If we're aiming at nothing, keep prev dist but make it at least 5.
			aimTargetDist = Mathf.Max(5, prevDist);
		}
		
		// Set the aimTarget position according to the distance we found.
		// Make the movement slightly smooth.
		aimTarget.position = cam.position + cam.forward * aimTargetDist;
	}
	
	// so you can change the camera from a static observer (level loading) or something else
	// to your player or something else. I needed that for network init... ben0bi
	public void SetTarget(Transform t)
	{
		player=t;
	}


	void OnGUI () 
	{
		if (Time.time != 0 && Time.timeScale != 0)
			GUI.DrawTexture(new Rect(Screen.width/2-(crosshair.width*0.5f), Screen.height/2-(crosshair.height*0.5f), crosshair.width, crosshair.height), crosshair);
	}

}

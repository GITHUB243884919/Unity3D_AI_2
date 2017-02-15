using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public bool playMode = false;
	public GameObject weapon;

	public float turnSpeed = 2;
	
	public Texture2D redblood;
	public Texture2D blackblood;

	[HideInInspector]
	public int health;
	
	[HideInInspector]
	public bool isFiring = false;

	private Animation anim;
	private CharacterController controller;
	private Transform _t;
	private Gun gunScript;

	private bool isReloading = false;
	private float reloadTime;

	private float input_x;
	private float input_y;

	private float antiBunny = 0.75f;
	private Vector3 _velocity = Vector3.zero;
	private float _speed = 1;
	private float gravity = 20;

	void Start () 
	{
		controller = GetComponent<CharacterController>();
		anim = GetComponent<Animation>();
		_t = transform;
		reloadTime = anim["StandingReloadM4"].length;
		gunScript = weapon.GetComponent<Gun>();
		
		if (weapon != null)
			gunScript.controller = _t.gameObject;

		health = 100;
	}
	

	void Update () 
	{
		if (playMode)
		{
			if (health <= 0)
			{
				Destroy(this.gameObject);
				Application.LoadLevel("GameOver");
			}
		}

		float step = turnSpeed * Time.deltaTime;
		Transform target = Camera.main.GetComponent<ThirdPersonShooterGameCamera>().aimTarget;
		Vector3 newVel = target.position - transform.position;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, newVel.normalized, step, 0.0f);
		transform.rotation = Quaternion.LookRotation(newDir);

		float input_modifier = (input_x != 0.0f && input_y != 0.0f) ? 0.7071f : 1.0f;

		input_x = Input.GetAxis("Horizontal");
		input_y = Input.GetAxis("Vertical");

		_velocity = new Vector3(input_x * input_modifier, -antiBunny, input_y * input_modifier);
		_velocity = _t.TransformDirection(_velocity) * _speed;

		_velocity.y -= gravity * Time.deltaTime;
		controller.Move(_velocity * Time.deltaTime);

		if (input_y > 0.01f)
			anim.CrossFade("Walk");
		else if (input_y < -0.01f)
			anim.CrossFade("WalkBackwards");
		else if (input_x > 0.01f)
			anim.CrossFade("StrafeWalkRight");
		else if (input_x < -0.01f)
			anim.CrossFade("StrafeWalkLeft");
		else if (!isReloading)
			anim.CrossFade("Idle");

		if (Input.GetButton("Fire1"))
		{
			anim.Play("StandingFire");
			isFiring = true;
			gunScript.Fire();

		}
		else
		{
			isFiring = false;
			gunScript.StopFire();
		}

		if (Input.GetButton("Reload")&&(!isReloading))
		{
			isReloading = true;
			anim.Play("StandingReloadM4");
			gunScript.Reload();
			StartCoroutine("WaitForReloading");
		}

		if (Input.GetButton("Aim"))
		{
			anim.Play("StandingAim");
		}

		if (Input.GetButton("Crouch"))
		{
			anim.Play("Crouch");
		}

	}


	IEnumerator WaitForReloading()
	{
		yield return new WaitForSeconds(reloadTime);
		isReloading = false;
	}


	void OnGUI()
	{
		if (playMode)
		{
			if (health < 0)
				health = 0;
			int blood_width = redblood.width * health / 100;
			GUI.DrawTexture(new Rect(100,100,blackblood.width,blackblood.height),blackblood);
			GUI.DrawTexture(new Rect(100,100,blood_width,redblood.height),redblood);
		}
	}
}

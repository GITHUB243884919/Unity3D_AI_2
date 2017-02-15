using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Animation anim;
	private CharacterController controller;
	private Transform _t;

	private float input_x;
	private float input_y;

	public float antiBunny = 0.75f;
	private Vector3 _velocity = Vector3.zero;
	public float _speed = 1;
	public float gravity = 20;

	private float rotateAngle;
	private float targetAngle = 0;
	private float currentAngle;
	private float yVelocity = 0.0F;

	private int health;
	public Texture2D redblood;
	public Texture2D blackblood;
	public GameObject bulletPrefab;
	public Transform bulletSpawnPoint;
	private float fireInterval = 0.2f;
	private float lastFireTime = 0;


	void Start () 
	{
		health = 100;
		controller = GetComponent<CharacterController>();
		anim = GetComponent<Animation>();
		_t = transform;

		currentAngle = targetAngle = HorizontalAngle(transform.forward);
	}
	

	void Update () 
	{
		rotateAngle = Input.GetAxis("Rotate") * Time.deltaTime * 50;
		targetAngle += rotateAngle;
		
		currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref yVelocity, 0.3f);
		transform.rotation = Quaternion.Euler(0,currentAngle,0);

		float input_modifier = (input_x != 0.0f && input_y != 0.0f) ? 0.7071f : 1.0f;

		input_x = Input.GetAxis("Horizontal");
		input_y = Input.GetAxis("Vertical");

		_velocity = new Vector3(input_x * input_modifier, -antiBunny, input_y * input_modifier);
		_velocity = _t.TransformDirection(_velocity) * _speed;

		_velocity.y -= gravity * Time.deltaTime;
		controller.Move(_velocity * Time.deltaTime);

		if ((input_y > 0.01f) || (rotateAngle > 0.01f)|| (rotateAngle < -0.01f))
			anim.CrossFade("Walk");

		if (input_y < -0.01f)
			anim.CrossFade("WalkBackwards");

		if (input_x > 0.01f)
			anim.CrossFade("StrafeWalkRight");

		if (input_x < -0.01f)
			anim.CrossFade("StrafeWalkLeft");

		if (Input.GetButton("Fire1"))
		{
			anim.Play("StandingFire");
			if (Time.time - lastFireTime > fireInterval)
			{
				lastFireTime = Time.time;
				GameObject bullet = Instantiate(bulletPrefab,bulletSpawnPoint.position,Quaternion.identity) as GameObject;
				bullet.GetComponent<Bullet>().direction = transform.forward;
			}
			//audio.PlayOneShot(M4Sound);
		}

	}

	private float HorizontalAngle (Vector3 direction)
	{
		float num = Mathf.Atan2 (direction.x, direction.z) * 57.29578f;
		if (num < 0f)
		{
			num += 360f;
		}
		return num;
	}
}

using UnityEngine;
using System.Collections;
using Pathfinding;

public class EnemyAIController : AIPath 
{
	public float ShelterDistance = 0.6f;
	public GameObject weapon;
	public GameObject explosionPrefab;
	public GameObject targetPrefab;

	private GameObject player;
	private PlayerController playerController;
	private Gun gunScript;
	private Shelter _Shelter;
	private Animation anim;
	private float aimingStartTime;

	[HideInInspector]
	public int health = 100;

	[HideInInspector]
	public bool runToDestination = false;

	public Shelter Shelter
	{
		get {return _Shelter; }
		set
		{
			if (Shelter != value)
			{
				if (_Shelter != null)
					_Shelter.Controller = null;
				_Shelter = value;
				if (_Shelter != null)
					_Shelter.Controller = this;
			}
		}
	}


	void Start () 
	{
		anim = GetComponent<Animation>();

		GameObject newTarget = Instantiate(targetPrefab, transform.position, transform.rotation) as GameObject;
		target = newTarget.transform;

		player = GameObject.FindGameObjectWithTag("Player");
		playerController = player.GetComponent<PlayerController>();
		gunScript = weapon.GetComponent<Gun>();

		if (weapon != null)
			gunScript.controller = transform.gameObject;

		base.Start();

	}

	public override Vector3 GetFeetPosition()
	{
		return tr.position;
	}
	

	void Update () 
	{
		if (health <= 0)
		{
			Instantiate(explosionPrefab,transform.position,Quaternion.identity);
			Destroy(this.gameObject);
		}

		if ((Shelter != null)&& runToDestination )
		{
			base.Update();

		}

		if (!runToDestination)
		{
			Vector3 dir = player.transform.position - transform.position;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, dir.normalized, turningSpeed * Time.deltaTime, 0.0f);
			transform.rotation = Quaternion.LookRotation(newDir);
		}
	}


	public void RunTowardsShelter()
	{
		anim.CrossFade("Run");
	}

	public void Crouch()
	{
		anim.CrossFade("Crouch");
	}

	public void Stand()
	{
		anim.CrossFade("Standing");
	}

	public void Aim()
	{
		anim.CrossFade("StandingAimCenter");
	}

	public void Reload()
	{
		anim.CrossFade("Reload");

		gunScript.Reload();
	}

	public void Fire()
	{
		anim.CrossFade("Fire");

		gunScript.Fire();
	}

	public void StopFire()
	{
		gunScript.StopFire();
	}

	public void SelectNewShelter()
	{

		for (int i=0; i<3; i++)
		{
			int shelterIndex = Random.Range(0,Shelter.Shelters.Count);
			if (Shelter.Shelters[shelterIndex] == Shelter)
				continue;
			if (Shelter.Shelters[shelterIndex].Controller == null)
			{
				this.Shelter = Shelter.Shelters[shelterIndex];

				target.position = this.Shelter.transform.position;
				seeker.StartPath(GetFeetPosition(),target.position);
				return;
			}
		}

		foreach (var s in Shelter.Shelters)
		{
			if (s == Shelter)
				continue;
			if (s.Controller == null)
			{
				this.Shelter = s;
				break;
			}
		}

		target.position = this.Shelter.transform.position;

		seeker.StartPath(GetFeetPosition(),target.position);
	}

	public bool IsPlayerFiring()
	{		
		if (playerController.isFiring)
		{
			print("The Player is firing!");
			return true;
		}
		else
			return false;
	}

	public bool shouldReload()
	{
		return gunScript.ammo < 1;
	}


}

using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour 
{
	public GameObject bulletPrefab;
	public Transform bulletSpawnPoint;

	public AudioClip fireSound;
	public AudioClip reloadSound;

	public float fireInterval;

	public float spread = 0.5f;

	public int ammo = 30;
	public int reloadAmount = 30;

	private Vector3 bulletDirection;
	private Quaternion bulletRotation;
	private float lastFireTime = -1;
	private bool isFiring = false;

	[HideInInspector]
	public GameObject controller;


	void Start () 
	{
		if (bulletSpawnPoint == null)
			bulletSpawnPoint = transform;
	}

	public void Fire()
	{
		isFiring = true;
	}


	public void StopFire()
	{
		isFiring = false;
	}


	void Update () 
	{

		if (controller != null)
		{
			Quaternion spreadRot = Quaternion.Euler(0,(Random.value-0.5f) * 2 * spread,0);
			bulletRotation = Quaternion.LookRotation(spreadRot * controller.transform.forward);
			bulletDirection = bulletRotation * Vector3.forward;
		}

		if (isFiring && ammo > 0)
		{
			if (Time.time - lastFireTime > fireInterval)
			{
				Shoot();
				lastFireTime = Time.time;
			}
		}	
	}

	void Shoot()
	{
		if (controller == null)
			return;

		GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletRotation) as GameObject;
		bulletObj.GetComponent<Bullet>().direction = bulletDirection;

		if (fireSound != null)
			audio.PlayOneShot(fireSound,1);

		if (ammo > 0)
			ammo --;

	}

	public void Reload()
	{
		ammo = reloadAmount;
		if (reloadSound != null)
			audio.PlayOneShot(reloadSound,1);
	}
}

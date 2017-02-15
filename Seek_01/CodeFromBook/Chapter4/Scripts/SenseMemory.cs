using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SenseMemory : MonoBehaviour 
{
	private bool alreadyInList = false;
	public float memoryTime = 4.0f;


	public List<MemoryItem> memoryList = new List<MemoryItem>();
	private List<MemoryItem> removeList = new List<MemoryItem>();

	void Start () 
	{
		
	}

	public bool FindInList()
	{
		foreach (MemoryItem mi in memoryList)
			if (mi.g.tag == "Player")
				return true;

		return false;
	}

	public void AddToList(GameObject g, float type)
	{
		alreadyInList = false;

		foreach (MemoryItem mi in memoryList)
		{
			if (g == mi.g)
			{
				alreadyInList = true;
				mi.lastMemoryTime = Time.time;
				mi.memoryTimeLeft = memoryTime;
				if (type > mi.sensorType)
					mi.sensorType = type;
				break;
			}
		}

		if (!alreadyInList)
		{
			MemoryItem newItem = new MemoryItem(g, Time.time, memoryTime, type);
			memoryList.Add(newItem);
		}
	}
	

	void Update () 
	{
		removeList.Clear();

		foreach (MemoryItem mi in memoryList)
		{
			mi.memoryTimeLeft -= Time.deltaTime;
			if (mi.memoryTimeLeft < 0)
			{
				//memoryList.Remove(mi);
				removeList.Add(mi);
			}
			else
			{
				if (mi.g != null)
					Debug.DrawLine(transform.position, mi.g.transform.position, Color.blue);
			}
		}

		foreach (MemoryItem m in removeList)
		{
			memoryList.Remove(m);
		}
	}
}

public class MemoryItem
{
	public GameObject g;
	public float lastMemoryTime;
	public float memoryTimeLeft;
	public float sensorType;

	public MemoryItem(GameObject objectToAdd, float time, float timeLeft,float type)
	{
		g = objectToAdd;
		lastMemoryTime = time;
		memoryTimeLeft = timeLeft;
		sensorType = type;
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public Vector3 spawnArea;
	public float defSpawnDelay;
	public float spawnTimer;

	void Start()
	{
		spawnTimer = defSpawnDelay;
	}

	void Update()
	{
		spawnTimer -= Time.deltaTime;
		if(spawnTimer <= 0f)
		{
			SpawnObjects();
			spawnTimer = defSpawnDelay;
		}
	}

	void SpawnObjects()
	{
		MyPooler.ObjectPooler.Instance.GetFromPool("Cube", GetRandomPos(), Quaternion.identity);
		MyPooler.ObjectPooler.Instance.GetFromPool("Sphere", GetRandomPos(), Quaternion.identity);
	}

	Vector3 GetRandomPos()
	{
		Vector3 randomPosition = new Vector3(Random.Range(0, spawnArea.x), 0f, Random.Range(0, spawnArea.z));
		randomPosition += transform.position;
		return randomPosition;
	}
}

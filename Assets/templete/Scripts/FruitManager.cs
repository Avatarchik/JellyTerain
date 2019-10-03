using System;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
	private void Start()
	{
		this.gManager = UnityEngine.Object.FindObjectOfType<GameManager>();
        UnityEngine.Object.Instantiate<GameObject>(this.fruits[UnityEngine.Random.Range(0, this.fruits.Length)], new Vector3(0f, 0f, 4f), Quaternion.identity).transform.parent = base.gameObject.transform;
	}

	private void Update()
	{
		if (base.transform.position.z < (float)(this.gManager.maxDis + 10))
		{
			base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(0f, 2f, 4f + Time.timeSinceLevelLoad * 1.5f * this.gManager.speed), Time.deltaTime * 10f);
		}
	}

	public GameObject[] fruits;

	public GameObject player;

	private GameManager gManager;
}

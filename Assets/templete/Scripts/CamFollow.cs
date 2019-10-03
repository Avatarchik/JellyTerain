using System;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
	private void Start()
	{
		this.offset = base.transform.position - this.player.transform.position;
	}

	private void Update()
	{
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, this.player.transform.position.z + this.offset.z);
	}

	public GameObject player;

	private Vector3 offset;
}

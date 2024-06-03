using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class WaypointArea : MonoBehaviour
{
	[SerializeField] private float width = 200;
	[SerializeField] private float depth = 50;
	[SerializeField] private int   index;

	private IAlienCounter _alienCounter;

	private float timer = 0;
	private void OnDrawGizmos()
	{
		Vector3 position = transform.position+new Vector3(0,1,0);
		position = new Vector3(0,.25f,0);
		Vector3 size     = new Vector3(width,.5f,depth);

		Gizmos.matrix = transform.localToWorldMatrix;

		Gizmos.color = new Color(1, 1, 1, .2f);
		Gizmos.DrawCube(position, size);
        
		Gizmos.color = new Color(0, 0, 1);
		Gizmos.DrawWireCube(position, size);
	}


	public Vector3 GetRandomWaypointInArea()
	{
		var     position         = transform.position;
		var     x                = Random.Range(-width / 2, width / 2);
		Vector3 waypointPosition = new Vector3(x, 0, 0);
		return transform.TransformPoint(waypointPosition);
	}
	
	
	void Start()
	{
		_alienCounter = GameObjectExtensions.FindObjectsOfTypeWithInterface<IAlienCounter>()[0];
	}
}

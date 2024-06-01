using System;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.Events;

public class LastStandStandEngine : MonoBehaviour
{
	[SerializeField] private float       StoreOpenTime = 10;
	[SerializeField] private Transform   standDoorTransform;
	[SerializeField] private Transform   standDoorClosedTransform; 
	[SerializeField] private Transform   standDoorOpenTransform;
	[SerializeField] private float       doorSpeed = .5f;
	[SerializeField] private AudioSource _audioSource;
	[SerializeField] private WaveEngine  _waveEngine;

	private Rigidbody _standDoorRigidbody;
	IAlienCounter     _alienCounter;

	
	enum StandState
	{
		Closed,
		DoorOpening,
		Open,
		DoorClosing,
	}

	private StandState _standState = StandState.Closed;

	StandState standState
	{
		get { return _standState;}
		set
		{
			_standState = value;
			Debug.Log($"---------------->  New Stand State = {value}");
		}
	}
	
	private float      StoreOpenTimer;

	void Start()
	{
		_standDoorRigidbody = standDoorTransform.GetComponent<Rigidbody>();
		_alienCounter       = GameObjectExtensions.FindObjectsOfTypeWithInterface<IAlienCounter>()[0];
	}
	
	
    
	void Update()
	{
		switch (standState)
		{
			case StandState.Closed:
				break;
			case StandState.DoorOpening:
				// ;Debug.Log($"Linear Velocity: {_standDoorRigidbody.linearVelocity}\t]tPosition: {_standDoorRigidbody.position}");
				if (standDoorTransform.position.y > standDoorOpenTransform.position.y)
				{
					standState                        = StandState.Open;
					_standDoorRigidbody.linearVelocity = Vector3.zero;
					_audioSource.Stop();
					StoreOpenTimer = StoreOpenTime;
				}

				
				break;
			case StandState.Open:
				StoreOpenTimer -= Time.deltaTime;
				if (StoreOpenTimer < 0)
				{
					standState                         = StandState.DoorClosing;
					_standDoorRigidbody.linearVelocity = doorSpeed * -1 * Vector3.up;
					_audioSource.Play();
				}

				break;
			case StandState.DoorClosing:
				if (standDoorTransform.position.y < standDoorClosedTransform.position.y)
				{
					standState                        = StandState.Closed;
					_standDoorRigidbody.linearVelocity = Vector3.zero;
					_audioSource.Stop();
					
					_waveEngine.StartWave();
				}
				break;
			default:
				throw new Exception("Stand State not supported");
		}
	}

	
	public void AlienWaveEnded()
	{
		Debug.Log("------  Wave Ended  ------");
		_standDoorRigidbody.linearVelocity = doorSpeed * Vector3.up;
		standState                         = StandState.DoorOpening;
		_audioSource.Play();
	}
}
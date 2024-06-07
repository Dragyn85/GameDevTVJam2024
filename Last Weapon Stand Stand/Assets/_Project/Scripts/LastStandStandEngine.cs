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

	private Rigidbody               _standDoorRigidbody;
	IAlienCounter                   _alienCounter;
	private DHTDebugPanel_1_Service debugPanel;

	private PlayerController _playerController;
	
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
		debugPanel          = DHTServiceLocator.Get<DHTDebugPanel_1_Service>();
		_playerController   = GameObjectExtensions.FindObjectsOfTypeWithInterface<PlayerController>()[0];
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
					_audioSource.Stop();
					StoreOpenTimer = StoreOpenTime;
				}
				else
				{
					Vector3 deltaPosition = doorSpeed * Time.deltaTime * Vector3.up;
					
					debugPanel.SetElement(0, $"Door position: {standDoorTransform.position}", "");
					
					standDoorTransform.position += deltaPosition;
				}

				
				break;
			case StandState.Open:
				StoreOpenTimer -= Time.deltaTime;
				if (StoreOpenTimer < 0)
				{
					standState                         = StandState.DoorClosing;
					_audioSource.Play();
				}

				break;
			case StandState.DoorClosing:
				if (standDoorTransform.position.y < standDoorClosedTransform.position.y)
				{
					standState = StandState.Closed;
					_audioSource.Stop();
					_waveEngine.StartWave();
				}
				else
				{
					Vector3 velocity = doorSpeed * Vector3.up;
					standDoorTransform.position += -1 * Time.deltaTime * velocity;
				}
				break;
			default:
				throw new Exception("Stand State not supported");
		}
	}


	public void UpdateLeaderBoard()
	{
		Debug.Log("Update Leader Board");
		if (LeaderBoardManager.Instance)
		{
			LeaderBoardManager.Instance.AddScore(_playerController._score);
		}
	}

	
	public void AlienWaveEnded()
	{
		Debug.Log("------  Wave Ended  ------");
		UpdateLeaderBoard();
		standState                         = StandState.DoorOpening;
		_audioSource.Play();
	}
}
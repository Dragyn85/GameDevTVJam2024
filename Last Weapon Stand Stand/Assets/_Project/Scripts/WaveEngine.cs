using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class WaveEngine : MonoBehaviour
{
	[SerializeField] private float[] waveStartTimer;
	[SerializeField] private float[] waveLength;
	[SerializeField] private float[] minSpawnTime;
	[SerializeField] private float[] maxSpawnTime;

	[FormerlySerializedAs("WaveEnded")] public UnityEvent WaveEndedEvent    = new UnityEvent();
	public  UnityEvent LastWaveCompleted = new UnityEvent();
	IAlienCounter      _alienCounter;

	private AlianSpawner[] _spawners;
	private int            wave      = 0;
	private float 		   unclampedWave = 0;
	private float          waveTimer;

	internal enum WaveState
	{
		WaitingToStartWave,
		SpawningAliens,
		StoppedSpawingAliens,
		LasttWaveCompleted,
		InStore
	}

	private WaveState _waveState_ = WaveState.InStore;
	
	
	internal WaveState waveState
	{
		get { return _waveState_;}
		set
		{
			_waveState_ = value;
			Debug.Log($"---------------->  New Wave State = {value}");
		}
	}

	
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		wave          = 0;
		unclampedWave = 0;
		_spawners     = FindObjectsByType<AlianSpawner>(FindObjectsSortMode.None);
		_alienCounter = GameObjectExtensions.FindObjectsOfTypeWithInterface<IAlienCounter>()[0];

		StartWave();
	}


	void Update()
	{
		waveTimer += Time.deltaTime;

		switch (waveState)
		{
			case WaveState.InStore:
				break;
			case WaveState.WaitingToStartWave:
				if (waveTimer > waveStartTimer[wave])
				{
					Debug.Log($"Wave #{wave} started");
					SpawnersEnabled(true);
					waveState = WaveState.SpawningAliens;
				}

				break;
			case WaveState.SpawningAliens:
				var totalTime = waveStartTimer[wave] + waveLength[wave];
				if (waveTimer > totalTime)
				{
					Debug.Log($"Wave #{wave} ended");
					SpawnersEnabled(false);
					waveState = WaveState.StoppedSpawingAliens;
				}

				break;
			case WaveState.StoppedSpawingAliens:
				if (_alienCounter.Count == 0)
				{
					if (wave + 1 < waveLength.Length)
					{
						wave += 1;
					}

					unclampedWave++;
					
					waveState = WaveState.InStore;
					WaveEndedEvent.Invoke();
				}

				break;
			case WaveState.LasttWaveCompleted:
				break;
			default:
				throw new Exception($"Wave State '{waveState}' not supported");
		}
	}

	private void SpawnersEnabled(bool state)
	{
		foreach (var spawner in _spawners)
		{
			spawner.enabled = state;
		}
	}


	public void StartWave()
	{
		Debug.Log($"Waiting to Start Wave #{wave}");
		waveTimer        = 0;
		var nextMinSpawnTime = minSpawnTime[wave];
		var nextMaxSpawnTime = maxSpawnTime[wave];
		if (!(wave + 1 < waveLength.Length))
		{
			nextMinSpawnTime /= unclampedWave/4;
			nextMaxSpawnTime /= unclampedWave/4;
		}
		foreach (var spawner in _spawners)
		{
			spawner.minSpawnRate = nextMinSpawnTime;
			spawner.maxSpawnRate = nextMaxSpawnTime;
		}
		SpawnersEnabled(false);
		waveState = WaveState.WaitingToStartWave;
	}
}

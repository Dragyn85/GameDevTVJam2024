using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.Events;

public class AlienCounter : MonoBehaviour, IAlienCounter
{
	private int                           _count;
	private DHTDebugPanel_1_Service       _debugPanel;
	private IAlienCounterChangedHandler[] _alienCounterChangedHandlers;
	
	//public UnityEvent<int> CountChangeEvent = new UnityEvent<int>();
	public int Count
	{
		get { return _count; }
	}

	void Start()
	{
		_debugPanel                  = DHTServiceLocator.Get<DHTDebugPanel_1_Service>();
		_alienCounterChangedHandlers = GameObjectExtensions.FindObjectsOfTypeWithInterface<IAlienCounterChangedHandler>();
	}


	void Update()
	{

	}

	
	public void AdjustCount(int delta)
	{
		_count += delta;
		_debugPanel.SetElement(0, $"Alien count: {_count}");

		foreach (IAlienCounterChangedHandler alienCounterChangedHandler in _alienCounterChangedHandlers)
		{
			alienCounterChangedHandler.AlienCountChanged(_count);
		}
	}
}

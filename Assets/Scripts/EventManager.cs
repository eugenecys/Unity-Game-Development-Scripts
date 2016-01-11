using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EventManager : Singleton<EventManager> {

    public delegate void GameEvent();
    private float startTime;

    private Dictionary<int, List<TimedEvent>> _eventStore;
    private AVL<TimedEvent> _currentEventSet;
    public int defaultStage = 0;
    
    private TimedEvent _nextEvent;
    public int currentStage { get; private set; }

    public void registerEvent(GameEvent gameEvent, float delay, int stage)
    {
        TimedEvent evt = new TimedEvent(gameEvent, delay);
        List<TimedEvent> _eventSet = _eventStore[stage];
        if (_eventSet == null)
        {
            _eventSet = new List<TimedEvent>();
            _eventStore.Add(stage, _eventSet);
        }
        _eventSet.Add(evt);
    }

    public bool addEvent(GameEvent gameEvent, float delay)
    {
        if (_currentEventSet == null)
        {
            return false;
        }
        else
        {
            _currentEventSet.Add(new TimedEvent(gameEvent, delay));
            return true;
        }
    }

    void Awake()
    {
        _eventStore = new Dictionary<int, List<TimedEvent>>();
        
    }

	// Use this for initialization
	void Start () {
        //startGame();
	}

    public void startGame(int i)
    {
        goToStage(i);
    }

    // Triggers the game to start
    public void startGame()
    {
        goToStage(defaultStage);
    }

    public void goToStage(int stage)
    {
        _currentEventSet = retrieveStage(stage);
        Debug.Log("Proceeding to stage " + stage);
        Debug.Log("Event List Count: " + _currentEventSet.Count);
        StopAllCoroutines();
        currentStage = stage;
        startTime = Time.time;
        _nextEvent = null;
    }

    public void advanceStage()
    {
        goToStage(currentStage + 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentEventSet != null && _nextEvent == null && _currentEventSet.Count > 0)
        {
            _nextEvent = _currentEventSet.Pop();
        }

        while (_nextEvent != null && (_nextEvent.delay + startTime) < Time.time)
        {
            _nextEvent.timedEvent();
            _nextEvent = _currentEventSet.Pop();
        }
	}

    private AVL<TimedEvent> retrieveStage(int i)
    {
        List<TimedEvent> eventList = _eventStore[i];
        AVL<TimedEvent> eventSet = new AVL<TimedEvent>();
        foreach (TimedEvent evt in eventList)
        {
            eventSet.Add(evt);
        }
        return eventSet;
    }

    public class TimedEvent : IComparable<TimedEvent>{

        public GameEvent timedEvent { get; private set; }
        public float delay { get; private set; }
        public int priority { get; private set; }

        public TimedEvent()
        {
            timedEvent = null;
        }

        public TimedEvent(GameEvent evt, float time) 
        {
            timedEvent = evt;
            delay = time;
        }

        public int CompareTo(TimedEvent other)
        {
            if (delay == other.delay)
            {
                return priority.CompareTo(other.priority);
            }
            else
            {
                return delay.CompareTo(other.delay);
            }
        }

    }

}

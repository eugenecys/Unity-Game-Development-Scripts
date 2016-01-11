using UnityEngine;
using System.Collections;

public class MenuController : Singleton<MenuController> {

    public enum State
    {
        Tutorial,
        Start
    }

    private WorldController _worldController;

    public GameObject startActive;
    public GameObject startInactive;
    public GameObject tutorialActive;
    public GameObject tutorialInactive;
    public GameObject icon;

    private GameObject _menuObject;

    private bool _active;
    public bool active
    {
        get
        {
            return _active;
        }
        set
        {
            gameObject.SetActive(value);
            transform.localScale = new Vector3(11.6f, 11.6f, 12.2f);
            transform.position = new Vector3(4.4f, 14.8f, -6.69f);
            _active = value;
        }

    }

    private State _state;
    public State state
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
            setState(value);
        }
    }

    public void scroll()
    {
        switch (state)
        {
            case State.Tutorial:
                state = State.Start;
                break;
            case State.Start:
                state = State.Tutorial;
                break;
        }
    }

    public void select()
    {
        switch (state)
        {
            case State.Tutorial:
                active = false;
                _worldController.readyTutorial();
                break;
            case State.Start:
                active = false;
                _worldController.readyGame();
                break;
        }
    }

    public void setState(State state)
    {
        switch (state)
        {
            case State.Start:
                _selectStart();
                break;
            case State.Tutorial:
                _selectTutorial();
                break;
            default:
                _selectStart();
                break;

        }
    }

    private void _selectStart()
    {
        startActive.SetActive(true);
        startInactive.SetActive(false);
        tutorialActive.SetActive(false);
        tutorialInactive.SetActive(true);
        icon.transform.localPosition = new Vector3(-12, -3.5f, -0.1f);//(-3, -1.25f, -0.1f);
    }

    private void _selectTutorial()
    {
        startActive.SetActive(false);
        startInactive.SetActive(true);
        tutorialActive.SetActive(true);
        tutorialInactive.SetActive(false);
        icon.transform.localPosition = new Vector3(-12, 3.5f, -0.1f);//(-3, 0.25f, -0.1f);
    }

    void Awake()
    {
        _worldController = WorldController.Instance;
        _menuObject = gameObject;
        active = true;
    }

	// Use this for initialization
	void Start () {
        state = State.Start;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

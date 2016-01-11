using UnityEngine;
using System.Collections;

public class WorldController : Singleton<WorldController> {

    public enum GameState
    {
        //Title Screen
        Title,
        //Menu Screen
        Menu,
        //Player choice screen
        Idle,
        //Readying game state
        Ready, 
        //Game start 
        Start,
        //Game end
        End

    }

    public enum GameLevel
    {
        Tutorial,
        Game
    }

    private EventManager _eventManager;
    private SoundManager _soundManager;
    private InputController _inputController;
    private RhythmController _rhythmController;
    private AssetManager _assetManager;
    private MenuController _menuController;
    private TitleController _titleController;
    private Config _config;
    
    public Player[] players;
    private int _activePlayers;

    public GameState currentGameState { get; private set; }
    public GameLevel gameLevel { get; private set; }

    public TextMesh centerDisplay;
    private int _assetIndex;
    public float countdownTime = 3;
    private float _readyTime;
    private AudioSource _bgm;
    private GameObject _text;


    void Awake()
    {
        currentGameState = GameState.Title;
        
        _eventManager = EventManager.Instance;
        _soundManager = SoundManager.Instance;
        _inputController = InputController.Instance;
        _rhythmController = RhythmController.Instance;
        _assetManager = AssetManager.Instance;
        _menuController = MenuController.Instance;
        _titleController = TitleController.Instance;
        _config = Config.Instance;
        players = new Player[4];
        _bgm = GetComponent<AudioSource>();
        _loadSceneAssets();

    }

    public void registerPlayer(Player player)
    {
        players[player.index] = player;
        player.activateActor();
        _activePlayers = _getActivePlayerCount();
        Debug.Log("Registering: " + player.index);
        Debug.Log(player);
		//GameObject.Find ("Canvas").transform.FindChild ("Player" + player.index).gameObject.SetActive (true);
    }

    public void unregisterPlayer(Player player)
    {
        players[player.index] = null;
        player.deactivateActor();
        _activePlayers = _getActivePlayerCount();
        GameObject.Find("Scoreboard").transform.FindChild("player" + player.index).gameObject.SetActive(false);
        //GameObject.Find ("Canvas").transform.FindChild ("Player" + player.index).gameObject.SetActive (false);
    }

    public void readyTutorial()
    {
        StartCoroutine(_fadeBGMVolume(0.0f, 1.5f));
        _titleController.gameObject.SetActive(false);
        _menuController.transform.FindChild("Start").gameObject.SetActive(false);
        _menuController.transform.FindChild("Tutorial").gameObject.SetActive(false);
        _menuController.transform.FindChild("Crown").gameObject.SetActive(false);
        _assetIndex = 0;
        currentGameState = GameState.Idle;
        gameLevel = GameLevel.Tutorial;
    }

    public void readyGame()
    {
        StartCoroutine(_fadeBGMVolume(0.0f, 1.5f));
        _titleController.gameObject.SetActive(false);
        _menuController.transform.FindChild("Start").gameObject.SetActive(false);
        _menuController.transform.FindChild("Tutorial").gameObject.SetActive(false);
        _menuController.transform.FindChild("Crown").gameObject.SetActive(false);
        _assetIndex = 1;
        currentGameState = GameState.Idle;
        gameLevel = GameLevel.Game;
    }

    public void startGame()
    {
        StartCoroutine(_fadeBGMVolume(0.0f, 1f));
        _menuController.active = false;
        _titleController.active = false;

        centerDisplay.text = "";
        foreach (Player player in players)
        {
            Debug.Log(player);
            if (player != null)
            {
                player.setState(Player.PlayerState.Active);
            }
        }
        _assetManager.loadAssets(_assetIndex);
        _rhythmController.play();
    }

    public void changeTimeSpeed(float frac, float time)
    {
        StartCoroutine(timeChange(frac, time));
    }

    public void changeTimeSpeed(float frac)
    {
        Time.timeScale = frac;
    }

    IEnumerator timeChange(float frac, float time)
    {
        float delta = frac - Time.timeScale;
        while (Mathf.Abs(Time.timeScale - frac) > Mathf.Abs(delta * Time.deltaTime * 2))
        {
            Time.timeScale += delta * Time.deltaTime / time;
            yield return null;
        }
        Time.timeScale = frac;
    }

	// Use this for initialization
	void Start () 
    {
        StartCoroutine(_fadeBGMVolume(_config.backgroundVolume, 1f));
        _initializePlayers();
        _initializeAssets();
	}

    public void enterTitle()
    {
        StartCoroutine(_fadeBGMVolume(_config.backgroundVolume, 1f));
        _bgm.volume = 0.6f;
        currentGameState = GameState.Title;
        _titleController.active = true;
        _menuController.active = false;
    }

    public void enterMenu()
    {
        StartCoroutine(_fadeBGMVolume(_config.backgroundVolume/2, 1f));
        //_titleController.active = false;
        _menuController.active = true;
        currentGameState = GameState.Menu;
        Invoke("options", 1f);
    }

    private void options() {
        _menuController.transform.FindChild("Start").gameObject.SetActive(true);
        _menuController.transform.FindChild("Tutorial").gameObject.SetActive(true);
        _menuController.transform.FindChild("Crown").gameObject.SetActive(true);
        //_titleController.active = false;
    }

    // Update is called once per frame
    void Update () 
    {
        switch (currentGameState)
        {
            case GameState.Title:
                break;
            case GameState.Menu:
                break;
            case GameState.Idle:
                break;
            case GameState.Ready:
                if (_readyTime + countdownTime < Time.time)
                {
                    startGame();
                    currentGameState = GameState.Start;
                }
                else
                {
                    if (_activePlayers == 0)
                    {
                        centerDisplay.text = "";
                        currentGameState = GameState.Idle;
                    }
                    else
                    {
                        centerDisplay.text = Mathf.CeilToInt(countdownTime + _readyTime - Time.time).ToString();
                    }
                }

                break;
            case GameState.End:
                break;
            case GameState.Start:
                break;
        }
	}

    public void advanceStage()
    {
        //advance stage count
        _eventManager.advanceStage();
    }

    public void ready()
    {
        _readyTime = Time.time;
        currentGameState = GameState.Ready;
    }

    public void reset()
    {
        currentGameState = GameState.Idle;
        //_rhythmController._resetEnding();
    }

    public void addPlayer(Player player)
    {
        registerPlayer(player);

        if (currentGameState.Equals(GameState.Idle)) {
            ready();
        }
    }

    public void removePlayer(Player player)
    {
        unregisterPlayer(player);
    }

    private int _getActivePlayerCount()
    {
        int count = 0;
        foreach (Player player in players)
        {
            if (player != null)
            {
                count++;
            }
        }
        return count;
    }

    private void _initializePlayers()
    {
    }

    private void _loadSceneAssets()
    {
        _text = GameObject.Find("Countdown");
    }

    private void _initializeAssets()
    {

    }

    IEnumerator _fadeBGMVolume(float volume, float duration)
    {
        float currentVol = _bgm.volume;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            _bgm.volume = Mathf.Lerp(currentVol, volume, t);
            yield return null;
        }
    }

}

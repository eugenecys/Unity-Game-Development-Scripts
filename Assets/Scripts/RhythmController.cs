using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class RhythmController : Singleton<RhythmController> {

    public enum PlayState 
    {
        Playing,
        Idle
    }

    public enum HitStatus
    {
        Perfect,
		Great,
        Miss,
        Invalid
    }

    private PlayState _state;

    private SoundManager _soundManager;
    private WorldController _worldController;
    private ScoreController _scoreController;
    private Config _config; 

    public AudioClip activeSong { get; private set; }
    private AVL<Beat>[] _playerBeatList;

    public float bpm { get; private set; }
    public float offset { get; private set; }

    public const float BeatAllowance = 0.5f;
    public const float PerfectAllowance = 0.3f;
    public const float ValidAllowance = 1f;
    
    private float _startTime;
    private bool _startPlay;

    private int _previousBeat;

    private GameObject[][] iconPrefabs;
    private List<GameObject>[] icons;
    private AVL<Beat>[] _preBeats;

    private MovieTexture[] endings;
    private bool isEnding; // initialized in function setSong()
    private bool isReset;

    public Utility.Pair<float, HitStatus> beat(Player player, int buttonIndex)
    {
        // Calculates the distance to the nearest beat and determines if its appropriate
        float elapsedTime = _getElapsedTime();
        Debug.Log("Nearest offset: " + _getNearestOffset(elapsedTime));
        int playerIndex = player.index;
        if (_preBeats[playerIndex].Count > 0)
        {
            //Not correct yet. Need to find the corresponding player
            //Beat activeBeat = _playerBeatList[playerIndex][0];
            Beat activeBeat = _preBeats[playerIndex][0];
            float offsetDuration = activeBeat.delay - elapsedTime;
            if (activeBeat.beatIndex != buttonIndex)
            {
                if (Mathf.Abs(offsetDuration) < ValidAllowance)
                {
                    //_playerBeatList[playerIndex].Pop();
                    _preBeats[playerIndex].Pop();
                    Destroy(icons[playerIndex][0]);
                    icons[playerIndex].RemoveAt(0);
                    return new Utility.Pair<float, HitStatus>(offsetDuration, HitStatus.Miss);
                }
                else
                {
                    return new Utility.Pair<float, HitStatus>(-1, HitStatus.Invalid);
                }
            }

            if (Mathf.Abs(offsetDuration) < BeatAllowance * PerfectAllowance) 
            {
                //_playerBeatList[playerIndex].Pop();
                _preBeats[playerIndex].Pop();
                Destroy(icons[playerIndex][0]);
                icons[playerIndex].RemoveAt(0);
                _soundManager.playSound(_soundManager.playerTriggers[playerIndex, buttonIndex, activeBeat.type], buttonIndex + 2 + 4 * playerIndex);
                return new Utility.Pair<float, HitStatus>(offsetDuration, HitStatus.Perfect);
			} else if(Mathf.Abs(offsetDuration) >= BeatAllowance * PerfectAllowance && Mathf.Abs(offsetDuration) < BeatAllowance)
			{
                //_playerBeatList[playerIndex].Pop();
                _preBeats[playerIndex].Pop();
                Destroy(icons[playerIndex][0]);
                icons[playerIndex].RemoveAt(0);
                _soundManager.playSound(_soundManager.playerTriggers[playerIndex, buttonIndex, activeBeat.type], buttonIndex + 2 + 4 * playerIndex);
				return new Utility.Pair<float, HitStatus>(offsetDuration, HitStatus.Great);
            }
            else if (Mathf.Abs(offsetDuration) >= BeatAllowance && Mathf.Abs(offsetDuration) < ValidAllowance)
            {
                //_playerBeatList[playerIndex].Pop();
                _preBeats[playerIndex].Pop();
                Destroy(icons[playerIndex][0]);
                icons[playerIndex].RemoveAt(0);
                return new Utility.Pair<float,HitStatus>(offsetDuration, HitStatus.Miss);
            }
            else
            {
                return new Utility.Pair<float, HitStatus>(-1, HitStatus.Invalid);
            }
        } 
        else 
        {
            return new Utility.Pair<float,HitStatus>(-1, HitStatus.Invalid);
        }

    }

    void Awake()
    {
        _state = PlayState.Idle;
        _soundManager = SoundManager.Instance;
        _worldController = WorldController.Instance;
        _scoreController = ScoreController.Instance;
        _config = Config.Instance;
        _previousBeat = -1;
        _startPlay = false;
        _loadIcons();
        _playerBeatList = new AVL<Beat>[4];
        _preBeats = new AVL<Beat>[4];
        icons = new List<GameObject>[4];
        for(int i=0; i<4; i++)
        {
            icons[i] = new List<GameObject>();
        }
        endings = new MovieTexture[4];
        for (int i = 0; i < 4; i++)
        {
            endings[i] = Resources.Load("Art/Endings/ending_"+i) as MovieTexture;
        }
        isReset = false;
    }

    public void loadSong(AssetManager.Asset song)
    {
        Debug.Log("Loading song");
        activeSong = song.song;
        List<Beat> beatList = song.beatList;
        for (int i = 0; i < 4; i++)
        {
            _playerBeatList[i] = new AVL<Beat>();
            _preBeats[i] = new AVL<Beat>();
        }
        foreach (Beat beat in beatList)
        {
            _playerBeatList[beat.player].Add(beat);
        }
        
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(_playerBeatList[i].Count);
        }
        bpm = song.songBPM;
        offset = song.songOffset;
    }

    public void setSong(AudioClip song, float bpm, float offset)
    {
        activeSong = song;
        this.bpm = bpm;
        this.offset = offset;
    }

    public void play()
    {
        _startSong();
        _state = PlayState.Playing;
    }

    public void play(AudioClip song, float bpm, float offset)
    {
        setSong(song, bpm, offset);
        play();
    }

    public void pulse()
    {
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update() {
        switch (_state)
        {
            case PlayState.Idle:
                isEnding = false;
                _previousBeat = -1;
                isReset = false;
                break;
            case PlayState.Playing:
                switch(_worldController.gameLevel){
                    /***Game mode***/
                    case WorldController.GameLevel.Game:
                        {
                            float elapsedTime = _getElapsedTime();
                            int currentBeat = _getCurrentBeat(elapsedTime);
                            if (((elapsedTime - offset) % (60f / bpm)) < 0.05f)
                            {
                                if (_previousBeat != currentBeat)
                                {
                                    _previousBeat = currentBeat;
                                    pulse();
                                }
                            }

                            for (int i = 0; i < 4; i++)
                            {
                                if (_worldController.players[i] != null)
                                {
                                    if (_playerBeatList[i] != null && _playerBeatList[i].Count > 0)
                                    {
                                        Beat activeBeat = _playerBeatList[i][0];
                                        if (activeBeat.delay - elapsedTime < 1f)
                                        {
                                            Beat newBeat = _playerBeatList[i].Pop();
                                            _preBeats[i].Add(newBeat);
                                            _popIcons(newBeat);
                                        }
                                    }


                                    if (_preBeats[i].Count > 0)
                                    {
                                        Beat latestBeat = _preBeats[i][0];
                                        if (elapsedTime - latestBeat.delay > BeatAllowance)
                                        {
                                            Beat missedBeat = _preBeats[i].Pop();
                                            Destroy(icons[i][0]);
                                            icons[i].RemoveAt(0);
                                            //Debug.Log("Missed Beat: " + latestBeat);
                                            _worldController.players[i].missBeat();
                                        }
                                    }

                                }
                            }
                            for (int i = 0; i < 4; i++)
                            {
                                Debug.Log("player" + i + ": " + _playerBeatList[i].Count + "+" + _preBeats[i].Count);
                                if (_worldController.players[i] != null)
                                {
                                    if (_playerBeatList[i].Count > 0 || _preBeats[i].Count > 0) break;
                                    if (!isEnding) {
                                        isEnding = true; Invoke("_playEnding", 2f); 
                                    }
                                }
                            }
                            _popCrown();
                            break;
                        }
                        /***game mode ends***/
                    /***tutorial mode***/
                    case WorldController.GameLevel.Tutorial:
                        {
                            int flag = 1;
                            for (int j = 0; j < 4; j++)
                            {
                                if (_worldController.players[j] != null) flag *= _scoreController.scores[j];
                            }
                            if (flag != 0 && !isReset)
                            {
                                GameObject.Find("Tutorial").GetComponent<SpriteRenderer>().enabled = true;
                                isReset = true;
                                for (int i = 0; i < 4; i++) {
                                    _playerBeatList[i].Clear();
                                    _preBeats[i].Clear();
                                }
                                foreach (Transform icon in GameObject.Find("Icon").transform) {
                                    Destroy(icon.gameObject);
                                }
                                Invoke("reset", 5f);
                            }
                            float elapsedTime_t = _getElapsedTime();
                            int currentBeat_t = _getCurrentBeat(elapsedTime_t);
                            if (((elapsedTime_t - offset) % (60f / bpm)) < 0.05f)
                            {
                                if (_previousBeat != currentBeat_t)
                                {
                                    _previousBeat = currentBeat_t;
                                    pulse();
                                }
                            }
                            for (int i = 0; i < 4; i++)
                            {
                                
                                if (_worldController.players[i] != null)
                                {
                                    //Debug.Log(i+":"+_playerBeatList[i].Count + "+" + _preBeats[i].Count);
                                    if (_playerBeatList[i] != null && _playerBeatList[i].Count > 0)
                                    {
                                        Beat activeBeat = _playerBeatList[i][0];
                                        Debug.Log("player" + i + ": " + (activeBeat.delay - elapsedTime_t));
                                        if (activeBeat.delay - elapsedTime_t < 1f)
                                        {
                                            Beat newBeat = _playerBeatList[i].Pop();
                                            if (icons[i].Count == 0 || _scoreController.scores[i] > 0)
                                            { _popIcons(newBeat); _preBeats[i].Add(newBeat); }
                                        }
                                    }
                                    if (_preBeats[i].Count > 0)
                                    {
                                        Beat latestBeat = _preBeats[i][0];
                                        if (_scoreController.scores[i] == 0)
                                        {
                                            if (latestBeat.delay - elapsedTime_t < 0.2f)
                                            {
                                                icons[i][0].GetComponent<Animator>().speed = 0.5f;
                                            }
                                        }
                                        if (elapsedTime_t - latestBeat.delay > BeatAllowance * 2)
                                        {
                                            Beat missedBeat = _preBeats[i].Pop();
                                            Destroy(icons[i][0]);
                                            icons[i].RemoveAt(0);
                                            _worldController.players[i].missBeat();
                                        }
                                    }

                                }
                            }
                            for (int i = 0; i < 4; i++)
                            {
                                //Debug.Log("player" + i + ": " + _playerBeatList[i].Count + "+" + _preBeats[i].Count);
                                if (_worldController.players[i] != null)
                                {
                                    if (_playerBeatList[i].Count > 0 || _preBeats[i].Count > 0) break;
                                    if(!isReset) reset();
                                }
                            }
                            break;
                        }
                        /***tutorial mode ends***/
                }
                break;
        }

    }

    private void reset() {
        isReset = true;
        Debug.Log("reset");
        GameObject.Find("Tutorial").GetComponent<SpriteRenderer>().enabled = false;
        foreach (Transform crown in GameObject.Find("Crowns").transform)
        {
            crown.gameObject.SetActive(false);
            Debug.Log("setfalse");
        }
        for (int j = 0; j < 4; j++)
        {
            _playerBeatList[j].Clear();
            _preBeats[j].Clear();
            icons[j].Clear();
            if (_worldController.players[j] != null)
            {
                _worldController.players[j].setState(Player.PlayerState.Inactive);
                _worldController.removePlayer(_worldController.players[j]);
            }
        }
        foreach(Transform existIcon in GameObject.Find("Icon").transform)
        {
            Destroy(existIcon.gameObject);
        }
        _endSong();
        _state = PlayState.Idle;
        _scoreController.reset();
        _soundManager.stopAllSounds();
        _worldController.reset();
        _worldController.enterTitle();

    }

    private void _playEnding() {
        Debug.Log("That's the end;");
        int maxScore = 0;
        int winner = 0;
        for (int i = 0; i < 4; i++) {
            if (_worldController.players[i] != null) winner = i; // if maxscore == 0
        }
        for (int i = 0; i < 4; i++)
        {
            if (_scoreController.scores[i] > maxScore)
            {
                maxScore = _scoreController.scores[i];
                winner = i;
            }
        }
        GameObject scoreboard = GameObject.Find("Scoreboard");
        GameObject screen = scoreboard.transform.FindChild("ending").gameObject;
        if (!screen.activeSelf) screen.SetActive(true);
        screen.GetComponent<RawImage>().texture = endings[winner] as MovieTexture;
        screen.GetComponent<AudioSource>().clip = endings[winner].audioClip;
        if (!endings[winner].isPlaying)
        {
            endings[winner].Play();
            screen.GetComponent<AudioSource>().Play();
        }
        Invoke("_resetEnding", 15f);
    }

    private void _resetEnding() {
        /**reset ending video**/
        isEnding = false;
        if (GameObject.Find("Scoreboard/ending") != null)
            GameObject.Find("Scoreboard/ending").SetActive(false);
        foreach (Transform crown in GameObject.Find("Crowns").transform)
        {
            crown.gameObject.SetActive(false);
            Debug.Log("setfalse");
        }
        /*_worldController.reset();
        for (int i = 0; i < 4; i++) {
            if (_worldController.players[i] != null) _worldController.removePlayer(_worldController.players[i]);
        }*/
        /*_state = PlayState.Idle;
        _worldController.enterMenu();*/
        reset();
    }

    private void _startSong()
    {
        _startTime = Time.time;
        _startPlay = true;
        _soundManager.playSound(activeSong, 1, _config.trackVolume);
        //Debug.Log("Playing " + activeSong.name + ", offset: " + offset + ", bpm: " + bpm);
    }

    private void _endSong()
    {
        _startPlay = false;
        _state = PlayState.Idle;
    }

    private Utility.Pair<float, int> _getNearestOffset(float time)
    {
        float beatDuration = 60f / bpm;
        int beat = _getCurrentBeat(time);
        float beatTime = beat * beatDuration + offset;
        return new Utility.Pair<float, int>(time - beatTime, beat);
    }

    private int _getCurrentBeat(float time)
    {
        float beatDuration = 60f / bpm;
        return Mathf.RoundToInt((time - offset) / beatDuration);
    }

    private float _getElapsedTime()
    {
        return Time.time - _startTime;
    }

    public class Beat : IComparable<Beat>
    {
        public enum Timing 
        {
            Whole,
            Half, 
            Quarter, 
            Eighth,
            Sixteenth
        }

        public float delay { get; private set; }
        public int player { get; private set; }
        public int beatIndex { get; private set; }
        public Timing timing { get; private set; }
        public int type { get; private set; }

        public Beat(int player_, float delay_, int beatIndex_, int type_)
        {
            delay = delay_;
            player = player_;
            beatIndex = beatIndex_;
            type = type_;
        }

        public Beat(int player_, float delay_, int beatIndex_, Timing timing_)
        {
            delay = delay_;
            player = player_;
            beatIndex = beatIndex_;
            timing = timing_;

        }

        public override string ToString()
        {
            return "Player: " + player + ", delay: " + delay + ", index: " + beatIndex + ", type: " + type.ToString();
        }

        public int CompareTo(Beat other)
        {
            return delay.CompareTo(other.delay);
        }
    }

    public static float GetDelay(float bpm, float offset, float beatCount)
    {
        float beatDuration = 60f / bpm;
        return beatDuration * beatCount + offset;
    }

    private void _loadIcons() {
        iconPrefabs = new GameObject[4][];
        for(int i=0; i<4; i++)
        {
            iconPrefabs[i] = new GameObject[2];
            for (int j=0; j < 2; j++) {
                iconPrefabs[i][j] = Resources.Load("Art/Icons/Prefabs/" + (i * 10 + j)) as GameObject;
            }
        }
    }

    private void _popIcons(Beat beat)
    {
        GameObject icon = Instantiate(iconPrefabs[beat.player][beat.beatIndex]);
        icon.transform.parent = GameObject.Find("Icon").transform;
        icon.GetComponent<Animator>().Play("player" + beat.player);
        icons[beat.player].Add(icon);
    }

    private void _popCrown() {
        GameObject crowns = GameObject.Find("Crowns");
        int max = Mathf.Max(_scoreController.scores);
        for (int i = 0; i < 4; i++)
        {
            if (max>0 && _scoreController.scores[i] >= max)
                crowns.transform.FindChild("Crown" + i).gameObject.SetActive(true);
            else
                crowns.transform.FindChild("Crown" + i).gameObject.SetActive(false);
        }
    }
}

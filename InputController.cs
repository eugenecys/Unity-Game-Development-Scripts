using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputController : Singleton<InputController> {

    private Dictionary<KeyCode, InputTrigger> _keyMap;

    public delegate void InputTrigger();

    void Awake()
    {
        _keyMap = new Dictionary<KeyCode, InputTrigger>();
    }

    public void registerTrigger(InputTrigger trigger, KeyCode key)
    {
        _keyMap.Add(key, trigger);
    }

    // Use this for initialization
    void Start()
    {
	
	}

    public KeyCode[] parseKeys(string[] keys)
    {
        KeyCode[] codes = new KeyCode[keys.Length];
        for(int i = 0; i < keys.Length; i++)
        {
            codes[i] = parseString(keys[i]);
        }
        return codes;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q) && _keyMap.ContainsKey(KeyCode.Q))
        {
            _keyMap[KeyCode.Q]();
        }
        if (Input.GetKeyDown(KeyCode.W) && _keyMap.ContainsKey(KeyCode.W))
        {
            _keyMap[KeyCode.W]();
        }
        if (Input.GetKeyDown(KeyCode.E) && _keyMap.ContainsKey(KeyCode.E))
        {
            _keyMap[KeyCode.E]();
        }
        if (Input.GetKeyDown(KeyCode.R) && _keyMap.ContainsKey(KeyCode.R))
        {
            _keyMap[KeyCode.R]();
        }
        if (Input.GetKeyDown(KeyCode.T) && _keyMap.ContainsKey(KeyCode.T))
        {
            _keyMap[KeyCode.T]();
        }
        if (Input.GetKeyDown(KeyCode.Y) && _keyMap.ContainsKey(KeyCode.Y))
        {
            _keyMap[KeyCode.Y]();
        }
        if (Input.GetKeyDown(KeyCode.U) && _keyMap.ContainsKey(KeyCode.U))
        {
            _keyMap[KeyCode.U]();
        }
        if (Input.GetKeyDown(KeyCode.I) && _keyMap.ContainsKey(KeyCode.I))
        {
            _keyMap[KeyCode.I]();
        }
        if (Input.GetKeyDown(KeyCode.O) && _keyMap.ContainsKey(KeyCode.O))
        {
            _keyMap[KeyCode.O]();
        }
        if (Input.GetKeyDown(KeyCode.P) && _keyMap.ContainsKey(KeyCode.P))
        {
            _keyMap[KeyCode.P]();
        }
        if (Input.GetKeyDown(KeyCode.A) && _keyMap.ContainsKey(KeyCode.A))
        {
            _keyMap[KeyCode.A]();
        }
        if (Input.GetKeyDown(KeyCode.S) && _keyMap.ContainsKey(KeyCode.S))
        {
            _keyMap[KeyCode.S]();
        }
        if (Input.GetKeyDown(KeyCode.D) && _keyMap.ContainsKey(KeyCode.D))
        {
            _keyMap[KeyCode.D]();
        }
        if (Input.GetKeyDown(KeyCode.F) && _keyMap.ContainsKey(KeyCode.F))
        {
            _keyMap[KeyCode.F]();
        }
        if (Input.GetKeyDown(KeyCode.G) && _keyMap.ContainsKey(KeyCode.G))
        {
            _keyMap[KeyCode.G]();
        }
        if (Input.GetKeyDown(KeyCode.H) && _keyMap.ContainsKey(KeyCode.H))
        {
            _keyMap[KeyCode.H]();
        }
        if (Input.GetKeyDown(KeyCode.J) && _keyMap.ContainsKey(KeyCode.J))
        {
            _keyMap[KeyCode.J]();
        }
        if (Input.GetKeyDown(KeyCode.K) && _keyMap.ContainsKey(KeyCode.K))
        {
            _keyMap[KeyCode.K]();
        }
        if (Input.GetKeyDown(KeyCode.L) && _keyMap.ContainsKey(KeyCode.L))
        {
            _keyMap[KeyCode.L]();
        }
        if (Input.GetKeyDown(KeyCode.Z) && _keyMap.ContainsKey(KeyCode.Z))
        {
            _keyMap[KeyCode.Z]();
        }
        if (Input.GetKeyDown(KeyCode.X) && _keyMap.ContainsKey(KeyCode.X))
        {
            _keyMap[KeyCode.X]();
        }
        if (Input.GetKeyDown(KeyCode.C) && _keyMap.ContainsKey(KeyCode.C))
        {
            _keyMap[KeyCode.C]();
        }
        if (Input.GetKeyDown(KeyCode.V) && _keyMap.ContainsKey(KeyCode.V))
        {
            _keyMap[KeyCode.V]();
        }
        if (Input.GetKeyDown(KeyCode.B) && _keyMap.ContainsKey(KeyCode.B))
        {
            _keyMap[KeyCode.B]();
        }
        if (Input.GetKeyDown(KeyCode.N) && _keyMap.ContainsKey(KeyCode.N))
        {
            _keyMap[KeyCode.N]();
        }
        if (Input.GetKeyDown(KeyCode.M) && _keyMap.ContainsKey(KeyCode.M))
        {
            _keyMap[KeyCode.M]();
		}
        if (Input.GetKeyDown(KeyCode.Space) && _keyMap.ContainsKey(KeyCode.Space))
		{
			_keyMap[KeyCode.Space]();
		}
        if (Input.GetKeyDown(KeyCode.Return) && _keyMap.ContainsKey(KeyCode.Return))
		{
			_keyMap[KeyCode.Return]();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && _keyMap.ContainsKey(KeyCode.Escape))
        {
            _keyMap[KeyCode.Escape]();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && _keyMap.ContainsKey(KeyCode.DownArrow))
        {
            _keyMap[KeyCode.DownArrow]();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && _keyMap.ContainsKey(KeyCode.UpArrow))
        {
            _keyMap[KeyCode.UpArrow]();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && _keyMap.ContainsKey(KeyCode.LeftArrow))        
        {
            _keyMap[KeyCode.LeftArrow]();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && _keyMap.ContainsKey(KeyCode.RightArrow))
        {
            _keyMap[KeyCode.RightArrow]();
        }
        if (Input.GetMouseButtonDown(1) && _keyMap.ContainsKey(KeyCode.Mouse1))
        {
            _keyMap[KeyCode.Mouse1]();
        }
        if (Input.GetMouseButtonDown(0) && _keyMap.ContainsKey(KeyCode.Mouse0))
        {
            _keyMap[KeyCode.Mouse0]();
        }
	}

    public KeyCode parseString(string key)
    {
        switch (key)
        {
            case "Q":
            case "q":
                return KeyCode.Q;
            case "W":
            case "w":
                return KeyCode.W;
            case "E":
            case "e":
                return KeyCode.E;
            case "R":
            case "r":
                return KeyCode.R;
            case "T":
            case "t":
                return KeyCode.T;
            case "Y":
            case "y":
                return KeyCode.Y;
            case "u":
            case "U":
                return KeyCode.U;
            case "I":
            case "i":
                return KeyCode.I;
            case "O":
            case "o":
                return KeyCode.O;
            case "P":
            case "p":
                return KeyCode.P;
            case "A":
            case "a":
                return KeyCode.A;
            case "S":
            case "s":
                return KeyCode.S;
            case "D":
            case "d":
                return KeyCode.D;
            case "F":
            case "f":
                return KeyCode.F;
            case "G":
            case "g":
                return KeyCode.G;
            case "H":
            case "h":
                return KeyCode.H;
            case "J":
            case "j":
                return KeyCode.J;
            case "K":
            case "k":
                return KeyCode.K;
            case "L":
            case "l":
                return KeyCode.L;
            case "Z":
            case "z":
                return KeyCode.Z;
            case "X":
            case "x":
                return KeyCode.X;
            case "C":
            case "c":
                return KeyCode.C;
            case "V":
            case "v":
                return KeyCode.V;
            case "B":
            case "b":
                return KeyCode.B;
            case "N":
            case "n":
                return KeyCode.N;
            case "m":
            case "M":
                return KeyCode.M;
            case "DOWN":
            case "Down":
            case "down":
                return KeyCode.DownArrow;
            case "UP":
            case "Up":
            case "up":
                return KeyCode.UpArrow;
            case "LEFT":
            case "Left":
            case "left":
                return KeyCode.LeftArrow;
            case "RIGHT":
            case "Right":
            case "right":
                return KeyCode.RightArrow;
            case "ENTER":
            case "enter":
            case "Enter":
            case "RETURN":
            case "return":
            case "Return":
                return KeyCode.Return;
            case "Spacebar":
            case "SPACEBAR":
            case "spacebar":
            case "SPACE":
            case "space":
            case "Space":
                return KeyCode.Space;
            case "ESCAPE":
            case "Escape":
            case "escape":
            case "ESC":
            case "esc":
            case "Esc":
                return KeyCode.Escape;
            case "MOUSE RIGHT":
            case "Mouse Right":
            case "mouse right":
                return KeyCode.Mouse1;
            case "MOUSE LEFT":
            case "Mouse Left":
            case "mouse left":
                return KeyCode.Mouse0;
            default:
                return KeyCode.Q;
        }
    }
}

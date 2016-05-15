using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;

public class Config : Singleton<Config> {
    public string filename = "config.ini";

    private InputController _inputController;

    void Awake()
    {
        //Initialize Singletons
        _inputController = InputController.Instance;

        loadFile(filename);
        // Other stuff 
    }

    void loadFile(string filename)
    {
        if (!File.Exists(filename))
        {
            File.CreateText(filename);
            return;
        }

        try
        {
            string line;
            StreamReader sReader = new StreamReader(filename, Encoding.Default);
            do
            {
                line = sReader.ReadLine();
                if (line != null)
                {
                    // Lines with # are for comments
                    if (!line.Contains("#"))
                    {
                        // Value property identified by string before the colon.
                        string[] data = line.Split(':');
                        if (data.Length == 2)
                        {
                            string val = data[1].Trim();
                            string key = data[0].Trim().ToLower();
                            switch (key)
                            {
                                case "player count":
                                    int count = int.Parse(val);
                                    //Do stuff
                                    break;
                                case "ip address":
                                    string address = val;
                                    //Do stuff
                                    break;
                                case "port":
                                    int port = int.Parse(val);
                                    //Do stuff
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            while (line != null);
            sReader.Close();
            return;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}

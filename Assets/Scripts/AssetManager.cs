using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetManager : Singleton<AssetManager> {

    private Dictionary<string, Asset> _assetStore;

    void Awake()
    {
        _assetStore = new Dictionary<string, Asset>();
    }

    public void saveAsset(Asset asset)
    {
        _assetStore.Add(asset.label, asset);
    }

    public void loadAssets(string label)
    {
        Asset asset = _assetStore[label].Clone();
    }

    public Asset getAsset(string label)
    {
        return _assetStore[label].Clone();
    }

    // Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public class Asset : ICloneable
    {
        public string label;
        public Asset(string label)
        {
            
        }

        public Asset Clone()
        {
            Asset assetClone = new Asset(label);
            return assetClone;
        }
    }
}

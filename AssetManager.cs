using UnityEngine;
using UnityEditor;

public class AssetManager : Singleton<AssetManager>
{
    public GameObject[] assetSources;

    public void findAllAssets()
    {
        foreach(GameObject assetSource in assetSources)
        {
            int linkCount = 0;
            Asset[] assets = assetSource.GetInterfacesInChildren<Asset>();
            foreach(Asset asset in assets)
            {
                if (asset.Link())
                {
                    linkCount++;
                }
            }
            Debug.Log(linkCount + " Assets linked in source: " + assetSource.name);       
        }
    }
}
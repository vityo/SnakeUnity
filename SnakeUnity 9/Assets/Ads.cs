using UnityEngine;
using UnityEngine.Advertisements;

public class Ads : MonoBehaviour
{
    static public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }
}
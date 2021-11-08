using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsHelper : MonoBehaviour
{

    string gameID = "2585694";

    // Use this for initialization
    void Start()
    {
        Advertisement.Show();

        if (Advertisement.isSupported)
            Advertisement.Initialize(gameID, true);

//        ShowRewardedVideo();

        StartCoroutine(AdbCoroutine());
    }


    IEnumerator AdbCoroutine()
    {
        while (!Advertisement.IsReady("rewardedVideo"))
        {
            Debug.Log("rewareded Load...");
            yield return null;
        }

        ShowRewardedVideo();
    }

    void ShowRewardedVideo()
    {
        ShowOptions opts = new ShowOptions();
        opts.resultCallback = HandleShowResult;

        Advertisement.Show("rewardedVideo", opts);
    }


    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("비디오 완료");
        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("비디오 스킵");
        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("비디오 보기 실패");
        }
    }

}

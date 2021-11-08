using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using GooglePlayGames.BasicApi;
//using GooglePlayGames;
//using UnityEngine.SocialPlatforms;

public class UIMain : MonoBehaviour
{

    [SerializeField]
    Text m_versionText;

    [SerializeField]
    Text m_offlineText;

    [SerializeField]
    GameObject m_loadObj;

    [SerializeField]
    Text m_msgText;

    [SerializeField]
    GameObject m_textObj;

    [SerializeField]
    GameObject m_reconnectObj;

    bool isLogin = false;
    bool isLoad = false;
    bool isSave = false;

    const float m_waitTime = 5f;
    Button m_button;

    void Awake()
    {
        Camera.main.ResetAspect();
        Screen.SetResolution(Screen.width, Screen.width * 16 / 9, true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        m_versionText.text = Application.version;

        m_offlineText.text = "";

        m_button = GetComponent<Button>();
        m_button.interactable = false;
        m_reconnectObj.SetActive(false);
        m_textObj.SetActive(false);
        m_loadObj.SetActive(true);

        FriendManager.GetInstance.initInstance();
        ParticleManager.GetInstance.initInstance();
        SoundManager.GetInstance.initInstance();

        gameObject.AddComponent<SoundPlay>();
        gameObject.GetComponent<SoundPlay>().audioPlay("BGMMain", TYPE_SOUND.BGM, true);

        PrepClass.isBGM = (PlayerPrefs.GetInt("isBGM", 1) == 1) ? true : false;
        PrepClass.isEffect = (PlayerPrefs.GetInt("isEffect", 1) == 1) ? true : false;

        SoundManager.GetInstance.setMute(TYPE_SOUND.EFFECT);
        SoundManager.GetInstance.setMute(TYPE_SOUND.BGM);

        //        SoundManager.GetInstance.setMute(TYPE_SOUND.VOICE);
    }

    void Start()
    {
        OnReconnectedClicked();

        FirebaseAPI.FirebaseApi.GetInstance.initInstance();
    }


    IEnumerator waitCoroutine()
    {
        float time = 0f;
        msgCallback("로그인 중...");
        while (m_waitTime >= time)
        {
            
            if(isLogin)
                break;

            time += PrepClass.frameTime;
            yield return new WaitForSeconds(PrepClass.frameTime);
        }



        if (isLogin)
        {
            //로그인 완료
            Debug.Log("로그인 성공");
            msgCallback("로그인 성공!");
            //구글 내 데이터 가져오기
            //다음 진행 패널 활성화
            loadPanel();
        }
        else
        {
            //재접속 여부
            Debug.Log("로그인 실패 - 오프라인으로 전환");
            msgCallback("로그인 실패...");
            m_reconnectObj.SetActive(true);
        }

    }


    IEnumerator loadCoroutine()
    {

        float time = 0f;
        while (m_waitTime >= time)
        {
            if (isLoad)
                break;

            time += PrepClass.frameTime;
            yield return new WaitForSeconds(PrepClass.frameTime);
        }


        if (isLoad)
        {
            //구글에서 열기 성공
            msgCallback("불러오기 완료");
            Debug.Log("로드 성공");
            //진행 버튼 활성화
        }
        else
        {
            Debug.Log("로드 실패");
            //파일에서 불러오기
            msgCallback("파일 불러오는 중");

            //오프라인이 아닐때 로드를 실패했으면
            //파일 불러오기
            if(!PrepClass.isOffline)
                Account.GetInstance.loadAccount(true, fileCallback, msgCallback);

            //로그인 되어있으면 현재 데이터 저장하기
            if (isLogin)
            {
                yield return StartCoroutine(saveCoroutine());
                Account.GetInstance.saveAccount(PrepClass.isOffline, saveCallback, msgCallback);
//                GoogleApi.GetInstance.openSavedGame("friend", TYPE_FILE_ACCESS.Save, saveCallback, msgCallback);
            }

        }


        activePanel();

    }

    IEnumerator saveCoroutine()
    {

        float time = 0f;
        while (m_waitTime >= time)
        {
            if (isSave)
                break;

            time += PrepClass.frameTime;
            yield return new WaitForSeconds(PrepClass.frameTime);
        }


        if (isSave)
        {
            //구글에서 열기 성공
            msgCallback("저장 완료");
            Debug.Log("저장 성공");
            //진행 버튼 활성화
        }
        else
        {
            Debug.Log("저장 실패");
            //파일에서 불러오기
            //다시 시작하라는 메시지를 보내야 함
        }
    }


    


    void fileCallback(bool isSuccess)
    {
        isLoad = isSuccess;
        if(isLoad)
            msgCallback("파일 불러오기 완료");
        else
            msgCallback("새 파일 불러오기 완료");

    }


    void loadCallback(bool isSuccess)
    {
        isLoad = isSuccess;
    }

    void saveCallback(bool isSuccess)
    {
        if(isSuccess)
            msgCallback("구글 저장 완료");
        else
            msgCallback("파일 저장 완료");
    }

    void msgCallback(string msg)
    {

        Debug.Log("메시지 : " + msg);
        m_msgText.text = msg;
        //데이터 불러오기
    }


    void loginCallback(bool success)
    {
        Debug.Log("로그인 : " + success);
//        m_isLogin = true;
        isLogin = success;
        PrepClass.isOffline = !success;
    }


    public void OnPanelClicked()
    {
        Account.GetInstance.nextScene = "Game@Lobby";
        SceneManager.LoadScene("Game@Load");
    }

    public void OnOfflineClicked()
    {
        //오프라인 모드
        PrepClass.isOffline = true;

        //폰에 저장된 데이터 가져오기
        //파일 내 데이터 가져오기
        m_offlineText.text = "Offline Mode";

        Debug.Log("오프라인 모드 전환");
        loadPanel();

        m_reconnectObj.SetActive(false);

    }

    public void OnReconnectedClicked()
    {
        //재접속 시도
        m_reconnectObj.SetActive(false);
        GoogleAPI.GoogleApi.GetInstance.setLogin(loginCallback);
        StartCoroutine(waitCoroutine());
    }

    void loadPanel()
    {
        //계정 데이터 불러오기
        Account.GetInstance.loadAccount(PrepClass.isOffline, loadCallback, msgCallback);
        StartCoroutine(loadCoroutine());
    }

    void activePanel()
    {
        m_textObj.SetActive(true);
        m_loadObj.SetActive(false);
        m_button.interactable = true;

    }
}


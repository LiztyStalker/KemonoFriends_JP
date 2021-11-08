using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using GamePanel;

public class UIResult : MonoBehaviour
{

    UIPlayer m_uiPlayer;

    [SerializeField]
    Text m_nowScoreText;

    [SerializeField]
    Image m_nowFriendImage;

    [SerializeField]
    Image m_adventureEndImage;


    [SerializeField]
    UIResultPanel m_uiResultPanel;

    [SerializeField]
    UITotalPanel m_uiTotalPanel;

    [SerializeField]
    Button m_lobbyButton;

    [SerializeField]
    Button m_retryButton;

    [SerializeField]
    Text m_retryCostText;

    [SerializeField]
    GameObject m_savePanel;

    [SerializeField]
    Text m_saveText;
    
    Field m_field;

    SoundPlay m_soundPlayer;

    bool isSave = false;
    Coroutine m_coroutine;


    int cost = 0;

    public bool isTotalPanel { get { return m_uiTotalPanel.isActiveAndEnabled; } }

    void Start()
    {
        m_savePanel.SetActive(false);
        m_retryButton.onClick.AddListener(() => OnRetryClick());
        m_uiResultPanel.gameObject.SetActive(true);
        m_uiTotalPanel.gameObject.SetActive(false);
        m_lobbyButton.gameObject.SetActive(false);
        m_retryButton.gameObject.SetActive(false);

        //1.6 게임이 종료되면 참여횟수 증가
        Account.GetInstance.getFriend(Account.GetInstance.friendKey).addCount();

        m_soundPlayer = GetComponent<SoundPlay>();
        if (m_soundPlayer == null)
            m_soundPlayer = gameObject.AddComponent<SoundPlay>();
    }

    public void setParent(UIPlayer uiPlayer)
    {
        m_uiPlayer = uiPlayer;
    }

    public void uiUpdate(Field field)
    {
        m_field = field;
        m_nowScoreText.text = string.Format("{0:d6}", field.score);
        m_nowFriendImage.sprite = field.friend.icon;

        if(field.isDefeat)
            m_adventureEndImage.sprite = field.adventure.defeatImage;
        else
            m_adventureEndImage.sprite = field.adventure.clearImage;


        m_uiResultPanel.uiUpdate(field);
    }

    public void OnRetryClick()
    {
        //총합 데이터 보이기
        if (!m_uiTotalPanel.gameObject.activeSelf)
        {

            //프렌즈 모집 재 초기화
            FriendWantedManager.GetInstance.listWanted();


            cost = m_field.adventure.cost;
            cost += ItemManager.GetInstance.getItem(Account.GetInstance.itemKey).cost;
            m_retryCostText.text = string.Format("{0}", -(cost));

            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
            m_uiTotalPanel.gameObject.SetActive(true);
            m_uiTotalPanel.uiUpdate(m_field, m_uiResultPanel.addBread);

            saveData();



            //빵 소비량
            //아이템 + 모험
        }
        //재시작
        else
        {
            //빵 소비
            if (Account.GetInstance.checkJapariBread(cost))
            {
                m_soundPlayer.audioPlay("EffectStart", TYPE_SOUND.EFFECT);
//                m_retryCostText.text = string.Format("{0}", -(cost));
                Account.GetInstance.nextScene = "Game@Play";

                SceneManager.LoadScene("Game@Load");
            }
            else
            {
                m_soundPlayer.audioPlay("EffectError", TYPE_SOUND.EFFECT);
                m_uiPlayer.uiMsg.setMsg("자파리빵이 부족합니다.", TYPE_MSG_PANEL.WARNING, TYPE_MSG_BTN.OK);
            }

        }
    }

    //public void adBtnClick()
    //{

    //}

    public void OnLobbyClick()
    {
        m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
        Account.GetInstance.nextScene = "Game@Lobby";
        Account.GetInstance.friendKey = "";
        SceneManager.LoadScene("Game@Load");
    }

    #region #################### 데이터 저장 ####################

    /// <summary>
    /// 1.6 데이터 저장
    /// </summary>
    void saveData()
    {

        Account.GetInstance.saveAccount(PrepClass.isOffline, saveCallBack, null);
        
        if (PrepClass.isOffline)
        {
//            Account.GetInstance.saveData();
            m_retryButton.gameObject.SetActive(true);
            m_lobbyButton.gameObject.SetActive(true);
        }
        else
        {
//            GoogleApi.GetInstance.openSavedGame("friend", TYPE_FILE_ACCESS.Save, saveCallBack, null);
            //저장 요청
            isSave = false;
            StartCoroutine(saveCoroutine());
        }


    }

    //모든 데이터가 저장되면 자동 로그아웃

    /// <summary>
    /// 1.6 데이터 저장 콜백
    /// </summary>
    /// <param name="success"></param>
    void saveCallBack(bool success)
    {
        isSave = success;
        //다음 활성화
    }


    IEnumerator saveCoroutine()
    {
        //저장 로딩 패널 열기
        //            if (isQuit)
        //종료중 패널 열기
        //            else
        //저장중 패널 열기

        m_savePanel.SetActive(true);

        int cnt = 0;
        bool isFlip = false;

        float time = 0f;
        float maxTime = 5f;
        while (maxTime >= time)
        {
            if (isSave)
                break;



            m_saveText.text = "저장중";
            for (int i = 0; i < cnt; i++)
                m_saveText.text += ".";

            if (!isFlip)
            {
                cnt++;
                if (cnt >= 3)
                    isFlip = !isFlip;
            }
            else
            {
                cnt--;
                if (cnt <= 0)
                    isFlip = !isFlip;
            }


            time += PrepClass.frameTime;
            yield return new WaitForSeconds(PrepClass.frameTime);
            //저장 여부 확인 - 저장이 되지 않았으면 파일 저장
            //
        }

        if (isSave)
        {
            Debug.Log("구글 저장 완료");
        }
        else
        {
//            Account.GetInstance.saveData();
            Debug.Log("파일 저장 완료");
        }

        m_savePanel.SetActive(false);
        m_coroutine = null;

        //버튼 보이기
        m_retryButton.gameObject.SetActive(true);
        m_lobbyButton.gameObject.SetActive(true);

    }


    #endregion
}


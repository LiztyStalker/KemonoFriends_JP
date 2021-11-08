using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GamePanel;

public class UIPlayer : UIPanel
{

    Field m_field = null;

    SoundPlay m_soundPlayer = null;
    SoundPlay m_timerSoundPlayer = null;
//    SoundPlay m_feralSoundPlayer = null;

    int m_timeCheck = 0;


    [SerializeField]
    UIMsg m_uiMsg;

    [SerializeField]
    UICharacter m_uiCharacter;

    [SerializeField]
    UIButton m_uiBtn;

    [SerializeField]
    Text m_comboText;

    [SerializeField]
    Image m_itemImage;

    [SerializeField]
    Text m_itemText;

    [SerializeField]
    Button m_itemButton;

    [SerializeField]
    Text m_timerText;

    [SerializeField]
    Slider m_timerSlider;

    [SerializeField]
    Text m_feverText;

    [SerializeField]
    Slider m_feverSlider;

    [SerializeField]
    UIFeral m_uiFeral;

    [SerializeField]
    Transform m_timePanel;

    [SerializeField]
    UIResult m_resultView;

    [SerializeField]
    UIStart m_startPanel;

    [SerializeField]
    UIPause m_pausePanel;

    [SerializeField]
    UIHelp m_helpPanel;

    UIPlayerView m_playerView;
    
    Field field { get { return m_field; } }

    public UIMsg uiMsg { get { return m_uiMsg; } }

    void initPanel()
    {

        m_soundPlayer = GetComponent<SoundPlay>();
        if(m_soundPlayer == null)
            m_soundPlayer = gameObject.AddComponent<SoundPlay>();

        m_playerView = GetComponent<UIPlayerView>();

        m_uiBtn.setBtnPanel(UIButton.TYPE_BTN_PANEL.NONE);

        m_itemButton.onClick.AddListener(() => OnItemClicked());

        m_uiFeral.closeFeral();
        m_timePanel.gameObject.SetActive(false);

        m_resultView.gameObject.SetActive(false);
        m_startPanel.gameObject.SetActive(true);

        m_pausePanel.setParent(this);
        m_pausePanel.setField(field);
        m_pausePanel.gameObject.SetActive(false);

        m_playerView.setPlayer(m_field);


        m_timerSlider.handleRect.GetComponent<Image>().sprite = field.friend.icon;

        m_helpPanel.setHelp(m_field.adventure);
        uiUpdate();
    }

    public void setField(Field field) {
        m_field = field;
        initPanel();
    }

    public void timerUpdate()
    {
        m_timerText.text = string.Format("{0:f0}초", field.gameTimeDown);
        m_timerSlider.value = m_field.timeRate;
        gameLowTime(field.gameTimeDown);
    }

    public override void uiUpdate()
    {
        m_playerView.uiUpdate();
        itemView();
        feverUpdate();
        m_comboText.text = string.Format("{0}", m_field.combo);
    }

    public void feverUpdate()
    {
        if (!m_uiFeral.gameObject.activeSelf)
            m_feverText.text = string.Format("{0}/{1}", field.nowFeral, field.maxFeral);
        else
            m_feverText.text = string.Format("야생해방!");

        m_feverSlider.value = field.feralRate;
    }

    public void gamePreivew(float time = 1f)
    {
        m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
        m_uiCharacter.setCharacter(m_field.friend, TYPE_SPEECH.START, time);
        m_startPanel.gamePreview(field, time);
    }


    public void gameReady(float time = 1f)
    {
        m_soundPlayer.audioPlay("EffectReady", TYPE_SOUND.EFFECT);
        m_startPanel.gameReady(field, time);
    }

    public void gameStart(float time = 1f)
    {
        m_soundPlayer.audioPlay("EffectStart", TYPE_SOUND.EFFECT);

        m_startPanel.gameStart(field, time);

//        if(m_field.adventure is AdventureExplore || m_field.adventure is AdventureDrive)
//        if(m_field.adventure.adventureType.BaseType == typeof(AdventureMover))
//        m_arrowPanel.gameObject.SetActive(m_field.adventure.useArrowPanel());

        m_uiBtn.setBtnPanel(m_field.adventure.useBtnPanel());

    }


    public void feralStart()
    {
        m_uiFeral.setFeral();
        m_uiCharacter.setCharacter(m_field.friend, TYPE_SPEECH.FERAL);
        uiUpdate();
    }

    public void feralEnd()
    {
        m_uiFeral.closeFeral();
        uiUpdate();
    }

    void gameLowTime(float nowTime)
    {
        //시간없음
        if (nowTime <= 10f && m_timeCheck == 0)
        {

            m_timePanel.gameObject.SetActive(true);

            if (m_timerSoundPlayer == null)
            {
                m_timerSoundPlayer = m_timePanel.gameObject.GetComponent<SoundPlay>();
                if (m_timerSoundPlayer == null)
                    m_timerSoundPlayer = m_timePanel.gameObject.AddComponent<SoundPlay>();
            }

            //일반
            m_timerSoundPlayer.audioPlay("EffectTime", TYPE_SOUND.EFFECT, true, 2f);
            m_timeCheck++;
        }

        else if (nowTime <= 5f && m_timeCheck == 1)
        {
            //2배로 빨라짐
            m_timerSoundPlayer.audioPlay("EffectTime", TYPE_SOUND.EFFECT, true, 4f);
            m_timeCheck++;
        }
    }

    public void gameEnd(float time = 1f, bool isDefeat = false)
    {
        if(m_timerSoundPlayer != null)
            m_timerSoundPlayer.audioStop();
        m_uiBtn.setBtnPanel(UIButton.TYPE_BTN_PANEL.NONE);
        m_timePanel.gameObject.SetActive(false);

        if (isDefeat)
        {
            m_soundPlayer.audioPlay("EffectFail", TYPE_SOUND.EFFECT);
            m_uiCharacter.setCharacter(m_field.friend, TYPE_SPEECH.DEFEAT, time);
        }
        else
        {
            m_soundPlayer.audioPlay("EffectSuccess", TYPE_SOUND.EFFECT);
            m_uiCharacter.setCharacter(m_field.friend, TYPE_SPEECH.END, time);

        }

        m_startPanel.gameEnd(m_field, time, isDefeat);
    }

    public void gameBonas(float time = 1f)
    {
        m_soundPlayer.audioPlay("EffectBonas", TYPE_SOUND.EFFECT);
        m_uiCharacter.setCharacter(m_field.friend, TYPE_SPEECH.BONAS, time);
        m_startPanel.gameBonas(m_field, time);
    }

    public void gameFinish()
    {
        m_soundPlayer.audioPlay("EffectResult", TYPE_SOUND.EFFECT);

        m_resultView.setParent(this);
        m_resultView.gameObject.SetActive(true);
        m_resultView.uiUpdate(field);
    }

    public void OnPauseClicked()
    {

        m_soundPlayer.audioPlay("EffectInfo", TYPE_SOUND.EFFECT);
        m_pausePanel.openPanel(this);
    }

    public void OnItemClicked()
    {
        m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);

        m_field.useItem();
        itemView();
    }

    void itemView() 
    {

        if (!m_field.isGameRun)
        {
            m_itemButton.interactable = false;
            m_itemImage.gameObject.SetActive(false);
            m_itemText.gameObject.SetActive(false);
        }
        else
        {
            if (m_field.item == ItemManager.GetInstance.defaultItem())
            {
                m_itemButton.interactable = false;
                m_itemImage.gameObject.SetActive(false);
                m_itemText.gameObject.SetActive(false);
            }
            else
            {
                m_itemButton.interactable = true;
                m_itemImage.gameObject.SetActive(true);
                m_itemText.gameObject.SetActive(true);
                m_itemText.text = "아이템 사용";
            }
        }
        m_itemImage.sprite = m_field.item.icon;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log(UIPanelManager.GetInstance.nowPanel());


            if (m_resultView.isActiveAndEnabled)
            {
                if (!m_resultView.isTotalPanel)
                    m_resultView.OnRetryClick();
                else
                    m_resultView.OnLobbyClick();
            }
            else if (UIPanelManager.GetInstance.nowPanel() is UIPlayer)
            {
                m_pausePanel.openPanel(this);
            }
            else
            {
                m_soundPlayer.audioPlay("EffectCancel", TYPE_SOUND.EFFECT);
                UIPanelManager.GetInstance.nowPanel().closePanel();
                UIPanelManager.GetInstance.nowPanel().uiUpdate();
            }
        }
    }


    public void OnArrowClicked(int selector)
    {
        //좌현 우현 설정
        //음수이면 좌현, 양수이면 우현

        m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
        m_field.setArrowChange(selector);
//        m_field.setArrowChange((TYPE_ARROW)arrow);
    }

    public void OnOneClicked()
    {
        m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
        m_field.setOneEvent();
        //한번 터치로 움직여야 함
        //각 행동자를 루프하여 닿았을 때 이벤트 발생
    }

    //void OnApplicationPause(bool isFocus)
    //{
    //    if (!isFocus)
    //        Account.GetInstance.saveData();
    //    else
    //        OnPauseClicked();
    //}

    //void OnApplicationQuit()
    //{
    //    Account.GetInstance.saveData();
    //}
}


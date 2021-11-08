
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePanel
{
    public class UIPause : UIPanel
    {

        // Use this for initialization

        UIPlayer m_uiPlayer;

        SoundPlay m_soundPlayer = null;

        [SerializeField]
        UIOption m_uiOpt;

        [SerializeField]
        UIHelp m_uiHelp;

        Field m_field;


        void Start()
        {
            m_soundPlayer = GetComponent<SoundPlay>();
            if (m_soundPlayer == null)
                m_soundPlayer = gameObject.AddComponent<SoundPlay>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Time.timeScale = 0f;
            if (m_field != null)
                m_field.setPause();
        }


        public void setParent(UIPlayer uiPlayer)
        {
            m_uiPlayer = uiPlayer;
        }

        public void setField(Field field)
        {
            m_field = field;
        }

        public void OnResumeClicked()
        {
            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
            closePanel();
        }

        public void OnOptionClicked()
        {
            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
            m_uiOpt.openPanel(this);
        }

        public void OnExitClicked()
        {
            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
            m_uiPlayer.uiMsg.openPanel(this);
            m_uiPlayer.uiMsg.setMsg("로비로 돌아가시겠습니까?", TYPE_MSG_PANEL.WARNING, TYPE_MSG_BTN.OK_CANCEL, playQuit);
        }

        public void OnHelpClicked()
        {
            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
            m_uiHelp.openPanel(this);
        }

        void playQuit()
        {
            Account.GetInstance.nextScene = "Game@Lobby";
            SceneManager.LoadScene("Game@Load");
        }
        
        protected override void OnDisable()
        {
            if (m_field != null) m_field.resetPause();
            Time.timeScale = 1f;
            base.OnDisable();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
    public class UIOption : UIPanel
    {

        SoundPlay m_soundPlayer;

        [SerializeField]
        Toggle m_bgmToggle;

        [SerializeField]
        Toggle m_effectToggle;

//        [SerializeField]
//        Toggle m_voiceToggle;

        void Awake()
        {

            m_soundPlayer = GetComponent<SoundPlay>();
            if (m_soundPlayer == null)
                m_soundPlayer = gameObject.AddComponent<SoundPlay>();

            m_bgmToggle.onValueChanged.AddListener((isOn) => OnBGMChanged(isOn));
            m_effectToggle.onValueChanged.AddListener((isOn) => OnEffectChanged(isOn));
        }


        void Start()
        {
            m_bgmToggle.isOn = PrepClass.isBGM;
            m_effectToggle.isOn = PrepClass.isEffect;
        }


        public void OnBGMChanged(bool isOn)
        {

            PrepClass.isBGM = isOn;
            int check = (PrepClass.isBGM) ? 1 : 0;
            PlayerPrefs.SetInt("isBGM", check);
            SoundManager.GetInstance.setMute(TYPE_SOUND.BGM);

            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
        }


        public void OnEffectChanged(bool isOn)
        {

            PrepClass.isEffect = isOn;
            int check = (PrepClass.isEffect) ? 1 : 0;
            PlayerPrefs.SetInt("isEffect", check);
            SoundManager.GetInstance.setMute(TYPE_SOUND.EFFECT);

            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
        }

    }
}


using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
    public class UIUpMenu : MonoBehaviour
    {
        SoundPlay m_soundPlayer;

        [SerializeField]
        Text m_offlineText;

        [SerializeField]
        Text m_japariBreadText;

        [SerializeField]
        Text m_coinText;

        void Start()
        {
            //1.6 오프라인 모드
            if (PrepClass.isOffline)
                m_offlineText.text = "Offline Mode";
            else
                m_offlineText.text = "";

            m_soundPlayer = GetComponent<SoundPlay>();
            if (m_soundPlayer == null)
                m_soundPlayer = gameObject.AddComponent<SoundPlay>();

            m_soundPlayer.audioPlay("BGMLobby", TYPE_SOUND.BGM);
        }

        public void uiUpdate()
        {
            m_japariBreadText.text = string.Format("{0}", Account.GetInstance.japariBread);
            m_coinText.text = string.Format("{0}", Account.GetInstance.coin);
        }
    }
}


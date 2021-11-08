using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
    public class UILowMenu : MonoBehaviour
    {
        [SerializeField]
        Toggle[] m_menuToggleArray;

        public delegate void MenuSelectDelegate(TYPE_LOBBY typeLobby);

        MenuSelectDelegate m_menuSelectDel;

//        bool m_isInit = false;

        SoundPlay m_soundPlayer = null;

        void Start()
        {

            m_soundPlayer = GetComponent<SoundPlay>();
            if (m_soundPlayer == null)
                m_soundPlayer = gameObject.AddComponent<SoundPlay>();

            for (int i = 0; i < m_menuToggleArray.Length; i++)
            {
                m_menuToggleArray[i].onValueChanged.AddListener((isOn) => OnValueChanged(isOn));
            }

            //로비 화면 제작이 되면 지울것
            OnValueChanged(true);
        }

        public void initToggle()
        {
//            m_isInit = true;
            for (int i = 0; i < m_menuToggleArray.Length; i++)
            {
                m_menuToggleArray[i].isOn = false;
            }
//            m_isInit = false;
        }

        public void setDelegate(MenuSelectDelegate menuSelectDel)
        {
            m_menuSelectDel = menuSelectDel;
        }

        void OnValueChanged(bool isOn)
        {
            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);

            if (isOn)
            {
                for (int i = 0; i < m_menuToggleArray.Length; i++)
                {
                    if (m_menuToggleArray[i].isOn)
                    {
                        //상위 메뉴에게 알려주기
                        m_menuSelectDel((TYPE_LOBBY)i);
                        break;
                    }
                }
            }
        }

    }
}

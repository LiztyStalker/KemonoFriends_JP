using System;
using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
    public class UIGardenToggle : MonoBehaviour
    {

        SoundPlay m_soundPlayer;

        public delegate void GardenToggleDelegate(Garden garden);

        GardenToggleDelegate m_gardenToggleDel;

        Garden m_garden;

        [SerializeField]
        Image m_icon;

        [SerializeField]
        Text m_nameText;


        void Awake()
        {
            if (m_soundPlayer == null)
                m_soundPlayer = ((UILobby)UIPanelManager.GetInstance.root).soundPlayer;

            GetComponent<Toggle>().onValueChanged.AddListener((isOn) => OnValueChanged(isOn));
        }


        public void setDelegate(GardenToggleDelegate gardenToggleDel)
        {
            m_gardenToggleDel = gardenToggleDel;
        }

        public void setToggle(Garden garden)
        {
            m_garden = garden;
            m_nameText.text = m_garden.name;
            m_icon.sprite = m_garden.icon;
//            GetComponent<Toggle>().graphic.GetComponent<Image>().sprite = m_garden.icon;

        }

        public void OnValueChanged(bool isOn)
        {
            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT, false);
            if (isOn)
            {
//                Account.GetInstance.adventureKey = m_adventure.key;
                m_gardenToggleDel(m_garden);
            }
        }

    }
}

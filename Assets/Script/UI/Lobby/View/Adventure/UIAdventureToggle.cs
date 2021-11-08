using System;
using UnityEngine;
using UnityEngine.UI;

namespace GamePanel{
	public class UIAdventureToggle : MonoBehaviour
	{

        SoundPlay m_soundPlayer;

        public delegate void AdventureToggleDelegate(Adventure adventure);

        AdventureToggleDelegate m_adventureToggleDel;

        Adventure m_adventure;

        [SerializeField]
        Text m_adventureName;

        [SerializeField]
        Image m_icon;


        void Awake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener((isOn) => OnValueChanged(isOn));
        }


        public void setDelegate(AdventureToggleDelegate adventureToggleDel)
        {
            m_adventureToggleDel = adventureToggleDel;
        }

        public void setToggle(Adventure adventure)
        {
            m_adventure = adventure;
            m_adventureName.text = m_adventure.name;
            m_icon.sprite = m_adventure.iconImage;
//            GetComponent<Toggle>().graphic.GetComponent<Image>().sprite = m_adventure.iconImage;
        }

        public void OnValueChanged(bool isOn)
        {
            if (m_soundPlayer == null)
                m_soundPlayer = ((UILobby)UIPanelManager.GetInstance.root).soundPlayer;

            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT, false);

            if (isOn)
            {
                Account.GetInstance.adventureKey = m_adventure.key;
                m_adventureToggleDel(m_adventure);
            }
        }

	}
}

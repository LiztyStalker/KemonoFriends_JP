using System;
using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
	public class UIItemToggle : MonoBehaviour
	{

        public delegate void ItemToggleDelegate(Item item);

        ItemToggleDelegate m_itemToggleDel;

        [SerializeField]
        Text m_nameText;

        [SerializeField]
        Image m_icon;

        Item m_item;

        void Awake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener((isOn) => OnValueChanged(isOn));
        }


        public void setDelegate(ItemToggleDelegate itemToggleDel)
        {
            m_itemToggleDel = itemToggleDel;
        }

        public void setToggle(Item item)
        {
            m_item = item;
            m_nameText.text = item.itemName;
            m_icon.sprite = item.icon;
        }

        public void OnValueChanged(bool isOn)
        {
            if (isOn)
            {
                m_itemToggleDel(m_item);
                Account.GetInstance.itemKey = m_item.key;
            }
        }

	}
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
    public class UIAdventureReady : UIPanel
    {

        SoundPlay m_soundPlayer;

        public delegate void StartBtnDelegate();

        StartBtnDelegate m_startBtnDel;

        enum TYPE_ITEM { FERAL = 100 }

        [SerializeField]
        Image m_image;

        [SerializeField]
        Text m_contentsText;

        [SerializeField]
        Text m_accumulateBreadText;

        [SerializeField]
        UIItemToggle m_itemToggle;

        [SerializeField]
        Transform m_itemPanel;


        List<UIItemToggle> m_itemToggleList = new List<UIItemToggle>();

        int m_cost;

        public int cost { get { return m_cost; } }
        
        protected override void OnEnable()
        {
            base.OnEnable();

            if (m_soundPlayer == null)
                m_soundPlayer = ((UILobby)UIPanelManager.GetInstance.root).soundPlayer;

            Adventure adventure = AdventureManager.GetInstance.getAdventure(Account.GetInstance.adventureKey);
            m_image.sprite = adventure.infoImage;

            if (m_itemToggleList.Count == 0)
            {

                for (int i = 0; i < ItemManager.GetInstance.ElementCount; i++)
                {
                    Item item = ItemManager.GetInstance.getItem(i);

                    if (item.isCheckLocation(Account.GetInstance.adventureKey))
                    {
                        if (item != null && item.isUse)
                        {
                            UIItemToggle itemToggle = Instantiate(m_itemToggle);
                            itemToggle.transform.SetParent(m_itemPanel);
                            itemToggle.setDelegate(setItemInfo);
                            itemToggle.setToggle(item);
                            itemToggle.GetComponent<Toggle>().group = m_itemPanel.GetComponent<ToggleGroup>();
                            itemToggle.GetComponent<Toggle>().isOn = false;
                            itemToggle.transform.localScale = Vector3.one;
                            m_itemToggleList.Add(itemToggle);
                        }
                    }
                }

            }
            else
            {
                for (int i = 0; i < m_itemToggleList.Count; i++)
                {
                    m_itemToggleList[i].GetComponent<Toggle>().isOn = false;
                }
            }

            m_itemToggleList[0].GetComponent<Toggle>().isOn = true;
        }


        protected override void OnDisable()
        {
            if(m_itemToggleList.Count > 0){
                int count = m_itemToggleList.Count;
                for (int i = count - 1; i >= 0; i--)
                {
                    Destroy(m_itemToggleList[i].gameObject);
                }
            }
            m_itemToggleList.Clear();
            base.OnDisable();
        }

        public void setDelegate(StartBtnDelegate del)
        {
            m_startBtnDel = del;
        }



        //void OnValueChanaged(bool isOn)
        //{
        //    m_accumulateBread = 0;

        //    for (int i = 0; i < m_itemToggleList.Count; i++)
        //    {
        //        if (m_itemToggleList[i].isOn)
        //        {
        //            m_accumulateBread += (int)((TYPE_ITEM)i);
        //            m_accumulateBreadText.text = string.Format("{0}", m_accumulateBread);
        //        }
        //    }
        //}

        void setItemInfo(Item item)
        {

            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);

            m_contentsText.text = item.contents;
            m_accumulateBreadText.text = string.Format("{0}", -(item.cost));
            m_cost = item.cost;
            Account.GetInstance.itemKey = item.key;
            
            m_startBtnDel();

        }

        
    }
}


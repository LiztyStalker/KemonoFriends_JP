using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//1.5
namespace GamePanel
{
    public class UIAchivement : UIPanel
    {

        List<UIAchivementData> m_achivementList = new List<UIAchivementData>();

        [SerializeField]
        UIAchivementData m_uiAchivementData;

        [SerializeField]
        Transform m_viewPanel;

        [SerializeField]
        Toggle[] m_toggles;

        SoundPlay m_soundPlayer;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (m_achivementList.Count == 0)
            {
                for (int i = 0; i < m_toggles.Length; i++)
                {
                    m_toggles[i].onValueChanged.AddListener((isOn) => OnToggleClicked(isOn));
                }

                foreach (Achivement achivement in AchivementManager.GetInstance.achivementDic.Values)
                {
                    UIAchivementData achivementData = Instantiate(m_uiAchivementData);
                    achivementData.transform.SetParent(m_viewPanel);
                    achivementData.transform.localScale = Vector2.one;
                    achivementData.setAchivement(achivement);
                    m_achivementList.Add(achivementData);
                }
            }
            else
            {
                //새로고침
                foreach (UIAchivementData achivementData in m_achivementList)
                {
                    achivementData.achivementUpdate();

                }
            }
            
            //업적 갱신
            ((UILobby)UIPanelManager.GetInstance.root).achivementView.achivementDataUpdate();

            //토글 버튼 초기화
            for (int i = 0; i < m_toggles.Length; i++)
            {
                m_toggles[i].isOn = false;
            }
            m_toggles[0].isOn = true;

        }

        public void OnToggleClicked(bool isOn)
        {
            //0 전체
            //1 달성
            //2 미달성

            //사운드
            if (m_soundPlayer == null)
                m_soundPlayer = ((UILobby)UIPanelManager.GetInstance.root).soundPlayer;
            m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);


            int index = 0;
            for (int i = 0; i < m_toggles.Length; i++)
            {
                if (m_toggles[i].isOn)
                {
                    index = i;
                    break;
                }
            }




            switch (index)
            {
                case 0:
                    foreach (UIAchivementData achivementData in m_achivementList)
                    {
                        achivementData.gameObject.SetActive(true);
                    }
                    break;
                case 1:
                    foreach (UIAchivementData achivementData in m_achivementList)
                    {
                        if(achivementData.isAchivement)
                            achivementData.gameObject.SetActive(true);
                        else
                            achivementData.gameObject.SetActive(false);
                    }

                    break;
                case 2:
                    foreach (UIAchivementData achivementData in m_achivementList)
                    {
                        if(achivementData.isAchivement)
                            achivementData.gameObject.SetActive(false);
                        else
                            achivementData.gameObject.SetActive(true);
                    }
                    break;
            }
        }
       
    }
}


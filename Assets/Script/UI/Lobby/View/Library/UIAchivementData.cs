using UnityEngine;
using UnityEngine.UI;


//1.5
namespace GamePanel
{
    public class UIAchivementData: MonoBehaviour
    {
        [SerializeField]
        Image m_icon;

        [SerializeField]
        Text m_nameText;

        [SerializeField]
        Text m_contentsText;

        [SerializeField]
        Image m_awardIcon;

        [SerializeField]
        Text m_awardText;

        [SerializeField]
        Text m_valueText;

        [SerializeField]
        Slider m_slider;

        [SerializeField]
        Transform m_successPanel;

        Achivement m_achivement;

        private Achivement achivement{get{return m_achivement;}}

        public bool isAchivement{get{return Account.GetInstance.isAchivement(achivement);}}

        public void setAchivement(Achivement achivement)
        {
            m_achivement = achivement;
            achivementUpdate();
        }

        public void achivementUpdate()
        {
            m_icon.sprite = achivement.icon;
            m_nameText.text = achivement.nameText;
            m_contentsText.text = string.Format("{0}", achivement.contents);
            m_awardText.text = string.Format("{0}", achivement.award);

            int value = getTypeAchivement(achivement.typeAchivement);
            if(value >= achivement.value)
                value = achivement.value;

            m_valueText.text = string.Format("{0}/{1}", value, achivement.value);

            m_slider.value = (float)value / (float)achivement.value;
            //타입에 따라 데이터 달라짐

            //업적을 습득했으면 달성 보이기
            if (isAchivement)
            {
                m_successPanel.gameObject.SetActive(true);
            }
            else
            {
                m_successPanel.gameObject.SetActive(false);
            }
        }

        int getTypeAchivement(Achivement.TYPE_ACHIVEMENT typeAchivement)
        {
            return Account.GetInstance.getTypeAchivement(typeAchivement);
        }

    }
}


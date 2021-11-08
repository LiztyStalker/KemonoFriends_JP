using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
    public class UIPlayerView : UIPanel
    {
        [SerializeField]
        Image m_friendIcon;

        [SerializeField]
        Text m_nameText;

        [SerializeField]
        Text m_scoreText;

        [SerializeField]
        Text m_coinText;

        [SerializeField]
        Text m_ceruleanText;

        [SerializeField]
        Text m_foodText;

        Field m_field;

        public void setPlayer(Field field)
        {
            m_field = field;
        }

        public override void uiUpdate()
        {
            m_friendIcon.sprite = m_field.friend.icon;
            m_nameText.text = m_field.friend.name;
            m_scoreText.text = string.Format("{0:d6}", m_field.score);
            m_coinText.text = string.Format("{0:d2}", m_field.coin);
            m_ceruleanText.text = string.Format("{0:d4}", m_field.cerulean);
            m_foodText.text = string.Format("{0:d5}", m_field.food);
        }


    }

}
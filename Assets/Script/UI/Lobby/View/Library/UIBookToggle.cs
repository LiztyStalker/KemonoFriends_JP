using UnityEngine.UI;
using UnityEngine;

namespace GamePanel
{
    public class UIBookToggle : MonoBehaviour
    {
        [SerializeField]
        Text m_nameText;

        [SerializeField]
        Text m_conditionText;

        [SerializeField]
        Image m_image;

        Book m_book;

        public void setBook(Book book)
        {
            m_book = book;
            setBookInfo();
        }


        void setBookInfo()
        {
        }
    }
}


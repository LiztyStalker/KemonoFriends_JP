using UnityEngine;


//1.4
public class UIButton : MonoBehaviour
{

    public enum TYPE_BTN_PANEL { NONE, ARROW, ONE }

    [SerializeField]
    Transform m_arrowBtnPanel;

    [SerializeField]
    Transform m_oneBtnPanel;

    /// <summary>
    /// 사용할 버튼 선택하기
    /// </summary>
    /// <param name="typeBtn"></param>
    public void setBtnPanel(TYPE_BTN_PANEL typeBtn)
    {
        switch (typeBtn)
        {
            case TYPE_BTN_PANEL.ARROW:
                m_arrowBtnPanel.gameObject.SetActive(true);
                m_oneBtnPanel.gameObject.SetActive(false);
                break;
            case TYPE_BTN_PANEL.ONE:
                m_arrowBtnPanel.gameObject.SetActive(false);
                m_oneBtnPanel.gameObject.SetActive(true);                
                break;
            default:
                m_arrowBtnPanel.gameObject.SetActive(false);
                m_oneBtnPanel.gameObject.SetActive(false);
                break;
        }
    }


}


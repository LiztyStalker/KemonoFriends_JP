using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class UIStart : MonoBehaviour
{
    [SerializeField]
    Text m_readyText;

    [SerializeField]
    Text m_startText;

    [SerializeField]
    Text m_endText;

    [SerializeField]
    Text m_bonasText;

    [SerializeField]
    Text m_baloon;


    void Start()
    {
        m_readyText.gameObject.SetActive(false);
        m_startText.gameObject.SetActive(false);
        m_endText.gameObject.SetActive(false);
        m_bonasText.gameObject.SetActive(false);
        m_baloon.gameObject.SetActive(false);
    }

    public void gamePreview(Field field, float time = 1f)
    {
        //각 캐릭터의 대사가 들어 있어야 함
        m_baloon.text = "그럼 시작해 볼까?";
        StartCoroutine(startCoroutine(m_baloon.gameObject, time));
    }

    public void gameReady(Field field, float time = 1f)
    {
        m_readyText.text = "준비~";
        StartCoroutine(startCoroutine(m_readyText.gameObject, time));
    }


    public void gameStart(Field field, float time = 1f)
    {
        m_startText.text = "시작!";
        if (m_readyText.isActiveAndEnabled)
            m_readyText.gameObject.SetActive(false);
        StartCoroutine(startCoroutine(m_startText.gameObject, time));
    }


    public void gameEnd(Field field, float time = 1f, bool isAllAcorn = false)
    {
        m_endText.text = "종료";
//        else m_endText.text = "으앙! 세룰리안한테 밭을 점령당했어!";
        StartCoroutine(startCoroutine(m_endText.gameObject, time));
    }

    public void gameBonas(Field field, float time = 1f)
    {
//        m_bonasText.text = "조금 더 남았네?";
//        if (m_endText.isActiveAndEnabled)
//            m_endText.gameObject.SetActive(false);
//        StartCoroutine(startCoroutine(m_bonasText.gameObject, time));
    }

    IEnumerator startCoroutine(GameObject gameobject, float time)
    {

        gameobject.SetActive(true);
        yield return new WaitForSeconds(time);
        gameobject.SetActive(false);

    }

}


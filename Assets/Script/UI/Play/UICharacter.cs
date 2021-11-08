using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class UICharacter : MonoBehaviour
{
    [SerializeField]
    Text m_scriptText;

    [SerializeField]
    Image m_friendImage;

    Coroutine m_coroutine;
    float m_timer = 0f;

    public void setCharacter(Friend friend, TYPE_SPEECH typeSpeech, float timer = 1f)
    {
        m_friendImage.sprite = friend.image;
        m_scriptText.text = friend.getSpeech(typeSpeech);
        gameObject.SetActive(true);

        if (m_coroutine == null)
            m_coroutine = StartCoroutine(characterCoroutine());
        else
            m_timer = timer;
    }


    IEnumerator characterCoroutine()
    {

        m_timer = 1f;
        while (m_timer > 0f)
        {
            m_timer -= PrepClass.frameTime;
            yield return new WaitForSeconds(PrepClass.frameTime);
        }

        m_coroutine = null;
        gameObject.SetActive(false);
    }


}


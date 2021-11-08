using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWait : MonoBehaviour
{
    [SerializeField]
    Text m_contentsText;


    public void setContents(string msg)
    {
        m_contentsText.text = msg;
        gameObject.SetActive(true);
    }

    public void close()
    {
        gameObject.SetActive(false);
    }
}


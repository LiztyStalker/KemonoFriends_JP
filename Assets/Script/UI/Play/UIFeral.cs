using UnityEngine;
using UnityEngine.UI;

public class UIFeral : MonoBehaviour
{

    SoundPlay m_SoundPlayer;

    void OnEnable()
    {
        if (m_SoundPlayer == null)
        {
            m_SoundPlayer = GetComponent<SoundPlay>();
            if (m_SoundPlayer == null)
                m_SoundPlayer = gameObject.AddComponent<SoundPlay>();
        }

        m_SoundPlayer.audioPlay("EffectFeral", TYPE_SOUND.EFFECT);

    }


    public void setFeral()
    {
        gameObject.SetActive(true);
    }
    
    public void closeFeral()
    {
        gameObject.SetActive(false);
    }
}


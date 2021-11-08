using System;
using UnityEngine;


public enum TYPE_BTN_SOUND{NONE, INFOR, OK, CANCEL, WARNING, ERROR, SELL, BUY}
public enum TYPE_SOUND {EFFECT, BGM, VOICE}

public class SoundPlay : MonoBehaviour
{
    string m_key = "";
	TYPE_SOUND m_typeSound = TYPE_SOUND.EFFECT;
//	bool m_isPlayOnAwake = true;
//	bool m_isLoop = false;
//	bool m_is3DSound = false;

    AudioSource m_audioSource = null;

	public TYPE_SOUND typeSound{ get { return m_typeSound; } }
    

	void Start(){
        m_audioSource = GetComponent<AudioSource>();
        if (m_audioSource == null)
        {
            m_audioSource = gameObject.AddComponent<AudioSource>();
        }

//		if(m_isPlayOnAwake) audioPlay (m_key, m_typeSound, m_is3DSound);
	}


    public void audioPlay(AudioClip audioClip, TYPE_SOUND typeSound, bool is3DSound = false, bool isLoop = false, float pitch = 1f)
    {

        Start();
		
		m_typeSound = typeSound;

//        Debug.Log("Audio : " + audioClip.name);

		if (m_audioSource != null) {
			m_audioSource.Stop ();

			if (typeSound == TYPE_SOUND.BGM) {
				m_audioSource.clip = audioClip;
				m_audioSource.mute = !PrepClass.isBGM;
                m_audioSource.loop = isLoop;
				m_audioSource.Play ();
			} else {
				if (is3DSound) {
					m_audioSource.spatialBlend = 0.9f;
					m_audioSource.spread = 360f;
				}
				
                m_audioSource.mute = !PrepClass.isEffect;

                if (isLoop)
                {
                    m_audioSource.loop = isLoop;
                    m_audioSource.clip = audioClip;
                    m_audioSource.Play();
                }
                else
                    m_audioSource.PlayOneShot(audioClip);
//				Destroy (gameObject, m_audioSource.clip.length);
			}
            audioPitch(pitch);

		}

	}

    public void audioPlay(TYPE_BTN_SOUND typeBtnSound, float pitch = 1f)
    {
        SoundManager.GetInstance.btnEffectPlay(this, typeBtnSound, pitch);
	}

//	public void audioPlayOneShot(string key, TYPE_SOUND typeSound){
//		soundPlayOneShot (key, typeSound);
//	}

    //public void audioPlay(string key, TYPE_SOUND typeSound, bool is3DSound = true, float runTime = 1f){
    //    soundPlay (key, typeSound, is3DSound, runTime);
    //}

    //public void audioPlay(TYPE_SOUND typeSound, bool is3DSound = true, float runTime = 1f){
    //    soundPlay (m_key, typeSound, is3DSound, runTime);
    //}

    public void audioPlay(string key, TYPE_SOUND typeSound, bool is3DSound, bool isLoop, float pitch = 1f)
    {
        audioPlayer(key, typeSound, is3DSound, isLoop, pitch);
    }

    public void audioPlay(TYPE_SOUND typeSound, bool is3DSound, bool isLoop, float pitch = 1f)
    {
        audioPlayer(m_key, typeSound, is3DSound, isLoop, pitch);
    }

    public void audioPlay(string key, TYPE_SOUND typeSound, bool isLoop, float pitch = 1f)
    {
        audioPlayer(key, typeSound, false, isLoop, pitch);
    }

    public void audioPlay(TYPE_SOUND typeSound, bool isLoop, float pitch = 1f)
    {
        audioPlayer(m_key, typeSound, false, isLoop, pitch);
    }

    public void audioPlay(string key, TYPE_SOUND typeSound, float pitch = 1f)
    {
        if (typeSound == TYPE_SOUND.BGM)
            audioPlayer(key, typeSound, false, true, pitch);
        else
            audioPlayer(key, typeSound, false, false, pitch);
    }

    public void audioPlay(TYPE_SOUND typeSound, float pitch = 1f)
    {
        if (typeSound == TYPE_SOUND.BGM)
            audioPlayer(m_key, typeSound, false, true, pitch);
        else
            audioPlayer(m_key, typeSound, false, false, pitch);
    }

    /// <summary>
    /// 사운드 변속
    /// </summary>
    /// <param name="pitch"></param>
    public void audioPitch(float pitch = 1f)
    {
        m_audioSource.pitch = pitch;
    }

	void audioPlayer(string key, TYPE_SOUND typeSound, bool is3Dsound, bool isLoop, float runTime){
		if (typeSound == TYPE_SOUND.EFFECT) {
            SoundManager.GetInstance.effectPlay(this, key, is3Dsound, isLoop, runTime);
		} else {
            SoundManager.GetInstance.bgmPlay(this, key, false, isLoop, runTime);
		}
	}

    public void audioStop()
    {
        m_audioSource.Stop();
    }

	public void setMute(bool isMute){
        try
        {
            if (gameObject.activeSelf)
            {
                if (m_audioSource == null)
                {
                    m_audioSource = GetComponent<AudioSource>();
                    if (m_audioSource == null)
                        m_audioSource = gameObject.AddComponent<AudioSource>();
                }
            }

            m_audioSource.mute = isMute;
        }
        catch
        {
            OnDisable();
        }
	}

	void OnDisable(){
        SoundManager.GetInstance.soundEnd(this);
	}



}



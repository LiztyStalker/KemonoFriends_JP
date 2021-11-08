using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundManager : SingletonClass<SoundManager>
{
	Dictionary <string, AudioClip> m_bgmDic = new Dictionary<string, AudioClip>();
	Dictionary <string, AudioClip> m_effectDic = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> m_voiceDic = new Dictionary<string, AudioClip>();

	List<SoundPlay> m_soundList = new List<SoundPlay>();


//	AudioSource m_MyselfAudioSource;
//	AudioSource m_RangeAudioSource;
//	AudioSource m_WorldAuidoSource;

	readonly string[] m_btnKeys = 
	{
		"BtnNone",
		"BtnInfor",
		"BtnOK",
		"BtnCancel",
		"BtnWarning",
		"BtnError",
		"BtnSell",
		"BtnBuy"
	};


    public SoundManager()
    {
        initDictionary();
    }


	public void initDictionary(){
		AudioClip[] bgmList = Resources.LoadAll<AudioClip> ("Sound/BGM");

		m_bgmDic.Clear ();
		foreach (AudioClip bgm in bgmList) {
            string bgmName = bgm.name.Trim();
//            Debug.Log("bgm : " + bgmName + " " + bgm);
            m_bgmDic.Add(bgmName, bgm);
		}


		AudioClip[] effectList = Resources.LoadAll<AudioClip>("Sound/Effect");

		m_effectDic.Clear ();
		foreach (AudioClip effect in effectList) {
            string effName = effect.name.Trim();
//            Debug.Log("effect : " + effName + " " + effect);
            m_effectDic.Add(effName, effect);
		}


        AudioClip[] voiceList = Resources.LoadAll<AudioClip>("Sound/Voice");

        m_voiceDic.Clear();
        foreach (AudioClip voice in voiceList)
        {
            string vicName = voice.name.Trim();
//            Debug.Log("effect : " + vicName + " " + voice);
            m_voiceDic.Add(vicName, voice);
        }
	}


	public void setMute(TYPE_SOUND typeSound){
		SoundPlay[] soundList = m_soundList.Where (sound => sound.typeSound == typeSound).ToArray<SoundPlay> ();
		foreach (SoundPlay sound in soundList) {
			if (typeSound == TYPE_SOUND.EFFECT)
				sound.setMute (!PrepClass.isEffect);
			else
				sound.setMute (!PrepClass.isBGM);
		}
	}


	/// <summary>
	/// 배경음 플레이
	/// </summary>
	/// <param name="soundCilp">Sound cilp.</param>
	/// <param name="key">Key.</param>
	public void bgmPlay(SoundPlay soundPlayer, string key, bool is3DSound = false, bool isLoop = false, float runTime = 1f){
		if (string.IsNullOrEmpty (key))
			return;

        if (m_bgmDic.ContainsKey(key))
        {
            soundPlayer.audioPlay(m_bgmDic[key], TYPE_SOUND.BGM, is3DSound, isLoop, runTime);
            m_soundList.Add(soundPlayer);
        }
        else
        {
            Debug.LogWarning("사운드 없음 : " + key);
        }
//		soundCilp.audioPlay ();
	}

	/// <summary>
	/// 효과음 플레이
	/// </summary>
	/// <param name="soundPlayer">Sound cilp.</param>
	/// <param name="key">Key.</param>
    public void effectPlay(SoundPlay soundPlayer, string key, bool is3DSound = true, bool isLoop = false, float runTime = 1f)
    {
		if (string.IsNullOrEmpty (key))
			return;
		
		if (m_effectDic.ContainsKey (key)) {
            soundPlayer.audioPlay(m_effectDic[key], TYPE_SOUND.EFFECT, is3DSound, isLoop, runTime);
			m_soundList.Add (soundPlayer);
		} else {
			Debug.LogWarning ("사운드 없음 : " + key);
		}
	}

	/// <summary>
	/// 버튼 효과음 플레이
	/// </summary>
	/// <param name="soundPlayer">Sound player.</param>
	/// <param name="typeBtnSound">Type button sound.</param>
    public void btnEffectPlay(SoundPlay soundPlayer, TYPE_BTN_SOUND typeBtnSound, float runTime = 1f)
    {
		effectPlay (soundPlayer, getBtnSoundKey (typeBtnSound), false, false, runTime);
	}


	string getBtnSoundKey(TYPE_BTN_SOUND typeBtnSound){		
		if (m_btnKeys.Length > 0 && m_btnKeys.Length > (int)typeBtnSound)
			return m_btnKeys [(int)typeBtnSound];
		return "";
	}
	/// <summary>
	/// 사운드 종료
	/// </summary>
	/// <param name="soundCilp">Sound cilp.</param>

	public void soundEnd(SoundPlay soundCilp){
		if(m_soundList.Contains(soundCilp)) m_soundList.Remove (soundCilp);
	}




}



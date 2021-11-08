using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UILoad : MonoBehaviour
{
	[SerializeField] Image m_imagePanel;
    [SerializeField] Text m_nameText;
	[SerializeField] Text m_tipText;
    [SerializeField] Image m_icon;
	[SerializeField] Slider m_loadBar;
	[SerializeField] Text m_loadPercentText;
//	[SerializeField] Text m_loadContentsText;

	AsyncOperation loadSceneAsync = null;
//	TipClass m_tipData = null;

    bool isSave = false;

	void Start(){

//		if(AccountClass.GetInstance.playPanel.nextPanel == null)
//			AccountClass.GetInstance.playPanel.nextPanel = PrepClass.c_MainPanelScene;
//		loadSceneAsync.allowSceneActivation
//		m_tipData = TipFactoryClass.GetInstance.getRandomTip();
//		m_imagePanel.sprite = m_tipData.image;
//		m_tipText.text = string.Format ("TIP {0}:{1}", m_tipData.name, m_tipData.contents);


//        Friend friend = FriendManager.GetInstance.getFriend(Account.GetInstance.friendKey);
//        if (friend == null)

        Friend friend;
        if(Account.GetInstance.friendKey != "")
            friend = FriendManager.GetInstance.getFriend(Account.GetInstance.friendKey);
        else
            friend = FriendManager.GetInstance.getRandomFriend(1, true)[0];

        m_icon.sprite = friend.icon;
        m_imagePanel.sprite = friend.image;
        m_nameText.text = friend.name;
        m_tipText.text = friend.getSpeech(TYPE_SPEECH.INFO);
        StartCoroutine(loadSceneCoroutine());
	}
	
	
	/// <summary>
	/// 게임 화면으로 이동
	/// </summary>
	/// <returns>The scene coroutine.</returns>
	IEnumerator loadSceneCoroutine(){	
        
        loadSceneAsync = SceneManager.LoadSceneAsync(Account.GetInstance.nextScene);

		while (!loadSceneAsync.isDone) {
//			Debug.Log("load Scene " + loadSceneAsync.progress);

            //저장 및 불러오기

			m_loadPercentText.text = string.Format("{0:f0}%", loadSceneAsync.progress * 100f);
			m_loadBar.value = loadSceneAsync.progress;
			yield return null;
		}
	}

}


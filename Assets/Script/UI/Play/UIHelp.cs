using UnityEngine;
using UnityEngine.UI;
using GamePanel;

public class UIHelp : UIPanel
{

    SoundPlay m_soundPlayer;

    [SerializeField]
    Image m_image;

    [SerializeField]
    Toggle m_toggle;


    void Start()
    {
        m_soundPlayer = GetComponent<SoundPlay>();
        if (m_soundPlayer == null)
            m_soundPlayer = gameObject.AddComponent<SoundPlay>();


        m_toggle.isOn = Account.GetInstance.isAdventureHelp(Account.GetInstance.adventureKey);

        m_toggle.onValueChanged.AddListener((isOn) => OnValueChanged(isOn));
    }


    public void setHelp(Adventure adventure)
    {
        m_image.sprite = adventure.helpImage;
        if (!Account.GetInstance.isAdventureHelp(adventure.key))
        {
            closePanel();
        }
        else
        {
            openPanel(null);
            Time.timeScale = 0f;
        }
    }

    public void OnValueChanged(bool isOn)
    {
        m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
        Account.GetInstance.setAdventureHelp(Account.GetInstance.adventureKey, isOn);
    }

    public void OnOkClicked()
    {
        m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
        closePanel();
    }

    public override void closePanel()
    {
        base.closePanel();
        if(!(parent is UIPause))
            Time.timeScale = 1f;
    }
}


using UnityEngine;
using UnityEngine.UI;

namespace GamePanel
{
    public class UIFriendAbility : MonoBehaviour
    {
        SoundPlay m_soundPlayer;

        Friend m_friend;

        [SerializeField]
        Text m_forceText;

        [SerializeField]
        Text m_forceIncreaseText;

        [SerializeField]
        Text m_dexText;

        [SerializeField]
        Text m_dexIncreaseText;

        [SerializeField]
        Text m_rangeText;

        [SerializeField]
        Text m_rangeIncreaseText;
        
        [SerializeField]
        Text m_feralText;

        [SerializeField]
        Text m_feralIncreaseText;
        
        [SerializeField]
        Text m_luckText;

        [SerializeField]
        Text m_luckIncreaseText;

        [SerializeField]
        UIFriendAbilityContents m_uiAbilityContent;


        void Start(){
            if (m_soundPlayer == null)
                m_soundPlayer = ((UILobby)UIPanelManager.GetInstance.root).soundPlayer;
        }

        public void setFriendInfo(Friend friend)
        {

            m_uiAbilityContent.gameObject.SetActive(false);

            m_friend = friend;
            
            m_dexText.text = string.Format("{0}", friend.dex);

            if(m_dexIncreaseText != null)
                m_dexIncreaseText.text = string.Format("{0}", friend.increaseDex);


            m_forceText.text = string.Format("{0}", friend.force);

            if (m_forceIncreaseText != null)
                m_forceIncreaseText.text = string.Format("{0}", friend.increaseForce);


            m_rangeText.text = string.Format("{0}", friend.range);

            if (m_rangeIncreaseText != null)
                m_rangeIncreaseText.text = string.Format("{0}", friend.increaseRange);


            m_feralText.text = string.Format("{0}", friend.feral);

            if (m_feralIncreaseText != null)
                m_feralIncreaseText.text = string.Format("{0}", friend.increaseFeral);


            m_luckText.text = string.Format("{0}", friend.luck);

            if (m_luckIncreaseText != null)
                m_luckIncreaseText.text = string.Format("{0}", friend.increaseLuck);
        }


        public void OnAbilityUpgradeClicked(int typeAbility)
        {
            int needBread = m_friend.getIncreaseAbilityCost(typeAbility);// increaseAbilities[typeAbility];

            if (Account.GetInstance.checkJapariBread(needBread))
            {
                m_soundPlayer.audioPlay("EffectOk", TYPE_SOUND.EFFECT);
                Account.GetInstance.useJapariBread(needBread);
                m_friend.upgradeAbility((TYPE_ABILITY)typeAbility);
            }
            else
            {
                //자파리빵 부족
                m_soundPlayer.audioPlay("EffectWarning", TYPE_SOUND.EFFECT);
                ((UILobby)(UIPanelManager.GetInstance.root)).uiMsg.setMsg("자파리빵이 부족합니다.", TYPE_MSG_PANEL.ERROR, TYPE_MSG_BTN.OK);
            }

            setFriendInfo(m_friend);
            UIPanelManager.GetInstance.root.uiUpdate();
        }

        public void OnAbilityContentsClicked(int typeAbility)
        {

            if (Camera.main.ScreenToViewportPoint(Input.mousePosition).x > 0.5f)
            {
                m_uiAbilityContent.GetComponent<RectTransform>().pivot = Vector2.right;
            }
            else
            {
                m_uiAbilityContent.GetComponent<RectTransform>().pivot = Vector2.zero;
            }

            m_uiAbilityContent.transform.position = Input.mousePosition;
            m_uiAbilityContent.setContents((TYPE_ABILITY)typeAbility);
        }

        void OnDisable()
        {
            m_uiAbilityContent.gameObject.SetActive(false);
        }


    }
}


using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

//1.5
public class AchivementManager : SingletonClass<AchivementManager>
{

    enum TYPE_ACHIVEMENT_DATA {KEY, ACHIVEMENT, VALUE, NAME, CONTENTS, AWARD}

    Dictionary<string, Achivement> m_achivementDic = new Dictionary<string, Achivement>();

    public Dictionary<string, Achivement> achivementDic { get { return m_achivementDic; } }

    public AchivementManager()
    {
        initParse();
    }

    void initParse()
    {
        Sprite[] achivementIcons = Resources.LoadAll<Sprite>(PrepClass.achivementIconPath);

        TextAsset achivementField = Resources.Load<TextAsset>(PrepClass.achivementDataPath);

        if (achivementField != null)
        {
            string[] achivementRecord = achivementField.text.Split('\n');

            for (int i = 0; i < achivementRecord.Length; i++)
            {
                string[] achivementWord = achivementRecord[i].Split('\t');
                string key = achivementWord[(int)TYPE_ACHIVEMENT_DATA.KEY];
                key = key.Trim();

                if (achivementWord.Length == Enum.GetValues(typeof(TYPE_ACHIVEMENT_DATA)).Length)
                {
//                    string imageKey = achivementWord[(int)TYPE_ACHIVEMENT_DATA.KEY].Trim();
                    Sprite icon = achivementIcons.Where(iconSp => iconSp.name == key).SingleOrDefault();
                    if (icon == null) Debug.LogWarning(string.Format("{0} 아이콘 없음", key));

                    Achivement.TYPE_ACHIVEMENT typeAchivement;

                    int intType = 0;
                    if (!int.TryParse(achivementWord[(int)TYPE_ACHIVEMENT_DATA.ACHIVEMENT], out intType))
                    {
                        Debug.LogWarning(string.Format("{0} 데이터 없음 {1}", key, achivementWord[(int)TYPE_ACHIVEMENT_DATA.ACHIVEMENT]));
                    }
                    typeAchivement = (Achivement.TYPE_ACHIVEMENT)intType;
                    
                    int value;
                    if (!int.TryParse(achivementWord[(int)TYPE_ACHIVEMENT_DATA.VALUE], out value))
                        value = 100;


                    string name = achivementWord[(int)TYPE_ACHIVEMENT_DATA.NAME];

                    string contents = achivementWord[(int)TYPE_ACHIVEMENT_DATA.CONTENTS];

                    int award;
                    if (!int.TryParse(achivementWord[(int)TYPE_ACHIVEMENT_DATA.AWARD], out award))
                        award = 1;

                    Achivement achivement =
                        new Achivement(
                            key,
                            icon,
                            typeAchivement,
                            value,
                            name,
                            contents,
                            award
                            );

                    m_achivementDic.Add(key, achivement);


                }
                else
                {
                    if (key != "")
                        Debug.LogError(string.Format("{0}의 데이터가 많거나 부족합니다. {1}", key, achivementWord.Length));
                }
            }
        }
        else
        {
            Debug.LogError(string.Format("{0}의 데이터를 찾지 못했습니다", PrepClass.achivementDataPath));
        }
    }


    
    /// <summary>
    /// 업적 체크하기
    /// </summary>
    /// <param name="typeAchivement"></param>
    /// <returns></returns>
    public Achivement getSuccessAchivement(Achivement.TYPE_ACHIVEMENT typeAchivement){

        //다음 업적 가져오기 - 완료된 업적은 가져오지 않음
        //

        List<Achivement> achivementList = m_achivementDic.Values.Where(achive => achive.typeAchivement == typeAchivement).ToList<Achivement>();


        if(achivementList.Count > 0){
            foreach(Achivement achive in achivementList){
                //업적이 없을때
                if(!Account.GetInstance.isAchivement(achive)){
                    //업적의 조건에 부합하면
                    if (checkAchivementValue(typeAchivement, achive.value))
                    {
                        //업적 등록
                        //업적 등록키 가져오기
                        return achive;
                    }
                }
            }
        }
        return null;
    }

    bool checkAchivementValue(Achivement.TYPE_ACHIVEMENT typeAchivement, int value)
    {
        return (Account.GetInstance.getTypeAchivement(typeAchivement) >= value);
    }

}


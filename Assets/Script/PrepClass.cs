using System;
using UnityEngine;

public class PrepClass
{

    public static readonly string achivementIconPath = "Image/Achivement";
    public static readonly string achivementDataPath = "Data/Achivement";

    public static readonly string moverPath = "Prefebs/Fields/Game@BlockMover";

//    public static readonly string blockPrefebsPath = "Prefebs/fields";
    public static readonly string gardenDataPath = "Data/Garden";
    public static readonly string gardenImagePath = "Image/Garden/Image";
    public static readonly string gardenIconPath = "Image/Garden/Icon";

    public static readonly string blockPrefebsPath = "Prefebs/fields";
    public static readonly string blockIconPath = "Image/Blocks";
    public static readonly string blockDataPath = "Data/Blocks";

    public static readonly string friendDataPath = "Data/Friends";
    public static readonly string friendImagePath = "Image/Friends/Images";
    public static readonly string friendScriptsPath = "Data/FriendScripts";
    public static readonly string friendIconPath = "Image/Friends/Icons";

    public static readonly string particleEffectPath = "Prefebs/Effect";

//    public static readonly string adventureIconPath = "Image/Adventure/Icon";
    public static readonly string adventureImagePath = "Image/Adventure";
//    public static readonly string adventureClearPath = "Image/Adventure/Clear";
//    public static readonly string adventureDefeatPath = "Image/Adventure/Defeat";
    public static readonly string adventureDataPath = "Data/Adventures";

    public static readonly string itemIconPath = "Image/Item";
    public static readonly string itemDataPath = "Data/Item";

//    public const int defaultExperiance = 100;
//    public const float increaseExperiance = 0.15f;

    public static readonly string blockTag = "Block";
    public static readonly string moverTag = "Mover";

    public static readonly string fileName = "friend";

    public const int defaultFeral = 100;
    public const int defaultIncreaseAbility = 100;
    public const int defaultIncreaseCost = 10;

    public static readonly float[] increaseAbilities = { 0.5f, 0.3f, 0.15f, 0.2f, 0.125f };
    public static readonly float[] defaultAbilities = { 1f, 0.01f, 0.0075f, 0.01f, 0.01f };

    public readonly int[] healthValue = {1, 150, 350, 600, 900};

    //100 이상 사용 금지 - 1~99
    public const int xCnt = 7;
    public const int yCnt = 7;
    public const int maxCnt = 100;

    public const int defaultScore = 10;

    public const float frameTime = 0.05f;


    public static bool isBGM = true;
    public static bool isEffect = true;
    public static bool isVoice = true;

    public static bool isOffline = false;


}


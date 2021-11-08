using System;
using System.Collections.Generic;
using UnityEngine;

public class Adventure
{


    public enum TYPE_ADVENTURE_IMAGE{Icon, Info, Clear, Defeat, BG, Help}

    string m_key;
    string m_bgmKey;
    
    Sprite[] m_images;
    Sprite[] m_icons;

    string m_name;
    string m_contents;
    string m_goal;
    int m_cost;
    float m_eventTime;

    string m_leaderboardKey;

    IAdventure m_iAdventure;


    public string key { get { return m_key; }  }
    public string bgmKey { get { return m_bgmKey; } }
    public string leaderboardKey { get { return m_leaderboardKey; } }
    public Sprite iconImage { get { return m_images[(int)TYPE_ADVENTURE_IMAGE.Icon]; } }
    public Sprite infoImage { get { return m_images[(int)TYPE_ADVENTURE_IMAGE.Info]; } }
    public Sprite clearImage { get { return m_images[(int)TYPE_ADVENTURE_IMAGE.Clear]; } }
    public Sprite defeatImage { get { return m_images[(int)TYPE_ADVENTURE_IMAGE.Defeat]; } }
    public Sprite BGImage { get { return m_images[(int)TYPE_ADVENTURE_IMAGE.BG]; } }
    public Sprite helpImage { get { return m_images[(int)TYPE_ADVENTURE_IMAGE.Help]; } }
    public string name { get { return m_name; } }
    public string contents { get { return m_contents; }  }
    public string goal { get { return m_goal; } }
    public int cost { get { return m_cost; }  }
    public float eventTime { get { return m_eventTime; } }
    public Type adventureType { get { return m_iAdventure.GetType(); } }

    public Adventure(string key, Sprite[] images, string name, string contents, string goal, int cost, float eventTime, string bgmKey)
    {
        m_key = key;
        m_bgmKey = bgmKey;
        m_images = (Sprite[])images.Clone();
        m_name = name;
        m_contents = contents;
        m_goal = goal;
        m_cost = cost;
        m_eventTime = eventTime;
        m_iAdventure = createAdventure(key);

    }


    public BlockMover createMover(AdventureActor.BlockMoverDelegate blockMoverDel)
    {
        return m_iAdventure.createMover(blockMoverDel);
    }

    public BlockMoverParent createBlockMover()
    {
        return m_iAdventure.createBlockMover();
    }

//    Adventure getAdventure(string key, Sprite[] images, string name, string contents, string goal, int cost, float eventTime, string bgmKey)
    IAdventure createAdventure(string key)
    {
        switch (key)
        {
            case "MaterialHarvest":
                m_leaderboardKey = GPGSIds.leaderboard_material_harvest;
                return new AdventureHarvest(key);
//                return new AdventureHarvest(key, images, name, contents, goal, cost, eventTime, bgmKey);
            case "CeruleanRepulse":
                m_leaderboardKey = GPGSIds.leaderboard_cerulean_refulse;
                return new AdventureRepulse(key);
//                break;
//                return new AdventureRepulse(key, images, name, contents, goal, cost, eventTime, bgmKey);
            case "RuinsExplore":
                m_leaderboardKey = GPGSIds.leaderboard_ruins_explore;
                return new AdventureExplore(key);
//                break;
//                return new AdventureExplore(key, images, name, contents, goal, cost, eventTime, bgmKey);
//            case "PPPDance":
//                return new AdventureDance(key);
//                break;

//                return new AdventureDance(key, images, name, contents, goal, cost, eventTime, bgmKey);
            case "BusDrive":
                m_leaderboardKey = GPGSIds.leaderboard_bus_drive;
                return new AdventureDrive(key);
//                break;
//                return new AdventureDrive(key, images, name, contents, goal, cost, eventTime, bgmKey);
            case "Football":
                m_leaderboardKey = GPGSIds.leaderboard_football_play;
                return new AdventureFootball(key);
            default:
                Debug.LogError(string.Format("{0} 해당하는 키에 맞는 모험을 찾을 수 없습니다.", key));
                break;
        }
        return null;
    }

    public Block getCPUBlock()
    {
        return m_iAdventure.getCPUBlock();
    }

    /// <summary>
    /// 블록 배치 초기화
    /// </summary>
    /// <param name="blockActorArray"></param>
    public void batchBlock(BlockActor[][] blockActorArray, float rate, AdventureActor.BlockActorDelegate blockActorDel)
    {
        m_iAdventure.batchBlock(blockActorArray, rate, blockActorDel);
    }

    public UIButton.TYPE_BTN_PANEL useBtnPanel()
    {
        return m_iAdventure.useBtnPanel();
    }

    public void feralStart(Field field)
    {
        m_iAdventure.feralStart(field);
    }

    public void feralEnd(Field field)
    {
        m_iAdventure.feralEnd(field);
    }

    /// <summary>
    /// 행동 이벤트
    /// </summary>
    /// <param name="blockActorField"></param>
    /// <param name="nowX"></param>
    /// <param name="nowY"></param>
    /// <param name="rangeX"></param>
    /// <param name="rangeY"></param>
    /// <param name="isCerulean"></param>
    /// <param name="isFeral"></param>
    /// <returns></returns>
    //protected virtual List<BlockActor> indexAction(
    //    BlockActor[][] blockActorField,
    //    int nowX,
    //    int nowY,
    //    int depth = 0,
    //    bool isFeral = false,
    //    int rangeX = 1,
    //    int rangeY = 1
    //    )
    //{
    //    int indexY = nowY;
    //    int indexX = nowX;

    //    for (int y = -rangeY; y <= rangeY; y++)
    //    {
    //        indexY = nowY + y;

    //        if (indexY < 0)
    //        {
    //            indexY = 0;
    //        }
    //        else if (indexY >= PrepClass.yCnt)
    //            break;

    //        for (int x = -rangeX; x <= rangeX; x++)
    //        {
    //            indexX = nowX + x;

    //            if (indexX < 0)
    //            {
    //                indexX = 0;
    //            }
    //            else if (indexX >= PrepClass.xCnt)
    //                break;


    //            indexAction(blockActorField, blockActorField[indexY][indexX], isFeral, depth + 1);

    //        }
    //    }

    //    return blockList;
    //}


    /// <summary>
    /// 행동 이벤트
    /// </summary>
    /// <param name="blockActorField"></param>
    /// <param name="blockActor"></param>
    /// <param name="isFeral"></param>
    /// <returns></returns>
    public virtual List<BlockActor> indexAction(
        BlockActor[][] blockActorField,
        BlockActor blockActor,
        bool isFeral,
        int depth = 0
        )
    {
        return m_iAdventure.indexAction(blockActorField, blockActor, isFeral, depth);

        //blockActor = checkBlock(blockActor, depth, isFeral);
        //if (blockActor != null)
        //{
        //    blockActor.depth = depth;
        //    blockList.Add(blockActor);

        //    if (depth == 0)
        //        indexAction(blockActorField, blockActor.indexX, blockActor.indexY, depth, isFeral);

        //    if (blockActor.typeBlock == TYPE_BLOCK.SANDSTAR)
        //        checkSandStar(blockActorField, blockActor, isFeral, depth);
        //}
        //return blockList;
    }


    public void indexAction(BlockMover blockMover, BlockActor blockActor)
    {
        m_iAdventure.indexAction(blockMover, blockActor);
    }

    /// <summary>
    /// 블록 상태 체크
    /// </summary>
    /// <param name="blockActor"></param>
    /// <param name="isFeral"></param>
    /// <returns></returns>
    public BlockActor checkBlock(BlockActor blockActor, int depth, bool isFeral = false)
    {
        return m_iAdventure.checkBlock(blockActor, depth, isFeral);
    }

    /// <summary>
    /// 모험 이벤트
    /// </summary>
    /// <param name="blockActorField"></param>
    public float adventureEvent(Field field)
    {
        return m_iAdventure.adventureEvent(field, eventTime);
    }

    /// <summary>
    /// 데미지 계산
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="isFeral"></param>
    /// <returns></returns>
    public int calculateDamage(int damage, bool isFeral)
    {
        return m_iAdventure.calculateDamage(damage, isFeral);
    }

    /// <summary>
    /// 샌드스타 체크
    /// </summary>
    /// <param name="blockActorField"></param>
    /// <param name="blockActor"></param>
    /// <param name="isFeral"></param>
//    protected void checkSandStar(BlockActor[][] blockActorField, BlockActor blockActor, bool isFeral, int depth);



    /// <summary>
    /// 등록되어있는 모든 블록 없애기
    /// </summary>
    public void clearBlockList() {
        m_iAdventure.clearBlockList();

        //foreach (BlockActor blockActor in blockList)
        //    blockActor.resetDepth();

        //blockList.Clear(); 
    }

    /// <summary>
    /// 블록 가져오기
    /// </summary>
    /// <param name="blockActor"></param>
    public virtual Block createBlock(BlockActor blockActor, float rate, bool isEmpty = true)
    {
        return m_iAdventure.createBlock(blockActor, rate, isEmpty);

        //Block block = (isEmpty) ?
        //    BlockManager.GetInstance.getEmptyBlock() :
        //    BlockManager.GetInstance.getRandomBlock(m_key, rate);

        //if (block != null)
        //    blockActor.setBlock(block);

        //return block;
    }


    /// <summary>
    /// 게임 패배 조건
    /// </summary>
    /// <param name="blockActorField"></param>
    /// <returns></returns>
    public virtual bool isDefeat(BlockActor[][] blockActorField)
    {
        return m_iAdventure.isDefeat(blockActorField);
//        return false;
    }

    /// <summary>
    /// 게임 패배 조건
    /// </summary>
    /// <param name="blockActor"></param>
    /// <returns></returns>
    public virtual bool isDefeat(BlockActor blockActor, bool isFeral)
    {
        return m_iAdventure.isDefeat(blockActor, isFeral);
//        return false;
    }

    /// <summary>
    /// 게임 패배 조건
    /// </summary>
    /// <param name="loseCount"></param>
    /// <returns></returns>
    public virtual bool isDefeat(int loseCount)
    {
        return m_iAdventure.isDefeat(loseCount);
//        return false;
    }

/// <summary>
/// 블록 모두 채우기
/// </summary>
/// <param name="blockActorArray">블록배열</param>
/// <param name="toTypeBlock">변경할 블록</param>
/// <param name="toTypeValue">블록 값</param>
    //protected void fieldFillBread(BlockActor[][] blockActorArray, TYPE_BLOCK toTypeBlock, TYPE_VALUE toTypeValue = TYPE_VALUE.VALUE_00)
    //{
    //    for (int y = 0; y < PrepClass.yCnt; y++)
    //    {
    //        for (int x = 0; x < PrepClass.xCnt; x++)
    //        {
    //            Block block = BlockManager.GetInstance.getTypeBlock(key, toTypeBlock, toTypeValue);

    //            if (block != null)
    //            {
    //                if (blockActorArray[y][x].typeBlock == TYPE_BLOCK.NONE)
    //                {
    //                    blockActorArray[y][x].setBlock(block);
    //                }
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 블록 변경하기
    /// </summary>
    /// <param name="blockActorArray">블록배열</param>
    /// <param name="fromTypeBlock">변경 당하는 블록</param>
    /// <param name="toTypeBlock">변경 하는 블록</param>
    /// <param name="toTypeValue">블록값</param>
    //protected void blockChange(BlockActor[][] blockActorArray, TYPE_BLOCK fromTypeBlock, TYPE_BLOCK toTypeBlock, TYPE_VALUE toTypeValue = TYPE_VALUE.VALUE_00)
    //{
    //    for (int y = 0; y < PrepClass.yCnt; y++)
    //    {
    //        for (int x = 0; x < PrepClass.xCnt; x++)
    //        {

    //            if (blockActorArray[y][x].typeBlock == fromTypeBlock)
    //            {

    //                Block block = BlockManager.GetInstance.getTypeBlock(key, toTypeBlock, toTypeValue);

    //                if (block != null)
    //                {
    //                    blockActorArray[y][x].setBlock(block);
    //                }
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 야성해방 계산하기
    /// </summary>
    /// <param name="block"></param>
    /// <param name="combo"></param>
    /// <returns></returns>
    public int calculateFeral(Block block, int combo)
    {
        return m_iAdventure.calculateFeral(block, combo);
    }
    
    public int getJapariBread(AdventureActor.TYPE_JAPARIRATE typeJapariRate, int value)
    {
        return m_iAdventure.getJapariBread(typeJapariRate, value);
//        return (int)((float)value * m_japariRate[(int)typeJapariRate]);
    }
}



using System.Collections.Generic;
using UnityEngine;


public class AdventureHarvest : AdventureBlock, IAdventure
{

    public AdventureHarvest(string key) : base(key) {
        m_japariRate = new float[] { 0.05f, 10f, 0.25f, 0.5f };
    }

    //public AdventureHarvest(string key, Sprite[] images, string name, string contents, string goal, int cost, float eventTime, string bgmKey) :
    //    base(key, images, name, contents, goal, cost, eventTime, bgmKey)
    //{
    //    m_japariRate = new float[]{0.05f, 10f, 0.25f, 0.5f};
    //}

    public override void batchBlock(BlockActor[][] blockActorArray, float rate, BlockActorDelegate blockActorDel)
    {
        for (int y = 0; y < PrepClass.yCnt; y++)
        {
            for (int x = 0; x < PrepClass.xCnt; x++)
            {
                createBlock(blockActorArray[y][x], rate, false);
            }
        }
    }

    //public override List<BlockActor> indexAction(BlockActor[][] blockActorField, int nowX, int nowY, int rangeX = 1, int rangeY = 1, bool isFeral = false)
    //{
    //    int indexY = nowY;
    //    int indexX = nowX;

    //    for (int y = -rangeY; y <= rangeY; y++)
    //    {
    //        indexY = nowY + y;

    //        if (indexY < 0)
    //            continue;
    //        else if (indexY >= PrepClass.yCnt)
    //            break;

    //        for (int x = -rangeX; x <= rangeX; x++)
    //        {
    //            indexX = nowX + x;

    //            if (indexX < 0)
    //                continue;
    //            else if (indexX >= PrepClass.xCnt)
    //                break;

    //            BlockActor blockActor = checkBlock(blockActorField[indexY][indexX], isFeral);

    //            if (blockActor != null)
    //            {
    //                blockList.Add(blockActor);
    //                if (blockActor.block.typeBlock == TYPE_BLOCK.SANDSTAR)
    //                    checkSandStar(blockActorField, blockActor, isFeral);
    //            }
    //        }
    //    }

    //    return blockList;
    //}

    //public override List<BlockActor> indexAction(BlockActor[][] blockActorField, BlockActor blockActor, bool isFeral)
    //{
    //    return null;
    //}
    

    public override BlockActor checkBlock(BlockActor blockActor, int depth, bool isFeral = false)
    {
        if (!blockList.Contains(blockActor))
        {
            switch (blockActor.typeBlock)
            {
                case TYPE_BLOCK.CERULEAN:
                    if (!isFeral)
                    {
                        return null;
                    }
                    break;
            }
            return blockActor;
        }
        return null;
    }

    protected override void checkSandStar(BlockActor[][] blockActorField, BlockActor blockActor, bool isFeral, int depth)
    {
        int indexX = blockActor.indexX;
        int indexY = blockActor.indexY;

        switch (blockActor.typeValue)
        {
            //크게먹기
            case TYPE_VALUE.VALUE_00:
                indexAction(blockActorField, indexX, indexY, depth, isFeral, 2, 2);
                break;
            //야성해방 스코어 증가
            //case TYPE_VALUE.VALUE_01:
            //    indexAction(blockActorField, indexX, indexY, 1, 1, isFeral);
            //    break;
            //가로공격
            case TYPE_VALUE.VALUE_02:
                indexAction(blockActorField, indexX, indexY, depth, isFeral, 7, 0);
                break;
            //세로공격
            case TYPE_VALUE.VALUE_03:
                indexAction(blockActorField, indexX, indexY, depth, isFeral, 0, 7);
                break;
            //가로세로 공격
            case TYPE_VALUE.VALUE_04:
                indexAction(blockActorField, indexX, indexY, depth, isFeral, 0, 7);
                indexAction(blockActorField, indexX, indexY, depth, isFeral, 7, 0);
                break;
            default:
                Debug.LogError(string.Format("{0} 해당하는 샌드스타 데이터가 없습니다.", blockActor.typeValue)); 
                break;
        }

    }

    /// <summary>
    /// 세룰리안으로 뒤덮이면 패배
    /// </summary>
    /// <param name="blockActorField"></param>
    /// <returns></returns>
    public override bool isDefeat(BlockActor[][] blockActorField)
    {
        int totalCerulean = PrepClass.yCnt * PrepClass.xCnt;
        for (int y = 0; y < PrepClass.yCnt; y++)
        {
            for (int x = 0; x < PrepClass.xCnt; x++)
            {
                if (blockActorField[y][x].typeBlock == TYPE_BLOCK.CERULEAN)
                {
                    totalCerulean--;
                }
            }
        }
        
        if (totalCerulean <= 0)
            return true;

        return false;
    }

    public override float adventureEvent(Field field, float eventTime)
    {
        return eventTime;
    }

    public override Block createBlock(BlockActor blockActor, float rate, bool isDefault = true)
    {
        return base.createBlock(blockActor, rate, false);
    }

    public override int calculateFeral(Block block, int combo)
    {
        return 1;
    }


}


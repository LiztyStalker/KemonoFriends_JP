using System.Collections.Generic;
using UnityEngine;

public class AdventureDance : AdventureBlock, IAdventure
{
    //public AdventureDance(string key, Sprite[] images, string name, string contents, string goal, int cost, float eventTime, string bgmKey) :
    //    base(key, images, name, contents, goal, cost, eventTime, bgmKey)
    //{
    //}

    public AdventureDance(string key) : base(key) { }

    public override void batchBlock(BlockActor[][] blockActorArray, float rate, BlockActorDelegate blockActorDel)
    {
        for (int y = 0; y < PrepClass.yCnt; y++)
        {
            for (int x = 0; x < PrepClass.xCnt; x++)
            {
                createBlock(blockActorArray[y][x], rate);
            }
        }
    }

    public override BlockActor checkBlock(BlockActor blockActor, int depth, bool isFeral = false)
    {
        if (!blockList.Contains(blockActor))
        {
            switch (blockActor.typeBlock)
            {
                case TYPE_BLOCK.BLOCK:
                    break;
                case TYPE_BLOCK.COIN:
                    break;
                case TYPE_BLOCK.SANDSTAR:
                    //각종 아이템 효과 발동
                    //각 아이템에 따른 인덱스 체크
                    break;
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

    //public override List<BlockActor> indexAction(BlockActor[][] blockActorField, int nowX, int nowY, int rangeX = 0, int rangeY = 0, bool isFeral = false)
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
    //                if (blockActor.typeBlock != TYPE_BLOCK.NONE)
    //                {
    //                    blockList.Add(blockActor);
    //                    if (blockActor.block.typeBlock == TYPE_BLOCK.SANDSTAR)
    //                        checkSandStar(blockActorField, blockActor, isFeral);
    //                }
    //            }
    //        }
    //    }

    //    return blockList;
    //}

    //public override bool isLose(BlockActor[][] blockActorField, int loseCount)
    //{
    //    return false;
    //}

    protected override void checkSandStar(BlockActor[][] blockActorField, BlockActor blockActor, bool isFeral, int depth)
    {

    }

    public override float adventureEvent(Field field, float eventTime)
    {
        bool isCheck = false;
        while (!isCheck)
        {
            int indexX = Random.Range(0, PrepClass.xCnt);
            int indexY = Random.Range(0, PrepClass.yCnt);

            if (field.blockActorField[indexY][indexX].typeBlock == TYPE_BLOCK.NONE)
            {
                createBlock(field.blockActorField[indexY][indexX], field.friend.realLuck, false);
                isCheck = true;
            }
        }
        return eventTime;
    }

    public override int calculateFeral(Block block, int combo)
    {
        return 0;
    }
}


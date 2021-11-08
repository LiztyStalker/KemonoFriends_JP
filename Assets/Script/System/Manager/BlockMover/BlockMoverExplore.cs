using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BlockMoverExplore : BlockMoverParent, IBlockMover
{

    public override void setFeral(BlockMover blockMover, bool isFeral) {
        base.setFeral(blockMover, isFeral);
    }


    public override void setArrowChange(BlockMover blockMover, int selector, TYPE_ARROW typeArrow)
    {
        setArrowChange(blockMover, getArrow(selector, typeArrow));
    }



    TYPE_ARROW getArrow(int selector, TYPE_ARROW typeArrow)
    {

        int intArrow = (int)typeArrow;
        //오른쪽
        if (selector > 0)
        {
            intArrow = intArrow * 2;
        }
        //왼쪽
        else if (selector < 0)
        {
            intArrow = intArrow / 2;
        }

        if (intArrow == 0)
            intArrow = (int)TYPE_ARROW.UP;
        else if (intArrow > (int)TYPE_ARROW.UP)
            intArrow = (int)TYPE_ARROW.RIGHT;

        return (TYPE_ARROW)intArrow;
    }

    public override float angleView(TYPE_ARROW typeArrow)
    {

        switch (typeArrow)
        {
            case TYPE_ARROW.RIGHT:
                return 270f;
            case TYPE_ARROW.DOWN:
                return 180f;
            case TYPE_ARROW.LEFT:
                return 90f;
//            case TYPE_ARROW.UP:
            default:
                return 0f;

        }
    }

    public override bool autoCheck()
    {
        return true;
    }

    public override bool crashAction(Field field, BlockMover blockMover, BlockMover crashBlockMover)
    {
        //야생해방상태이면 반대로 잡아먹힘
        if (field.isFeral)
        {

            //중앙으로 이동
            crashBlockMover.nextBlockActor = field.blockActorField[3][3];
            //8초 후에 부활
            //스코어 올리기
            field.catchCerulean(crashBlockMover.block);

            crashBlockMover.soundPlay("EffectHit", TYPE_SOUND.EFFECT);
            return true;

        }
        
        return false;
    }

    public override bool moverController(Field field, BlockMover blockMover)
    {

        blockMover.setFeral(field.isFeral);

        if (blockMover.typeUser == BlockMover.TYPE_USER.CPU)
        {
            blockMover.typeArrow = randomArrow();
        }


        //아이템 습득 - 충돌체에서 이벤트 발생
        catchBlock(field, blockMover, blockMover.nowBlockActor, blockMover.nextBlockActor);


        if (getNextBlockActor(blockMover, blockMover.isAuto) != null)
        {
            moveAction(blockMover);
        }
        return true;

    }
}


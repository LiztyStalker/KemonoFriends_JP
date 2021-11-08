using System;
using UnityEngine;

public class BlockMoverDrive : BlockMoverParent, IBlockMover
{
    public override void setArrowChange(BlockMover blockMover, int selector, TYPE_ARROW typeArrow)
    {
        setArrowChange(blockMover, getArrow(selector, typeArrow));
    }

    TYPE_ARROW getArrow(int selector, TYPE_ARROW typeArrow)
    {
        if (selector > 0)
            return TYPE_ARROW.RIGHT;
        else if (selector < 0)
            return TYPE_ARROW.LEFT;

        return typeArrow;
    }

    public override bool autoCheck()
    {
        return false;
    }

    public override void setFeral(BlockMover blockMover, bool isFeral)
    {
        base.setFeral(blockMover, isFeral);

        if (isFeral)
        {
            blockMover.transform.localScale = new Vector3(2f, 2f, 2f);
            var particle = blockMover.feralParticle.main;
            particle.startSizeMultiplier = 4f;
        }
        else
        {
            blockMover.transform.localScale = defaultSize;
            var particle = blockMover.feralParticle.main;
            particle.startSizeMultiplier = 2f;
        }
    }

    public override bool crashAction(Field field, BlockMover blockMover, BlockMover crashBlockMover)
    {
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


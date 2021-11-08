using System;
using UnityEngine;

//1.4
public class BlockMoverBall : BlockMoverParent, IBlockMover
{
    public override bool autoCheck()
    {
        return false;
    }

    public override void setArrowChange(BlockMover blockMover, int selector, TYPE_ARROW typeArrow)
    {

    }

    public override bool crashAction(Field field, BlockMover blockMover, BlockMover crashBlockMover)
    {
        //반대쪽으로 이동
        return false;
    }

    public override bool moverController(Field field, BlockMover blockMover)
    {
        //가속도
        //Vector2 velocity = blockMover.GetComponent<Rigidbody2D>().velocity;

        //줄여나가야 함

        //최소 속도까지




        //스크린 밖으로 나가면 게임 오버
        //
        Vector3 nowPos = Camera.main.WorldToViewportPoint(blockMover.transform.position);

        //        Debug.Log(nowPos);


        if (!field.isFeral)
        {
            if (nowPos.x <= -0.1f || nowPos.x >= 1.1f)
            {
                field.setDefeat();
            }
            if (nowPos.y <= 0.15f || nowPos.y >= 0.95f)
            {
                field.setDefeat();
            }
        }
        else
        {
            Vector2 dir = blockMover.GetComponent<Rigidbody2D>().velocity;

            if (nowPos.x <= -0.1f || nowPos.x >= 1.1f)
                dir.x = -dir.x;

            if (nowPos.y <= 0.15f || nowPos.y >= 0.95f)
                dir.y = -dir.y;

            blockMover.GetComponent<Rigidbody2D>().velocity = dir;
        }

        return true;
    }
}


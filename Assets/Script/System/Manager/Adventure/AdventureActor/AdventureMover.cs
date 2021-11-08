using System;
using UnityEngine;

public abstract class AdventureMover : AdventureActor
{



    BlockMover m_resourceBlockMover;

    protected BlockMover resourceBlockMover { get { return m_resourceBlockMover; } }

    public AdventureMover(string key)
        : base(key)
    {
        m_resourceBlockMover = Resources.Load<BlockMover>(PrepClass.moverPath);
    }

}


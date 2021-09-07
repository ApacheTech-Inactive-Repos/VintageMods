﻿using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace VintageMods.Mods.WaypointExtensions.Behaviours.Blocks
{
    public class TeleporterInteractBehaviour : BlockBehavior
    {
        public TeleporterInteractBehaviour(Block block) : base(block)
        {

        }

        public override void OnBlockInteractStop(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel,
            ref EnumHandling handling)
        {
            base.OnBlockInteractStop(secondsUsed, world, byPlayer, blockSel, ref handling);
            
        }
    }
}

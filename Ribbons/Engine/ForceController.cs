﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ribbons.Input;

namespace Ribbons.Engine
{
    /// <summary>
    /// Polls input for Seamstress and Ribbon related button presses, and notifies those objects appropriately.
    /// </summary>
    public class ForceController : IUpdate
    {
        private InputController inputController;
        private Player player;

        public ForceController(InputController inputController, Player player)
        {
            this.inputController = inputController;
            this.player = player;
        }

        /// <summary>
        /// Polls input and notifies player and ribbon.
        /// </summary>
        public void Update(float dt)
        {
            // seamstress input:
            if (inputController.SeamstressLeft.Pressed)
            {
                player.MoveLeft(inputController.SeamstressLeft.Value);
            }
            if (inputController.SeamstressRight.Pressed)
            {
                player.MoveRight(inputController.SeamstressRight.Value);
            }

            if (inputController.SeamstressJump.JustPressed)
            {
                player.Jump();
            }
            if (inputController.SeamstressJump.Pressed)
            {
                player.ContinueJump();
            }

            // TODO: ribbon input
            if (player.Ribbon != null)
            {
                if (inputController.RibbonLeft.Pressed)
                {
                    player.Ribbon.MoveLeft(inputController.RibbonLeft.Value);
                }
                if (inputController.RibbonRight.Pressed)
                {
                    player.Ribbon.MoveRight(inputController.RibbonRight.Value);
                }
                if (inputController.RibbonFlip.Pressed)
                {
                    player.RibbonFlip();
                }
            }
        }
    }
}

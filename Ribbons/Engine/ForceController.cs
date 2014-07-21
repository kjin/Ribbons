using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ribbons.Input;

namespace Ribbons.Engine
{
    /// <summary>
    /// Polls input for Seamstress and Ribbon related button presses, and notifies those objects appropriately.
    /// </summary>
    public class ForceController
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
        public void Update()
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
        }
    }
}

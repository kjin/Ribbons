using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ribbons.Input;

namespace Ribbons.Engine
{
    public class ForceController
    {
        private InputController inputController;
        private Player player;

        public ForceController(InputController inputController, Player player)
        {
            //inputController
            this.inputController = inputController;
            this.player = player;
        }

        public void Update()
        {
            if (inputController.SeamstressLeft.Pressed)
            {
                player.MoveLeft(1.0f);
            }
            if (inputController.SeamstressRight.Pressed)
            {
                player.MoveRight(1.0f);
            }
        }
    }
}

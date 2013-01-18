using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Glitch.Engine.Core;

namespace Glitch.Engine.Input.Devices
{
    /// <summary>
    /// TODO Joypad support
    /// </summary>
#if WINDOWS
    public class JoypadDevice : Device
    {
        private JoystickState _joyState, _previousJoystate;

        public JoypadDevice(LogicalPlayerIndex logicalPlayerIndex, int index)
            : base(logicalPlayerIndex,DeviceType.Joystick, index)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _joyState = GlitchApplication.InputManager.GetJoystickState(Index);

            // Mapping
            // TODO Joypad mapping

            // End
            _previousJoystate = _joyState;
        }

        public override bool IsConnected
        {
            get { return GlitchApplication.InputManager.JoystickManager.ConnectedJoystick[Index] == true; }
        }
    }
#endif
}

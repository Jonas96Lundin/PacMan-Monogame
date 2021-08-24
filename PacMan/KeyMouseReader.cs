using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

static class KeyMouseReader
{
	public static KeyboardState keyState, oldKeyState = Keyboard.GetState();
	public static MouseState mouseState, oldMouseState = Mouse.GetState();
    public static GamePadState padState = GamePad.GetState(PlayerIndex.One), oldPadState = GamePad.GetState(PlayerIndex.One);
    public static GamePadCapabilities c = GamePad.GetCapabilities(PlayerIndex.One);
    public static bool KeyPressed(Keys key) {
        return keyState.IsKeyDown(key);
	}
    public static bool KeyPressedOnce(Keys key)
    {
        return keyState.IsKeyDown(key) && oldKeyState.IsKeyUp(key);
    }
    public static bool GamePadPressed(Buttons button)
    {
        return padState.IsButtonDown(button);
    }
    public static bool GamePadPressedOnce(Buttons button)
    {
        return padState.IsButtonDown(button) && oldPadState.IsButtonUp(button);
    }
    public static bool LeftClick() {
		return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
	}
	public static bool RightClick() {
		return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
	}

	//Should be called at beginning of Update in Game
	public static void Update() {
		oldKeyState = keyState;
		keyState = Keyboard.GetState();
		oldMouseState = mouseState;
		mouseState = Mouse.GetState();
        oldPadState = padState;
        padState = GamePad.GetState(PlayerIndex.One);
	}
}
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Query.Core;

sealed class KeyboardHook {
	public event EventHandler Triggered;

	// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
	private readonly NativeMethods.HookProc keyboardHookDelegate;
	private IntPtr keyboardHook;

	public KeyboardHook() {
		keyboardHookDelegate = KeyboardHookProc;
	}

	public void StartHook() {
		if (keyboardHook != IntPtr.Zero) {
			NativeMethods.UnhookWindowsHookEx(keyboardHook);
		}

		keyboardHook = NativeMethods.SetWindowsHookEx(NativeMethods.WH_KEYBOARD_LL, keyboardHookDelegate, IntPtr.Zero, 0);
	}

	public void StopHook() {
		if (keyboardHook != IntPtr.Zero) {
			NativeMethods.UnhookWindowsHookEx(keyboardHook);
			keyboardHook = IntPtr.Zero;
		}
	}

	private IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam) {
		if (wParam == NativeMethods.WM_KEYDOWN) {
			Keys key = (Keys) Marshal.ReadInt32(lParam);

			if (key is Keys.LWin or Keys.RWin && Control.ModifierKeys.HasFlag(Keys.Control)) {
				Triggered?.Invoke(this, EventArgs.Empty);
				return NativeMethods.HookHandled;
			}
		}

		return NativeMethods.CallNextHookEx(keyboardHook, nCode, wParam, lParam);
	}

	private static class NativeMethods {
		public const int WH_KEYBOARD_LL = 13;
		public const int WM_KEYDOWN = 0x0100;

		public static readonly IntPtr HookHandled = new (-1);

		public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

		[DllImport("user32.dll")]
		public static extern bool UnhookWindowsHookEx(IntPtr idHook);

		[DllImport("user32.dll")]
		public static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);
	}
}

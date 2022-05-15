﻿// See https://aka.ms/new-console-template for more information
Console.WriteLine("Starting Vivid3D Editor.");

OpenTK.Windowing.Desktop.GameWindowSettings game_settings = new OpenTK.Windowing.Desktop.GameWindowSettings();
OpenTK.Windowing.Desktop.NativeWindowSettings native_settings = new OpenTK.Windowing.Desktop.NativeWindowSettings();

game_settings.IsMultiThreaded = false;
game_settings.UpdateFrequency = 90;
game_settings.RenderFrequency = 120;

native_settings.WindowState = OpenTK.Windowing.Common.WindowState.Normal;
native_settings.StartVisible = true;
native_settings.StartFocused = true;
native_settings.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
native_settings.APIVersion = new Version(4, 1);
native_settings.AutoLoadBindings = true;
native_settings.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
native_settings.IsEventDriven = false;
native_settings.IsFullscreen = false;
native_settings.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
native_settings.Size = new OpenTK.Mathematics.Vector2i(1424, 980);
native_settings.Title = "Q3 Editor (c)Q Research 2022";
native_settings.WindowBorder = OpenTK.Windowing.Common.WindowBorder.Resizable;


Q3Editor.EditorApp app = new Q3Editor.EditorApp(game_settings, native_settings);

app.Run();


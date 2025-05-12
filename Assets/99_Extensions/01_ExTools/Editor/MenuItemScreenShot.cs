using UnityEditor;
using UnityEngine;

public sealed class MenuItemScreenShot : Editor
{
    /*-<MenuItemScreenShot.cs>---
    Gameビューをスクリーンショット
    ---------------------------*/

    const string MENU_PATH = "ExTools/Screen Shot #%F12";

    [MenuItem(MENU_PATH, priority = 60)]
    static void CaptureScreenShot()
    {
        var filename = $"ScreenShot/{System.DateTime.Now:yyyyMMdd-HHmmss}.png";

        ScreenCapture.CaptureScreenshot(filename);

        var assembly = typeof(EditorWindow).Assembly;
        var type = assembly.GetType("UnityEditor.GameView");
        var gameView = EditorWindow.GetWindow(type);
        gameView.Repaint();

        Debug.Log($"ScreenShot captured to {filename}.");
    }
}

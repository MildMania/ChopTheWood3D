using UnityEditor;
using UnityEngine;

public static class UIMenuResolutionValidator
{
    [MenuItem("Tools/TakeScreenshots")]
    public static void MenuItem_TakeScreenshots()
    {
        //get game view
        System.Type gameViewType = System.Type.GetType("UnityEditor.GameView, UnityEditor");
        var gameView = EditorWindow.GetWindow(gameViewType);
        if (gameView == null)
            EditorApplication.ExecuteMenuItem("Waindow/Game");

        gameView = EditorWindow.GetWindow(gameViewType);
        if (gameView == null)
        {
            Debug.LogError("Game View not find!");
            return;
        }

        //resize game view
        int width = 1024;
        int height = 768;
        int gameViewBarHeight = 17;

        var rect = gameView.position;
        rect.x = 0;
        rect.y = 0;
        rect.width = width;
        rect.height = height + gameViewBarHeight;
        gameView.position = rect;

        //take screenshot
        string fileName = "screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".png";
        string folderPath = Application.persistentDataPath;
        fileName = System.IO.Path.Combine(folderPath, fileName);
        ScreenCapture.CaptureScreenshot(fileName);
        Application.OpenURL(folderPath);
    }
}

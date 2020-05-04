using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class PixelPerfectCanvasScaler : CanvasScaler
{
    public Camera ScreenCameraToGetScreenSize;

    protected override void Start()
    {
        base.Start();

        if (Application.isPlaying)
            CalculateNewScaleFactor(true);
        else
            CalculateNewScaleFactor(false);
    }

    public void CalculateNewScaleFactor(bool handleUI)
    {
        if (ScreenCameraToGetScreenSize == null)
            ScreenCameraToGetScreenSize = Camera.main;

        float curScaleFactor = ScreenCameraToGetScreenSize.pixelWidth / referenceResolution.x;

        int flooredScaleFactor = Mathf.FloorToInt(curScaleFactor);
        if (flooredScaleFactor < 1)
            flooredScaleFactor = 1;
         
        scaleFactor = flooredScaleFactor;

        if (handleUI)
            HandleConstantPixelSize();
    }
}

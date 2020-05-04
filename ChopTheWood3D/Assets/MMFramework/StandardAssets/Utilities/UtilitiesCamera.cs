using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBorderSettings
{
    public float BorderWidthLower { get; private set; }
    public float BorderWidthUpper { get; private set; }

    public float BorderHeightLower { get; private set; }
    public float BorderHeightUpper { get; private set; }

    public ScreenBorderSettings(float borderPercentageX, float borderPercentageY)
    {
        BorderWidthLower = (1 - borderPercentageX / 100.0f) * Screen.width;
        BorderWidthUpper = (borderPercentageX / 100.0f) * Screen.width;

        BorderHeightLower = (1 - borderPercentageY / 100.0f) * Screen.height;
        BorderHeightUpper = (borderPercentageY / 100.0f) * Screen.height;
    }

    public ScreenBorderSettings(float borderPercentageXLower, float borderPercentageXUpper, float borderPercentageYLower, float borderPercentageYUpper)
    {
        BorderWidthLower = (borderPercentageXLower / 100.0f) * Screen.width;
        BorderWidthUpper = (borderPercentageXUpper / 100.0f) * Screen.width;

        BorderHeightLower = (borderPercentageYLower / 100.0f) * Screen.height;
        BorderHeightUpper = (borderPercentageYUpper / 100.0f) * Screen.height;
    }
}

static partial class Utilities
{
    // target positions is targets world position
    public static bool IsTargetVisible(this Camera cam, Vector3 targetPosition)
    {
        Vector3 viewPortPoint = cam.WorldToViewportPoint(targetPosition);

        if ((viewPortPoint.x >= 0.0f && viewPortPoint.x <= 1.0f) && (viewPortPoint.y >= 0.0f && viewPortPoint.y <= 1.0f))
            return true;
        else
            return false;
    }

    // find corresponding rotation of indicator
    public static float GetRotation(this Camera cam, Vector3 sourcePos, Vector3 targetPos)
    {
        Vector2 sourceScreenPos = MMUISystemUtilities.WorldToCanvasOverlayPoint(cam, sourcePos);
        Vector2 targetScreenPos = MMUISystemUtilities.WorldToCanvasOverlayPoint(cam, targetPos);

        float angle = Mathf.Atan2(sourceScreenPos.y - targetScreenPos.y, sourceScreenPos.x - targetScreenPos.x) * Mathf.Rad2Deg;
        angle -= 90;

        return angle;
    }
}

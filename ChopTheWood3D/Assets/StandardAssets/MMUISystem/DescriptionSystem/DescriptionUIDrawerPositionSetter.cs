using UnityEngine;
using UnityEngine.UI;

public class DescriptionUIDrawerPositionSetter : MonoBehaviour
{
    public RectTransform CanvasTransform;
    public CanvasScaler CanvasScaler;
    public float NodeContainerSpawnYOffset;
    public RectTransform TopIndicator, BottomIndicator;

    public MMTweenPosition PosTween;
    public MMTweenAlpha AlphaTween;

    public void ResetTweens()
    {
        PosTween.KillTween();
        PosTween.InitValueToFROM();

        AlphaTween.KillTween();
        AlphaTween.InitValueToFROM();
    }

    public void SetPosition(GameObject targetObj)
    {
        bool belowCenter = false;

        Vector3 inversePos = CanvasTransform.InverseTransformPoint(targetObj.transform.position);

        if (targetObj.transform.position.y < Screen.height / 2.0f)
            belowCenter = true;

        ActivateAtposition(inversePos, belowCenter);
    }

    private void ActivateAtposition(Vector2 activationPos, bool underTheCenter)
    {
        if (underTheCenter)
            activationPos.y += NodeContainerSpawnYOffset;
        else
            activationPos.y -= NodeContainerSpawnYOffset;

        ((RectTransform)transform).anchoredPosition = activationPos;

        Vector2 newActivationPos = MakeSureContainerIsInCanvas(activationPos);
        Vector2 indicatorOffset = activationPos - newActivationPos;

        if (underTheCenter)
        {
            TopIndicator.gameObject.SetActive(false);

            BottomIndicator.gameObject.SetActive(true);

            BottomIndicator.anchoredPosition = indicatorOffset;
        }
        else
        {
            BottomIndicator.gameObject.SetActive(false);

            TopIndicator.gameObject.SetActive(true);

            TopIndicator.anchoredPosition = indicatorOffset;
        }

        PosTween.From = newActivationPos - new Vector2(0f, 10f);
        PosTween.To = newActivationPos;

        PosTween.PlayForward();
        AlphaTween.PlayForward();
    }

    private Vector2 MakeSureContainerIsInCanvas(Vector2 activationPos)
    {
        if (CanvasTransform.RectEnvelopes((RectTransform)transform, CanvasTransform.localScale.x))
            return activationPos;

        Rect canvasRect = CanvasTransform.GetRect(CanvasTransform.localScale.x);
        Rect descRect = ((RectTransform)transform).GetRect(CanvasTransform.localScale.x);

        Vector2 topRightDiff = canvasRect.max - descRect.max;
        topRightDiff.y = 0;

        if (topRightDiff.x > 0)
            topRightDiff.x = 0;

        Vector2 bottomLeft = canvasRect.min - descRect.min;
        bottomLeft.y = 0;

        if (bottomLeft.x < 0)
            bottomLeft.x = 0;

        return activationPos + (topRightDiff / CanvasTransform.localScale.x) + (bottomLeft / CanvasTransform.localScale.x);
    }
}

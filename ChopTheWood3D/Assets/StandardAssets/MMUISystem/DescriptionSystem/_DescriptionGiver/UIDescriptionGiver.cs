using MMUISystem.UIButton;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDescriptionGiver
{
    #region Events
    public static Action<IConvertible, GameObject> OnDescriptionRequested;
    protected void FireOnDescriptionRequested()
    {
        if (OnDescriptionRequested != null)
            OnDescriptionRequested(_itemType, _targetButton.gameObject);
    }
    #endregion

    private IConvertible _itemType;
    private UnityUIButton _targetButton;

    public UIDescriptionGiver(UnityUIButton targetButton)
    {
        _targetButton = targetButton;
    }

    public void InitDescriptionGiver(IConvertible itemType)
    {
        _itemType = itemType;
    }

    public void ActivateListeners()
    {
        _targetButton.OnButtonPressedUp += TargetButtonPressed;
        _targetButton.OnButtonTapped += TargetButtonPressed;

        _targetButton.StartListening();
    }

    public void DeactivateListeners()
    {
        if (_targetButton == null)
            return;

        _targetButton.OnButtonPressedUp -= TargetButtonPressed;
        _targetButton.OnButtonTapped -= TargetButtonPressed;

        _targetButton.StopListening();
    }

    private void TargetButtonPressed(PointerEventData eventData)
    {
        if (eventData.dragging)
            return;

        FireOnDescriptionRequested();
    }
}

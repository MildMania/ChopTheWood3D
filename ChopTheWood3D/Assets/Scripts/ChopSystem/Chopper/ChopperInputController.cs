using System;
using System.Collections.Generic;
using UnityEngine;

public class ChopperInputController : MonoBehaviour, IInputReceiver
{
    public List<InputTransmitter> AttachedInputTransmitterList { get; set; }
    public Dictionary<Type, InputTransmitter.EventDelegate> Delegates { get; set; }
    public Dictionary<Delegate, InputTransmitter.EventDelegate> DelegateLookUp { get; set; }

    #region Events
    public Action<Input_WI_OnFingerDown> OnMoveStartInput { get; set; }
    public Action<Input_WI_OnFingerUp> OnMoveEndInput { get; set; }
    public Action<Input_WI_OnDragMove> OnMoveInput { get; set; }
    #endregion

    private void Awake()
    {
        RegisterToPhaseBaseNode();
    }

    private void OnDestroy()
    {
        UnregisterFromPhaseBaseNode();
    }

    private void RegisterToPhaseBaseNode()
    {
        PhaseBaseNode.OnTraverseStarted_Static += OnPhaseNodeTraverseStarted;
        PhaseBaseNode.OnTraverseFinished_Static += OnPhaseNodeTraverseFinished;
    }

    private void UnregisterFromPhaseBaseNode()
    {
        PhaseBaseNode.OnTraverseStarted_Static -= OnPhaseNodeTraverseStarted;
        PhaseBaseNode.OnTraverseFinished_Static -= OnPhaseNodeTraverseFinished;
    }

    private void OnPhaseNodeTraverseStarted(PhaseBaseNode phaseNode)
    {
        if (!(phaseNode is GhostCutPhase))
            return;

        ChoppableController.Instance.OnFirstChoppableBecameVisible += OnFirstChoppableBecameVisible;
        ChoppableController.Instance.OnNoVisibleChoppableLeft += OnNoVisibleChoppableLeft;

    }

    private void OnPhaseNodeTraverseFinished(PhaseBaseNode phaseNode)
    {
        if (!(phaseNode is GhostCutPhase))
            return;

        StopInput();
    }

    private void OnFirstChoppableBecameVisible()
    {
        ChoppableController.Instance.OnFirstChoppableBecameVisible -= OnFirstChoppableBecameVisible;

        RegisterToInputTransmitter();
    }


    private void OnNoVisibleChoppableLeft()
    {
        StopInput();
    }

    private void StopInput()
    {
        ChoppableController.Instance.OnFirstChoppableBecameVisible -= OnFirstChoppableBecameVisible;
        ChoppableController.Instance.OnNoVisibleChoppableLeft -= OnNoVisibleChoppableLeft;
        UnregisterFromInputTransmitter();
    }

    private void RegisterToInputTransmitter()
    {
        this.AddInputListener<Input_WI_OnFingerDown>(OnFingerDown);
        this.AddInputListener<Input_WI_OnFingerUp>(OnFingerUp);
        this.AddInputListener<Input_WI_OnDragMove>(OnDragMove);
    }

    private void UnregisterFromInputTransmitter()
    {
        this.RemoveInputListener<Input_WI_OnFingerDown>(OnFingerDown);
        this.RemoveInputListener<Input_WI_OnFingerUp>(OnFingerUp);
        this.RemoveInputListener<Input_WI_OnDragMove>(OnDragMove);
    }

    private void OnFingerDown(Input_WI_OnFingerDown e)
    {
        OnMoveStartInput?.Invoke(e);
    }

    private void OnFingerUp(Input_WI_OnFingerUp e)
    {
        OnMoveEndInput?.Invoke(e);
    }

    private void OnDragMove(Input_WI_OnDragMove e)
    {
        OnMoveInput?.Invoke(e);
    }
}

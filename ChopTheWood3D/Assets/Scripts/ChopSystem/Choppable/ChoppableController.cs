using System;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableController : MonoBehaviour
{
    private static ChoppableController _instance;
    public static ChoppableController Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<ChoppableController>();

            return _instance;
        }
    }

    private Choppable[] _allChoppables;
    public Choppable[] AllChoppables
    {
        get
        {
            if (_allChoppables == null)
                _allChoppables = FindObjectsOfType<Choppable>();

            return _allChoppables;
        }
    }

    private List<Choppable> _visibleChoppables = new List<Choppable>();

    #region Events
    public Action OnFirstChoppableBecameVisible { get; set; }
    public Action OnNoVisibleChoppableLeft { get; set; }
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
        PhaseBaseNode.OnTraverseStarted_Static += OnTraverseStarted;
        PhaseBaseNode.OnTraverseFinished_Static += OnTraverseFinished;
    }

    private void UnregisterFromPhaseBaseNode()
    {
        PhaseBaseNode.OnTraverseStarted_Static -= OnTraverseStarted;
        PhaseBaseNode.OnTraverseFinished_Static -= OnTraverseFinished;
    }

    private void OnTraverseStarted(PhaseBaseNode phaseNode)
    {
        if (phaseNode is GhostCutPhase)
            OnGhostCutPhaseStarted();
    }

    private void OnGhostCutPhaseStarted()
    {
        Choppable.OnBecameVisible_Static += OnChoppableBecameVisible;
    }

    private void OnTraverseFinished(PhaseBaseNode phaseNode)
    {
        if (phaseNode is GhostCutPhase)
            OnGhostCutPhaseEnded();
    }

    private void OnGhostCutPhaseEnded()
    {
        foreach (Choppable c in _visibleChoppables)
            c.ResetChoppable();

        _visibleChoppables.Clear();

        Choppable.OnBecameVisible_Static -= OnChoppableBecameVisible;
    }

    private void OnChoppableBecameVisible(Choppable choppable, bool isVisible)
    {
        if (isVisible
            && !_visibleChoppables.Contains(choppable))
        {
            _visibleChoppables.Add(choppable);

            if (_visibleChoppables.Count == 1)
                OnFirstChoppableBecameVisible?.Invoke();
        }
        else if (!isVisible)
        {
            _visibleChoppables.Remove(choppable);

            if (_visibleChoppables.Count == 0)
                OnNoVisibleChoppableLeft?.Invoke();
        }
    }
}

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

    private List<Choppable> _availableChoppables = new List<Choppable>();
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
        foreach (Choppable c in AllChoppables)
        {
            if (c.IsAvailable)
                _availableChoppables.Add(c);

            if (c.IsVisible)
                _visibleChoppables.Add(c);
        }

        Choppable.OnSetAvailable_Static += OnChoppableSetAvailable;
        Choppable.OnBecameVisible_Static += OnChoppableBecameVisible;
    }

    private void OnTraverseFinished(PhaseBaseNode phaseNode)
    {
        if (phaseNode is GhostCutPhase)
            OnGhostCutPhaseEnded();
    }

    private void OnGhostCutPhaseEnded()
    {
    }

    private void OnChoppableSetAvailable(Choppable choppable, bool isAvailable)
    {
        if (isAvailable
            && !_availableChoppables.Contains(choppable))
            _availableChoppables.Add(choppable);
        else if (!isAvailable)
            _availableChoppables.Remove(choppable);
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

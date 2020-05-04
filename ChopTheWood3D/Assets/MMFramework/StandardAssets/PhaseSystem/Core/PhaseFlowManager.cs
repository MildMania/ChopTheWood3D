﻿using UnityEngine;

public abstract class PhaseFlowManager : MonoBehaviour
{
    private void Awake()
    {
        PhaseFlowController flowController = CreatePhase();

        GameManager.Instance.StartFlowController(flowController);
    }

    protected abstract PhaseFlowController CreatePhase();
}

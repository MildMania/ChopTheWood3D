using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ParticleExtensions
{
    static Dictionary<ParticleSystem, Action> _activeParticleDict;

    public static void OnComplete(this ParticleSystem ps, MonoBehaviour mb, Action onCompleteCallback)
    {
        if (onCompleteCallback == null)
            return;

        if (_activeParticleDict == null)
            _activeParticleDict = new Dictionary<ParticleSystem, Action>();

        if (_activeParticleDict.ContainsKey(ps))
        {
            _activeParticleDict[ps] += onCompleteCallback;
            return;
        }


        Action innerAction = new Action(onCompleteCallback);

        _activeParticleDict.Add(ps, innerAction);

        mb.StartCoroutine(WaitForParticleSystem(ps, innerAction));
    }

    public static void UnregisterFromOnComplete(this ParticleSystem ps, MonoBehaviour mb, Action onCompleteCallback)
    {
        if (_activeParticleDict == null)
            return;

            if (!_activeParticleDict.ContainsKey(ps))
            return;

        Action a = _activeParticleDict[ps];

        a -= onCompleteCallback;

        if (a == null)
            _activeParticleDict.Remove(ps);
    }

    static IEnumerator WaitForParticleSystem(ParticleSystem ps, Action callback)
    {
        while (ps
            && ps.IsAlive())
            yield return null;

        if (!ps)
            yield break;

        if (callback != null)
            callback();

        _activeParticleDict.Remove(ps);
    }
}

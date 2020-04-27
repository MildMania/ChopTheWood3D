using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DooberingParticle : MonoBehaviour
{
    #region Events
    public Action OnParticleFinished;
    private void FireOnParticleFinished()
    {
        if (OnParticleFinished != null)
            OnParticleFinished();
    }
    #endregion

    public ParticleSystem TargetParticleSystem;
    public List<ParticleSystem> SubParticleSystemColl;
    public float Duration;

    public bool IsReachedToTarget { get; private set; }

    Transform _targetTransform;

    ParticleSystem.Particle[] _particleArray;
    Vector3[] _particleArrayInitialPos;
    IEnumerator _dooberingParticleRoutine;

    public void Activate(DooberingArgs args)
    {
        IsReachedToTarget = false;

        transform.position = args.SpawnPos;

        _targetTransform = args.TargetTransform;

        var main = TargetParticleSystem.main;
        main.maxParticles = args.DooberCount;

        for (int i = TargetParticleSystem.textureSheetAnimation.spriteCount - 1; i >= 0; i--)
            TargetParticleSystem.textureSheetAnimation.RemoveSprite(i);

        TargetParticleSystem.textureSheetAnimation.AddSprite(args.Sprite);

        gameObject.SetActive(true);

        _dooberingParticleRoutine = EmitDooberingParticle(args.DooberCount);
        StartCoroutine(_dooberingParticleRoutine);
    }

    public void Deactivate()
    {
        TargetParticleSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);

        SubParticleSystemColl.ForEach(par => par.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear));

        if (_dooberingParticleRoutine != null)
            StopCoroutine(_dooberingParticleRoutine);

        _dooberingParticleRoutine = null;

        gameObject.SetActive(false);
    }

    IEnumerator EmitDooberingParticle(int emitCount)
    {
        _particleArray = new ParticleSystem.Particle[TargetParticleSystem.main.maxParticles];
        _particleArrayInitialPos = new Vector3[TargetParticleSystem.main.maxParticles];

        StartParticleEmitting(emitCount);

        yield return new WaitForSecondsRealtime(TargetParticleSystem.main.duration);

        TargetParticleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);

        int particleAmount = TargetParticleSystem.GetParticles(_particleArray);

        for (int i = 0; i < particleAmount; i++)
            _particleArrayInitialPos[i] = _particleArray[i].position;

        float passedTime = 0f;
        while (passedTime <= Duration)
        {
            for (int i = 0; i < particleAmount; i++)
            {
                Vector3 particleWorldPos = transform.TransformPoint(_particleArray[i].position);
                Vector3 targetWorldPos = _targetTransform.TransformPoint(Vector3.zero);
                Vector3 updatedPos = Vector3.Lerp(particleWorldPos, targetWorldPos, passedTime / Duration);
                _particleArray[i].position = transform.InverseTransformPoint(updatedPos);
            }

            TargetParticleSystem.SetParticles(_particleArray, _particleArray.Length);

            passedTime += Time.unscaledDeltaTime;

            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }

        for (int i = 0; i < particleAmount; i++)
            _particleArray[i].position = transform.InverseTransformPoint(_targetTransform.position);

        TargetParticleSystem.SetParticles(_particleArray, _particleArray.Length);

        IsReachedToTarget = true;
        _dooberingParticleRoutine = null;

        FireOnParticleFinished();
    }

    void StartParticleEmitting(int emitCount)
    {
        TargetParticleSystem.Simulate(0, false, true);

        SubParticleSystemColl.ForEach(par => par.Simulate(0, false, true));

        TargetParticleSystem.Emit(emitCount);

        SubParticleSystemColl.ForEach(par => par.Emit(emitCount));
    }
}

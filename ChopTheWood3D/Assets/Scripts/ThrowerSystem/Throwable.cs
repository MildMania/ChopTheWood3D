using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private float _gravity;
    public float Gravity
    {
        get
        {
            return _gravity;
        }
    }

    private float _throwDelay;
    private Vector2 _throwVelocity;
    private float _angularVelocity;

    private Vector3 _initPos;
    private Quaternion _initRotation;

    private IEnumerator _throwRoutine;

    public void Throw(
        float delay,
        Vector2 velocity,
        float angularVelocity)
    {
        _throwDelay = delay;
        _throwVelocity = velocity;
        _angularVelocity = angularVelocity;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        _initPos = transform.position;
        _initRotation = transform.rotation;

        _throwRoutine = ThrowProgress();
        StartCoroutine(_throwRoutine);
    }

    private IEnumerator ThrowProgress()
    {
        float curTime = 0;

        while(true)
        {
            OrientedPoint p = GetOrientedPointAtTime(
                _initPos,
                _initRotation,
                _throwDelay,
                _throwVelocity,
                _angularVelocity,
                Gravity,
                curTime);

            transform.position = p.Position;
            transform.rotation = p.Rotation;

            curTime += Time.deltaTime;

            yield return null;
        }
    }

    public OrientedPoint GetOrientedPointAtTime(
        Vector3 throwPos,
        Quaternion throwRotation,
        float delay,
        Vector2 throwVelocity,
        float angularVelocity,
        float gravity,
        float time)
    {
        time -= delay;

        if (time < 0)
        {
            return new OrientedPoint()
            {
                Position = throwPos,
                Rotation = throwRotation
            };
        }

        Vector3 initPos = throwPos;

        Vector3 deltaPos = throwVelocity * time + 0.5f * new Vector2(0, -gravity) * time * time;

        Vector3 newPos = initPos + deltaPos;

        float rotateAngle = time * angularVelocity;

        Quaternion newRotation = throwRotation * Quaternion.Euler(0, 0, rotateAngle);

        return new OrientedPoint() { Position = newPos, Rotation = newRotation };
    }
}

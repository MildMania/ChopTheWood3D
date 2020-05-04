using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct OrientedPoint
{
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
}

public class Thrower : MonoBehaviour
{
    [SerializeField] private Throwable _throwable;
    public Throwable Throwable
    {
        get
        {
            return _throwable;
        }
    }

    [SerializeField] private Vector2 _throwVelocity;

    [SerializeField] private float _angularVelocity;

    [SerializeField] private float _simulateTime;
    public float SimulateTime
    {
        get
        {
            return _simulateTime;
        }
    }

    public float SimulateDuration { get; set; }

    [SerializeField][Range(10, 50)] private int _resolution;
    public int Resolution { get; set; }

    public OrientedPoint GetOrientedPointAtTime(float time)
    {
        if (_throwable == null)
            return default;

        Vector3 initPos = transform.position;

        Vector3 deltaPos = _throwVelocity * time + 0.5f * new Vector2(0, -_throwable.Gravity) * time * time;

        Vector3 newPos = initPos + deltaPos;

        float rotateAngle = time * _angularVelocity;

        Quaternion newRotation = transform.rotation * Quaternion.Euler(0, 0, rotateAngle);

        return new OrientedPoint() { Position = newPos, Rotation = newRotation };
    }





}

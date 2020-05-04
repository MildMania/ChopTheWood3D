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

    [SerializeField] private float _throwDelay;

    [SerializeField] private Color _handleColor = Color.red;
    public Color HandleColor
    {
        get
        {
            return _handleColor;
        }
    }

    [SerializeField][HideInInspector] private bool _showSimulation;
    public bool ShowSimulation
    {
        get
        {
            return _showSimulation;
        }
        set
        {
            _showSimulation = value;
        }
    }

    [SerializeField] [HideInInspector] private float _simulateDuration;

    public float SimulateDuration
    {
        get
        {
            return _simulateDuration;
        }
        set
        {
            _simulateDuration = value;
        }
    }

    [SerializeField] [HideInInspector] private float _simulateTime;
    public float SimulateTime
    {
        get
        {
            return _simulateTime;
        }
        set
        {
            _simulateTime = value;
        }
    }


    [SerializeField] [HideInInspector] private int _resolution;
    public int Resolution
    {
        get
        {
            return _resolution;
        }
        set
        {
            _resolution = value;
        }
    }

    public void Throw()
    {
        _throwable.Throw(_throwDelay, _throwVelocity, _angularVelocity);
    }

    public OrientedPoint GetOrientedPointAtTime(float time)
    {
        if(_throwable == null)
        {
            return new OrientedPoint()
            {
                Position = transform.position,
                Rotation = transform.rotation
            };
        }

        return _throwable.GetOrientedPointAtTime(
            transform.position,
            transform.rotation,
            _throwDelay,
            _throwVelocity,
            _angularVelocity,
            _throwable.Gravity,
            time);
    }
}

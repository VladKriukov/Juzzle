using UnityEngine;

public class PointInTime
{

    public Vector3 position;
    public Quaternion rotation;
    public bool isOn;
    public int positionIndex;

    public PointInTime(Vector3 _position, Quaternion _rotation)
    {
        position = _position;
        rotation = _rotation;
    }

    public PointInTime(Vector3 _position, Quaternion _rotation, bool _isOn = true)
    {
        position = _position;
        rotation = _rotation;
        isOn = _isOn;
    }

    public PointInTime(Vector3 _position, Quaternion _rotation, int _positionIndex = 0)
    {
        position = _position;
        rotation = _rotation;
        positionIndex = _positionIndex;
    }

}

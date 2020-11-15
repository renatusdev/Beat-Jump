using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMovementAnim : MonoBehaviour
{

    public enum XDirection { None, Left, Right }
    public enum YDirection { None, Up, Down }
    public enum ZDirection { None, Forward, Backwards }

    public XDirection xDir;
    public YDirection yDir;
    public ZDirection zDir;

    public LeanTweenType curve;
    [Range(0, 100)] public int magnitude;
    public int duration;
    public bool loop;
    public bool atStart;

    void Start()
    {
        if (!atStart)
            return;

        if (loop)
            LeanTween.move(this.gameObject, CreateDirection() * magnitude, duration).setEase(curve).setLoopPingPong();
        else
            LeanTween.move(this.gameObject, CreateDirection() * magnitude, duration).setEase(curve);
    }

    Vector3 CreateDirection()
    {
        Vector3 dir = Vector3.one;

        switch (xDir)
        {
            case XDirection.Left:
                dir = new Vector3(-1, dir.y, dir.z);
                break;
            case XDirection.None:
                dir = new Vector3(0, dir.y, dir.z);
                break;
        }
        switch (yDir)
        {
            case YDirection.Down:
                dir = new Vector3(dir.x, -1, dir.z);
                break;
            case YDirection.None:
                dir = new Vector3(dir.x, 0, dir.z);
                break;
        }
        switch (zDir)
        {
            case ZDirection.Backwards:
                dir = new Vector3(dir.x, dir.y, -1);
                break;
            case ZDirection.None:
                dir = new Vector3(dir.x, dir.y, 0);
                break;
        }

        return dir;
    }

    public void Rotate()
    {
        if (loop)
            LeanTween.move(this.gameObject, CreateDirection() * magnitude, duration).setEase(curve).setLoopPingPong();
        else
            LeanTween.move(this.gameObject, CreateDirection() * magnitude, duration).setEase(curve);
    }
}

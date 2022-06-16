using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PushDir
{
    Left = -1,
    Right = 1,
}

public interface IPushable
{
    void Push(PushDir dir, float power);
}

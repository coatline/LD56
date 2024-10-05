using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITaskable
{
    Vector2 Position { get; }
    void Work(Flemington flemington);
}

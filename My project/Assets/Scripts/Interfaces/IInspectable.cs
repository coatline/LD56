using UnityEngine;

public interface IInspectable
{
    public event System.Action Destroyed;

    Transform Transform { get; }
    string Name { get; }
    string Position { get; }
    string Content { get; }
}
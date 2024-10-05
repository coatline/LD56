using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, ITaskable
{
    public Vector2 Position => transform.position;

    void Start()
    {
        Village.I.AddTask(new Task(transform.position, this));
    }

    public void Work(Flemington flemington)
    {

    }
}

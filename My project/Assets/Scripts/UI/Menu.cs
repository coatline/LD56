using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void SwitchScene(string name)
    {
        SceneFader.I.LoadNewScene(name, 0.25f);
    }
}

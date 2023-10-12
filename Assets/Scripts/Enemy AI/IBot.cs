using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBot
{
    public void Idling();
    public void Moving();

    public void PerformAction();
}

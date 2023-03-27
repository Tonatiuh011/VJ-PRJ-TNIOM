using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitAction<InType, OutType> : MonoBehaviour
    //where InType : class
    //where OutType : class
{
    public delegate OutType UAction(InType input);
    public float Duration { get; private set; }
    public UAction Action { get; private set; }
    public bool Active => Cooldown <= 0;

    private float Cooldown;

    public UnitAction(float duration, UAction action)
    {
        Duration = duration;
        Action = action;
    }

    public void Update() => Cooldown -= Time.deltaTime;

    public OutType Exec(InType input)
    {
        if(!Active)
        {
            Cooldown = Duration;
            return Action(input);
        }

        return default;
    }
}
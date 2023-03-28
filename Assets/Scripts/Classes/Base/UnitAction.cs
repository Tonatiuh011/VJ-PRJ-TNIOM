using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UnitAction <InType>
{
    public delegate void UAction(InType input);
    public float Duration { get; private set; }
    public UAction Action { get; private set; }
    public bool Active => Cooldown > 0;

    private float Cooldown;

    public UnitAction(float duration, UAction action)
    {
        Duration = duration;
        Action = action;
    }

    public UnitAction(float duration, float cooldown, UAction action)
    {
        Duration = duration;
        Action = action;
        Cooldown = cooldown;
    }

    public UnitAction(UnitAction<InType> uAction, UAction action)
    {
        Duration = uAction.Duration;
        Action = action;
        Cooldown += uAction.Cooldown;
    }

    public void Reset() => Cooldown = 0;

    public void Update() => Cooldown -= Time.deltaTime;

    public bool Exec(InType input)
    {
        if(!Active)
        {
            Cooldown = Duration;
            Action(input);
            return true;
        }
        return false;
    }
}
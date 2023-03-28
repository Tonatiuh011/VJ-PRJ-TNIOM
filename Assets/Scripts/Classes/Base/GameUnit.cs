using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class GameUnit : MovingObject, IUnit
{
    [Header("Health - Damage")]
    public float hp;
    public float damage;
    public float hitDuration;

    // Disabled Hit timer
    protected UnitAction <float> HitAction;

    public float HP { get; set; }
    public float Damage { get; set; }
    public UnitBase Unit { get; set; }

    public override void Start()
    {
        base.Start();
        HP = hp;
        Damage = damage;
        Unit = new UnitBase(HP, Damage, OnHPChange, Death);
        HitAction = new UnitAction<float>(hitDuration, Unit.Hit);
    }

    public override void Update()
    {
        base.Update();
        HitAction?.Update();
    }

    protected abstract void OnHPChange(float hp);

    protected abstract void Death(UnitBase unit);

    public virtual void Hit(float damage) => HitAction.Exec(damage);
}
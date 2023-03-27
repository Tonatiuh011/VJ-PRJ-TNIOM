using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class GameUnit : MovingObject, IUnit
{
    [Header("Health - Damage")]
    public float hp = 100;
    public float damage = 50;
    public float hitDuration = 0.4f;

    // Disabled Hit timer
    protected float disableHit;
    protected UnitAction<float, bool> HitAction;

    public float HP { get; set; }
    public float Damage { get; set; }
    public UnitBase Unit { get; set; }
        
    public GameUnit()
    {
        HP = hp;
        Damage = damage;
        Unit = new UnitBase(HP, Damage, OnHPChange, Death);

        HitAction = new UnitAction<float, bool>(hitDuration, dmg => {
            Unit.Hit(damage);
            return HitAction.Active;
        });
    }

    protected abstract void OnHPChange(float hp);

    protected abstract void Death(UnitBase unit);

    public virtual void Hit(float damage){
        HitAction.Exec(damage);
        //Unit.Hit(damage);
    }
}
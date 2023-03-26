using Assets.Scripts.Classes;
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
    public float hitDuration = 0.2f;

    public float HP { get; set; }
    public float Damage { get; set; }
    public UnitBase Unit { get; set; }
        
    public GameUnit()
    {
        HP = hp;
        Damage = damage;
        Unit = new UnitBase(HP, Damage, OnHPChange, Death);
    }    

    protected abstract void OnHPChange(float hp);

    protected abstract void Death(UnitBase unit);

    public virtual void Hit(float damage) => Unit.Hit(damage);
    
}
using Assets.Scripts.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembly_CSharp
{
    public abstract class Enemy : MovingObject, IEnemy
    {
        public float HP { get; set; }
        public float Damage { get; set; }
        public float Speed { get; set; }
        public UnitBase Unit { get; set; }
        
        public Enemy(float hp, float damage, float speed)
        {
            HP = hp;
            Damage = damage;
            Speed = speed;
            Unit = new UnitBase(HP, Damage, OnHPChange, Death);
        }

        protected abstract void Death(UnitBase unit);

        protected abstract void OnHPChange(float hp);
    }
}
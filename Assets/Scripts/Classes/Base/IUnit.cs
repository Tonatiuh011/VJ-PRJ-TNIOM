using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IUnit
{
    public float HP { get; set; }
    public float Damage { get; set; }
    public UnitBase Unit { get; set; }
}
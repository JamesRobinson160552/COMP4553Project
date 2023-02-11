using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//allows to right click, click "create", click "card", click "Create new Card"
[CreateAssetMenu(fileName = "Unit", menuName = "Unit/Create new Unit")]

public class UnitBase : ScriptableObject
{
    [SerializeField] string name_;

    [SerializeField] int level_;
    [SerializeField] int exp_;

    [SerializeField] int maxHP_;
    [SerializeField] int range_;

    [SerializeField] float hitBoxMultiplier_;

    [SerializeField] int baseDamage_;
    [SerializeField] int attackSpeed_;
    [SerializeField] int projectileSpeed_;
    [SerializeField] int moveSpeed_;

    public string Name{
        get { return name_; }
    }

    public float HitBoxMultiplier{
        get { return hitBoxMultiplier_; }
    }

    public int Level{
        get { return level_; }
    }

    public int Exp{
        get { return exp_; }
    }

    public int MaxHP{
        get { return maxHP_; }
    }

    public int Range{
        get { return range_; }
    }

    public int BaseDamage{
        get { return baseDamage_; }
    }

    public int AttackSpeed{
        get { return attackSpeed_; }
    }

    public int ProjectileSpeed{
        get { return projectileSpeed_; }
    }

    public int MoveSpeed{
        get { return moveSpeed_; }
    }
}


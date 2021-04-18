using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Weapons
{
    int GetID();
    string GetName();
    int GetDamage();
    int GetRange();
    int GetAmmo();
    int GetFireRate();
    int GetTimeDestroyShoot();
    int GetPrice();
    GameObject GetGameObject();
}
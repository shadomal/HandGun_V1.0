using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pistola : Weapons
{
    public int GetID()
    {
        return 1;
    }
    public string GetName()
    {
        return "pistola";
    }
    public int GetDamage()
    {
        return 10;
    }
    public int GetRange()
    {
        return 10;
    }
    public int GetAmmo()
    {
        return 10;
    }
    public int GetFireRate()
    {
        return 50;
    }
    public int GetTimeDestroyShoot()
    {
        return 10;
    }
    public int GetPrice()
    {
        return 100;
    }
    public GameObject GetGameObject()
    {
        return GameObject.Find("First_Gun");
    }
}
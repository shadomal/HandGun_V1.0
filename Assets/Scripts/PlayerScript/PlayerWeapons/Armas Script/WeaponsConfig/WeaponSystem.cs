using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponSystem : MonoBehaviour
{
    #region Lists And Dicitonary
    private Dictionary<int, Weapons> weapons;
    #endregion
    #region vars
    [SerializeField] private GameObject player;

    private GameObject weaponActive;
    #endregion
    void Start()
    {
        weaponActive = null;
        weapons = new Dictionary<int, Weapons>();
        List<Weapons> WeaponsConvert = new List<Weapons>();
        WeaponsConvert.Add(new Pistola());

        WeaponsConvert.ForEach(Key =>
        {
            weapons.Add(Key.GetID(), Key);
        });

    }

    public Weapons GetWeaponByID(int id)
    {
        Weapons weapon = null;
        weapons.TryGetValue(id, out weapon);
        return weapon;
    }

    public Weapons GetWeaponByName(string name)
    {
        Weapons weapon = null;
        if (name != null)
        {
            foreach (Weapons wea in weapons.Values)
            {
                if (wea.GetName().ToLower() == name.ToLower())
                {
                    weapon = wea;
                }
            }
        }
        return weapon;
    }
    public void SetWeaponUsing(string name)
    {
        Weapons weapon = GetWeaponByName(name);
        SetWeaponUsing(weapon);
    }
    public void SetWeaponUsing(int id)
    {
        Weapons weapon = GetWeaponByID(id);
        SetWeaponUsing(weapon);
    }

    public void SetWeaponUsing(Weapons weapon)
    {
        if (weapon != null)
        {
            if (weaponActive != null)
            {
                Destroy(weaponActive);
            }
            GameObject weaponObject = weapon.GetGameObject();

            weaponObject.transform.forward = player.transform.forward;
            Vector3 pos = player.transform.position;
            weaponObject.transform.position = new Vector3(pos.x + 2.0f, pos.y + 1.0f, 0 + pos.z);
            weaponObject.transform.parent = player.transform;
            //Criar maneira de alocar a arma para dentro do player;
            //Verificar compra do item;
            weaponActive = weaponObject;
            Instantiate(weaponObject);
        }
    }
    public void Shoot()
    {
        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    [SerializeField]private Transform arma;
    // Update is called once per frame
    void FixedUpdate()
    {
        Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(rayOrigin, out hitInfo))
        {
            if (hitInfo.collider != null)
            {
                Vector3 direction = hitInfo.point - arma.position;
                arma.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}

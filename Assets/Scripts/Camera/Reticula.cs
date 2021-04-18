using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticula : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }
        transform.position = Input.mousePosition;
    }
}

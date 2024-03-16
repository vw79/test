using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public GameObject cam1, cam2, cam3;
    public Health health;
    private bool hasDied;

    void Start()
    {
        cam1.SetActive(true);
        cam2.SetActive(false);
        cam3.SetActive(false);
    }

    void Update()
    {
        if (!health.isDead)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                cam1.SetActive(true);
                cam2.SetActive(false);
                cam3.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                cam1.SetActive(false);
                cam2.SetActive(true);
                cam3.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                cam1.SetActive(false);
                cam2.SetActive(false);
                cam3.SetActive(true);
            }
        }
        else
        {
            if (!hasDied)
            {
                cam1.SetActive(true);

                hasDied = true;
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                cam1.SetActive(true);
                cam2.SetActive(false);
                cam3.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                cam1.SetActive(false);
                cam2.SetActive(false);
                cam3.SetActive(true);
            }
        }
    }
}

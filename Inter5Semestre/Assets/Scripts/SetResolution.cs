using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetResolution : MonoBehaviour
{
    private void Start()
    {
        Screen.SetResolution(1920, 1080, true, 60);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public static bool pausado = false;

    public GameObject pauseUI;

    private InputManager input;
    
    [SerializeField]
    private LockCamera lockCamera_;

    [SerializeField]
    private CursorLockMode anteriorModo;



    private void Start()
    {
        input = InputManager.Instance;
        pauseUI.SetActive(false);
    }

    void Update()
    {
        if (input.PlayerPause())
        {
            if (pausado)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (pausado)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Resume()
    {
        
        if(anteriorModo == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            lockCamera_.UnlockPlayerCamera();
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
            pausado = false;
        }

        else
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
            pausado = false;
        }
    }

    void Pause()
    {
        anteriorModo = Cursor.lockState;
        lockCamera_.LockPlayerCamera();
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        pausado = true;
    }
}

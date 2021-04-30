using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoomComment : Interactable
{
    // Start is called before the first frame update
    private float time;
    private float timeReset;
    [SerializeField]
    private float timeToReact;
    [SerializeField]
    private float cooldownReset;
    private bool cooldown = false;
    public DialogueBase dialogue;
    [SerializeField]
    private Diario diario;
    [SerializeField]
    private GameObject atualizacaoDiario;
    

    private bool coroutineStart = true;

    [SerializeField]
    private CinemachineVirtualCamera vcamNormal, vcamZoom;
    
    private void Update()
    {
        if (cooldown)
        {
            timeReset += Time.deltaTime;

            if (timeReset >= cooldownReset)
            {
                time = 0;
                timeReset = 0;
                cooldown = false;
            }
        }

        //Debug.Log(time);
    }
    public override void Interact()
    {
        Debug.Log("go");
        cooldown = true;
        time += Time.deltaTime;
        
            if(coroutineStart) StartCoroutine("ZoomPlusComment");
                              
    }


    IEnumerator ZoomPlusComment()
    {
        coroutineStart = false;
        vcamZoom.Priority = 2;
        yield return new WaitForSeconds(1f);
        DialogueManager.instance.EnqueueDialogue(dialogue);
        this.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(2f);
        DialogueManager.instance.DequeueDialogue();
        vcamZoom.Priority = 0;

    }
}

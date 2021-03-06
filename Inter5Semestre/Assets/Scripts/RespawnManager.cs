using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class RespawnManager : MonoBehaviour
{
    public enum Estados { ADM, DEPOSITO, VESTIARIO, REFEITORIO, SEGUNDA }

    private GameObject player;
    private GameObject myCam;
    private VolumeManager volume;
    public Volume stress;
    public Volume pretoEBranco;
    private LockCamera lc;
    private bool transicionaStress = false;
    public float transicaoStressIn;
    public float transicaoStressOut;
    private float transicaoStressValor = 0;
    private bool transicionaPb = false;
    public float transicaoPb;

    [Header("Respawn ADM")]
    public Transform respawnPoint1;
    public Animator portaADM;
    public GameObject monstro1;
    private Vector3 monstroPosI;
    public GameObject falasFlutuantes;
    public GameObject obstaculos;
    public AdminDocuments doc;

    [Header("Respawn Deposito")]
    public Transform respawnPointDeposito;
    public GameObject monstroOlho;
    private Vector3 monstroOlhoPosI;
    public GameObject portaDeposito;
    public GameObject trancaDeposito;
    private PuzzleDepositoManager depositoManager;
    public GameObject objetoInicial;

    [Header("Respawn Vestiario")]
    [SerializeField]
    private PuzzleDoor[] puzzleDoors;
    [SerializeField]
    private VestiarioPuzzle vestiarioPuzzle;
    [SerializeField]
    private Transform respawnPointVestiario;


    [Header("Respawn Refeitorio")]
    public Transform respawnPointRefeitorio;
    public GameObject objetoInicialRefeitorio;
    public GameObject objetoFinalrefeitorio;
    public GameObject gas;
    private Vector3 gasPosI;
    public GameObject sombras;
    public GameObject grades;

    [Header("Respawn Segunda Fuga")]
    public GameObject monstroTelefone;
    public Transform respawnFugaDois;
    private Vector3 monstro2PosI;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myCam = GameObject.FindGameObjectWithTag("MainCamera");
        volume = GameObject.FindGameObjectWithTag("VolumeManager").GetComponent<VolumeManager>();
        lc = GameObject.FindGameObjectWithTag("LockCamera").GetComponent<LockCamera>();

        monstroPosI = monstro1.transform.position;

        monstroOlhoPosI = monstroOlho.transform.position;
        depositoManager = GameObject.FindGameObjectWithTag("DepositoManager").GetComponent<PuzzleDepositoManager>();

        gasPosI = gas.transform.position;

        monstro2PosI = monstroTelefone.transform.position;
    }

    private void Update()
    {
        if (transicionaStress)
        {
            stress.weight += transicaoStressValor * Time.deltaTime;
        }

        if (transicionaPb)
        {
            pretoEBranco.weight += transicaoPb * Time.deltaTime;
            if (pretoEBranco.weight >= 1) pretoEBranco.weight = 1;
        }
    }

    public void RespawnADM()
    {
        player.transform.position = respawnPoint1.position;
        myCam.transform.rotation = respawnPoint1.rotation;
        portaADM.SetBool("Open", false);
        monstro1.transform.position = monstroPosI;
        monstro1.SetActive(false);
        monstro1.GetComponent<NavMeshAgent>().speed = 0.2f;
        falasFlutuantes.SetActive(false);
        obstaculos.SetActive(false);
        doc.startCoroutine = true;
        stress.weight = 0;
        //volume.TransicaoOut(1);
    }

    public void RespawnVestiario()
    {
        player.transform.position = respawnPointVestiario.position;
        for (int i = 0; i < puzzleDoors.Length; i++)
        {
            puzzleDoors[i].correctLocker = false;
            puzzleDoors[i].opened = false;
            puzzleDoors[i].animator.SetBool("Open", false);
        }
        vestiarioPuzzle.RandomizeNewLocker();
    }

    public void RespawnDeposito()
    {
        player.transform.position = respawnPointDeposito.position;
        myCam.transform.rotation = respawnPointDeposito.rotation;
        monstroOlho.SetActive(false);
        monstroOlho.transform.position = monstroOlhoPosI;
        if (portaDeposito)
        {
            portaDeposito.GetComponent<BoxCollider>().enabled = true;
        }
        trancaDeposito.SetActive(false);
        depositoManager.progressao = 0;
        objetoInicial.SetActive(true);
        stress.weight = 0;
        //volume.TransicaoOut(1);
    }

    public void RespawnRefeitorio()
    {
        player.transform.position = respawnPointRefeitorio.position;
        myCam.transform.rotation = respawnPointRefeitorio.rotation;
        objetoInicialRefeitorio.SetActive(true);
        objetoFinalrefeitorio.SetActive(false);
        gas.SetActive(false);
        gas.transform.position = gasPosI;
        grades.SetActive(false);
        sombras.SetActive(false);
        //volume.TransicaoOut(1);
    }

    public void RespawnSegundaFuga()
    {
        player.transform.position = respawnFugaDois.position;
        myCam.transform.rotation = respawnFugaDois.rotation;
        monstroTelefone.transform.position = monstro2PosI;
        stress.weight = 0;
    }

    IEnumerator Transicao(Estados estadoTransicao)
    {
        player.GetComponent<PlayerController>().interacting = true;
        lc.LockPlayerCamera();
        transicionaPb = true;
        transicaoStressValor = transicaoStressIn;
        transicionaStress = true;

        while(stress.weight < 1)
        {
            yield return null;
        }
        transicionaStress = false;
        transicionaPb = false;

        //funcao de respawn
        switch (estadoTransicao)
        {
            case Estados.ADM:
                RespawnADM();
                break;
            case Estados.DEPOSITO:
                RespawnDeposito();
                break;
            case Estados.VESTIARIO:
                RespawnVestiario();
                break;
            case Estados.REFEITORIO:
                RespawnRefeitorio();
                break;
            case Estados.SEGUNDA:
                RespawnSegundaFuga();
                break;
        }

        player.GetComponent<PlayerController>().interacting = false;
        lc.UnlockPlayerCamera();
        pretoEBranco.weight = 0;
        transicaoStressValor = transicaoStressOut;
        transicionaStress = true;

        while (stress.weight > 0)
        {
            yield return null;
        }
        transicionaStress = false;
    }
}

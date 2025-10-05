using System.Collections;
using UnityEngine;

public class Banheiro_Minigame : MonoBehaviour
{
    [Header("Configuração de Etapas e UI")]
    public Etapa[] etapas;
    public Mecanica_Passo mecanica;
    public GameObject seta;
    public Vector3 offsetSeta = new Vector3(0, 1f, 0);
    public bool setaLoopColisao = true;

    private int etapaAtual = 0;
    private int passoAtual = 0;
    private SeguirObjeto scriptSeta;

    void Awake()
    {
        if (seta != null)
        {
            scriptSeta = seta.GetComponent<SeguirObjeto>();
            if (scriptSeta != null)
                scriptSeta.offset = offsetSeta;
        }
    }

    void Start()
    {
        if (etapas == null || etapas.Length == 0)
        {
            Debug.LogWarning("Nenhuma etapa configurada!");
            return;
        }

        mecanica?.ConfigurarUI(etapas.Length);

        etapaAtual = 0;
        passoAtual = 0;

        IniciarEtapaAtual();
    }

    void Update()
    {
        if (etapaAtual >= etapas.Length) return;

        var etapa = etapas[etapaAtual];
        if (passoAtual >= etapa.passos.Length) return;

        Passo p = etapa.passos[passoAtual];

        if (p.tipo == Passo.TipoPasso.Clique && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == p.objetoClique)
                ConcluirPasso();
        }
    }

    void IniciarEtapaAtual()
    {
        var etapa = etapas[etapaAtual];
        passoAtual = 0;

        if (etapa.passos == null || etapa.passos.Length == 0)
        {
            Debug.LogWarning($"Etapa {etapaAtual} sem passos!");
            AvancarEtapa();
            return;
        }

        etapa.passos[0].IniciarPasso();
        mecanica?.MudarEtapa(etapaAtual);
        AtualizarSetaParaPasso(etapa.passos[0]);
    }

    void ConcluirPasso()
    {
        var etapa = etapas[etapaAtual];
        var passo = etapa.passos[passoAtual];

        passo.FinalizarPasso();
        passoAtual++;

        if (passoAtual >= etapa.passos.Length)
        {
            etapa.concluida = true;
            mecanica?.MarcarEtapaConcluida(etapaAtual);
            AvancarEtapa();
        }
        else
        {
            etapa.passos[passoAtual].IniciarPasso();
            AtualizarSetaParaPasso(etapa.passos[passoAtual]);
        }
    }

    void AvancarEtapa()
    {
        etapaAtual++;
        if (etapaAtual >= etapas.Length)
        {
            Debug.Log("🎉 Todas as etapas concluídas!");
            if (seta != null) seta.SetActive(false);
            return;
        }

        IniciarEtapaAtual();
    }

    void AtualizarSetaParaPasso(Passo passo)
    {
        if (seta == null || scriptSeta == null || passo == null) return;
        scriptSeta.DefinirAlvos(
            passo.objetoClique?.transform ?? passo.objetoA?.transform,
            passo.objetoB?.transform,
            true, setaLoopColisao, true
        );
        seta.SetActive(true);
    }
}

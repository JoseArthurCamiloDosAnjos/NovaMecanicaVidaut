using UnityEngine;

public class Banheiro_Minigame : MonoBehaviour
{
    public Mecanica_Passo mecanica;
    public Passo[] passos;
    private int passoAtual = 0;

    // ----- NOVAS VARIÁVEIS PARA CONTROLAR O INDICADOR -----
    [Header("Configurações do Indicador Visual")]
    [Tooltip("O objeto visual (seta, círculo) que será posicionado e animado.")]
    public GameObject indicadorVisual;

    [Tooltip("A distância que o indicador ficará do alvo.")]
    public Vector3 offsetIndicador = new Vector3(0, 1.2f, 0);

    [Tooltip("A altura da animação de 'sobe e desce'.")]
    public float amplitudeAnimacao = 0.25f;

    [Tooltip("A velocidade da animação.")]
    public float velocidadeAnimacao = 2f;
    // ----------------------------------------------------

    void Start()
    {
        // Garante que o indicador comece desligado
        if (indicadorVisual != null)
        {
            indicadorVisual.SetActive(false);
        }

        if (passos.Length > 0)
        {
            passos[0].IniciarPasso();
        }
    }

    // ----- NOVA FUNÇÃO: LATEUPDATE -----
    // Usamos LateUpdate para garantir que o alvo já se moveu antes de posicionarmos o indicador.
    void LateUpdate()
    {
        // Se o jogo acabou ou não há indicador, não faz nada.
        if (passoAtual >= passos.Length || indicadorVisual == null)
        {
            if (indicadorVisual != null) indicadorVisual.SetActive(false); // Garante que ele suma no final
            return;
        }

        // Pega o alvo do passo atual
        GameObject alvo = passos[passoAtual].GetAlvoPrincipal();

        // Se não houver um alvo para este passo, esconde o indicador.
        if (alvo == null)
        {
            indicadorVisual.SetActive(false);
            return;
        }

        // Se havia um alvo, garante que o indicador está visível
        if (!indicadorVisual.activeSelf)
        {
            indicadorVisual.SetActive(true);
        }

        // Calcula a posição e animação (lógica que estava no IndicadorAnimado.cs)
        Vector3 posicaoBase = alvo.transform.position + offsetIndicador;
        float deslocamentoY = Mathf.Sin(Time.time * velocidadeAnimacao) * amplitudeAnimacao;
        indicadorVisual.transform.position = posicaoBase + new Vector3(0, deslocamentoY, 0);
    }
    // ------------------------------------

    void Update()
    {
        if (passoAtual >= passos.Length) return;

        Passo passo = passos[passoAtual];

        if (passo.tipo == Passo.TipoPasso.Clique && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == passo.objetoClique)
            {
                ConcluirPasso();
                Debug.Log($"Passo {passoAtual - 1} concluído (Clique)!");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (passoAtual >= passos.Length) return;
        Passo passo = passos[passoAtual];
        if (passo.tipo == Passo.TipoPasso.Colisao)
        {
            GameObject colisor = collision.collider.gameObject;
            if ((colisor == passo.objetoA && collision.otherCollider.gameObject == passo.objetoB) ||
                (colisor == passo.objetoB && collision.otherCollider.gameObject == passo.objetoA))
            {
                ConcluirPasso();
                Debug.Log($"Passo {passoAtual - 1} concluído (Colisão)!");
            }
        }
    }

    void ConcluirPasso()
    {
        passos[passoAtual].FinalizarPasso();
        mecanica.ProximoPasso();
        passoAtual++;
        if (passoAtual < passos.Length)
        {
            passos[passoAtual].IniciarPasso();
        }
    }

    public void VoltarPasso()
    {
        if (passoAtual > 0)
        {
            passos[passoAtual].FinalizarPasso();
            passoAtual--;
            mecanica.AtualizarParaPasso(passoAtual);
            passos[passoAtual].IniciarPasso();
        }
    }
}
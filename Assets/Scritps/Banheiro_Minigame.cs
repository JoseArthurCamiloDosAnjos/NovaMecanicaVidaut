using UnityEngine;

public class Banheiro_Minigame : MonoBehaviour
{
    public Mecanica_Passo mecanica;
    public Passo[] passos;
    private int passoAtual = 0;

    void Start()
    {
        if (passos.Length > 0)
        {
            // já prepara os objetos do primeiro passo
            passos[0].IniciarPasso();
        }
    }

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
        // Finaliza passo atual
        passos[passoAtual].FinalizarPasso();
        mecanica.ProximoPasso();

        // Avança para o próximo
        passoAtual++;
        if (passoAtual < passos.Length)
        {
            // prepara objetos do próximo passo
            passos[passoAtual].IniciarPasso();
        }
    }

    public void VoltarPasso()
    {
        if (passoAtual > 0)
        {
            passos[passoAtual].FinalizarPasso(); // limpa estado do passo atual

            passoAtual--;
            mecanica.PassoAnterior();

            passos[passoAtual].IniciarPasso(); // reativa objetos do passo anterior
        }
    }
}

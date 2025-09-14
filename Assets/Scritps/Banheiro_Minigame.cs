using UnityEngine;

public class Banheiro_Minigame : MonoBehaviour
{
    public Mecanica_Passo mecanica;
    public Passo[] passos;
    private int passoAtual = 0;

    void Start()
    {
        if (passos.Length > 0)
            passos[passoAtual].IniciarPasso();

        // Sincroniza UI no início
        mecanica.MudarPasso(passoAtual);
    }

    void Update()
    {
        if (passoAtual >= passos.Length) return;

        Passo passo = passos[passoAtual];

        // --- Clique ---
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

    // --- Colisão física ---
    void OnCollisionEnter2D(Collision2D collision)
    {
        VerificarColisao(collision.collider.gameObject, collision.otherCollider.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        VerificarColisao(this.gameObject, other.gameObject);
    }

    void VerificarColisao(GameObject obj1, GameObject obj2)
    {
        if (passoAtual >= passos.Length) return;

        Passo passo = passos[passoAtual];

        if (passo.tipo == Passo.TipoPasso.Colisao)
        {
            if ((obj1 == passo.objetoA && obj2 == passo.objetoB) ||
                (obj1 == passo.objetoB && obj2 == passo.objetoA))
            {
                ConcluirPasso();
                Debug.Log($"Passo {passoAtual - 1} concluído (Colisão/Trigger)!");
            }
        }
    }

    void ConcluirPasso()
    {
        // Finaliza passo atual
        passos[passoAtual].FinalizarPasso();
        mecanica.DefinirPasso(passoAtual, true); // Marca concluído na UI

        // Avança para o próximo
        passoAtual++;
        if (passoAtual < passos.Length)
        {
            passos[passoAtual].IniciarPasso();
            mecanica.MudarPasso(passoAtual); // Atualiza UI
        }

        Debug.Log("Avançou para o passo: " + passoAtual);
    }

    public void VoltarPasso()
    {
        if (passoAtual > 0)
        {
            passoAtual--;
            mecanica.MudarPasso(passoAtual);
            passos[passoAtual].IniciarPasso();
        }
    }
}

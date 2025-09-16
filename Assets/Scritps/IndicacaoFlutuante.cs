using UnityEngine;

public class IndicadorAnimado : MonoBehaviour
{
    [Header("Configurações da Animação")]
    [Tooltip("A altura máxima que o objeto vai subir e descer a partir do ponto inicial.")]
    public float amplitude = 0.2f;

    [Tooltip("A velocidade do movimento de sobe e desce.")]
    public float velocidade = 1.5f;

    private Vector3 posicaoInicial;
    private bool posicaoInicialDefinida = false;

    // OnEnable é chamado quando o script é ativado (ou o objeto se torna ativo)
    void OnEnable()
    {
        // Salva a posição inicial apenas na primeira vez que for ativado
        if (!posicaoInicialDefinida)
        {
            posicaoInicial = transform.position;
            posicaoInicialDefinida = true;
        }
    }

    // OnDisable é chamado quando o script é desativado
    void OnDisable()
    {
        // Garante que o objeto volte para sua posição original quando a animação parar
        if (posicaoInicialDefinida)
        {
            transform.position = posicaoInicial;
        }
    }

    void Update()
    {
        float deslocamentoY = Mathf.Sin(Time.time * velocidade) * amplitude;
        transform.position = posicaoInicial + new Vector3(0, deslocamentoY, 0);
    }
}
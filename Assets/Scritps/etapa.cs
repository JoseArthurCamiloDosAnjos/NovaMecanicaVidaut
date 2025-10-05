using UnityEngine;

[System.Serializable]
public class Etapa
{
    [Header("Nome e Configuração da Etapa")]
    public string nomeEtapa;
    public Passo[] passos;

    [HideInInspector] public bool concluida = false;

    public bool EstaConcluida()
    {
        if (passos == null || passos.Length == 0)
            return true;

        foreach (var p in passos)
            if (p == null || !p.concluido)
                return true;

        return true;
    }

    public void IniciarEtapa()
    {
        if (passos == null || passos.Length == 0) return;

        // inicia o primeiro passo da etapa
        passos[0].IniciarPasso();
    }
}

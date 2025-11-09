using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Etapa
{
    [Header("Nome e Configuração da Etapa")]
    public string nomeEtapa;
    public Passo[] passos;

    [Header("Falas de introdução da etapa")]
    public AudioClip[] audiosIntroducao;
    [TextArea(2, 5)]
    public string[] textosIntroducao;
    public float delayEntreFalas = 1f;

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

    // ?? NOVO MÉTODO — compatível com chamada antiga
    public void IniciarEtapa()
    {
        // Chamada padrão antiga, sem áudio/texto
        if (passos == null || passos.Length == 0) return;
        passos[0].IniciarPasso();
    }

    // ?? SOBRECARGA — usada pelo Banheiro_Minigame com 3 parâmetros
    public void IniciarEtapa(MonoBehaviour contexto, AudioSource fonteAudio, Text textoUI)
    {
        contexto.StartCoroutine(ReproduzirIntroducao(fonteAudio, textoUI));
    }

    private IEnumerator ReproduzirIntroducao(AudioSource fonte, Text textoUI)
    {
        if (audiosIntroducao != null && textosIntroducao != null)
        {
            int total = Mathf.Min(audiosIntroducao.Length, textosIntroducao.Length);
            for (int i = 0; i < total; i++)
            {
                if (textoUI != null)
                    textoUI.text = textosIntroducao[i];

                if (fonte != null && audiosIntroducao[i] != null)
                {
                    fonte.clip = audiosIntroducao[i];
                    fonte.Play();
                    yield return new WaitForSeconds(fonte.clip.length + delayEntreFalas);
                }
                else
                    yield return new WaitForSeconds(delayEntreFalas);
            }
        }

        // inicia o primeiro passo da etapa
        if (passos != null && passos.Length > 0)
            passos[0].IniciarPasso();
    }

}

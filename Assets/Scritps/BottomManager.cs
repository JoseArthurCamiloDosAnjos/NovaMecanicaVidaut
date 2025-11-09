using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BottomManager : MonoBehaviour
{
    private Button btnFinalizar;
    private Button btnIniciar;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene cena, LoadSceneMode modo)
    {
        // Tenta encontrar os botões na cena atual
        btnFinalizar = GameObject.Find("BtnFinalizar")?.GetComponent<Button>();
        btnIniciar = GameObject.Find("BtnIniciar")?.GetComponent<Button>();

        // Conecta os listeners se os botões existirem
        if (btnFinalizar != null)
        {
            btnFinalizar.onClick.RemoveAllListeners();
            btnFinalizar.onClick.AddListener(EndGame);
        }

        if (btnIniciar != null)
        {
            btnIniciar.onClick.RemoveAllListeners();
            btnIniciar.onClick.AddListener(() => SceneManager.LoadScene("SampleScene"));
        }
    }

    void EndGame()
    {
        SceneManager.LoadScene("InicialGame");
    }
}

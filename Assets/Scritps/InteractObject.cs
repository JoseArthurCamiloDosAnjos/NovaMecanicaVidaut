using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameManager.EscovaSteps myStep;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnMouseDown()
    {
        if (gameManager != null)
        {
            gameManager.OnObjectClicked(myStep);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class FinisherFlag : MonoBehaviour
{
    public string level;
    private bool isPlayerInRange = false;
    private PlayerController playerController; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerController = collision.GetComponent<PlayerController>(); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerController = null; 
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetButtonDown("Submit"))
        {
            if (playerController != null)
            {
                playerController.SaveLife(); 
                SceneManager.LoadScene(level); 
            }
        }
    }
}

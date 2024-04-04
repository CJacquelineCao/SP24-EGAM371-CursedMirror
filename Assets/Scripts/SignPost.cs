using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignPost : MonoBehaviour
{
    public GameObject Box;
    public TextMeshProUGUI textBox; // Reference to the UI text box
    public string message; // Default message to display

    private void Start()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Set the text content of the UI text box
            textBox.text = message;

            // Activate the UI text box
            Box.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Deactivate the UI text box when the player exits the trigger
            Box.gameObject.SetActive(false);
        }
    }
}

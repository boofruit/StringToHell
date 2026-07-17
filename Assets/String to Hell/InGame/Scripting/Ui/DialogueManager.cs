using UnityEngine;
using TMPro; // Required for TextMeshPro

namespace StringToHell.InGame.Ui
{
    public class DialogueManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject dialoguePanel; // The entire pop-up container
        [SerializeField] private TextMeshProUGUI dialogueText; // The text component

        void Start()
        {
            // Hide the dialogue box automatically when the game starts
            CloseDialogue();
        }

        // Call this public function from other scripts to trigger the pop-up
        public void ShowDialogue(string message)
        {
            dialogueText.text = message;      // Set custom message text
            dialoguePanel.SetActive(true);    // Make the box visible
        }

        // Call this function via your UI Close Button click event
        public void CloseDialogue()
        {
            dialoguePanel.SetActive(false);   // Hide the box
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

namespace StringToHell.InGame.GameManager
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        public GameObject player;
        [SerializeField] KeyCode select = KeyCode.Tab;
        IUiInput input;
        ISpiderInteractionContols interactionContols;
        bool start;
        public float TimeLength = 2f;

        void Start()
        {
           
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            input = player.GetComponent<IUiInput>();
            interactionContols = player.GetComponent<ISpiderInteractionContols>();
        }

        void Update()
        {
            if (input.IsOpenMenu)
            {
                TogglePause();
                
            }
        }
        public void TooggleAutoCling()
        {
            
            interactionContols.AutoCling = !interactionContols.AutoCling;
        }
        public void ToggleActive()
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }

        public bool IsPaused { get; private set; }

        public void TogglePause()
        {
            if (IsPaused)
                ResumeGame();
            else
                PauseGame();
        }

        public void PauseGame()
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            IsPaused = true;
        }

        public void ResumeGame()
        {
         
                pauseMenu.SetActive(false);
            

            Time.timeScale = 1f;
            IsPaused = false;
        }
    }
}

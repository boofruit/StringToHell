using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System;
// come back to this its messy

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
        bool timePaused = false;
        public bool TimePaused => timePaused;
        void Start()
        {
            timePaused = false;
            Time.timeScale = 1f;
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
        public void PauseTime()
        {
            timePaused = !timePaused;
            if (TimePaused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
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
            
            pauseMenu.SetActive(true);
            PauseTime();
            IsPaused = true;
        }

        public void ResumeGame()
        {
         
                pauseMenu.SetActive(false);


            PauseTime();
            IsPaused = false;
        }
    }
}

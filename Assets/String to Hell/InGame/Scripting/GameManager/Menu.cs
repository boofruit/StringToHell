using UnityEngine;

namespace StringToHell.InGame.GameManager
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        public GameObject player;
        [SerializeField] KeyCode select = KeyCode.Tab;
        IUiInput input;
        ISpiderInteractionContols interactionContols;

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
                pauseMenu.SetActive(!pauseMenu.activeSelf);
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
    }
}

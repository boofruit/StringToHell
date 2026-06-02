using UnityEngine;

namespace StringToHell.InGame.GameManager
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private GameObject map;
        [SerializeField] KeyCode select = KeyCode.Tab;

        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKeyDown(select))
            {
                map.SetActive(!map.activeSelf);
            }
        }
        private void ToggleActive()
        {
            map.SetActive(!map.activeSelf);
        }
    }
}

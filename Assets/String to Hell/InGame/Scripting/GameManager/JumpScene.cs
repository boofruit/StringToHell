using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class JumpScene : MonoBehaviour
{
    [SerializeField] private GameObject target;
    
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        //Turn.text = "Turn " + GameManager.Turn.ToString("D2");
        if (target == null)
        {

            target = GetComponent<GameObject>();   
        }

    }

    public void jumpScene(string jumpSceneName)
    {
        SceneManager.LoadScene(jumpSceneName);
    }
    public void ToggleActive()
    {
       
        target.SetActive(!target.activeSelf);
    }
 
}

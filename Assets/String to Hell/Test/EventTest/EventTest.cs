using UnityEngine;
using UnityEngine.Events;
namespace StringToHell.Test.EventTest
{
    public class EventTest : MonoBehaviour
    {
        private UnityEvent OnDeath = new UnityEvent();
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            int g = 3;
            int f = 4;
            Add(g, f, out int c);
            if (TryGetComponent<ListenerTest>(out var ct))
            {
                ct.Here();
            }
            OnDeath.AddListener(Hello);
            OnDeath.AddListener(()=>Debug.Log("Hello Lamda"));
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                OnDeath?.Invoke();
            }
        }
        public void Hello()
        {
            Debug.Log("Hello");
        }
        void Add(int a, int b, out int sum)
        {
            sum = a + b;
        }
    }
}
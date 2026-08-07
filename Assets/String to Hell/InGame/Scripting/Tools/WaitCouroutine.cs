using UnityEngine;
using System.Collections;
using System;
namespace StringToHell.InGame
{
    public static class Wait
    {
        public static IEnumerator DoWait(float waitTime, Action ToDo)
        {

            yield return new WaitForSeconds(waitTime);
           
            // Additional logic can be added here if needed after the wait
            ToDo?.Invoke();
        }
    }
}
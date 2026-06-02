using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
namespace StringToHell.Test.EventTest
{
    public class LamdaLinq : MonoBehaviour
    {
        void Start()
        {
            System.Action action = () => Debug.Log("yo");//引数なし、返り値なし
            action();
            System.Action<string> action2 = s => Debug.Log(s) ;//引数あり、返り値なし
            action2("fight!");
            Func<int> func = () => 1; //引数なし、返り値あり
            print("func:" + func());
            //Func<int, bool> func2; //引数あり、返り値あり

            //should only be called once
            List<int> list = new List<int> { 1, 2, 3, 4, 5 };
            // var evens = list.Where(IsEven).ToArray();
            var evens = list
                .Where(n => n % 2== 0)
                .Select(n=> n *10)
                .OrderBy(n => n)
                .ToArray();// for {} array or .ToList(); to change to list
            for (int i = 0; i < evens.Length; i++)
            {
                Debug.Log(evens[i]);
            }
        }
        bool IsEven(int val)
        {
            return val %2 == 0;
        }
    }
}
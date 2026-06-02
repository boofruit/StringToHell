using UnityEngine;
namespace StringToHell.Test.EventTest
{
    public class LamdaTest : MonoBehaviour
    {
        private delegate int Calc(int a, int b);
        private delegate void Action();
        private delegate bool Result(int value);


        void Start()
        {
            //lambda examplles:
            //Calc theCalculation = Add; //regular method with named 
            //Calc theCalculation = (int x, int y) => { return x + y; }; //same result but anonymous in one line
            Calc theCalculation = (x, y) => x + y;//most abrieviated version
            theCalculation(1, 2);
            Action action = () => Debug.Log("Hey, Listen!!!");
            action();
            Result result = val => val % 2 == 0; //
            result(2);
        }


        int Add(int x, int y)
        {
            return x + y;
        }
    }
}

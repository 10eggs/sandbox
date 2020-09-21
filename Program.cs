using System;
using System.Collections;
using System.Collections.Generic;

namespace DelegatesLambdasEvents
{
    delegate void MeDelegate();
    delegate bool VerifcationMethod(int number);


     delegate string MeDelegateTakeStringReturnString(string param);
    class Program
    {
        static IEnumerable<int> SelectedNumbers(IEnumerable<int> list,VerifcationMethod verify)
        {
            IEnumerable<int> results = null;
            foreach(int elem in list)
            {
                if (verify(elem))
                    yield return elem;
            }

        }

        public bool VerifyIfNumberIsBiggerThan(int number)
        {
            return number > 10;
        }
        static void MyMethod()
        {
            Console.WriteLine("I'm my method and I was assigned to MyDelegate");
        }

        
        string ReturnName(string name)
        {
            return "My name is " + name;
        }

        static void ConsumeAndInvokeMethod(MeDelegate md)
        {
            md.Invoke();
        }



        static void Main(string[] args)
        {
            MeDelegate Md = new MeDelegate(MyMethod);
            Md += MyMethod;
            Console.WriteLine("***********");
            Console.WriteLine("Latest added method to Md delegate: "+Md.Method);
            Console.WriteLine("Target of Md delegate: " + Md.Target);
            Md += () => Console.WriteLine("I'm coming from lambda expression");
            Console.WriteLine("After adding lambda: "+Md.Method);
            //ConsumeAndInvokeMethod(Md);
            //ConsumeAndInvokeMethod(new MeDelegate(() => Console.WriteLine("What am I doing here!?")));
            //Md.Invoke();


            //MeDelegateTakeStringReturnString Mdstr = (name) => { return "My name is:" + name; };
            //Console.WriteLine(Mdstr("Don Juan"));
            MeDelegateTakeStringReturnString Mdstr = new Program().ReturnName;
            Console.WriteLine(Mdstr("Tomek"));
            Console.WriteLine("Small check for Mdstr - target: " + Mdstr.Target + " || method: " + Mdstr.Method);
            if (Mdstr.Method.ToString().Equals("System.String ReturnName(System.String)"))
                Console.WriteLine("Oh my god, return type for Mdstr delegate is a string!");
            else
                Console.WriteLine("I don't know what is it !");

            IEnumerable<int> NumbersBiggerThanTen = SelectedNumbers(new[] { 1, 2, 3, 10, 15, 25, 276, 326 }, new Program().VerifyIfNumberIsBiggerThan);

            Console.WriteLine("****************");
            foreach(int number in NumbersBiggerThanTen)
            {
                Console.WriteLine("Number bigger than five: "+number);
            }

            Console.WriteLine("*********");
            IEnumerable<int> NumbersBiggerThan200 = SelectedNumbers(new[] { 1, 2, 3, 10, 15, 25, 276, 326 }, (n) => n > 200);
            foreach(int number in NumbersBiggerThan200)
            {
                Console.WriteLine("Number bigger than two hundread: " + number);
            }

        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;

namespace DelegatesLambdasEvents
{
    delegate void MeDelegate();
    delegate bool VerifcationMethod(int number);
    delegate T GenericDelegate<T>();


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

        //int returnHighestPrime(int i)
        //{
        //    List<int> results = new List<int>();
        //    int d = 2;
        //    while (i > 1)
        //    {
        //        while (i % 2 == 0)
        //        {
        //            results.Add(i);
        //            i /= d;
        //        }
        //        d++;
        //    }
        //    return results.
        //}
        static int returnTen() { return 5; }
        static int returnTwenty() { return 20; }
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

            Console.WriteLine("Invocation of SelectedNumbers method with using lambda method");
            IEnumerable<int> NumbersBiggerThan200 = SelectedNumbers(new[] { 1, 2, 3, 10, 15, 25, 276, 326 }, (n) => n > 200);
            foreach(int number in NumbersBiggerThan200)
            {
                Console.WriteLine("Number bigger than two hundread: " + number);
            }


            Console.WriteLine("********************");
            Console.WriteLine("************DELEGATE CHAINING***********");

            MeDelegate delegateChain = () => Console.WriteLine("First method");
            delegateChain += () => Console.WriteLine("Second method method in chain");
            delegateChain += MyMethod;

            //Last delegate in chain returns value
            Console.WriteLine("Invoke chain");
            Console.WriteLine("####################");
            foreach(MeDelegate md in delegateChain.GetInvocationList())
            {
                Console.WriteLine("Method: " + md.Method + " , target: " + md.Target);
            }


            Console.WriteLine("*****");
            Console.WriteLine("Generic delegate in action");


            static IEnumerable<TArgs> InvokeChain<TArgs>(GenericDelegate<TArgs> genDel)
            {
                foreach(GenericDelegate<TArgs> del in genDel.GetInvocationList())
                {
                    yield return del();
                }
            }

            static IEnumerable<TArgs> InvokeFuncChain<TArgs>(Func<TArgs> genDel)
            {
                foreach (Func<TArgs> del in genDel.GetInvocationList())
                {
                    yield return del();
                }
            }

            GenericDelegate<int> GenDel = returnTen;
            GenDel += returnTwenty;

            IEnumerable<int> resultsForGenericChain = InvokeChain<int>(GenDel);
            foreach(int i in resultsForGenericChain)
            {
                Console.WriteLine("Result from generic chain: " + i);
            }

            //static IEnumerable<TArgs> InvokeLambdasFromAFuncDelegate<TArgs,TReturn>(Func<TArgs,TReturn> func)
            //{
            //    foreach(Func<TArgs,TReturn> f in func.GetInvocationList())
            //        {
            //            yield return f();
            //        }
                

            //}

        /**
         * Func and Action
         */
        Console.WriteLine("***************");
            Console.WriteLine("Func and Action delegates");

            Func<int> ActionChain = returnSixteen;
            Func<int, bool> TakeIntReturnBool = null;
            TakeIntReturnBool += (n) => n > 30;
            TakeIntReturnBool += (n) => n < 50;


            ActionChain += returnSixty;
            ActionChain += () => 666;
            ActionChain += () => 69;
            static int returnSixteen() { return 16; }
            static int returnSixty() { return 60; }
            
            resultsForGenericChain = InvokeFuncChain<int>(ActionChain);
            foreach (int i in resultsForGenericChain)
            {
                Console.WriteLine("Result from generic chain: " + i);
            }


        }


    }
}

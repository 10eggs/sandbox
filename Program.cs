using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace DelegatesLambdasEvents
{
    delegate void MeDelegate();
    delegate bool VerifcationMethod(int number);
    delegate T GenericDelegate<T>();

    delegate void BaseDelegate(Base b);
    delegate void DerivedDelegate(Derived d);

    delegate Base ReturnBaseObj();
    delegate Derived ReturnDerivedObj();




    delegate string MeDelegateTakeStringReturnString(string param);
    
    public enum Volume
    {
        Loud,
        Louder,
        TheLoudest
    }
    public class CustomizedEventArgs : EventArgs
    {
        private Volume vol;

        public Volume ReturnVol()
        {
            return this.vol;
        }
        public CustomizedEventArgs(Volume vol)
        {
            this.vol = vol;
        }
    }
    class NoSugar
    {
        int i = 10;
        public void IncrementI()
        {
            i++;
        }
        public void IncrementIByTwo()
        {
            i += 2;
        }

    }

    class Base { }
    class Derived : Base { }

    class Program
    {
        public static void CheckVolume(object obj, CustomizedEventArgs arg)
        {
            Subject s = obj as Subject;
            Console.WriteLine("Name: " + s.Name);
            if (arg.ReturnVol().Equals(Volume.Loud))
            {
                Console.WriteLine("It's not so bad");
            }
            else if (arg.ReturnVol().Equals(Volume.Louder))
            {
                Console.WriteLine("It's too loud!");
            }
        }
        static IEnumerable<int> SelectedNumbers(IEnumerable<int> list, VerifcationMethod verify)
        {
            IEnumerable<int> results = null;
            foreach (int elem in list)
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
            Console.WriteLine("Latest added method to Md delegate: " + Md.Method);
            Console.WriteLine("Target of Md delegate: " + Md.Target);
            Md += () => Console.WriteLine("I'm coming from lambda expression");
            Console.WriteLine("After adding lambda: " + Md.Method);
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
            foreach (int number in NumbersBiggerThanTen)
            {
                Console.WriteLine("Number bigger than five: " + number);
            }

            Console.WriteLine("Invocation of SelectedNumbers method with using lambda method");
            IEnumerable<int> NumbersBiggerThan200 = SelectedNumbers(new[] { 1, 2, 3, 10, 15, 25, 276, 326 }, (n) => n > 200);
            foreach (int number in NumbersBiggerThan200)
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
            foreach (MeDelegate md in delegateChain.GetInvocationList())
            {
                Console.WriteLine("Method: " + md.Method + " , target: " + md.Target);
            }


            Console.WriteLine("*****");
            Console.WriteLine("Generic delegate in action");


            static IEnumerable<TArgs> InvokeChain<TArgs>(GenericDelegate<TArgs> genDel)
            {
                foreach (GenericDelegate<TArgs> del in genDel.GetInvocationList())
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
            foreach (int i in resultsForGenericChain)
            {
                Console.WriteLine("Result from generic chain: " + i);
            }

            static IEnumerable<TReturn> InvokeLambdasFromAFuncDelegate<TArgs, TReturn>(Func<TArgs, TReturn> func, TArgs number)
            {
                foreach (Func<TArgs, TReturn> f in func.GetInvocationList())
                {
                    yield return f(number);
                }


            }

            /**
             * Func and Action
             */
            Console.WriteLine("***************");
            Console.WriteLine("Func and Action delegates");

            Func<int> FuncChain = returnSixteen;
            Func<int, bool> TakeIntReturnBool = null;


            TakeIntReturnBool += (n) => n > 30;
            TakeIntReturnBool += (n) => n < 50;


            FuncChain += returnSixty;
            FuncChain += () => 666;
            FuncChain += () => 69;
            static int returnSixteen() { return 16; }
            static int returnSixty() { return 60; }


            resultsForGenericChain = InvokeFuncChain<int>(FuncChain);
            foreach (int i in resultsForGenericChain)
            {
                Console.WriteLine("Result from generic chain: " + i);
            }
            //IEnumerable<bool> tableOfTruth= InvokeLambdasFromAFuncDelegate<int, bool>(TakeIntReturnBool, 60);
            //foreach(bool b in tableOfTruth)
            //{
            //    Console.WriteLine("Table of truth says: " + b);
            //}

            //We can use something else
            foreach (bool b in InvokeLambdasFromAFuncDelegate(TakeIntReturnBool, 60))
            {
                Console.WriteLine("Table of truth says: " + b);
            };

            //Annonymous Methods
            Func<int, bool> f = delegate (int i) { return i > 10; };
            Console.WriteLine("Anonymous method: " + f(30));


            //Closures
            Action ReturnAction()
            {
                int i = 0;
                return () => i++;

            }

            Action ReturnBlendedAction()
            {
                Action a = null;
                int i = 0;
                a += () =>
                 {
                     Console.WriteLine("First method");
                     i++;
                     Console.WriteLine("i value: " + i);

                 };
                a += () =>
                {
                    Console.WriteLine("Second method");
                    i++;
                    Console.WriteLine("i value: " + i);

                };
                return a;
            }

            Action SugarizedSyntax()
            {
                Action a = null;
                int i = 0;
                a += () => i++;
                a += () => i += 5;
                return a;
            }

            Action UnsugarizedSyntax()
            {
                Action foo = null;
                var ns = new NoSugar();
                foo += ns.IncrementI;
                foo += ns.IncrementIByTwo;
                return foo;
            }

            Action FirstAction = ReturnAction();
            Action SecondAction = ReturnAction();


            FirstAction(); FirstAction();
            SecondAction();

            Action FirstBlend = ReturnBlendedAction();
            FirstBlend();
            FirstBlend();
            FirstBlend();
            FirstBlend();

            Action UnsugarizedActionOne = UnsugarizedSyntax();
            Action UnsugarizedActionTwo = UnsugarizedSyntax();
            UnsugarizedActionOne();
            UnsugarizedActionTwo();

            UnsugarizedActionOne();
            UnsugarizedActionTwo();

            //Observer pattern
            Subject sub = new Subject();
            new Listener(sub);
            new Listener(sub);
            new Listener(sub);
            new Listener(sub);

            sub.InvokeAction();


            //Events
            //Action can be called directly - events not
            //Action can be directly assigned to null - events cant be assigned to null directly [ but u can add as many methods as u wish ]
            static void MethodHandlerSubstitue(object s, EventArgs ea)
            {
                Subject sub = s as Subject;

                Console.WriteLine("My name is: "+sub.Name);
            }

            Subject secSub = new Subject() { Name = "Dorothy"};
            Subject thirdSub = new Subject() { Name = "Judy"};
            secSub.TriggerListenersByEventHandler += MethodHandlerSubstitue;
            thirdSub.TriggerListenersByEventHandler += MethodHandlerSubstitue;

            secSub.BeTippedOver();
            thirdSub.BeTippedOver();

            Console.WriteLine("************************************");
            Console.WriteLine("***********Event handlers in action**************");
            Subject objectWithEventHandlers = new Subject();
            objectWithEventHandlers.EventHandlerWithEventArgs += CheckVolume;
            objectWithEventHandlers.CalEventHandlerWithEventArgs();



            /**
             * Delegate contravariance
             */
            static void TakeBase(Base b) { }
            static void TakeDerived(Derived d) { }

            static Base ReturnBase() { return null; }
            static Derived ReturnDerived() { return null; }

            BaseDelegate bd1 = TakeBase;
            //Line below it's not valid one 
            //BaseDelegate bd2 = TakeDerived;

            bd1(new Base());
            bd1(new Derived());

            DerivedDelegate dd1 = TakeBase;
            DerivedDelegate dd2 = TakeDerived;

            //Invalid, cause of contravariance
            //dd1(new Base());

            dd1(new Derived());
            dd2(new Derived());
            //Invalid
            //dd2(new Base());

            /**
             * Delegate contravariance
             */

            ReturnBaseObj RetBaseObj;
            ReturnDerivedObj RetDerObj;

            RetBaseObj = ReturnBase;
            RetBaseObj = ReturnDerived;

            //Invalid line, return type is not specific
            //RetDerObj = ReturnBase;
            RetDerObj = ReturnDerived;


            //Extension methods
            DateTime MartasBday = DateTime.Parse("14/10/2020");
            DateTime time = DateTime.Parse("10:00pm");

            DateTime combined1 = ExtensionMethods.Combine(MartasBday, time);
            DateTime combined2 = MartasBday.Combine(time);

            Console.WriteLine("Made from two args: " + combined1);
            Console.WriteLine("Made from extension method invoked from datetime object: " + combined2);

            //LINQ Introduction

            int[] numbers = new[] { 3, 4, 5, 10, 23, 333 };
            var result =
                   from n in numbers
                   where n % 2 == 0
                   select n;
            var resultsFromMethod =
                numbers.Where(n => n > 10)
                .Select(n => n);

            var results =
                Enumerable.Select(
                    Enumerable.Where(numbers, n => n > 10),
                    n=>n);

            var resultsFromMyExtension =
                numbers.Where(n => n > 0);
                //We are able to omit this Select clause
                //.Select(n => n);

   
            foreach(var res in results)
            {
                Console.WriteLine("Result: " + res);
            }

            foreach (var res in resultsFromMyExtension)
            {
                Console.WriteLine("Result: " + res);
            }

            //Deferred execution


        }



    }
}

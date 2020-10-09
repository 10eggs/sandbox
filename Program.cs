using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Reflection;

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

        static Random Random = new Random();


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
                    n => n);

            var resultsFromMyExtension =
                numbers.Where(n => n > 0);
            //We are able to omit this Select clause
                //.Select(n => n);


            foreach (var res in results)
            {
                Console.WriteLine("Result: " + res);
            }

            foreach (var res in resultsFromMyExtension)
            {
                Console.WriteLine("Result: " + res);
            }

            Console.WriteLine("***************************");
            Console.WriteLine("***************************");
            Console.WriteLine("***************************");
            Console.WriteLine("***************************");

            Console.WriteLine("DEFERRED EXECUTION!!!!");
            int[] randomNumbers = { 1, 2, 3, 4, 5, 6, 11, 12, 15 };


            //Due to some reason it doesnt work
            //var results = randomNumbers.Where(t => t > 5).Select(t => t);


            //Dependency injection

            //Create an instance of a class using Activator class 
            //var msgService = new MessageService();
            //var serviceByActivator =(HelloService)Activator.CreateInstance(typeof(HelloService));
            //There is no parameterless constructor, so need to pass ct arg to CreateInstance method
            //var consumerByActivator = (ServiceConsumer)Activator.CreateInstance(typeof(ServiceConsumer),((HelloService)Activator.CreateInstance(typeof(HelloService),msgService)));

            //foreach(Object o in typeof(ServiceConsumer).GetConstructors())
            //{
            //    ConstructorInfo c = (ConstructorInfo)o;
            //    var par= c.GetParameters();
            //    foreach (Object x in par)
            //    {
            //        Console.WriteLine(x);
            //    }
            //}

            //var singleConstructor = typeof(ServiceConsumer).GetConstructors().Single();

            var type = typeof(HelloService);

            //serviceByActivator.Print();

            var container = new DependencyContainer();

            container.AddTransient<HelloService>();
            container.AddTransient<ServiceConsumer>();
            container.AddSingleton<MessageService>();

            Console.WriteLine("Count for dependencies: " + container._dependencies.Count);


            var resolver = new DependencyResolver(container);

            var consumer1 = resolver.GetService<ServiceConsumer>();
            var consumer2 = resolver.GetService<ServiceConsumer>();
            var consumer3 = resolver.GetService<ServiceConsumer>();
            consumer1.Print();
            consumer2.Print();
            consumer3.Print();
            //var resolvedService = resolver.GetService<ServiceConsumer>();

            Console.WriteLine("*****************************");
            Console.WriteLine("*****************************");
            Console.WriteLine("*****************************");
            /**
             *      YIELD KEYWORD
             */
            //Syntactic sugar explanation

            foreach (int i in Program.GetRandomNumbers(10))
            {
                Console.WriteLine("MY RANDOM NUMBER: " + i);
            }

            foreach (int i in HybridNumbers)
            {
                Console.WriteLine(i);
            }

            //Desugarized foreach loop
            IEnumerable<int> Desugarized = HybridNumbers;
            IEnumerator rator = HybridNumbers.GetEnumerator();
            while (rator.MoveNext())
            {
                Console.WriteLine("Desugarized foreach loop!");
                Console.WriteLine(rator.Current);
            }

            //Grouping

            List<Customer> customers = new List<Customer>
            {
                new Customer { Country = "Poland", Name = "Tomek", Age = 21},
                new Customer { Country = "Poland", Name = "Mateusz", Age = 18},
                new Customer { Country = "Poland", Name = "Agata", Age = 27},
                new Customer { Country = "UK", Name = "Raheel", Age = 25},
                new Customer { Country = "UK", Name = "Luke", Age = 48},
                new Customer { Country = "UK", Name = "Steve", Age = 77},
                new Customer { Country = "Russia", Name = "Nikita", Age = 20},
                new Customer { Country = "Argentina", Name = "Domminica", Age = 16},
                new Customer { Country = "Argentina", Name = "RichBich", Age = 33},
                new Customer { Country = "Israel", Name = "JesusChristus", Age = 600},
                new Customer { Country = "Moldovia", Name = "Siergiej", Age = 40},
            };

            foreach(Customer c in customers.OrderBy(c => c.Country))
            {
                Console.WriteLine(c.Country + ":" + c.Name);
            }

            var customersGroupedByCountry = customers.GroupBy(c => c.Country).OrderByDescending(g=>g.Count());
            Console.WriteLine("Time for grouping: ");
            foreach(IGrouping<string,Customer> g in customersGroupedByCountry)
            {
                Console.WriteLine("Group name: " + g.Key);
                foreach(Customer c in g)
                {
                    Console.WriteLine("Name: " + c.Name);
                }
            }

            var useLetType =
                from g in customersGroupedByCountry
                    //introduce variable to query
                let count = g.Count()
                orderby count descending
                //Return new anon type
                select new { Country = g.Key, NumCustomers = count };


            var withoutLetKeyword =
                customersGroupedByCountry.Select(g => new { g, NumCustomers = g.Count() })
                .OrderBy(at => at.NumCustomers)
                .Select(at => new { at.g.Key, at.NumCustomers });



            //Introduce INTO keyword
            //Selecting (Projecting) While Grouping
            //purplemath.com QUADRATIC FORMULA
            var selectingWhileGrouping=customers.GroupBy(g => new { g.Country }, g => g);
            foreach(var g in selectingWhileGrouping)
            {
                Console.WriteLine(g.Key+": ");
                foreach(Customer c in g)
                {
                    Console.WriteLine(c.Name + ": " + c.Age);
                }
            }

            //Let Clauses And Even Deeper Transparent Identifiers
            var inputs = new[]
            {
                new { a=1, b=2, c=3 },
                new { a=2, b=9, c=4 },
                new{ a=7, b=3, c=6}
            };
            //Two approaches
            //First
            var roots =
                from coef in inputs
                let negB = -coef.b
                let discriminant = coef.b * coef.b - 4 * coef.a * coef.c
                let twoA = 2 * coef.a
                select new
                {
                    FirstRoot = (negB + discriminant) / twoA,
                    SecondRoot = (negB - discriminant) / twoA
                };

            //Second
            var rootsByExtensions =
                inputs
                .Select(coef => new { coef, negB = -coef.b })
                .Select(t1 => new { t1, discriminant = t1.coef.b * t1.coef.b - 4 * t1.coef.a * t1.coef.c })
                .Select(t2 => new { t2, twoA = 2 * t2.t1.coef.a })
                .Select(t3 => new { FirstRoot = (t3.t2.t1.negB + t3.t2.discriminant) / t3.twoA, SecondRoot = (t3.t2.t1.negB - t3.t2.discriminant) / t3.twoA });
        }
        static IEnumerable<int> GetRandomNumbers(int count)
        {
            GetRandomNumberClass ret = new GetRandomNumberClass();
            ret.count = count;

            return ret;
        }

        public static IEnumerable<int> Numbers
        {
            get
            {
                Console.WriteLine("Start");
                Console.WriteLine("Return 3");
                yield return 3;
                Console.WriteLine("Return 5");
                yield return 5;
                Console.WriteLine("Return 66");
                yield return 66;
                Console.WriteLine("This blocked was called after last invocation of yield return - now it's finished");
            }
        }

        public static IEnumerable<int> HybridNumbers
        {
            get { return new NumberHybrid(); }
        }
        class NumberHybrid : IEnumerable<int>, IEnumerator<int>
        {
            int state;
            int current;
            public int Current
            {
                get { return current; }
            }
            public bool MoveNext()
            {
                switch (state)
                {
                    case 0:
                        Console.WriteLine("Start");
                        Console.WriteLine("Yield 3");
                        current = 3;
                        state = 1;
                        break;
                    case 1:
                        Console.WriteLine("Yield 5");
                        state = 2;
                        current = 5;
                        break;
                    case 2:
                        Console.WriteLine("Yield 13");
                        current = 13;
                        state = 3;
                        break;
                    case 3:
                        Console.WriteLine("End!");
                        return false;

                }

                return true;
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
            public IEnumerator<int> GetEnumerator()
            {
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Dispose()
            {
            }

            public void Reset()
            {
            }

        }
        class GetRandomNumberClass : IEnumerable<int>, IEnumerator<int>
        {
            public int count;
            public int i;
            public int current;
            int state;

            public int Current
            {
                get { return current; }
            }
            public bool MoveNext()
            {
                switch (state)
                {
                    //Initialization of for loop
                    case 0:
                        i = 0;
                        goto case 1;
                    case 1:
                        state = 1;
                        if (!(i < count))
                            return false;
                        current = Program.Random.Next();
                        state = 2;
                        return true;
                    case 2:
                        i++;
                        goto case 1;
                }
                return false;
            }
            object IEnumerator.Current
            {
                get { return Current; }
            }

            public IEnumerator<int> GetEnumerator()
            {
                //Return itself, cause this class implement IEnumerator interface
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                //It wont call itself recursively, at first it will looking for method which is not implemented explicitly
                return GetEnumerator();
            }
            public void Reset(){}
            public void Dispose() { }

        }

    }
}

using System.Diagnostics;

namespace TestExercises
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //PrintNumberSquared(5);

            //Console.WriteLine("Hello, World!");

            //Thread.Sleep(1000);
            //await Task.Delay(1000);

            // теперь потестим мой вариант
            // у меня есть какая-то работа
            // я ее передаю в машину состояний            
            var stateMachine = new StateMachine();
            var asyncOperationResult = stateMachine.MoveNext(new AsyncOperation(() => SomeWork()));

            // Понял, это будет следующий уровень, и это будет регаться не здесь. Это по сути должно выполниться в последнем case машины состояний
            // точнее, я думаю я делал все правильно, в рамках своей идеи, но grok ведет меня по другому пути. проявлю гибкость, и пройду тогда
            // прделагаемый им путь до конца
            //asyncOperationResult.OnCompleted(() =>
            //{
            //    // если я правильно понимаю, я должен запихнуть сюда хвост тещкуго метода
            //    // нет такой магии, чтоб я тут мог сказать потоку - иди продолжи выполняться со следующей строки после этого вызова
            //    Console.WriteLine($"Last line in main. This is thread: {Thread.CurrentThread.ManagedThreadId}");
            //});

            await Task.Delay(6000);

            // Console.WriteLine($"Last line in main. This is thread: {Thread.CurrentThread.ManagedThreadId}");
         }

        private static void SomeWork()
        {
            Console.WriteLine($"Starting some work. This is thread: {Thread.CurrentThread.ManagedThreadId}");

            var timer = new Stopwatch();
            timer.Start();

            while(timer.ElapsedMilliseconds < 5000)
            {
                Thread.Sleep(1000);
            }

            Console.WriteLine($"Ending some work. This is thread: {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}

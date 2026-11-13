namespace TestExercises;

internal static class AsyncWorkOrchestrator
{
    // Как эта хуйня должна работать:
    // короче, мне надо выполнить какую-то работу асинхронно
    // у меня должно быть место, которое говорит - отсюда асинхронно
    // короче, я путаю опять механизмы и зоны ответственности
    // значит есть механизм машины состояний, он только переключает состояния и видимо хранит их
    // и нужен механизм, который будет ответственнен за то, чтоб в нем регистрировать какую-то работу, которую надо сделать асинхронно
    // и который будет продолжать ее делать

    public static void RunSomeWorkAsynchronously(AsyncOperation work)
    {
        // типа запускаю пулом потоков
        // и мне надо сохранить где-то что я выполняю эту задачу
        // а еще зарегистрировать callback, который значит может дернуть машина состояний, чтоб продолжить работу типа...

        /*
            типа:
            ThreadPool.QueueWork(work);
            when done - call OperationCompleted
            give this callback to caller
        */

        // так, WaitCallback это делегат вида Waitcallback(object? state), а значит он соответствует методу, который принимает один аргумент типа object
        // значит я могу отправить сюда метод, который принимает один аргумент
        ThreadPool.QueueUserWorkItem(_ =>
        {
            // про прокидывания состояния подумаем потом
            // полагаю при простом Action без входных параметров и сохранять ниче не нужно...
            // а не, хуйню несу, ведь внутри Action могут быть разные перменные...
            // хотя нет, переменные внутри Action используются только в нем... короче, щас запустаюсь, пока null просто
            work.DoWork(null);
        });
    }

    // public static event EventHandler? AsynchronousOperationCompleted;

    // бля я не могу передать this если класс статичный
    // бля а можно обойтись и без него
    //public static void OperationCompleted()
    //{
    //    AsynchronousOperationCompleted.Invoke(this, EventArgs.Empty);
    //}
}

namespace TestExercises;

// Не путать концепции... здесь типа уже был вызван await (допустим для моего простого примера), поэтому создалась машина состояний
// мне здесь не надо думать про await и потоки... ааа, или надо, мне надо запустить работу, если она не завершена
// и отпустить поток, и запомнить где вернуться
internal class StateMachine
{
    // what this should have?
    // states... obviously...
    // saved variables?
    // what else?

    // So it has to move the state somehow from one to another

    // it needs to register callbacks... in my Orchestrator I guess?

    // need some sort of result...

    private int _state = 0;

    private readonly AsyncOperationResult _stateMachineResult = new();

    public AsyncOperationResult StateMachineResult
    {
        get { return _stateMachineResult; }
    }

    // public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    // допустим принимает работу, которая принимает переменную любого типа
    //public static AsyncOperationResult MoveNext<T>(Action<T> someWork)
    // а бля вот как раз мне нужо получать здесь саму задачу, которую надо выполнить асинхронно, потому что мне надо проверять завершена или нет эта работа...
    // одно следует из другого, прикольно... Заводим класс AsyncOperation

    // кто-то должен каждый раз вызывать MoveNext(), чтоб код продолжился дальше с нужного места... AsyncWorkOrchestrator получается
    public AsyncOperationResult MoveNext(AsyncOperation someAsynchronousOperation)
    {
        switch (_state)
        {
            case 0:
                {
                    _stateMachineResult.OperationStatus = OperationStatus.InProgress;

                    // мне надо запустить работу, если она не завершена
                    if (someAsynchronousOperation.OperationResult.OperationStatus != OperationStatus.Completed)
                    {
                        // хуй знает пока сделаю метод для примера start
                        // бля а это работа при текущей моей реализации пойдет выполняться синхронно
                        //someAsynchronousOperation.Start();

                        // мне надо что-то типа создания потока и запуска работы в нем
                        // ThreadPool.QueueUserWorkItem();

                        // допустим я сохраняю как-то состояние тут, дальше мой оркестратор

                        // бля а кто должен дергать колбэк... запутался опять... потерял мысль...
                        // var callback = AsyncWorkOrchestrator.RegisterWork();

                        // Колбэк по сути это вызов этого самого метода опять с этого места
                        // возможно есть разные способы реализации этого, но
                        // если представить что используются события...
                        // чекнул че такое события, при таком раскладе при вызове события мне надо передать туда такой метод MoveNext, в котором state = 1 я просто щас в режиме
                        // ничего не знаю и просто пробую

                        // здесь у меня еще и закмыкание и захват _state в объект из-за лямбды. чем это чревато? долго думать... сейчас моя задача не в микро-оптимизациях
                        // а добиться рабочего варианта
                        someAsynchronousOperation.OperationResult.OnCompleted(() =>
                        {
                            this._state = 1;
                            this.MoveNext(someAsynchronousOperation);
                        });


                        AsyncWorkOrchestrator.RunSomeWorkAsynchronously(someAsynchronousOperation);
                    }
                    else
                    {
                        _state = 1;
                        this.MoveNext(someAsynchronousOperation);

                        // goto - по сути просто не делать return - и тогда код пойдет к следующему кейсу...
                    }

                    return someAsynchronousOperation.OperationResult;
                }
            case 1:
                {
                    _stateMachineResult.OperationStatus = OperationStatus.Completed;
                    
                    if(_stateMachineResult.Continuation is not null)
                        _stateMachineResult.Continuation();

                    _stateMachineResult.SetAsyncOperationResult();

                    // Console.WriteLine($"Last line of supposedly transferred tail code from original method (in this case main). This is thread: {Thread.CurrentThread.ManagedThreadId}");
                    return someAsynchronousOperation.OperationResult;
                }
            default:
                throw new InvalidOperationException();
        }
    }

    // private event AsynchronousOperationCompleted
    // public event EventHandler<OrderEventArgs> OrderPlaced;

    // Пока не усложняем аргументами 
    // переносим в оркестратор, типа, он должен нам сказать, что работа завершена...
    //public event EventHandler AsynchronousOperationCompleted;

    //public void OperationCompleted()
    //{
    //    AsynchronousOperationCompleted.Invoke(this, EventArgs.Empty);
    //}
}

namespace TestExercises;

public class AsyncOperation
{
    private AsyncOperationResult _thisOperationResult = new AsyncOperationResult();
    private readonly Action _someWorkToDoAsynchronously;

    public AsyncOperationResult OperationResult
    {
        get { return _thisOperationResult; }
    }

    // понял, здесь будет куча методов... как там это называется... типа одинаковое название, разные параметры, набор для Func, набор для Action
    // (1-12 параметров типа полагаю, как в самих Func и Action). Но для упрощения и для себя реализую парочку
    //public AsyncOperationResult AsyncOperation<T>(Action<T> someWorkToDoAsynchronously)
    //{

    //}

    // давай пока не усложнять дженериками...
    public AsyncOperation(Action someWorkToDoAsynchronously)
    {
        _someWorkToDoAsynchronously = someWorkToDoAsynchronously;
    }

    // i need somehow start it 
    // and at the same time i need to know whether it ended successfully or failed
    // wait... don't overcomplicate for now, I just need to know whether it is completed
    // internal AsyncOperationResult Start()
    // this does not just start the work, this does all the work..
    //internal AsyncOperationResult DoWork(object? state)
    //{
    //    someWorkToDoAsynchronously();
    //    _thisOperationResult.IsCompleted = true;
    //    return _thisOperationResult;
    //}
    // а как это иначе сделать. тогда придется выставлять наружу саму работу, которую положил в someWorkToDoAsynchronously
    // ааа, про мои виды результатов - try - catch - но это потом

    // бля если костылить с ThreadPool, то метод не должен иметь возвращаемый тип
    internal void DoWork(object? state)
    {
        _thisOperationResult.OperationStatus = OperationStatus.InProgress;
        _someWorkToDoAsynchronously();
        _thisOperationResult.OperationStatus = OperationStatus.Completed;
        //return _thisOperationResult;

        if(_thisOperationResult.Continuation is not null)
            _thisOperationResult.Continuation();
    }
}

namespace AsyncAwaitInDepth;

// why the fuck by default the property is of init type... perhaps records work that way
// internal record AsyncOperationResult(bool IsCompleted = false);

public enum OperationStatus
{
    NotStarted,
    InProgress,
    Completed
}

public class AsyncOperationResult
{
    private readonly TaskCompletionSource _taskCompletionSource = new();

    private Action? _continuation;

    public OperationStatus OperationStatus { get; set; }

    // короче, вызывающий код создает AsyncOperation и отправляет в машину состояний, получает вот этот результат (который может быть незавершенной операцией
    // позднее вызывающий код может подписаться на завершение операции и передать че надо делать после
    // соответственно, если операция к этому моменту уже завершена - надо сразу выполнить хвост
    // если еще не завершена - сохраняем, чтоб вызвать по завершении
    // теперь вопрос как вызвать по заврешении, если переменная private... Публичный метод здесь или сам делегат сделать публичным?
    // Если бы делегат мог быть Action, Func и т. д., пришлось бы делать кучу перегрузок... а не гоню... в оригинале наверное отдается state как объект с полями - переменные, используемые в хвосте
    // а сам метод какой генерится? бля... наверное такой, который просто принимает state... тогда это всегда Func, и не надо кучи перегрузок
    // но в моем случае мне думаю достаточно свойство публичное добавить, геттер то есть
    public Action? Continuation => _continuation;

    public Task AsyncOperationTask => _taskCompletionSource.Task;

    public void SetAsyncOperationResult()
    {
        _taskCompletionSource.SetResult();
    }

    public void OnCompleted(Action continuation)
    {
        if (OperationStatus == OperationStatus.Completed)
        {
            continuation(); // уже готово — выполняем сразу
        }
        else
        {
            _continuation = continuation;  // ещё не готово — запоминаем
        }

        // неправильно, здесь мы даем вызывающему коду подписаться на завершение работы, и если тут же установим таску в RunToCompletion
        // то в main await будет на завершенном таске сразу, и работа не успеет выполниться, так как ThreadPool потоки background...
        // _taskCompletionSource.SetResult();
    }
}
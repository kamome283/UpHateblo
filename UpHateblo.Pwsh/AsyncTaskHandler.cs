using System.Threading.Channels;

namespace UpHateblo.Pwsh;

/// <summary>
/// 並列処理に対応した非同期タスク実行ハンドラー
/// </summary>
/// <remarks>
/// PowerShellのCmdletは、同期的な実行のみをサポートしています。
/// 本クラスはこれらの制約のもと非同期タスクを簡易に扱うために、
/// プロデューサー・コンシューマーパターンで非同期タスクを管理することを目的としています。
/// </remarks>
internal class AsyncTaskHandler<TSource, TResult, TFailure> where TFailure : Exception
{
    private readonly Channel<TSource> _inChannel = Channel.CreateUnbounded<TSource>();
    private readonly Channel<(TSource source, TResult result, TFailure exception)> _outChannel =
        Channel.CreateUnbounded<(TSource source, TResult result, TFailure exception)>();
    public required ParallelOptions ParallelOptions { get; init; }
    public required Func<TSource, CancellationToken, Task<TResult>> Body { private get; init; }
    public IEnumerable<(TSource source, TResult result, TFailure exception)> BlockingOut =>
        _outChannel.Reader.ReadAllAsync(CancellationToken).ToBlockingEnumerable();
    private CancellationToken CancellationToken => ParallelOptions.CancellationToken;

    public void Write(TSource source)
    {
        _inChannel.Writer.TryWrite(source);
    }

    public void Complete()
    {
        _inChannel.Writer.Complete();
    }

    public AsyncTaskHandler<TSource, TResult, TFailure> StartProcessing()
    {
        Task.Run(StartProcessingAsync, CancellationToken);
        return this;
    }

    private async Task StartProcessingAsync()
    {
        await Parallel.ForEachAsync(
            _inChannel.Reader.ReadAllAsync(CancellationToken),
            ParallelOptions, Process);
        _outChannel.Writer.Complete();
    }

    private async ValueTask Process(TSource source, CancellationToken token)
    {
        try
        {
            var result = await Body(source, CancellationToken);
            await _outChannel.Writer.WriteAsync((source, result, null), CancellationToken);
        }
        catch (TFailure ex)
        {
            await _outChannel.Writer.WriteAsync((source, default, ex), CancellationToken);
        }
    }
}
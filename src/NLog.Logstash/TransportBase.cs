using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NLog.Logstash
{
    abstract class TransportBase : IDisposable
    {
        private Stream _stream;

        protected int QueueCapacity { get; set; } = 50000;

        private readonly ConcurrentQueue<byte[]> _queue = new ConcurrentQueue<byte[]>();

        CancellationTokenSource _cts;

        public void Push(byte[] packet)
        {
            if (_queue.Count == QueueCapacity || packet == null)
            {
                return;
            }
            _queue.Enqueue(packet);

            if (_cts != null) return;

            _cts = new CancellationTokenSource();
            DoWork(_cts.Token);
        }

        private void DoWork(CancellationToken token)
        {
            Task.Run(async () =>
            {
                do
                {
                    byte[] packet;
                    while (_queue.TryPeek(out packet))
                    {
                        try
                        {
                            if (_stream == null)
                                _stream = await GetStream();

                            await _stream.WriteAsync(packet, 0, packet.Length, token);
                            await _stream.FlushAsync(token);
                        }
                        catch (Exception)
                        {
                            CloseStream();
                            break;
                        }
                        _queue.TryDequeue(out packet);
                    }
                    await Task.Delay(1, token);
                } while (!token.IsCancellationRequested);
            }, token);
        }

        protected abstract Task<Stream> GetStream();

        private void CloseStream()
        {
            _stream?.Dispose();
            _stream = null;
        }

        public void Dispose()
        {
            _cts?.Cancel();
            CloseStream();
        }
    }
}

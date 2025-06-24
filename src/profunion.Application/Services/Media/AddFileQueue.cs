using profunion.Domain.Models.UploadModel;
using profunion.Shared.Dto.Uploads;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Services.Media
{
    public class AddFileQueue
    {
        private readonly ConcurrentQueue<Uploads> _queue = new();
        private readonly SemaphoreSlim _signal = new(0);

        public void Enqueue(Uploads file)
        {
            _queue.Enqueue(file);
            _signal.Release();
        }

        public async Task<Uploads?> DequeueAsync(CancellationToken cancellation)
        {
            await _signal.WaitAsync(cancellation);
            _queue.TryDequeue(out var file);
            return file;
        }
    }
}

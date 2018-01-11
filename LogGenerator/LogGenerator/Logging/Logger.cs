using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LogGenerator.Logging
{
    public class Logger
    {
        private const int BufferSize = 2048;
  
        private const string File = @"C:\source\Logs\access-log.txt";
        private readonly Queue<LogRecord> _cache = new Queue<LogRecord>();

        public Logger()
        {
            if (System.IO.File.Exists(File))
            {
                System.IO.File.Delete(File);
            }
            using (var stream = new FileStream(File, FileMode.CreateNew))
            using (var writer = new StreamWriter(stream, Encoding.UTF8, BufferSize))
            {
                writer.WriteLine("Event Time,Workflow,Request,Role,User,IP Address");
            }
        }

        public void Log(DateTime eventTime, string workflow, string request, string role, string user, string ipAddress)
        {
            _cache.Enqueue(new LogRecord(eventTime, workflow, request, role, user, ipAddress));
        }

        public async Task Purge()
        {
            using (var stream = new FileStream(File, FileMode.Append))
            using (var writer = new StreamWriter(stream, Encoding.UTF8, BufferSize))
            {
                while (_cache.Count > 0)
                {
                    var current = _cache.Dequeue();
                    await writer.WriteLineAsync($"{current.EventTime:O},{current.Workflow},{current.Request},{current.Role},{current.User},{current.IpAddress}");
                }
            }
        }
    }
}
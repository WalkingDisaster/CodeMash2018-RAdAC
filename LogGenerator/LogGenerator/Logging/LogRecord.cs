using System;

namespace LogGenerator.Logging
{
    public class LogRecord
    {
        public LogRecord(
            DateTime eventTime
            , string workflow
            , string request
            , string role
            , string user
            , string ipAddress
        )
        {
            EventTime = eventTime;
            Workflow = workflow;
            Request = request;
            Role = role;
            User = user;
            IpAddress = ipAddress;
        }

        public DateTime EventTime { get; }
        public string Workflow { get; }
        public string Request { get; }
        public string Role { get; }
        public string User { get; }
        public string IpAddress { get; }

        public void Deconstruct(
            out DateTime eventTime
            , out string workflow
            , out string request
            , out string role
            , out string user
            , out string ipAddress
        )
        {
            (eventTime, workflow, request, role, user, ipAddress) =
                (EventTime, Workflow, Request, Role, User, IpAddress);
        }
    }
}
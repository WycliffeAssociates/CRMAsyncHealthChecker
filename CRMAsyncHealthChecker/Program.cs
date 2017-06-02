using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using CRMAsyncHealthChecker.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json;

namespace CRMAsyncHealthChecker
{
    /// <summary>
    /// Main application
    /// </summary>
    public static class Program
    {
        private const int WAITINGSTATUS = 0;

        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Config file must be specified");
                Environment.Exit(1);
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("Config file not found");
                Environment.Exit(2);
            }

            Console.WriteLine("Loading config");
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(args[0]));

            Console.WriteLine("Connecting");
            CrmServiceClient service = new CrmServiceClient(config.ConnectionString);

            // if we are past the limit then send an email
            Console.WriteLine("Querying");
            if (CheckRecordsPastLimit(service, config.Limit))
            {
                Console.WriteLine("Sending mail");
                SmtpClient client = new SmtpClient(config.EmailServerAddress, config.EmailServerPort);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(config.EmailServerUsername, config.EmailServerPassword);
                MailMessage message = new MailMessage();
                message.From = new MailAddress(config.FromAddress);
                foreach (string address in config.ToAddress)
                {
                    message.To.Add(address);
                }

                message.Subject = "Async processor warning";
                message.Body = $"Number of records waiting is over the limit of {config.Limit}";
                client.Send(message);
            }
        }

        /// <summary>
        /// Check to see if the number of async records is at or past the limit
        /// </summary>
        /// <param name="service">The crm service we are using to connect</param>
        /// <param name="limit">The limit to test against</param>
        /// <returns>True if the records are greater then or equal to the limit false if otherwise</returns>
        public static bool CheckRecordsPastLimit(IOrganizationService service, int limit)
        {
            QueryExpression query = new QueryExpression("asyncoperation");
            query.Criteria.AddCondition("statuscode", ConditionOperator.Equal, WAITINGSTATUS);
            return service.RetrieveMultiple(query).Entities.Count >= limit;
        }
    }
}

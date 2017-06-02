using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMAsyncHealthChecker.Models
{
    public class Config
    {
        public string EnvironmentName { get; set; }

        public string ConnectionString { get; set; }

        public string EmailServerAddress { get; set; }

        public int EmailServerPort { get; set; }

        public string EmailServerUsername { get; set; }

        public string EmailServerPassword { get; set; }

        public string FromAddress { get; set; }

        public List<string> ToAddress { get; set; }

        public int Limit { get; set; }
    }
}

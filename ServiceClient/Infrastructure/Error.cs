using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClient.Infrastructure
{
    public class Error : Exception
    {
        private string _Description;
        private string _Method;

        public Error(string description, string method)
        {
            this._Description = description;
            this._Method = method;
        }

        public string Description { get; set; }

        public string Method { get; set; }
    }
}

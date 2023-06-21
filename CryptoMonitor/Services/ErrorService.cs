using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMonitor.Services
{
    public class ErrorService
    {
        string errorMessage;
        public string ErrorMessage { get=> errorMessage;}

        public void SetErrorMessage(string message)
        {
            errorMessage = message;
        }
    }
}

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Exceptions
{
    public class DataAccessException : Exception
    {
        public DataAccessException(Exception ex , string message , ILogger logger)
        {
            logger.LogError($"main Exception {ex.Message} developer custom Exception : {message}");
        }
    }
}

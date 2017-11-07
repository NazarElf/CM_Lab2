using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM_Lab2_WPF
{
    class InvalidTransitionException : Exception
    {
        public InvalidTransitionException(string message)
            : base(message)
        {
        }
    }
}

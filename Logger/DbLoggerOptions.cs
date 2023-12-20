using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApp.Logger
{
    public class DbLoggerOptions
    {
        public string ConnectionString { get; init; }

	public string[] LogFields { get; init; }

	public string LogTable { get; init; }

	public DbLoggerOptions()
	{
	}
    }
}
using System.Reflection;
using System.IO;

namespace timepunch
{
    public partial class DataAccess
    {
        static private string GetConnectionString()
        {
            // Get the directory of the current assembly:
            string baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return $"Data Source={baseDir}/databases/timepunch.sqlite;";
        }
    }
}
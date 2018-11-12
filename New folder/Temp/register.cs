using System.Management;

namespace Compiler.Temp
{
    internal class kk
    {
        public kk()
        {
            var query = new
                ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'");
            var queryCollection = query.Get();
            queryCollection.ToString();
        }
    }
}
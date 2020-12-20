using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace jolugbokb.Interface
{
    public interface IConverters
    {
        public string DecodeEncrypt(string b);
        public string EncryptDecode(string b);
    }
}

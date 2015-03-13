using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace NetCode
{
    class NetCode
    {
        public static long ToLong(string addr)
        {
            // careful of sign extension: convert to uint first;
            // unsigned NetworkToHostOrder ought to be provided.
            return (long)(uint)IPAddress.NetworkToHostOrder((long)IPAddress.Parse(addr).Address);
        }

        public static string ToAddr(long address)
        {
            return IPAddress.Parse(address.ToString()).ToString();
            // This also works:
            // return new IPAddress((uint) IPAddress.HostToNetworkOrder(
            //    (int) address)).ToString();
        }


        public static IPAddress ToIPAddress(string address)
        {
            return IPAddress.Parse(address);
        }
    }
}

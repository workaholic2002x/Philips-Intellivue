using System.Net;
using System.Text;
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;

namespace capture
{
    public class DNSHeader
    {
        //DNS header fields
        private ushort usIdentification;        //Sixteen bits for identification
        private ushort usFlags;                 //Sixteen bits for DNS flags
        private ushort usTotalQuestions;        //Sixteen bits indicating the number of entries 
                                                //in the questions list
        private ushort usTotalAnswerRRs;        //Sixteen bits indicating the number of entries
                                                //entries in the answer resource record list
        private ushort usTotalAuthorityRRs;     //Sixteen bits indicating the number of entries
                                                //entries in the authority resource record list
        private ushort usTotalAdditionalRRs;    //Sixteen bits indicating the number of entries
                                                //entries in the additional resource record list
        //End DNS header fields

        public DNSHeader(byte []byBuffer, int nReceived)
        {
            MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived);
            BinaryReader binaryReader = new BinaryReader(memoryStream);    
   
            //First sixteen bits are for identification
            usIdentification = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Next sixteen contain the flags
            usFlags = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Read the total numbers of questions in the quesion list
            usTotalQuestions = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Read the total number of answers in the answer list
            usTotalAnswerRRs = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Read the total number of entries in the authority list
            usTotalAuthorityRRs = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Total number of entries in the additional resource record list
            usTotalAdditionalRRs = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
        }

        public string Identification
        {
            get
            {
                return string.Format("0x{0:x2}", usIdentification);
            }
        }

        public string Flags
        {
            get
            {
                return string.Format("0x{0:x2}", usFlags);
            }
        }

        public string TotalQuestions
        {
            get
            {
                return usTotalQuestions.ToString();
            }
        }

        public string TotalAnswerRRs
        {
            get
            {
                return usTotalAnswerRRs.ToString();
            }
        }

        public string TotalAuthorityRRs
        {
            get
            {
                return usTotalAuthorityRRs.ToString();
            }
        }

        public string TotalAdditionalRRs
        {
            get
            {
                return usTotalAdditionalRRs.ToString();
            }
        }
	}
}

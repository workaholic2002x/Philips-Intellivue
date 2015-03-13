//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace capture
//{
//     public class ICMPPacket // rfc792
//    {
//        public byte Type;
//        public byte Code;
//        public ushort Checksum;
//        public byte[] PacketData;
//        public ICMPMessage Message;

//        public ICMPPacket() : base() { }

//        public ICMPPacket(ref byte[] Packet)
//            : base()
//        {
//            try
//            {
//                Type = (byte)Packet[0];
//                Code = (byte)Packet[1];
//                Checksum = (ushort)System.BitConverter.ToInt16(Packet, 2);
//                PacketData = new byte[Packet.Length - 4];
//                System.Buffer.BlockCopy(Packet, 4, PacketData, 0, Packet.Length - 4);
//            }
//            catch { }

//            switch (Type)
//            {
//                case 0: Message = new ICMPEchoReply(ref PacketData); break;
//                case 3: Message = new ICMPDestinationUnreachable(ref PacketData); break;
//                case 4: Message = new ICMPSourceQuench(ref PacketData); break;
//                case 5: Message = new ICMPRedirect(ref PacketData); break;
//                case 8: Message = new ICMPEcho(ref PacketData); break;
//                case 11: Message = new ICMPTimeExceeded(ref PacketData); break;
//                case 12: Message = new ICMPParameterProblem(ref PacketData); break;
//                case 13: Message = new ICMPTimestamp(ref PacketData); break;
//                case 14: Message = new ICMPTimestampReply(ref PacketData); break;
//                case 15: Message = new ICMPInformationRequest(ref PacketData); break;
//                case 16: Message = new ICMPInformationReply(ref PacketData); break;
//            }
//        }

//        public byte[] GetBytes()
//        {
//            if (Message != null) PacketData = Message.GetBytes();
//            if (Message is ICMPEchoReply) Type = 0;
//            else if (Message is ICMPDestinationUnreachable) Type = 3;
//            else if (Message is ICMPSourceQuench) Type = 4;
//            else if (Message is ICMPRedirect) Type = 5;
//            else if (Message is ICMPEcho) Type = 8;
//            else if (Message is ICMPTimeExceeded) Type = 11;
//            else if (Message is ICMPParameterProblem) Type = 12;
//            else if (Message is ICMPTimestamp) Type = 13;
//            else if (Message is ICMPTimestampReply) Type = 14;
//            else if (Message is ICMPInformationRequest) Type = 15;
//            else if (Message is ICMPInformationReply) Type = 16;

//            if (PacketData == null) PacketData = new byte[0];
//            byte[] Packet = new byte[4 + PacketData.Length];
//            (byte)Packet[0] = Type;
//            (byte)Packet[1] = Code;
//            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)0), 0, Packet, 2, 2);
//            System.Buffer.BlockCopy(PacketData, 0, Packet, 4, PacketData.Length);
//            Checksum = GetChecksum(ref Packet, 0, Packet.Length - 1);
//            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)Checksum), 0, Packet, 2, 2);
//            return Packet;
//        }

//        public ushort GetChecksum(ref byte[] Packet, int start, int end)
//        {
//            uint CheckSum = 0;
//            int i;
//            for (i = start; i < end; i += 2) CheckSum += (ushort)System.BitConverter.ToInt16(Packet, i);
//            if (i == end) CheckSum += (ushort)Packet[end];
//            while (CheckSum >> 16 != 0) CheckSum = (CheckSum & 0xFFFF) + (CheckSum >> 16);
//            return (ushort)~CheckSum;
//        }
//    }

//    public abstract class ICMPMessage
//    {
//        public abstract byte[] GetBytes();
//    }

//    public class ICMPIPHeaderReply : ICMPMessage
//    {
//        public byte[] Data;
//        public IPPacket IP;

//        public ICMPIPHeaderReply() : base() { }

//        public ICMPIPHeaderReply(ref byte[] Packet)
//            : base()
//        {
//            try
//            {
//                Data = new byte[Packet.Length - 4];
//                System.Buffer.BlockCopy(Packet, 4, Data, 0, Data.Length);
//                IP = new IPPacket(ref Data);
//            }
//            catch { }
//        }

//        public override byte[] GetBytes()
//        {
//            if (Data == null) Data = new byte[0];
//            byte[] Packet = new byte[4 + Data.Length];
//            System.Buffer.BlockCopy(Data, 0, Packet, 4, Data.Length);
//            return Packet;
//        }
//    }

//    public class ICMPEcho : ICMPMessage
//    {
//        public ushort Identifier;
//        public ushort SequenceNumber;
//        public string Data;

//        public ICMPEcho() : base() { }

//        public ICMPEcho(ref byte[] Packet)
//            : base()
//        {
//            try
//            {
//                Identifier = (ushort)System.BitConverter.ToInt16(Packet, 0);
//                SequenceNumber = (ushort)System.BitConverter.ToInt16(Packet, 2);
//                Data = System.Text.Encoding.ASCII.GetString(Packet, 4, Packet.Length - 4);
//            }
//            catch { }
//        }

//        public override byte[] GetBytes()
//        {
//            if (Data == null) Data = "";
//            byte[] Packet = new byte[4 + Data.Length];
//            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)Identifier), 0, Packet, 0, 2);
//            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)SequenceNumber), 0, Packet, 2, 2);
//            System.Buffer.BlockCopy(System.Text.Encoding.ASCII.GetBytes(Data), 0, Packet, 4, Data.Length);
//            return Packet;
//        }
//    }

//    public class ICMPEchoReply : ICMPEcho
//    {
//        public ICMPEchoReply() : base() { }
//        public ICMPEchoReply(ref byte[] Packet) : base(ref Packet) { }
//    }

//    public class ICMPRedirect : ICMPMessage
//    {
//        public ulong GatewayInternetAddress;
//        public byte[] Data;

//        public enum CodeEnum
//        {
//            RedirectDatagramsForTheNetwork = 0,
//            RedirectDatagramsForTheHost = 1,
//            RedirectDatagramsForTheTypeOfServiceAndNetwork = 2,
//            RedirectDatagramsForTheTypeOfServiceAndHost = 3
//        }

//        public ICMPRedirect() : base() { }

//        public ICMPRedirect(ref byte[] Packet)
//            : base()
//        {
//            try
//            {
//                GatewayInternetAddress = (ulong)System.BitConverter.ToInt32(Packet, 0);
//                Data = new byte[Packet.Length - 4];
//                System.Buffer.BlockCopy(Packet, 0, Data, 4, Packet.Length);
//            }
//            catch { }
//        }

//        public override byte[] GetBytes()
//        {
//            if (Data == null) Data = new byte[0];
//            byte[] Packet = new byte[4 + Data.Length];
//            System.Buffer.BlockCopy(System.BitConverter.GetBytes((long)GatewayInternetAddress), 0, Packet, 0, 4);
//            System.Buffer.BlockCopy(Data, 0, Packet, 4, Data.Length);
//            return Packet;
//        }
//    }

//    public class ICMPDestinationUnreachable : ICMPIPHeaderReply
//    {
//        public enum CodeEnum
//        {
//            NetUnreachable = 0,
//            HostUnreachable = 1,
//            ProtocolUnreachable = 2,
//            PortUnreachable = 3,
//            FragmentationNeededAndDFSet = 4,
//            SourceRouteFailed = 5
//        }

//        public ICMPDestinationUnreachable() : base() { }
//        public ICMPDestinationUnreachable(ref byte[] Packet) : base(ref Packet) { }
//    }

//    public class ICMPSourceQuench : ICMPIPHeaderReply
//    {
//        public ICMPSourceQuench() : base() { }
//        public ICMPSourceQuench(ref byte[] Packet) : base(ref Packet) { }
//    }

//    public class ICMPTimeExceeded : ICMPIPHeaderReply
//    {
//        public enum CodeEnum
//        {
//            TimeToLiveExceededInTransit = 0,
//            FragmentReassemblyTimeExceeded = 1
//        }

//        public ICMPTimeExceeded() : base() { }
//        public ICMPTimeExceeded(ref byte[] Packet) : base(ref Packet) { }
//    }

//    public class ICMPParameterProblem : ICMPMessage
//    {
//        public byte Pointer;
//        public byte[] Data;

//        public ICMPParameterProblem() : base() { }

//        public ICMPParameterProblem(ref byte[] Packet)
//            : base()
//        {
//            try
//            {
//                Pointer = Packet[0];
//                Data = new byte[Packet.Length - 4];
//                System.Buffer.BlockCopy(Packet, 0, Data, 4, Packet.Length);
//            }
//            catch { }
//        }

//        public override byte[] GetBytes()
//        {
//            if (Data == null) Data = new byte[0];
//            byte[] Packet = new byte[4 + Data.Length];
//            Packet[0] = Pointer;
//            System.Buffer.BlockCopy(Data, 0, Packet, 4, Data.Length);
//            return Packet;
//        }
//    }

//    public class ICMPTimestamp : ICMPMessage
//    {
//        public ushort Identifier;
//        public ushort SequenceNumber;
//        public ulong OriginateTimestamp;
//        public ulong ReceiveTimestamp;
//        public ulong TransmitTimestamp;

//        public ICMPTimestamp() : base() { }

//        public ICMPTimestamp(ref byte[] Packet)
//            : base()
//        {
//            try
//            {
//                Identifier = (ushort)System.BitConverter.ToInt16(Packet, 0);
//                SequenceNumber = (ushort)System.BitConverter.ToInt16(Packet, 2);
//                OriginateTimestamp = (ulong)System.BitConverter.ToInt32(Packet, 4);
//                ReceiveTimestamp = (ulong)System.BitConverter.ToInt32(Packet, 8);
//                TransmitTimestamp = (ulong)System.BitConverter.ToInt32(Packet, 12);
//            }
//            catch { }
//        }

//        public override byte[] GetBytes()
//        {
//            byte[] Packet = new byte[16];
//            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)Identifier), 0, Packet, 0, 2);
//            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)SequenceNumber), 0, Packet, 2, 2);
//            System.Buffer.BlockCopy(System.BitConverter.GetBytes((long)OriginateTimestamp), 0, Packet, 4, 4);
//            System.Buffer.BlockCopy(System.BitConverter.GetBytes((long)ReceiveTimestamp), 0, Packet, 8, 4);
//            System.Buffer.BlockCopy(System.BitConverter.GetBytes((long)TransmitTimestamp), 0, Packet, 12, 4);
//            return Packet;
//        }
//    }

//    public class ICMPTimestampReply : ICMPTimestamp
//    {
//        public ICMPTimestampReply() : base() { }
//        public ICMPTimestampReply(ref byte[] Packet) : base(ref Packet) { }
//    }

//    public class ICMPInformationRequest : ICMPMessage
//    {
//        public ushort Identifier;
//        public ushort SequenceNumber;

//        public ICMPInformationRequest() : base() { }

//        public ICMPInformationRequest(ref byte[] Packet)
//            : base()
//        {
//            try
//            {
//                Identifier = (ushort)System.BitConverter.ToInt16(Packet, 0);
//                SequenceNumber = (ushort)System.BitConverter.ToInt16(Packet, 2);
//            }
//            catch { }
//        }

//        public override byte[] GetBytes()
//        {
//            byte[] Packet = new byte[4];
//            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)Identifier), 0, Packet, 0, 2);
//            System.Buffer.BlockCopy(System.BitConverter.GetBytes((short)SequenceNumber), 0, Packet, 2, 2);
//            return Packet;
//        }
//    }

//    public class ICMPInformationReply : ICMPInformationRequest
//    {
//        public ICMPInformationReply() : base() { }
//        public ICMPInformationReply(ref byte[] Packet) : base(ref Packet) { }
//    }
//}

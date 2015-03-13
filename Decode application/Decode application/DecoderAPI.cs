using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using u_16 = System.UInt16;
using MeasurementState = System.UInt16;
using OIDType = System.UInt16;
using FLOATType = System.UInt32;
using RelativeTime = System.UInt32;
using AbsoluteTime = System.UInt64;
using PrivateOID = System.UInt16;
using _HANDLE = System.UInt16;
using CMDTYPE = System.UInt16;
using TextId=System.UInt32;
using MdsContext = System.UInt16;
using System.Runtime.InteropServices;





namespace Decode_application
{
    class DecoderAPI:DataStructure
    {
        
        // APDU
const UInt16 ROIV_APDU=1;
const UInt16 RORS_APDU = 2;
const UInt16 ROER_APDU = 3;
const UInt16 ROLRS_APDU = 5;

        protected Dictionary<Int64, string> APDU = new Dictionary<Int64, string>()
        {
       
            { 0x0001	 , "ROIV_APDU" },
            { 0x0002	 , "RORS_APDU" },
            { 0x0003	 , "ROER_APDU" },
            { 0x0005	 , "ROLRS_APDU" },
        };


  //CMD Type    
const UInt16 CMD_EVENT_REPORT=0;
const UInt16 CMD_CONFIRMED_EVENT_REPORT=1;
const UInt16 CMD_GET=3;
const UInt16 CMD_SET=4;
const UInt16 CMD_CONFIRMED_SET=5;
const UInt16 CMD_CONFIRMED_ACTION = 7;
const UInt16 CMD_UNKNOWN = 8;
        



//######################################################################################//


public const string _FRAME_HEADER="E100";
        public List<string> _GXUndecoded_Attributes = new List<string>();
        public   List<string> _GUndecoded_Attributes= new List<string>();

     public List<string> _GXUndecoded_Chunk = new List<string>();
        public List<string> _GUndecoded_Chunk = new List<string>();

        public List<byte[]> _GBNewDataChunk = new List<byte[]>();
        public List<string> _GSNewDataChunk = new List<string>();
        public Dictionary<UInt16 ,UInt16> NewAttribute =new Dictionary<UInt16,UInt16>();

        public UInt32 G_CTR;

        //public List<string> getdata(string data, string Rel_TimeRef)
        public List<string> getdata(byte[] G_DATA, string Rel_TimeRef)
        {
            
            bool STATUS = false;
            string str = "";
            
            List<string> tempList=new List<string>();
            string[] dataList=new string[100];   // Max nos of expected packets
            //byte[] l_val = new byte[1024];
            List<byte[]> _datachunks = new List<byte[]>();
            

            UInt16 OBJ_TYPE = 0;  // Either WAVE or Numeric
            RelativeTime _RELATIVE_TIME = 0;
            

            //const UInt16 CMD_EVENT_REPORT = 0;
            //const UInt16 CMD_CONFIRMED_EVENT_REPORT = 1;
            //const UInt16 CMD_GET = 3;
            //const UInt16 CMD_SET = 4;
            //const UInt16 CMD_CONFIRMED_SET = 5;
            //const UInt16 CMD_CONFIRMED_ACTION = 7;
            //const UInt16 CMD_UNKNOWN = 8;
            List<byte[]> _FrameChunk=new List<byte[]>();
            

            //if (ExtractValidDataStream(data, ref l_val) == true)
            //if (ExtractValidDataStream(data, ref _FrameChunk) == true)
            if (ExtractValidDataStream(G_DATA, ref _FrameChunk) == true)
                tempList.Add("Total Frames= " + _FrameChunk.Count);// + _RELATIVE_TIME);
            
            tempList.Clear();
            
           // foreach(byte[] l_val in _FrameChunk)
            for (UInt16 _lli=0; _lli<_FrameChunk.Count;_lli++)
            {

                //_GUndecoded_Chunk.Clear();
                //_GUndecoded_Attributes.Clear();


                G_CTR = _lli;
                _datachunks.Clear();
                _GBNewDataChunk.Clear();
                //_NewDataChunk.Clear();
                STATUS = DataDefragment(_FrameChunk[_lli], ref _datachunks, ref OBJ_TYPE, ref _RELATIVE_TIME, ref _GBNewDataChunk);
               // STATUS = DataDefragment_Incomplete(_FrameChunk[_lli], ref _datachunks, ref OBJ_TYPE);
               

                if (_datachunks.Count > 0)
                {
                    tempList.Add("");
                    tempList.Add("");
                    tempList.Add("############################# New Raw Packet: [Count= " + _lli.ToString() + " ] : [Packet nos = " + _datachunks.Count.ToString() + " ] : [Length= " + _FrameChunk[_lli].Length.ToString() + "] ###############################");
                    tempList.Add(ByteArrayToHEXString(_FrameChunk[_lli]));
                    tempList.Add("_________________________ Decoded Packets _______________________");
                    tempList.Add("Stating Relative Time Stamp->   [ " + Convert.ToString((_RELATIVE_TIME - Convert.ToDouble(Rel_TimeRef)) / 8000) + " ] Sec   ");// + _RELATIVE_TIME);
                }
                else
                {
                                    // .Add("");
                    _GUndecoded_Chunk.Add("");
                    _GUndecoded_Chunk.Add("############################# New Raw Packet: [Count= " + _lli.ToString() + " ] : [Packet nos = " + _datachunks.Count.ToString() + " ] : [Length= " + _FrameChunk[_lli].Length.ToString() + "] ###############################");
                    _GUndecoded_Chunk.Add(ByteArrayToHEXString(_FrameChunk[_lli]));
                    _GUndecoded_Chunk.Add("_________________________ Un-Parsed Packets _______________________");

                    
                        
                }



                if (_GBNewDataChunk.Count > 0)
                {
                    _GSNewDataChunk.Add("");
                    _GSNewDataChunk.Add("############################# New Frames: [Count= " + _GBNewDataChunk.Count.ToString() + " ] : [Packet nos = " + _datachunks.Count.ToString() + " ] : [Length= " + _FrameChunk[_lli].Length.ToString() + "] ###############################");
                    _GSNewDataChunk.Add(ByteArrayToHEXString(_FrameChunk[_lli]));
                    _GSNewDataChunk.Add("_________________________ New Frames Packets _______________________");

                    UInt16 _attid = 0;
                    
                    foreach (byte[] item in _GBNewDataChunk)
                    {
                        string _str=ByteArrayToHEXString(item);
                        _GSNewDataChunk.Add(_str);
                    
                        _attid = (UInt16)((UInt16)(item[0] << 8) + item[1]);

                        
                        if (!NewAttribute.ContainsKey(_attid))
                            NewAttribute.Add(_attid, 1);
                        else
                        {
                            //_count =(UInt16) (NewAttribute[_attid] + 1);
                            NewAttribute[_attid] = (UInt16)(NewAttribute[_attid] + 1);

                        }
                    }
                }

                //_GUndecoded_Attributes.Add("");
                //_GUndecoded_Attributes.Add("");
                //_GUndecoded_Attributes.Add("############################# New Raw Packet: [Count= " + _lli.ToString() + " ] : [Packet nos = " + _datachunks.Count.ToString() + " ] : [Length= " + _FrameChunk[_lli].Length.ToString() + "] ###############################");
                //_GUndecoded_Attributes.Add(ByteArrayToHEXString(_FrameChunk[_lli]));
                //_GUndecoded_Attributes.Add("_________________________ Un-Decoded Packets _______________________");
                

                
                //OBJ_TYPE = 0;
                if (OBJ_TYPE == CMD_EVENT_REPORT || OBJ_TYPE == CMD_CONFIRMED_SET || OBJ_TYPE == CMD_CONFIRMED_EVENT_REPORT)
                    foreach (byte[] _value in _datachunks)
                   // for(UInt16 lxi=0; lxi<_datachunks.Count;lxi++)
                    {
                        ParseByATTID(_value, OBJ_TYPE, ref tempList);
                        //ParseByATTID(_datachunks[lxi], OBJ_TYPE, ref tempList);
                    }
                else
                {
                    tempList.Add("Unnecessary command : OBJ_TYPE=" + OBJ_TYPE);
                    //System.Windows.Forms.MessageBox.Show("Unnecessary command : OBJ_TYPE=" + OBJ_TYPE);
                }

                //  tempList.AddRange(Numeric_Observed_Value(l_val));//50ba00100f2000000000"));//     87cb0001001c");


                if (_GUndecoded_Attributes.Count > 0)
                {

                    _GUndecoded_Attributes.Insert(0,"");
                    _GUndecoded_Attributes.Insert(1,"");
                    _GUndecoded_Attributes.Insert(2,"############################# New Raw Packet: [Count= " + _lli.ToString() + " ] : [Packet nos = " + _datachunks.Count.ToString() + " ] : [Length= " + _FrameChunk[_lli].Length.ToString() + "] ###############################");
                    _GUndecoded_Attributes.Insert(3,ByteArrayToHEXString(_FrameChunk[_lli]));
                    _GUndecoded_Attributes.Insert(4,"_________________________ Un-Decoded Packets _______________________");

                    _GXUndecoded_Attributes.AddRange(_GUndecoded_Attributes);
                    _GUndecoded_Attributes.Clear();
                }


            }


           


            return tempList;
        }


        bool ParseByATTID(byte[] _value, UInt16 _Object_type, ref List<string> message)
        {
            UInt16 _lvalue = (UInt16)((UInt16)(_value[0] << 8) + _value[1]);
            message.Add("0x" + _lvalue.ToString("X") + "  ::  " + ByteArrayToHEXString(_value));// + System.Environment.NewLine);
            if (_Object_type == 0)
            {


                  //message.Add("0x" + _lvalue.ToString("X") +"  ::  " + ByteArrayToHEXString(_value));// + System.Environment.NewLine);
                switch (_lvalue)
                {
                    //case 0x0101:
                    //    message.AddRange(GenericWAVE_Value(_value)); // to be changed later
                    //    //_GUndecoded_message.Add(_lvalue.ToString("X"));
                    //    break;

                    case 0x0902:
                        message.AddRange(Generic_LongArrayString(_value)); // to be changed later
                        //_GUndecoded_message.Add(_lvalue.ToString("X"));
                        break;
                    case 0x0904:
                        message.AddRange(Generic_LongArrayString(_value)); // to be changed later
                        //_GUndecoded_message.Add(_lvalue.ToString("X"));
                        break;
                    case 0x0916:

                        message.AddRange(Generic_LongArrayString(_value));
                        //_GUndecoded_message.Add(_lvalue.ToString("X"));
                        break;
                    case 0x091A:

                        message.AddRange(SaVisual_Grid16(_value));
                        break;

                        
                    //Generic 3 Param Values

                    case 0x090C:
                    case 0x090D:
                    case 0x0911:
                    case 0x0917:
                    case 0x091D:
                    case 0x091E:
                    case 0x0921:
                    case 0x0924:
                    case 0x0928:
                    case 0x092F:
                    case 0x0935:
                    case 0x0937:
                    case 0x0940:
                    case 0x0945:
                    case 0x0946:
                    case 0x0948:
                    case 0x096A:
                    case 0x096F:
                    case 0x0982:
                    case 0x0984:
                    case 0x0986:
                    case 0x098D:
                    case 0x0990:
                    case 0x0991:
                    case 0x0996:
                    case 0x09A7:
                    case 0xF001:
                    case 0xF008:
                    case 0xF13A:
                    case 0x86DE:
                    case 0xF14D:
                    case 0xF25E:
                    case 0xF25F:
                    case 0xF261:
                                        
                    // _GUndecoded_message.Add(_lvalue.ToString("X"));
                        message.AddRange(Generic3Param_Value(_value));
                        break;
                    
                    case 0x0927:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x092D:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x093C:
                        //_GUndecoded_message.Add(_lvalue.ToString("X"));
                       // message.AddRange(Generic3Param_Value(_value));
                        message.AddRange(Generic_LongArrayString (_value));
                        break;
                    case 0x093D:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x093F:
                        message.AddRange(Metric_Spec(_value));
                        //_GUndecoded_message.Add(_lvalue.ToString("X"));
                        break;
                    case 0x094B:    // Compound Numeric Observed Value

                        message.AddRange(Decode_CMPD_NU_OBSVal(_value));
                        break;
                    case 0x0950:
                        message.AddRange(Numeric_Observed_Value(_value));
                        break;
                    case 0x0956:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x0957:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x0958:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x095A:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x095C:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x095D:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x0961:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x0962:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x0964:    // Calibration
                        //_GUndecoded_message.Add(_lvalue.ToString("X"));
                        
                        message.AddRange(CalData16_code(_value));
                        break;
                    case 0x0967:    // Decode_Compound sample array value
                        //_GUndecoded_message.Add(_lvalue.ToString("X"));
                        message.AddRange(Decode_CMPD_SA_OBSVal(_value));
                        break;
                    case 0x096D:
                        message.AddRange(Sample_Array_Spec(_value));
                        //_GUndecoded_message.Add(_lvalue.ToString("X"));
                        break;
                    case 0x096E:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x0985:
                       // _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        message.AddRange(SystemSpec_Value(_value));
                        break;
                    case 0x0987:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x098F:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x099E:
                        message.AddRange(EnumObsVal_Value(_value));
                        //_GUndecoded_message.Add(_lvalue.ToString("X"));
                        break;
                    case 0x09D8:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x09DC:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x09DF:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x0A16:
                        message.AddRange(SaFixedValSpec16_code(_value));
                       // _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0x0A1E:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0xF009:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0xF100:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0xF101:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0xF129:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0xF12A:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0xF13E:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0xF1EC:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0xF1FA:
                        //_GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        message.AddRange(MdsGenSystemInfo_code(_value));
                        break;
                    case 0xF228:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0xF239:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;
                    case 0xF23A:
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X"));
                        break;

                    ///////////////////////////////////////////////////////////////////////////////////
                    /*
                    case 0x098B:
                    case 0x0983:
                    case 0x098C:
                    case 0x0922:
                    case 0x0926:
                    case 0x0920:
                    case 0x92B:
                    case 0x092C:
                    case 0x93E:
                    case 0x094E:
                    case 0x094F:
                    case 0x0953:
                    case 0x0976:
                    case 0x0998:
                    case 0x097F:
                    case 0x933:
                    case 0x93A:
                    case 0x0972:
                    case 0x0974:
                    case 0x981:
                    case 0x993:
                    case 0x99D:
                    case 0x9A4:
                    case 0x9A0:
                    case 0x9A1:
                    case 0xF237:
                    case 0xF12E:
                    case 0xF208:
                    case 0xF1FC:
                        //message.AddRange(Generic3Param_Value(_value));
                        // _GUndecoded_message.Add(_lvalue.ToString("X"));
                        message.AddRange(Unknown_ATT_code(_value));
                        break;

                        */
                    default:
                        message.RemoveAt(message.Count - 1); // Remove ATT-id if that was not parsed;
                        _GUndecoded_Attributes.Add(_lvalue.ToString("X") + ": Attribute-ID Mentioned but not Linked");
                        return false;
                }
            }
            else if (_Object_type == 1) // WAVE TYPE
            {
                message.AddRange(GenericWAVE_Value(_value));
            }
           
             //message.Add(_lvalue.ToString("X"));
            
            return true;
        }


        //bool ExtractValidDataStream(string _data,   ref List<byte[]> Frame_list )//ref byte[] _dataArrayXX)
        bool ExtractValidDataStream(byte[] _DATA_Array, ref List<byte[]> Frame_list)//ref byte[] _dataArrayXX)
        {


            //_data = System.Text.RegularExpressions.Regex.Replace(_data, @"\s+", "");
            //_data = _data.ToUpper();  // convert all to UPPER
            //byte[] _dataArray= new byte[_data.Length];

            byte[] _dataArray = _DATA_Array;

            bool _STATUS=false;
            string _dummyString="";
            

            Int32 leng1=0, leng2 = 0;
            string _substr = "";
            //byte[] _dummy=new byte[1000];
         //   _dataArray = new byte[_data.Length];

            
            //if (_data.Length % 2 != 0)  // Just to findout even number which is exceptional
            //{
            //    _data = _data.Substring(1);
            //}

            //_dataArray = HEXStringToByteArray(_data);

            UInt16 _header = 0;

            for (int _li = 0; _li < (_dataArray.Length - 1); _li++)
            {
                
                 _header = (UInt16)((UInt16)(_dataArray[0] << 8) + _dataArray[1]);
                if (_header == 0xE100)
                {
                    if (_dataArray.Length > 7)
                    {
                        _header = (UInt16)((UInt16)(_dataArray[6] << 8) + _dataArray[7]);
                        if (_dataArray.Length >= (_header + 8))
                        {
                            //_header = _dataArray.Take(8 + _header).ToArray();
                            byte[] _dummy = new byte[ _header+8];
                            _dummy=_dataArray.Take(8 + _header).ToArray();
                            Frame_list.Add(_dummy);
                            
                            //_dataArrayXX = _dummy;
                             _STATUS = true;
                            //break;
                        }
                        else
                            _STATUS = false;
                    }
                    else
                        _STATUS = false;
                }
                else
                    _STATUS = false;


                if (_STATUS == false)
                    _dataArray = _dataArray.Skip(2).ToArray();
                else
                {
                    _dataArray = _dataArray.Skip(_header+8).ToArray();
                    _STATUS = false;
                }

                
            }




            if (Frame_list.Count > 0)
                _STATUS = true;
            else
                _STATUS = false;


          return _STATUS;   // return suncess
        }




        //This function breaks the whole data stream to multiple frame/chunks
        public bool DataDefragment(byte[] l_val, ref List<byte[]> _lChunks, ref UInt16 Object_Type, ref RelativeTime REL_TIMESTAMP, ref List<byte[]> _NewChunks)
        {

            //   List<string> _lstring = new List<string>();
            //            List<byte[]> _lChunks = new List<byte[]>();
            bool _STATUS = false;

            
            //Remove the header part : Header= 4+4+6 =14 Bytes, Next frame starting must indicate object-ID, but never found as on dated 24/05/2014
            //So, let us take every two bytes and proceed to check untill it matches with Attribute ID.
            UInt16 _temp = 0;
           // byte[] _headerStuff = new byte[100];
            
            //_headerStuff = l_val.Take(FirstBytes).ToArray();
            
            int Element_count = 0;// elements in the stream;
            UInt16 Ro_Type = (UInt16)((UInt16)(l_val[4] << 8) + l_val[5]); // gets the Ro Type
            UInt16 Command_Type = (UInt16)((UInt16)(l_val[10] << 8) + l_val[11]); ; // gets the command Type
            

            //if (Ro_Type == 3 || Ro_Type == 5)
            //    System.Windows.Forms.MessageBox.Show("Undecoded Ro Type");

            if (Ro_Type == 1 || Ro_Type == 2)
            {
                //// CMD_EVENT_REPORT = 0;
                //// CMD_CONFIRMED_EVENT_REPORT = 1;
                //// CMD_GET = 3;
                //// CMD_SET = 4;
                //// CMD_CONFIRMED_SET = 5;
                //// CMD_CONFIRMED_ACTION = 7;
                //// CMD_UNKNOWN = 8;

                if (Command_Type == CMD_EVENT_REPORT || Command_Type == CMD_CONFIRMED_EVENT_REPORT ||
                    Command_Type == CMD_CONFIRMED_SET || Command_Type == CMD_UNKNOWN)   //==0
                {

                    //if (Ro_Type == 1 && Command_Type == 1)
                    //    System.Windows.Forms.MessageBox.Show("Check Rel Time and Abs Time");

                    l_val = l_val.Skip(14).ToArray();  //14 bytes Left. 

                    REL_TIMESTAMP = (UInt32)((UInt32)(l_val[6] << 24) + (UInt32)(l_val[7] << 16) + (UInt32)(l_val[8] << 8) + l_val[9]);
                    
                   // Element_count = (UInt16)((UInt16)(l_val[6] << 8) + l_val[7]);

                    while (l_val.Length > 0)  // though, it should be terminated after 10-30 bytes
                    {
                        _temp = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);
                        //if ((Attribute_ID.ContainsKey(_temp) == false) && (Attribute_ID_Wave.ContainsKey(_temp) == false))
                        if ((Attribute_ID.ContainsKey(_temp) == false) && (Phy_ID_Wave.ContainsKey(_temp) == false))
                        {
                            //  _headerStuff = _headerStuff + l_val.Take(2).ToArray();
                            l_val = l_val.Skip(2).ToArray();
                            _STATUS = false;
                        }
                        else
                        {
                            _STATUS = true;
                            break;
                        }
                    }

                    if (_STATUS == false)
                        REL_TIMESTAMP = 0;

                    //Now, skip initial 4B, then next immidiate 2 Bytes indicated how many further 2xbytes to be skipped !
                    //l_val = l_val.Skip(4).ToArray();
                    //_temp = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);
                    //_temp = (UInt16)(_temp * 2 + 2);
                    //l_val = l_val.Skip(_temp).ToArray();



                    //Now look at the 2,3rd bytes to get next data length, trim the frame and then proceed  

                    int _offset = 0;
                    UInt16 _key = 0;
                    string _lstr = "";
                    byte[] _frameChunk = new byte[1024];
                    byte[] _NewFrameChunk = new byte[0];
                    Object_Type = 0;
                    bool _LFlag = false;

                    for (int _lctr = 0; l_val.Length > 0; _lctr++)//= _lctr + _offset)
                    {
                        //Lets get the Attribute ID
                        _key = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);
                        UInt16 _iLen=0;
                        if (l_val.Length >= 4)
                            _iLen = (UInt16)((UInt16)(l_val[2] << 8) + l_val[3]);
                        else
                            _iLen = 9999;


                        bool _Flg1 = Attribute_ID.ContainsKey(_key);
                        //bool _Flg2 = Attribute_ID_Wave.ContainsKey(_key);
                        bool _Flg2 = Phy_ID_Wave.ContainsKey(_key);
                        if ((_Flg1==true || _Flg2==true)&& _iLen<=(l_val.Length-4))  // If Valid ATTRIBUTE ID Avail
                        {
                            _STATUS = true;
                            _LFlag = true;
                            _NewFrameChunk = new byte[0];
                            byte[] _dump= new byte[2];
                            _dump = l_val.Take(2).ToArray();
                            if (_Flg2 == true )   // If wave object, leave two immidiate bytes before length_indicator
                            {
                                l_val = l_val.Skip(2).ToArray();
                                if(_lChunks.Count==0)
                                Object_Type = 1;
                            }
                           
                               
                            

                            _temp = (UInt16)((UInt16)(l_val[2] << 8) + l_val[3]);
                            _offset = 2 + 2 + _temp;
                            _frameChunk = l_val.Take(_offset).ToArray();
                            if (_Flg2 == true)
                            {
                                _frameChunk[0] = _dump[0];
                                _frameChunk[1] = _dump[1];
                            }
                            _lChunks.Add(_frameChunk);
                            _STATUS = true;

                    
                            l_val = l_val.Skip(_offset).ToArray();
                            //_lstring.Add( l_val);
                        }
                        else
                        {
                            
                            if (l_val.Length > 0)
                            {
                                //check, last entry was valid or not.
                                if (_NewChunks.Count > 0 && _lChunks.Count > 0)
                                    if(_NewChunks[_NewChunks.Count-1].Length==6)  // if it is more than 6, 99% chances, it is another chunk
                                {
                                    
                                    byte[] _tempo = _NewChunks[_NewChunks.Count - 1];
                                    UInt16 _count = (UInt16)((UInt16)(_tempo[2] << 8) + _tempo[3]);
                                    UInt16 _length = (UInt16)((UInt16)(_tempo[4] << 8) + _tempo[5]);
                                    UInt16 _lengthX = 0;
                                    if (_lChunks.Count >= _count)
                                    {
                                        for (int _lo = 1; _lo <= _count; _lo++)
                                        {
                                            _lengthX =(UInt16)( _lengthX + _lChunks[_lChunks.Count - _lo].Length);
                                        }
                                        if (_lengthX == _length)
                                            _NewChunks.RemoveAt(_NewChunks.Count - 1);
                                    }
                                }




                                Array.Resize(ref _NewFrameChunk, _NewFrameChunk.Length + 2);
                                Array.Copy(l_val,0, _NewFrameChunk,_NewFrameChunk.Length-2, 2);
                                UInt16 _head = (UInt16)((UInt16)(_NewFrameChunk[0] << 8) + _NewFrameChunk[1]);
                                if (_head < 0x0100)
                                    _NewFrameChunk = _NewFrameChunk.Skip(2).ToArray();
                                if (_NewFrameChunk.Length >= 6)// && _LFlag==true)
                                {
                                    
                                    UInt16 _leng = (UInt16)((UInt16)(_NewFrameChunk[2] << 8) + _NewFrameChunk[3]);
                                    if (_NewFrameChunk.Length - 4 == _leng)
                                    {
                                        _NewChunks.Add(_NewFrameChunk);
                                        _LFlag = false;
                                        _NewFrameChunk = new byte[0];

                                    }
                                        
                                }


                                l_val = l_val.Skip(2).ToArray();

                            }
                            //else
                            //{

                            //    _STATUS = false;
                            //    break;
                            //}
                        }

                        // Check for last element of "_NewChunks" valid or invalid

                        if (_NewChunks.Count > 0 && _lChunks.Count > 0)
                            if (_NewChunks[_NewChunks.Count - 1].Length == 6)  // if it is more than 6, 99% chances, it is another chunk
                            {

                                byte[] _tempo = _NewChunks[_NewChunks.Count - 1];
                                UInt16 _count = (UInt16)((UInt16)(_tempo[2] << 8) + _tempo[3]);
                                UInt16 _length = (UInt16)((UInt16)(_tempo[4] << 8) + _tempo[5]);
                                UInt16 _lengthX = 0;
                                if (_lChunks.Count >= _count)
                                {
                                    for (int _lo = 1; _lo <= _count; _lo++)
                                    {
                                        _lengthX = (UInt16)(_lengthX + _lChunks[_lChunks.Count - _lo].Length);
                                    }
                                    if (_lengthX == _length)
                                        _NewChunks.RemoveAt(_NewChunks.Count - 1);
                                }
                            }



                    }





                }
                else if (Command_Type == CMD_GET) //==3
                {

                    GetArgument _Getargument = new GetArgument();
                    l_val = l_val.Skip(14).ToArray();  //14 bytes Left.
                    _Getargument._ManagedObjectId.m_obj_class = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);
                    _Getargument._ManagedObjectId.context_id = (UInt16)((UInt16)(l_val[2] << 8) + l_val[3]);
                    _Getargument._ManagedObjectId.handle = (UInt16)((UInt16)(l_val[4] << 8) + l_val[5]);

                    _Getargument.scope = (UInt32)((UInt32)(l_val[6] << 24) + (UInt32)(l_val[7] << 16) + (UInt32)(l_val[8] << 8) + l_val[9]);

                    _Getargument.Attributelist.count = (UInt16)((UInt16)(l_val[10] << 8) + l_val[11]);
                    _Getargument.Attributelist.length = (UInt16)((UInt16)(l_val[12] << 8) + l_val[13]);

                    _Getargument.Attributelist.OIDType=new OIDType[_Getargument.Attributelist.count];

                    l_val=l_val.Skip(14).ToArray();
                    for(int _li=0; _li<_Getargument.Attributelist.count; _li++)
                    {
                        _Getargument.Attributelist.OIDType[_li]=(UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);
                        l_val=l_val.Skip(2).ToArray();
                    }


                    Object_Type = 3;
                    /// What next?????
                      //  _lChunks.Add(l_val);


                    
                    _STATUS = true;




                }
                else //if (Command_Type == CMD_CONFIRMED_SET) //==5
                {
                   // System.Windows.Forms.MessageBox.Show("Un-coded CommandType");
                    //_lChunks.Add("Un-decoded CommandType");
                    _STATUS = false;
                }

            }


            //if (Element_count != _lChunks.Count)
            //    System.Windows.Forms.MessageBox.Show("Element_Count:Some data not persed properly");

            return _STATUS;
        }
        //----------------------------------------------------------------/
        



        public Object ByteArrayToNormalStructureXSpecialaaa(ref byte[] bytearray, Object _refobj)
        {
            if (bytearray.Length < 12) // If length is too small dont parse it
                return _refobj;

            UInt16 _count = (UInt16)((UInt16)(bytearray[2] << 8) + bytearray[3]);
            UInt16 _leng = (UInt16)((UInt16)(bytearray[4] << 8) + bytearray[5]);

            if ((_count * 6 > (bytearray.Length - 6)) || _leng == 0 || _leng > (bytearray.Length - 6))
                return _refobj;

            

            object o = Activator.CreateInstance(_refobj.GetType());

            int _len = Marshal.SizeOf(o);

            IntPtr IPTR_databytes = Marshal.AllocHGlobal(_len);

            SwapIt(_refobj.GetType(), bytearray, 0);

            Marshal.Copy(bytearray, 0, IPTR_databytes, _len);

            _refobj = Marshal.PtrToStructure(IPTR_databytes, _refobj.GetType());//typeof(_refobj));

            bytearray = bytearray.Skip(_len).ToArray();

            
            return _refobj;

        }



        //NNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN
        //MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
        private bool IsEndPacket(byte[] _chpacket)
        {
            bool _STASTUS = false;

            if (_chpacket.Length < 5)
                return true;
            else
            {
                UInt16 _Attid = (UInt16)((UInt16)(_chpacket[0] << 8) + _chpacket[1]);
                UInt16 _length = (UInt16)((UInt16)(_chpacket[4] << 8) + _chpacket[5]);
                UInt16 _count = (UInt16)((UInt16)(_chpacket[2] << 8) + _chpacket[3]);
                if (_length == 0 || _count == 0)
                    return true;
                else if (Attribute_ID.ContainsKey(_Attid))
                    return true;
                //else if (Attribute_ID_Wave.ContainsKey(_Attid))
                else if (Phy_ID_Wave.ContainsKey(_Attid))
                    return true;
                else
                    return false;



            }


            return _STASTUS;
        }

        //This function breaks the whole data stream to multiple frame/chunks
        public bool DataDefragment_Incomplete(byte[] l_val, ref List<byte[]> _lChunks, ref UInt16 Command_Type)
        {

            bool _STATUS = false;
            
            UInt16 _temp = 0;

            SPpdu _sppdu = new SPpdu();
            ROapdus _ROapdus = new ROapdus();
            Generic_apdu _generic_apdu = new Generic_apdu();

            // Relax ! Any one of below four Structure will be activated at a time
            ROIV_apdu _ROIV_apdu = new ROIV_apdu();
            RORS_apdu _RORS_apdu = new RORS_apdu();
            ROER_apdu _ROER_apdu = new ROER_apdu();
            ROLRS_apdu _ROLRS_apdu = new ROLRS_apdu();
            AttributeList _AttributeList= new AttributeList();

            CMDTYPE l_command_type=0;
            OIDType l_Obj_class=0;

            

            _sppdu = (SPpdu)ByteArrayToComplexStructureX(ref l_val, _sppdu);
            _ROapdus = (ROapdus)ByteArrayToComplexStructureX(ref l_val, _ROapdus);
            EventReportArg _EventReportArg = new EventReportArg();
            
            switch (_ROapdus.ro_type)
            {
                case 1: _ROIV_apdu = (ROIV_apdu)ByteArrayToComplexStructureX(ref l_val, _ROIV_apdu);
                    switch (_ROIV_apdu.command_type)
                    {
                        case CMD_EVENT_REPORT: //==0
                        case CMD_CONFIRMED_EVENT_REPORT:    //==1
                            _EventReportArg = (EventReportArg)ByteArrayToComplexStructureX(ref l_val, _EventReportArg);

                            //Lets try to parse the AttributeList
                            ATTLISTHeader _ATTLISTHeader = new ATTLISTHeader();
                            ATTLISTHeader _ATTLISTHeader1 = new ATTLISTHeader();
                            ATTLISTHeader _ATTLISTHeader2 = new ATTLISTHeader();
                            ATTLISTHeader _ATTLISTHeader3 = new ATTLISTHeader();
                            _ATTLISTHeader = (ATTLISTHeader)ByteArrayToComplexStructureX(ref l_val, _ATTLISTHeader);

                            UInt16 _handle=0;// = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);
                            UInt16 _lcount=0;// = (UInt16)((UInt16)(l_val[2] << 8) + l_val[3]);
                            UInt16 _length =0;// (UInt16)((UInt16)(l_val[4] << 8) + l_val[5]); 
                            ArrayList _bytelist = new ArrayList(_ATTLISTHeader.count);
                            byte[] _bytearray = new byte[_ATTLISTHeader.length];
                            byte[] _bytearray1 = new byte[_ATTLISTHeader.length];
                            byte[] _bytearray2 = new byte[_ATTLISTHeader.length];
                            byte[] _bytearray3 = new byte[_ATTLISTHeader.length];

                            for (int _li = 0; _li < l_val.Length; _li++)
                            {
                                //_ATTLISTHeader = (ATTLISTHeader)ByteArrayToNormalStructureXSpecial(ref l_val, _ATTLISTHeader);
                                _ATTLISTHeader = (ATTLISTHeader)ByteArrayToComplexStructureX(ref l_val, _ATTLISTHeader);
                                

                                _bytearray = l_val.Take(_ATTLISTHeader.length).ToArray();
                                l_val = l_val.Skip(_ATTLISTHeader.length).ToArray();

                                if (_ATTLISTHeader.count > 1 || !IsEndPacket(_bytearray))
                                for (int _lli = 0; _lli < _bytearray.Length; _lli++)
                                {
                                    _ATTLISTHeader1 = (ATTLISTHeader)ByteArrayToComplexStructureX(ref _bytearray, _ATTLISTHeader1);
                                    _bytearray1 = _bytearray.Take(_ATTLISTHeader1.length).ToArray();
                                    _bytearray = _bytearray.Skip(_ATTLISTHeader1.length).ToArray();


                                    if (_ATTLISTHeader1.count > 1 || !IsEndPacket(_bytearray1))
                                        for (int _lli1 = 0; _lli1 < _bytearray1.Length; _lli1++)
                                        {



                                            _ATTLISTHeader2 = (ATTLISTHeader)ByteArrayToComplexStructureX(ref _bytearray1, _ATTLISTHeader2);
                                            _bytearray2 = _bytearray1.Take(_ATTLISTHeader2.length).ToArray();
                                            _bytearray1 = _bytearray1.Skip(_ATTLISTHeader2.length).ToArray();


                                            if (_ATTLISTHeader2.count > 1 || !IsEndPacket(_bytearray2))
                                                for (int _lli2 = 0; _lli2 < _bytearray2.Length; _lli2++)
                                                {
                                                    _ATTLISTHeader3 = (ATTLISTHeader)ByteArrayToComplexStructureX(ref _bytearray2, _ATTLISTHeader3);
                                                    _bytearray3 = _bytearray2.Take(_ATTLISTHeader3.length).ToArray();
                                                    _bytearray2 = _bytearray2.Skip(_ATTLISTHeader3.length).ToArray();

                                                    if (IsEndPacket(_bytearray3) && _bytearray3.Length>5)
                                                        _bytelist.Add(_bytearray3);

                                                }
                                            else if (_bytearray2.Length > 5)
                                                _bytelist.Add(_bytearray2);
                                            else if (_bytearray1.Length > 5 && _ATTLISTHeader2.length == 0)
                                            {
                                                _bytelist.Add(_bytearray1);
                                                _bytearray1 = _bytearray1.Skip(_bytearray1.Length).ToArray();
                                            }
        
                                        }
                                    else if (_bytearray1.Length > 5)
                                        _bytelist.Add(_bytearray1);
                                    else if (_bytearray.Length > 5 && _ATTLISTHeader1.length == 0)
                                    {
                                        _bytelist.Add(_bytearray);
                                        _bytearray = _bytearray1.Skip(_bytearray.Length).ToArray();
                                    }

                                }
                                else if (_bytearray.Length > 5)// || _ATTLISTHeader.length == 0)
                                    _bytelist.Add(_bytearray);



                            }


                            break;
                        case CMD_GET:                       //==3
                        case CMD_SET:                       //==4
                        case CMD_CONFIRMED_SET:             //==5
                        case CMD_CONFIRMED_ACTION:          // == 7
                        case CMD_UNKNOWN:                   // == 8

                            
                            break;
                        
                        

                    }
                    
                    break;
                case 2: _RORS_apdu = (RORS_apdu)ByteArrayToComplexStructureX(ref l_val, _RORS_apdu); 
                    
                    break;
                case 3: _ROER_apdu = (ROER_apdu)ByteArrayToComplexStructureX(ref l_val, _ROER_apdu);
                    
                    break;
                case 5: _ROLRS_apdu = (ROLRS_apdu)ByteArrayToComplexStructureX(ref l_val, _ROLRS_apdu); 
                    
                    break;

                default: _generic_apdu = (Generic_apdu)ByteArrayToComplexStructureX(ref l_val, _generic_apdu); break;
            }



            _sppdu.session_id = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);
            _sppdu.p_context_id = (UInt16)((UInt16)(l_val[2] << 8) + l_val[3]);
            if (_sppdu.session_id != 0xE100)  //The session ID == E100. It must match, otherwise get lost.
            {
                _STATUS = false;
                return _STATUS;
            }
            else
            {
              l_val = l_val.Skip(4).ToArray();  //Remove SPpdu part
                _ROapdus.ro_type = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);
                _ROapdus.length = (UInt16)((UInt16)(l_val[2] << 8) + l_val[3]);

                if (_ROapdus.length == (l_val.Length - 4))
                {
                    if ((_ROapdus.ro_type == ROIV_APDU) || (_ROapdus.ro_type == RORS_APDU) || (_ROapdus.ro_type == ROER_APDU) || (_ROapdus.ro_type == ROLRS_APDU))
                    {
                        l_val = l_val.Skip(4).ToArray();  //Remove SPpdu part

                        //# define ROIV_APDU 1
                        //# define RORS_APDU 2
                        //# define ROER_APDU 3
                        //# define ROLRS_APDU 5
                        if (_ROapdus.ro_type == ROIV_APDU)
                        {
                            _ROIV_apdu.invoke_id = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);
                            _ROIV_apdu.command_type = (UInt16)((UInt16)(l_val[2] << 8) + l_val[3]);
                            _ROIV_apdu.length = (UInt16)((UInt16)(l_val[4] << 8) + l_val[5]);

                            l_command_type = _ROIV_apdu.command_type;
                            //There will be 6 cases f0r command

                        }
                        if (_ROapdus.ro_type == RORS_APDU)
                        {
                            _RORS_apdu.invoke_id = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);
                            _RORS_apdu.command_type = (UInt16)((UInt16)(l_val[2] << 8) + l_val[3]);
                            _RORS_apdu.length = (UInt16)((UInt16)(l_val[4] << 8) + l_val[5]);

                            l_command_type = _RORS_apdu.command_type;

                        }
                        if (_ROapdus.ro_type == ROER_APDU)
                        {
                            _ROER_apdu.invoke_id = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);
                            _ROER_apdu.error_value = (UInt16)((UInt16)(l_val[2] << 8) + l_val[3]);
                            _ROER_apdu.length = (UInt16)((UInt16)(l_val[4] << 8) + l_val[5]);
                        }
                        if (_ROapdus.ro_type == ROLRS_APDU)
                        {
                            _ROLRS_apdu.RorlsId_StateCount = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);
                            _ROLRS_apdu.command_type = (UInt16)((UInt16)(l_val[2] << 8) + l_val[3]);
                            _ROLRS_apdu.length = (UInt16)((UInt16)(l_val[4] << 8) + l_val[5]);

                            l_command_type = _ROLRS_apdu.command_type;

                        }
                        l_val = l_val.Skip(6).ToArray();  //Remove APDU part
                        _STATUS = true;
                    }
                    else
                        _STATUS = false;
                }
                else
                    _STATUS = false;

            }


     

            

            int Element_count = 0;// elements in the stream;
            UInt16 Ro_Type = (UInt16)((UInt16)(l_val[4] << 8) + l_val[5]); // gets the Ro Type
            Command_Type = (UInt16)((UInt16)(l_val[10] << 8) + l_val[11]); ; // gets the command Type


            //if (Ro_Type == 3 || Ro_Type == 5)
             //   System.Windows.Forms.MessageBox.Show("Undecoded Ro Type");

            if (Ro_Type == 1 || Ro_Type == 2)
            {

                if (Command_Type == CMD_EVENT_REPORT || Command_Type == CMD_CONFIRMED_SET || Command_Type == CMD_UNKNOWN)   //==0
                {

                    l_val = l_val.Skip(14).ToArray();  //14 bytes Left. 

                    UInt16 m_obj_id = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);
                    UInt32 _RelTime = (UInt32)((UInt32)(l_val[6] << 24) + (UInt32)(l_val[7] << 16) + (UInt32)(l_val[8] << 8) + l_val[9]); 
                    
                    Element_count = (UInt16)((UInt16)(l_val[6] << 8) + l_val[7]);

                    while (l_val.Length > 0)  // though, it should be terminated after 10-30 bytes
                    {
                        _temp = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);
                        if (!Attribute_ID.ContainsKey(_temp))
                        {
                            //  _headerStuff = _headerStuff + l_val.Take(2).ToArray();
                            l_val = l_val.Skip(2).ToArray();
                        }
                        else
                            break;
                    }



                    //Now, skip initial 4B, then next immidiate 2 Bytes indicated how many further 2xbytes to be skipped !
                    //l_val = l_val.Skip(4).ToArray();
                    //_temp = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);
                    //_temp = (UInt16)(_temp * 2 + 2);
                    //l_val = l_val.Skip(_temp).ToArray();



                    //Now look at the 2,3rd bytes to get next data length, trim the frame and then proceed  

                    int _offset = 0;
                    UInt16 _key = 0;
                    string _lstr = "";
                    byte[] _frameChunk = new byte[1024];
                    for (int _lctr = 0; l_val.Length > 5; _lctr++)//= _lctr + _offset)
                    {
                        //Lets get the Attribute ID
                        _key = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);


                        if (Attribute_ID.ContainsKey(_key))  // If Valid ATTRIBUTE ID Avail
                        {
                            _STATUS = true;
                            //Calculate the lrngth of this chunk
                            _temp = (UInt16)((UInt16)(l_val[2] << 8) + l_val[3]);
                            _offset = 2 + 2 + _temp;
                            _frameChunk = l_val.Take(_offset).ToArray();
                            _lChunks.Add(_frameChunk);
                            _STATUS = true;

                            // if (_key == 0x0950 || _key==0x094B)   //special condition; I am unable to decode
                            //     _offset = _offset + 6;

                            l_val = l_val.Skip(_offset).ToArray();
                            //_lstring.Add( l_val);
                        }
                        else
                        {
                            if (l_val.Length > 6)
                            {
                                //_offset = _offset + 6;
                                //l_val = l_val.Skip(6).ToArray();
                                l_val = l_val.Skip(2).ToArray();
                            }
                            else
                            {

                                _STATUS = false;
                                break;
                            }
                        }

                    }

                }
                else if (Command_Type == 100) //==5
                {


                    l_val = l_val.Skip(14).ToArray();  //14 bytes Left.
                    _temp = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);
                    if (Obj_Class.ContainsKey(_temp))
                    {
                        l_val = l_val.Skip(2).ToArray(); // just skip Object_class
                        l_val = l_val.Skip(4).ToArray(); // skip scope
                        UInt16 _count = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]); //count
                        l_val = l_val.Skip(2).ToArray(); // skip count
                        UInt16 _length = (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);
                        l_val = l_val.Skip(2).ToArray(); // skip length

                       // if (_count > 1 || (l_val.Length != _length))
                       //     System.Windows.Forms.MessageBox.Show("need parsing for Command_Type 5");

                        _lChunks.Add(l_val);


                    }

                    _STATUS = true;




                }
                else //if (Command_Type == CMD_CONFIRMED_SET) //==5
                {
                    //System.Windows.Forms.MessageBox.Show("Un-coded CommandType");
                    _STATUS = false;
                }

            }


           // if (Element_count != _lChunks.Count)
           //     System.Windows.Forms.MessageBox.Show("Element_Count:Some data not persed properly");

            return _STATUS;
        }












    }


    



}

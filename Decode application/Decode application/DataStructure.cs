using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;


using u_16 = System.UInt16;
using MeasurementState = System.UInt16;
using OIDType = System.UInt16;
using FLOATType = System.UInt32;
using RelativeTime = System.UInt32;
using AbsoluteTime = System.UInt64;
using PrivateOID = System.UInt16;
using _HANDLE = System.UInt16;
using CMDTYPE = System.UInt16;
using TextId = System.UInt32;
using MdsContext = System.UInt16;



namespace Decode_application
{
    [Serializable()]
    class DataStructure:LOOKUP
    {
        
        const Int32 state_INVALID=0x8000;
        const Int32 state_QUESTIONABLE=0x4000;
        const Int32 state_UNAVAILABLE=0x2000;
        const Int32 state_CALIBRATION_ONGOING=0x1000;
        const Int32 state_TEST_DATA=0x0800;
        const Int32 state_DEMO_DATA=0x0400;
        const Int32 state_VALIDATED_DATA=0x0080;
        const Int32 state_EARLY_INDICATION=0x0040;
        const Int32 state_MSMT_ONGOING=0x0020;
        const Int32 state_MSMT_STATE_IN_ALARM=0x0002;
        const Int32 state_MSMT_STATE_AL_INHIBITED=0x0001;


        protected Dictionary<Int64, string> MDSStatus = new Dictionary<Int64, string>()
        {
       
            { 0x0000	 , "DISSCONNECTED" },
            { 0x0001	 , "UNASSOCIATED" },
            { 0x0004	 , "CONNECTED (Undocumented-Version F.0)" },
           // { 0x0005	 , "UNKNOWN" },
            { 0x0006	 , "OPERATING" },
        };



        public struct SPpdu
        {
            public UInt16 session_id;
            public UInt16 p_context_id;
        }  ;

        public struct ROapdus
        {
            public UInt16 ro_type;
            public UInt16 length;
        }  ;
        //****************************************************************************//

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct ManagedObjectId    // 6 Bytes
        {
            public u_16 m_obj_class;
            public u_16 context_id;
            public u_16 handle;
        };

        public struct TTYPE    // 4 Bytes
        {
            public u_16 partition;
            public OIDType code;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct GeneralATTHeader    // 4 Bytes
        {
            public u_16 ATT_id;
                 u_16 _TLength;
           // public u_16 count;
           // public u_16 _Length;
        };

        GeneralATTHeader _GeneralATTHeader = new GeneralATTHeader();

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct ATTLISTHeader    // 4 Bytes
        {
            public u_16 Handle;
            public u_16 count;
            public u_16 length;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct AVAType    // 4 Bytes
        {
            public OIDType attribute_id;
            public u_16 length;
            public OIDType attribute_val;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct AttributeList    // 4 Bytes
        {
            public u_16 count;
            public u_16 length;
            // public AVAType[] _AVAType; 
            public OIDType[] OIDType;
        };

        public struct AttributeModEntry    // 4 Bytes
        {
            public u_16 modifyOperator;
            public AVAType attribute;
        };

        public struct ModificationList    // 4 Bytes
        {
            public u_16 count;
            public u_16 length;
            public AttributeModEntry value;
        };

        public struct TextIdList   // 4 Bytes
        {
            public u_16 count;
            public u_16 length;
            public TextId[] value;
        };

        public struct PollInfoList   // 4 Bytes
        {
            public u_16 count;
            public u_16 length;
            public SingleContextPoll[] value;
        };

        public struct SingleContextPoll   // 4 Bytes
        {
            public MdsContext context_id;
            public u_16 count;
            public u_16 length;
            public ObservationPoll[] value;
        };

        public struct ObservationPoll   // 4 Bytes
        {

            public _HANDLE obj_handle;
            public AttributeList attributes;
        };





        //----------------------------------------------------------------------------------//

        //# define ROIV_APDU 1
        //# define RORS_APDU 2
        //# define ROER_APDU 3
        //# define ROLRS_APDU 5

        public struct Generic_apdu
        {
            public u_16 Param1;
            public u_16 Param2;
            public u_16 Param3;
        }  ;

        public struct ROIV_apdu
        {
            public u_16 invoke_id;
            public CMDTYPE command_type;
            public u_16 length;
        }  ;

        public struct RORS_apdu
        {
            public u_16 invoke_id;
            public CMDTYPE command_type;
            public u_16 length;
        }  ;


        public struct ROER_apdu
        {
            public u_16 invoke_id;
            public u_16 error_value;
            public u_16 length;
        }  ;

        public struct ROLRS_apdu
        {
            public u_16 RorlsId_StateCount;
            public CMDTYPE command_type;
            public u_16 length;
        }  ;

        //--------------------------------------------------------------------------------
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct EventReportArg                      //14B
        {
            public ManagedObjectId _ManagedObjectId;      // 6B
            public RelativeTime _RelativeTime;            //4B
            public OIDType event_type;                    //2B
            public u_16 length;                           //2B

            // public AttributeList[] _AttributeList;
        }  ;

        public struct MDSCreateInfo                      //6B+
        {
            public ManagedObjectId _ManagedObjectId;      // 6B
            public AttributeList[] _AttributeList;      // 6B
        }  ;

        public struct EventReportRes                      //14B
        {
            public ManagedObjectId _ManagedObjectId;      // 6B
            public RelativeTime _RelativeTime;                          //4B
            public OIDType event_type;                    //2B
            public u_16 length;                           //2B

        }  ;


        public struct ActionArgument                      //14B
        {
            public ManagedObjectId _ManagedObjectId;      // 6B
            public UInt32 scope;                          //4B
            public OIDType event_type;                    //2B
            public u_16 length;                           //2B

        }  ;

        public struct PollMdibDataReqExt                                //8B
        {
            public u_16 poll_number;                                  // 2B
            public TTYPE _TYPE;                                        //4B
            public OIDType polled_attr_grp;                           //2B

        }  ;

        public struct PollMdibDataReq                                 //8B
        {
            public u_16 poll_number;                                  // 2B
            public TTYPE _TYPE;                                        //4B
            public OIDType polled_attr_grp;                           //2B

        }  ;


        public struct ActionResult                      //14B
        {
            public ManagedObjectId _ManagedObjectId;      // 6B
            public OIDType action_type;                    //2B
            public u_16 length;                           //2B

        }  ;

        public struct PollMdibDataReply                                 //8B
        {
            public u_16 poll_number;                                  // 2B
            public RelativeTime rel_time_stamp;                       //4B
            public AbsoluteTime abs_time_stamp;                       //2B
            public TTYPE polled_obj_type;                                        //2B
            public OIDType polled_attr_grp;                           //2B
            public PollInfoList poll_info_list;
        }  ;



        public struct PollMdibDataReplyExt                                 //8B
        {
            public u_16 poll_number;                                  // 2B
            public u_16 sequence_number;                                  // 2B
            public RelativeTime rel_time_stamp;                       //4B
            public AbsoluteTime abs_time_stamp;                       //2B
            public TTYPE _TYPE;                                        //2B
            public OIDType polled_attr_grp;                           //2B
            public PollInfoList[] _PollInfoList;                           //2B
        }  ;

        public struct SetArgument                      //14B
        {
            public ManagedObjectId _ManagedObjectId;      // 6B
            public UInt32 scope;                          //4B
            public ModificationList _ModificationList;                    //2B
            public AttributeModEntry modifyOperator;                           //2B
            public AVAType _AVAType;                           //2B

            public TextIdList[] TextIdList;
        }  ;

        public struct SetResult                      //14B
        {
            public ManagedObjectId _ManagedObjectId;      // 6B
            public AttributeList _AttributeList;                          //4B
            public AVAType _AAVAType;                           //2B
            public TextIdList[] TextIdList;
        }  ;

        public struct GetArgument                      //16B
        {
            public ManagedObjectId _ManagedObjectId;      // 6B
            public UInt32 scope;                         //4B
            public AttributeList Attributelist;          //6B

        }  ;





       [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct SaFixedValSpec16
        {
        public u_16 count;
        public u_16 length;
        public SaFixedValSpecEntry16[] _SaFixedValSpecEntry16;
        } ;

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct SaFixedValSpecEntry16
       {
        public u_16 sa_fixed_val_id;
        public u_16 sa_fixed_val;
        } ;

        SaFixedValSpec16 _SaFixedValSpec16 = new SaFixedValSpec16();


        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct SampleType
        {
        public byte sample_size;
        public byte significant_bits;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct SaSpec
        {
        public u_16 array_size;
        public SampleType sample_type;
        public u_16 flags;
        } ;

        
        public SaSpec SaSpec1= new SaSpec();

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
       public struct MetricStructure {
            public byte ms_struct;
            public byte ms_comp_no;
            }  ;

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct MetricSpec
            {
            public RelativeTime update_period;     //4 B
            public u_16 category;
            public u_16 access;
            public MetricStructure structure;
            public u_16 Metric_relevance;
            };

        public MetricSpec MetricSpec1 = new MetricSpec();

        
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
         public struct VariableLabel
            {
           public u_16 length;
            public byte[] value;
            };

       [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
       public unsafe struct SystemModel
        {
        public VariableLabel manufacturer;
        public VariableLabel model_number;
        } ;

        ////public struct SystemModel
        ////{
        ////    public int iii;
        ////    public static struct manufacturer
        ////    {
        ////        public u_16 length { get; set; }
        ////        public byte[] value;
        ////    };
        ////    public struct model_number
        ////    {
        ////        public u_16 length;
        ////        public byte[] value;
        ////    };
        ////} ;


         [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
         public struct SaCalData16
            {
            public FLOATType lower_absolute_value;
            public FLOATType upper_absolute_value;
            public u_16 lower_scaled_value;
            public u_16 upper_scaled_value;
            public u_16 increment;
            public u_16 cal_type;
            };
         SaCalData16 _SaCalData16 = new SaCalData16();

     //  [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto, Size = 10)]
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct NuObsValue
            {
          //   [FieldOffset(0)]
          public OIDType physio_id;
          public MeasurementState state;
          public OIDType unit_code;
          public FLOATType value;

        };

       [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct SaVisualGrid16
        {
                    public u_16 count;
                    public UInt16 length;
                    public SaGridEntry16[] value ;//= new NuObsValue[xsize];
                    
        } ;

public SaVisualGrid16 _SaVisualGrid16 = new SaVisualGrid16();

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct SaGridEntry16
        {
            public FLOATType absolute_value;
            public u_16 scaled_value;
            public u_16 level;

        } ;
        

 [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
      public  unsafe struct NuObsValueCmp
        {
                    public u_16 count;
                    public u_16 length;
                    public NuObsValue[] value ;//= new NuObsValue[xsize];
                    
        } ;





      [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
      public struct SaObsValue
      {
          //   [FieldOffset(0)]
          public OIDType physio_id;
          public MeasurementState state;
          public u_16 Length;
          public byte[] _value;

      };

     [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
      public unsafe struct SaObsValueCmp
      {
          public UInt16 count;
          public u_16 length;
          public SaObsValue[] _SaObsValue;//= new NuObsValue[xsize];

      } ;


         [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
      public unsafe struct SystemSpec
         {
            public u_16 count;
            public u_16 length;
            public SystemSpecEntry[] _SystemSpecEntry;
         };

         [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
         public unsafe struct SystemSpecEntry
         {
            public PrivateOID component_capab_id;
            public u_16 length;
            public u_16[] value;
         };
        


        
         [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
         public unsafe struct MdsGenSystemInfo
         {
            
            public u_16 count;
            public u_16 length;
             public MdsGenSystemInfoEntry[] _MdsGenSystemInfoEntry;
         };

         [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
         public unsafe struct MdsGenSystemInfoEntry
         {

             public u_16 choice;
             public u_16 length;
             public byte[] value;
         };


         [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
         public unsafe struct EnumObsVal 
         {

             public OIDType physio_id;
             public MeasurementState state;
             public EnumVal value;
         };

         [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
         public unsafe struct EnumVal 
         {
            // [FieldOffset(0)]
             public u_16 choice;
             //[FieldOffset(2)]
              public u_16 length;
              
             //[FieldOffset(4)]
             //public OIDType enum_obj_id;
             // [FieldOffset(4)]
             //public EnumObjIdVal enum_obj_id_val;
         };


    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
         public unsafe struct EnumObjIdVal 
         {
              public OIDType obj_id;
              public FLOATType num_val;
                public OIDType unit_code;
    };




     

//***********************************************************************************//
      public static void SwapIt(Type type, byte[] recvbyte, int offset)
      {
         // int l_ctr = 0;
          foreach (System.Reflection.FieldInfo fi in type.GetFields())
          {
              
           //   if(l_ctr<=length)
              {
                  int index = 0;
                  try
                  {
                      index = Marshal.OffsetOf(type, fi.Name).ToInt32() + offset;
                  }
                  catch (Exception ex)
                  {
                      index = index + offset;
                  }
             // l_ctr = index;
             if (fi.FieldType == typeof(byte))
             {
                 Array.Reverse(recvbyte, index, sizeof(byte));
              }
              else if (fi.FieldType == typeof(int))
              {
                  
                  Array.Reverse(recvbyte, index, sizeof(int));
              }
              else if (fi.FieldType == typeof(RelativeTime))
              {
                  Array.Reverse(recvbyte, index, sizeof(RelativeTime));
              }
              else if (fi.FieldType == typeof(u_16))
              {
                  Array.Reverse(recvbyte, index, sizeof(u_16));
              }
              else if (fi.FieldType == typeof(FLOATType))
              {
                  Array.Reverse(recvbyte, index, sizeof(FLOATType));
              }
              else if (fi.FieldType == typeof(float))
              {
                  Array.Reverse(recvbyte, index, sizeof(float));
              }
              else if (fi.FieldType == typeof(double))
              {
                  Array.Reverse(recvbyte, index, sizeof(double));
              }
              else
              {
                  // Maybe we have an array
                  if (fi.FieldType.IsArray)
                  {
                      // Check for MarshalAs attribute to get array size
                      object[] ca = fi.GetCustomAttributes(false);
                      if (ca.Count() > 0 && ca[0] is MarshalAsAttribute)
                      {
                          int size = ((MarshalAsAttribute)ca[0]).SizeConst;
                          // Need to use GetElementType to see that int[] is made of ints
                          if (fi.FieldType.GetElementType() == typeof(int))
                          {
                              for (int i = 0; i < size; i++)
                              {
                                  Array.Reverse(recvbyte, index + (i * sizeof(int)), sizeof(int));
                              }
                          }
                          else if (fi.FieldType.GetElementType() == typeof(float))
                          {
                              for (int i = 0; i < size; i++)
                              {
                                  Array.Reverse(recvbyte, index + (i * sizeof(float)), sizeof(float));
                              }
                          }
                          else if (fi.FieldType.GetElementType() == typeof(double))
                          {
                              for (int i = 0; i < size; i++)
                              {
                                  Array.Reverse(recvbyte, index + (i * sizeof(double)), sizeof(double));
                              }
                          }
                          else
                          {
                              // An array of something else?
                              Type t = fi.FieldType.GetElementType();
                              int s = Marshal.SizeOf(t);
                              for (int i = 0; i < size; i++)
                              {
                                  SwapIt(t, recvbyte, index + (i * s));
                              }
                          }
                      }
                  }
                  else
                  {
                      SwapIt(fi.FieldType, recvbyte, index);
                  }
              }
          }

              if (type.MemberType == MemberTypes.TypeInfo) // single loop
                  break;

      }
      }




      



      void StructureToByteArray(byte[] arr, ref Object obj, int size)
      {


          // int size = Marshal.SizeOf(obj);
          IntPtr ptr = Marshal.AllocHGlobal(size);

          Marshal.Copy(arr, 0, ptr, size);

          obj = (Object)Marshal.PtrToStructure(ptr, obj.GetType());
          Marshal.FreeHGlobal(ptr);

          // return str;
      }



      //########################################################################################


        //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&






     double valueParse(UInt64 val)
     {

         double valint = val << 8;
         valint = (UInt32)valint >> 8;
         byte exponent = (byte)(val >> 24);

         if (exponent > 128)
         {
             exponent = (byte)(256 - exponent);
             valint = valint / Math.Pow(10, exponent);
         }
         else
         {
             valint = valint * Math.Pow(10, exponent);
         }

         return valint;
     }




     public static string ByteArrayToHEXString(byte[] _ba)    // does it work??
     {
         StringBuilder hex = new StringBuilder(_ba.Length * 2);
         foreach (byte b in _ba)
             hex.AppendFormat("{0:x2}", b);
         return hex.ToString();
     }


        public static byte[] HEXStringToByteArray(string _hexstring)
        {
            
       Regex r = new Regex(@"^[0-9A-F\r\n]+$");
       if (r.Match(_hexstring).Success == false)
           _hexstring = "";



            return Enumerable.Range(0, _hexstring.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(_hexstring.Substring(x, 2), 16))
                             .ToArray();
        }
        //----------------------------------------------------------------------------------------------//





        public List<string> Decode_CMPD_SA_OBSVal(byte[] _value)
        {

            List<string> _lstring = new List<string>();
            string _lstr = "";
            u_16 _ATT_id = 0;
            u_16 _leng = 0;
            
          //  SaSpec1.sample_type = new SampleType();

            SaObsValueCmp _SaObsValueCmp = new SaObsValueCmp();
            _SaObsValueCmp._SaObsValue = new SaObsValue[1];


          ////  SaObsValue SaObsValue1 = new SaObsValue();

          //////  byte[] _Xvalue = _value;

          ////  _GeneralATTHeader = (GeneralATTHeader)ByteArrayToComplexStructureX(ref _Xvalue, _GeneralATTHeader);
          ////  UInt16 _count = (UInt16)((UInt16)(_Xvalue[0] << 8) + _Xvalue[1]);
          ////  _Xvalue = _Xvalue.Skip(4).ToArray();
          ////  for(UInt16 _li=0; _li<_count; _li++)
          ////  {
          ////      UInt16 _Xlength = (UInt16)((UInt16)(_Xvalue[4] << 8) + _Xvalue[5]);
          ////      SaObsValue1._value = new byte[_Xlength];
          ////      SaObsValue1 = (SaObsValue)ByteArrayToComplexStructureX(ref _Xvalue, SaObsValue1);


          ////      if (Phy_ID.ContainsKey(SaObsValue1.physio_id))
          ////      {
          ////          _lstr = Phy_ID[SaObsValue1.physio_id];
          ////      }
          ////      else
          ////          _lstr = SaObsValue1.physio_id + "[H-Unknown]";

          ////      _lstr = _lstr + " : " + ByteArrayToHEXString(SaObsValue1._value);
          ////      _lstring.Add(_lstr);

          ////  }

            

            _ATT_id = (UInt16)((UInt16)(_value[0] << 8) + _value[1]);

            _leng = (UInt16)((UInt16)(_value[2] << 8) + _value[3]);   //May not useful in future

            _SaObsValueCmp.count = (UInt16)((UInt16)(_value[4] << 8) + _value[5]);

            _SaObsValueCmp.length = (UInt16)((UInt16)(_value[6] << 8) + _value[7]);   //dont forget the 2Bytes of itself

            _SaObsValueCmp._SaObsValue = new SaObsValue[_SaObsValueCmp.count];


            _value = _value.Skip(8).ToArray(); // Leave all previous 8 B

            if (Attribute_ID.ContainsKey(_ATT_id))
                _lstr = Attribute_ID[_ATT_id];



            for (int _li = 0; _li < _SaObsValueCmp.count; _li++)
            {
                _SaObsValueCmp._SaObsValue[_li].physio_id = (UInt16)((UInt16)(_value[0] << 8) + _value[1]);
                _SaObsValueCmp._SaObsValue[_li].state = (UInt16)((UInt16)(_value[2] << 8) + _value[3]);
                _SaObsValueCmp._SaObsValue[_li].Length = (UInt16)((UInt16)(_value[4] << 8) + _value[5]);
                _SaObsValueCmp._SaObsValue[_li]._value = new byte[_SaObsValueCmp._SaObsValue[_li].Length];
                //                Array.Copy(
                _value = _value.Skip(6).ToArray();
                _SaObsValueCmp._SaObsValue[_li]._value = _value.Take(_SaObsValueCmp._SaObsValue[_li].Length).ToArray();
                _value = _value.Skip(_SaObsValueCmp._SaObsValue[_li].Length).ToArray();

                if (Phy_ID.ContainsKey(_SaObsValueCmp._SaObsValue[_li].physio_id))
                {
                    _lstr = Phy_ID[_SaObsValueCmp._SaObsValue[_li].physio_id];
                }
                else if (Phy_ID_Wave.ContainsKey(_SaObsValueCmp._SaObsValue[_li].physio_id))
                {
                    _lstr = Phy_ID_Wave[_SaObsValueCmp._SaObsValue[_li].physio_id];
                }
                else
                    _lstr = _SaObsValueCmp._SaObsValue[_li].physio_id + "[H-Unknown]";

                _lstr = _lstr + " : " + ByteArrayToHEXString(_SaObsValueCmp._SaObsValue[_li]._value);
                _lstring.Add(_lstr);


            }

            

            return _lstring;

        }



        public List<string> EnumObsVal_Value(byte[] _value)
        {

            List<string> _lstring = new List<string>();
            string _lstr = "";
            u_16 _ATT_id = 0;
            u_16 _leng = 0;

            EnumObsVal _EnumObsVal = new EnumObsVal();
            EnumObjIdVal _EnumObjIdVal = new EnumObjIdVal();
            //  byte[] _Xvalue = _value;

            _GeneralATTHeader = (GeneralATTHeader)ByteArrayToComplexStructureX(ref _value, _GeneralATTHeader);
            UInt16 _count = (UInt16)((UInt16)(_value[0] << 8) + _value[1]);
            if (Attribute_ID.ContainsKey(_GeneralATTHeader.ATT_id))
                _lstr = Attribute_ID[_GeneralATTHeader.ATT_id];
            _EnumObsVal.value = new EnumVal();
           
            _EnumObsVal = (EnumObsVal)ByteArrayToComplexStructureX(ref _value, _EnumObsVal);

            if (Phy_ID.ContainsKey(_EnumObsVal.physio_id))
                _lstr = _lstr + ":" + Phy_ID[_EnumObsVal.physio_id];
            else if (Phy_ID_Wave.ContainsKey(_EnumObsVal.physio_id))
                _lstr = _lstr + ":" + Phy_ID_Wave[_EnumObsVal.physio_id];
            else
                _lstr = _lstr + ":" + _EnumObsVal.physio_id.ToString("X") + " [H-Unknown]; ";



            if (_EnumObsVal.value.length == 8)
            {
                _EnumObjIdVal = (EnumObjIdVal)ByteArrayToComplexStructureX(ref _value, _EnumObjIdVal);
                _lstr = _lstr + "Obj_id-> " + _EnumObjIdVal.obj_id.ToString("X") + ", numVal-> " + _EnumObjIdVal.num_val.ToString("X") + ", Unit Code->" + _EnumObjIdVal.unit_code.ToString("X");
            }
            else if (_EnumObsVal.value.length == 2)
            {
                OIDType enum_obj_id = (UInt16)((UInt16)(_value[0] << 8) + _value[1]);
                _lstr = _lstr + " EnumObj_id-> " + enum_obj_id.ToString("X");
            }

            _lstring.Add(_lstr);

            return _lstring;

        }





        public List<string> MdsGenSystemInfo_code(byte[] _value)
        {

            List<string> _lstring = new List<string>();
            string _lstr = "";
            u_16 _ATT_id = 0;
            u_16 _leng = 0;

            MdsGenSystemInfoEntry _MdsGenSystemInfoEntry = new MdsGenSystemInfoEntry();
            //  byte[] _Xvalue = _value;

            _GeneralATTHeader = (GeneralATTHeader)ByteArrayToComplexStructureX(ref _value, _GeneralATTHeader);
            UInt16 _count = (UInt16)((UInt16)(_value[0] << 8) + _value[1]);
            if (Attribute_ID.ContainsKey(_GeneralATTHeader.ATT_id))
                _lstr = Attribute_ID[_GeneralATTHeader.ATT_id];


            //UInt16 _length = (UInt16)((UInt16)(_value[2] << 8) + _value[3]);
            _value = _value.Skip(4).ToArray();
            for (UInt16 _li = 0; _li < _count; _li++)
            {
                UInt16 _Xlength = (UInt16)((UInt16)(_value[2] << 8) + _value[3]);
                _MdsGenSystemInfoEntry.value = new byte [_Xlength];
                _MdsGenSystemInfoEntry = (MdsGenSystemInfoEntry)ByteArrayToComplexStructureX(ref _value, _MdsGenSystemInfoEntry);



                _lstr = _lstr + "choice->" + _MdsGenSystemInfoEntry.choice + " values(" + _Xlength + "B)->";

                _lstr = _lstr + ByteArrayToHEXString(_MdsGenSystemInfoEntry.value);

                //for (UInt16 _lx = 0; _lx < _Xlength; _lx++)
                //{
                //    _lstr = _lstr + _MdsGenSystemInfoEntry.value[_lx].ToString() + ", ";

                //}
                _lstring.Add(_lstr);
            }

            return _lstring;

        }



        public List<string> SystemSpec_Value(byte[] _value)
        {

            List<string> _lstring = new List<string>();
            string _lstr = "";
            u_16 _ATT_id = 0;
            u_16 _leng = 0;

            //  SaSpec1.sample_type = new SampleType();

            

            //1SaObsValueCmp _SaObsValueCmp = new SaObsValueCmp();
            //_SaObsValueCmp._SaObsValue = new SaObsValue[1];


            //SaObsValue SaObsValue1 = new SaObsValue();
            //SystemSpec _SystemSpec = new SystemSpec();
            SystemSpecEntry _SystemSpecEntry = new SystemSpecEntry();
            //  byte[] _Xvalue = _value;

            _GeneralATTHeader = (GeneralATTHeader)ByteArrayToComplexStructureX(ref _value, _GeneralATTHeader);
            UInt16 _count = (UInt16)((UInt16)(_value[0] << 8) + _value[1]);
            if (Attribute_ID.ContainsKey(_GeneralATTHeader.ATT_id))
                _lstr = Attribute_ID[_GeneralATTHeader.ATT_id];

            
            //UInt16 _length = (UInt16)((UInt16)(_value[2] << 8) + _value[3]);
            _value = _value.Skip(4).ToArray();
            for (UInt16 _li = 0; _li < _count; _li++)
            {
                UInt16 _Xlength = (UInt16)((UInt16)(_value[2] << 8) + _value[3]);
                _SystemSpecEntry.value = new u_16[_Xlength/2];
                _SystemSpecEntry = (SystemSpecEntry)ByteArrayToComplexStructureX(ref _value, _SystemSpecEntry);

                

                //if (Phy_ID.ContainsKey(SaObsValue1.physio_id))
                //{
                //    _lstr = Phy_ID[SaObsValue1.physio_id];
                //}
                //else
                //    _lstr = SaObsValue1.physio_id + "[H-Unknown]";
                _lstr = _lstr + "component_capab_id->";
                if (Component_ID.ContainsKey(_SystemSpecEntry.component_capab_id))
                    _lstr = _lstr + Component_ID[_SystemSpecEntry.component_capab_id];
                else
                _lstr=_lstr+_SystemSpecEntry.component_capab_id + " : values("+_Xlength/2 +" nos) ->";
                for (UInt16 _lx = 0; _lx <_Xlength/2; _lx++)
                {
                    _lstr = _lstr + _SystemSpecEntry.value[_lx].ToString()+", ";
                   
                }
                _lstring.Add(_lstr);
            }

            return _lstring;

        }

        //-----------------------------------------------------
        /// <summary>
        /// /////
        /// </summary>
        /// <param name="l_val"></param>
        /// <returns></returns>

        public List<string> Decode_CMPD_NU_OBSVal(byte[] l_val)
        {

            List<string> _lstring=new List<string>();
           NuObsValueCmp l_NuObsValueCmp = new NuObsValueCmp();
           

           //GeneralATTHeader _GeneralATTHeader = new GeneralATTHeader();
           _GeneralATTHeader = (GeneralATTHeader)ByteArrayToComplexStructureX(ref l_val, _GeneralATTHeader);

            
            int count=  (UInt16)((UInt16)(l_val[0] << 8) + l_val[1]);

            l_NuObsValueCmp.value = new NuObsValue[count];

            l_NuObsValueCmp= (NuObsValueCmp)ByteArrayToComplexStructureX(ref l_val, l_NuObsValueCmp);

           
            string lstr="";
            long key=0;

            if (Attribute_ID.ContainsKey(_GeneralATTHeader.ATT_id))
                lstr = Attribute_ID[_GeneralATTHeader.ATT_id];

            
                for (int li = 0; li < l_NuObsValueCmp.count; li++)
                {

                    key = l_NuObsValueCmp.value[li].physio_id;
                    if (Phy_ID.ContainsKey(key))
                    {
                        lstr = Phy_ID[key];
                    }
                    else if (Phy_ID_Wave.ContainsKey(key))
                    {
                        lstr = Phy_ID_Wave[key];
                    }
                    else
                        lstr = key + " H-Unknown";
                    lstr = lstr + " :   "  + Convert.ToString(valueParse(l_NuObsValueCmp.value[0].value));

                    key = l_NuObsValueCmp.value[0].unit_code;
                    if (Unit_ID.ContainsKey(key))
                    {
                        lstr = lstr + "   " + Unit_ID[key];
                    }

                    //   System.Windows.Forms.MessageBox.Show(lstr);
                    //lstr = lstr + "\n";

                    _lstring.Add(lstr);
                }
            

            return _lstring;
        }

//***************************************************************************************************/
        /// <summary>
        /// case 0x0950, NOM_ATTR_NU_VAL_OBS
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>

        public List<string> Numeric_Observed_Value(byte[] _value)
        {
            List<string> _lstring = new List<string>();
            string _lstr = "";
            _GeneralATTHeader = (GeneralATTHeader)ByteArrayToComplexStructureX(ref _value, _GeneralATTHeader);

            NuObsValue _NumericObsVal = new NuObsValue();
            _NumericObsVal = (NuObsValue)ByteArrayToComplexStructureX(ref _value, _NumericObsVal); 
            

            UInt16 key = _NumericObsVal.physio_id;
            if (Phy_ID.ContainsKey(key))
            {
                _lstr = Phy_ID[key];
            }
            else if (Phy_ID_Wave.ContainsKey(key))
            {
                _lstr = Phy_ID_Wave[key];
            }
            else
                _lstr = key.ToString("X")+"[Unknown Phy_ID]";

            _lstr = _lstr + " :  " + Convert.ToString(valueParse(_NumericObsVal.value));

            key = _NumericObsVal.unit_code;
            if (Unit_ID.ContainsKey(key))
            {
                _lstr = _lstr + " : " + Unit_ID[key];
            }
            else
                _lstr = _lstr +key + " [Unknown] Unit";


            _lstring.Add(_lstr);


            return _lstring;
        }




        public string ByteArrayToString(byte[] input)
        {
            
            UTF8Encoding enc = new UTF8Encoding();
            string str = enc.GetString(input);

            str = str.Replace("\0", " ");
            //BigString = System.Text.RegularExpressions.Regex.Replace(BigString, @"\s+", "");
            str = str.Replace("\t", "");
           // str = Regex.Replace(str, @"[^\u0000-\u007F]", string.Empty);
            str= Regex.Replace(str, @"[^a-zA-Z0-9`!@#$%^&*()_+|\-=\\{}\[\]:"";'<>?,./]", " "); 
            return str;
        }
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^


        public List<string> Metric_Spec(byte[] _value)   // for 093F
        {
            List<string> _lstring = new List<string>();
            string _lstr = "";
            u_16 _ATT_id = 0;
            u_16 _leng = 0;
            UInt64 _datavalue = 0;
            string BigString = "";
            MetricSpec1.structure = new MetricStructure();

            _GeneralATTHeader = (GeneralATTHeader)ByteArrayToComplexStructureX(ref _value, _GeneralATTHeader);

            if (Attribute_ID.ContainsKey(_GeneralATTHeader.ATT_id))
                _lstr = Attribute_ID[_GeneralATTHeader.ATT_id];

            MetricSpec1 = (MetricSpec)ByteArrayToComplexStructureX(ref _value, MetricSpec1);


                _lstr = _lstr + " : Update Period->" + MetricSpec1.update_period.ToString();
                if (MetricCategory.ContainsKey(MetricSpec1.category))
                {
                    _lstr = _lstr + ", Categoty->" + MetricCategory[MetricSpec1.category];
                }
                else
                    _lstr = _lstr + ", Categoty->" + MetricSpec1.category + "(H_unknown)";


                if (MetricAccess.ContainsKey(MetricSpec1.access))
                {
                    _lstr = _lstr + ", Access->" + MetricAccess[MetricSpec1.access];
                }
                else
                    _lstr = _lstr + ", Access->" + MetricSpec1.access + "(H_unknown)";


                _lstr = _lstr + ", Ms_Struct[0=Simple,1=Compound]->" + MetricSpec1.structure.ms_struct
                    + ", Ms_compnents_no[0=Simple objects]->" + MetricSpec1.structure.ms_comp_no 
                    + ",Relevance->" + MetricSpec1.Metric_relevance.ToString();

               


            _lstring.Add(_lstr);



            return _lstring;

        }



        //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
        public List<string> Sample_Array_Spec(byte[] _value)   // for 096D
        {
            List<string> _lstring = new List<string>();
            string _lstr = "";
            u_16 _ATT_id = 0;
            u_16 _leng = 0;
            UInt64 _datavalue = 0;
            string BigString = "";
            SaSpec1.sample_type = new SampleType();


            _GeneralATTHeader = (GeneralATTHeader)ByteArrayToComplexStructureX(ref _value, _GeneralATTHeader);

         
            if (Attribute_ID.ContainsKey(_GeneralATTHeader.ATT_id))
                _lstr = Attribute_ID[_GeneralATTHeader.ATT_id];
            else
                _lstr = _GeneralATTHeader.ATT_id.ToString("X") + " [ Unknown Type]";//_datavalue.ToString();

            //SaSpec1.
            SaSpec1 = (SaSpec)ByteArrayToComplexStructureX(ref _value, SaSpec1);

            
            

                _lstr = _lstr + " : Array SIze->" + SaSpec1.array_size.ToString() + ", SA_Type[SA_SIZ]->" +SaSpec1.sample_type.sample_size.ToString()
                    + ", SA_Type[SigBit]->"+SaSpec1.sample_type.significant_bits.ToString() + ", SaFlags->";

                if (SaFlags.ContainsKey(SaSpec1.flags))
                {
                    _lstr =_lstr+ SaFlags[SaSpec1.flags];
                }
                else
                    _lstr = _lstr+ SaSpec1.flags.ToString()+" (H_unknown)";

            
            _lstring.Add(_lstr);



            return _lstring;

        }

        //???????????????????????????????????????????????????????????//
        /// <summary>
        /// case Generic
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>

        public List<string> Generic3Param_Value(byte[] _value)
        {
            List<string> _lstring = new List<string>();
            string _lstr = "";
            u_16 _ATT_id = 0;
            u_16 _leng = 0;
            UInt64 _datavalue = 0;
            string BigString = "";
            SaSpec1.sample_type = new SampleType();
            GeneralATTHeader _GeneralATTHeader = new GeneralATTHeader();

            
            //NuObsValue _NumericObsVal = new NuObsValue();
            _ATT_id = (UInt16)((UInt16)(_value[0] << 8) + _value[1]);  
             _leng = (UInt16)((UInt16)(_value[2] << 8) + _value[3]);   
            if (_value.Length == (_leng+4))   // Measures the length
            {
               // _ATT_id = (UInt16)((UInt16)(_value[2] << 8) + _value[3]);
                int _offset = 4;
                _datavalue=0;
                if (_leng <= 8) // ensure it is value or string
                {
                    for (int _i = 0; _i < _leng; _i++)
                    {
                        _datavalue = _datavalue << 8;
                        _datavalue = _datavalue + (UInt64)(_value[_offset + _i]);

                    }
                    BigString = ByteArrayToString(_value.Skip(_offset).ToArray()); ;
                }
                else
                {
                   
                    BigString = ByteArrayToString(_value.Skip(_offset).ToArray()); 
                }
    

            }



            if (Attribute_ID.ContainsKey(_ATT_id))
                _lstr = Attribute_ID[_ATT_id];


            
            if (_ATT_id == 0x0996) 
            {
                _lstr = _lstr + " : " + Unit_ID[(UInt16) _datavalue];
            }
            else if (_ATT_id == 0x09A7)
            {

                _lstr = _lstr + " : " + _datavalue.ToString();
               // string _ldmp;
                if (MDSStatus.ContainsKey((UInt16)_datavalue))
              _lstr=_lstr+ " [" + MDSStatus[(UInt16)_datavalue] + "]";//_datavalue.ToString();
                else
                    _lstr = _lstr + " [ Unknown Type]";//_datavalue.ToString();

            }
            else if (_ATT_id == 0x0946)
            {
                _lstr = _lstr + " : ";
                if (OPMode.ContainsKey((u_16)_datavalue))
                    _lstr = _lstr + OPMode[(u_16) _datavalue];
                else
                    _lstr = _lstr +_datavalue+ "(H_Unknown)";

            }
            else if (_ATT_id == 0x090D)
            {
                _lstr = _lstr + " : ";
                if (Application_Area.ContainsKey((u_16)_datavalue))
                    _lstr = _lstr + Application_Area[(u_16)_datavalue];
                else
                    _lstr = _lstr +_datavalue+ "(H_Unknown)";

            }
            else if (_ATT_id == 0x092F)
            {

                _GeneralATTHeader = (GeneralATTHeader)ByteArrayToComplexStructureX(ref _value, _GeneralATTHeader);

                TTYPE _TTYPE = new TTYPE();
                _TTYPE = (TTYPE)ByteArrayToComplexStructureX(ref _value, _TTYPE);
             
                _lstr = _lstr + ": Partition ->";

                //_lstr = NomPartitionActual.TryGetValue(_TTYPE.partition, out [_TTYPE.code];

                if (NomPartitionActual.ContainsKey(_TTYPE.partition))
                    if (NomPartitionActual[_TTYPE.partition].ContainsKey(_TTYPE.code))
                        _lstr = NomPartitionActual[_TTYPE.partition][_TTYPE.code];
                    else
                        _lstr = _lstr + NomPartition[_TTYPE.partition] + " ; " + _TTYPE.code.ToString("X") + "[H-Unknown]";
                else
                    _lstr = _lstr + NomPartition[_TTYPE.partition] + " [H_Unknown]";

               
            }
            else if (_ATT_id == 0x0935)
            {
                _lstr = _lstr + " : ";
                if (Line_Frequency.ContainsKey((u_16)_datavalue))
                    _lstr = _lstr + Line_Frequency[(u_16)_datavalue];
                else
                    _lstr = _lstr +_datavalue+ "(H_Unknown)";
            }
                
            else if ((_ATT_id == 0x091E) || (_ATT_id == 0xF13A) || (_ATT_id == 0xF14D) || (_ATT_id == 0xF25E) ||
                (_ATT_id == 0xF25F) || (_ATT_id == 0xF261) || (_ATT_id == 0x0928) )
            {
               
                _lstr = _lstr + " : " + BigString;//Unit_ID[(UInt16)_datavalue];
            }
            else if (_ATT_id == 0x0990)
            {
                string DateTime = _datavalue.ToString("X");
                DateTime = DateTime.Insert(4, "-");
                DateTime = DateTime.Insert(7, "-");
                DateTime = DateTime.Insert(10, ":");
                DateTime = DateTime.Insert(13, "-");
                DateTime = DateTime.Insert(16, "-");
                DateTime = DateTime.Insert(19, "-");
                _lstr = _lstr + " : " + DateTime;//Unit_ID[(UInt16)_datavalue];
            }
            else if ((_ATT_id == 0x0991) || (_ATT_id == 0x0921) || (_ATT_id == 0x0986)
                || (_ATT_id == 0x091D) || (_ATT_id == 0x0982) || (_ATT_id == 0x090C) || (_ATT_id == 0x098D) )
            {
                string _tempData = _datavalue.ToString();
                _lstr = _lstr + " : " + _tempData;//Unit_ID[(UInt16)_datavalue];

            }
            
            else if (_ATT_id == 0x096A)
            {
                byte[] lbyte = BitConverter.GetBytes(_datavalue);
                lbyte = lbyte.Take(_leng).ToArray();
                Array.Reverse(lbyte, 0, lbyte.Length);              // Little endian- bigendian issue !!
                u_16 lower_scaled_value = (UInt16)((UInt16)(lbyte[0] << 8) + lbyte[1]);
                u_16 upper_scaled_value = (UInt16)((UInt16)(lbyte[2] << 8) + lbyte[3]);
                _lstr = _lstr + " : lower_scaled_value->" + lower_scaled_value + ", upper_scaled_value->" + upper_scaled_value;
            }
            else if ((_ATT_id == 0x86DE) || (_ATT_id == 0x0924) || (_ATT_id == 0xF001))       // Indicates in Hex string
            {
                byte[] lbyte = BitConverter.GetBytes(_datavalue);
                lbyte = lbyte.Take(_leng).ToArray();
                Array.Reverse(lbyte, 0, lbyte.Length);              // Little endian- bigendian issue !!
                _lstr = _lstr + " [Hex string]: " + ByteArrayToHEXString(lbyte);
            }
            else if ((_ATT_id == 0x0948))
            {
                byte[] lbyte = BitConverter.GetBytes(_datavalue);
                //int[] lIntarray = Array.ConvertAll(lbyte, c => (int)c);
                u_16 lMajVersion = (UInt16)((UInt16)(lbyte[0] << 8) + lbyte[1]);
                u_16 lMinVersion = (UInt16)((UInt16)(lbyte[2] << 8) + lbyte[3]);
                
                             // Little endian- bigendian issue !!
                _lstr = _lstr + " : " + lMajVersion.ToString()+ "."+ lMinVersion.ToString();
            }
            else if ((_ATT_id == 0x0937))
            {
                byte[] lbyte = BitConverter.GetBytes(_datavalue);
                Array.Reverse(lbyte, 0, lbyte.Length);              // Little endian- bigendian issue !!

                if (lbyte.Length == 8)
                {
                    _lstr = _lstr + " : Text Catalog Revision->" + lbyte[0].ToString() + "." + lbyte[1].ToString();
                    _lstr = _lstr + ", Language Revision ->" + lbyte[2].ToString() + "." + lbyte[3].ToString();

                    u_16 _LANG = (UInt16)((UInt16)(lbyte[4] << 8) + lbyte[5]);
                    _lstr = _lstr + ", Language ->";
                    if (Language_ID.ContainsKey(_LANG))
                        _lstr = _lstr + Language_ID[_LANG];
                    else
                        _lstr = _lstr +_LANG+ "(H_Unknown)";

                    u_16 _Format = (UInt16)((UInt16)(lbyte[6] << 8) + lbyte[7]);
                    _lstr = _lstr + ", Format ->";
                    if (StringFormat_ID.ContainsKey(_Format))
                        _lstr = _lstr + StringFormat_ID[_Format];
                    else
                        _lstr = _lstr +_Format+ "(H_Unknown)";

                    //  u_16 lMinVersion = (UInt16)((UInt16)(lbyte[2] << 8) + lbyte[3]);

                }
              
            }
                  //else if ((_ATT_id == 0x0984) || (_ATT_id == 0x0998) ||  (_ATT_id == 0x094F) || (_ATT_id == 0x093E) || (_ATT_id == 0x093A))
                else if (_ATT_id == 0x0984)
                {
                    byte[] lbyte = BitConverter.GetBytes(_datavalue);
                    Array.Reverse(lbyte, 0, lbyte.Length);              // Little endian- bigendian issue !!
                    _lstr = _lstr + " : " + ByteArrayToHEXString(lbyte);
                }
                    
            else // convert the value to float
                _lstr = _lstr + " :  " + Convert.ToString(valueParse(_datavalue));

           
            _lstring.Add(_lstr);



            return _lstring;
        }



        public List<string> GenericWAVE_Value(byte[] _value)
        {
            List<string> _lstring = new List<string>();
            string _lstr = "";
            u_16 _ATT_id = 0;
            u_16 _leng = 0;
            string BigString = "";
            

            _ATT_id = (UInt16)((UInt16)(_value[0] << 8) + _value[1]);   //dont forget the 2Bytes of itself
            _leng = (UInt16)((UInt16)(_value[2] << 8) + _value[3]);   //dont forget the 2Bytes of itself
            if (_value.Length == (_leng + 4))   // Measures the length
            {
                // _ATT_id = (UInt16)((UInt16)(_value[2] << 8) + _value[3]);
                int _offset = 4;
                

                BigString = ByteArrayToHEXString(_value.Skip(_offset).ToArray());//ByteArrayToString(_value.Skip(_offset).ToArray());
                


            }

            

            //if (Attribute_ID_Wave.ContainsKey(_ATT_id))
            //    _lstr = Attribute_ID_Wave[_ATT_id];
            if (Phy_ID_Wave.ContainsKey(_ATT_id))
                _lstr = Phy_ID_Wave[_ATT_id];


            _lstr = _lstr + " :[W]: " + BigString;
            
            _lstring.Add(_lstr);

            return _lstring;


        }



        public List<string> Generic_LongArrayString(byte[] _value)
        {
            List<string> _lstring = new List<string>();
            string _lstr = "";
            u_16 _ATT_id = 0;
            u_16 _leng = 0;
            UInt64 _datavalue = 0;
            string BigString = "";
            //SaSpec1.sample_type = new SampleType();


            //NuObsValue _NumericObsVal = new NuObsValue();
            _ATT_id = (UInt16)((UInt16)(_value[0] << 8) + _value[1]);   //dont forget the 2Bytes of itself
            _leng = (UInt16)((UInt16)(_value[2] << 8) + _value[3]);   //dont forget the 2Bytes of itself
            if (_value.Length == (_leng + 4))   // Measures the length
            {
                // _ATT_id = (UInt16)((UInt16)(_value[2] << 8) + _value[3]);
                int _offset = 4;
                BigString = ByteArrayToHEXString(_value.Skip(_offset).ToArray());//ByteArrayToString(_value.Skip(_offset).ToArray());
             }

            if (Attribute_ID.ContainsKey(_ATT_id))
                _lstr = Attribute_ID[_ATT_id];

            _lstr = _lstr + " [raw data] : " + BigString;

            _lstring.Add(_lstr);

            return _lstring;

        }


        public List<string> SaVisual_Grid16(byte[] _value)
        {
            List<string> _lstring = new List<string>();
            string _lstr = "";
            string BigString = "";

            _GeneralATTHeader = (GeneralATTHeader)ByteArrayToComplexStructureX(ref _value, _GeneralATTHeader);
            int _count = (UInt16)((UInt16)(_value[0] << 8) + _value[1]);
            _SaVisualGrid16.value = new SaGridEntry16[_count];

            _SaVisualGrid16 = (SaVisualGrid16)ByteArrayToComplexStructureX(ref _value, _SaVisualGrid16); 

             BigString = "Abs_Val,Scaled_Val,level->";//ByteArrayToString(_value.Skip(_offset).ToArray());
                for (int _li = 0; _li < _SaVisualGrid16.count; _li++)
                {
                    
                    BigString = BigString+ "["+ _SaVisualGrid16.value[_li].absolute_value.ToString() + "," + _SaVisualGrid16.value[_li].scaled_value.ToString()
                        + "," + _SaVisualGrid16.value[_li].level.ToString() + "]; ";
                }
            
            if (Attribute_ID.ContainsKey(_GeneralATTHeader.ATT_id))
                _lstr = Attribute_ID[_GeneralATTHeader.ATT_id];
            else
                _lstr = _GeneralATTHeader.ATT_id.ToString() +"[H-Unknown]";

            _lstr = _lstr + ":" + BigString;
            _lstring.Add(_lstr);
            return _lstring;

        }



        public List<string> CalData16_code(byte[] _value)
        {
            List<string> _lstring = new List<string>();
            string _lstr = "";
            
            _GeneralATTHeader = (GeneralATTHeader)ByteArrayToComplexStructureX(ref _value, _GeneralATTHeader);
            _SaCalData16 = (SaCalData16)ByteArrayToComplexStructureX(ref _value, _SaCalData16);

            if (Attribute_ID.ContainsKey(_GeneralATTHeader.ATT_id))
                _lstr = Attribute_ID[_GeneralATTHeader.ATT_id];
            else
                _lstr = _GeneralATTHeader.ATT_id.ToString() +" [H-Unknown]";

             
            _lstr = _lstr + ":" + "lower_absolute_value-> "+ _SaCalData16.lower_absolute_value.ToString()+ 
            ", upper_absolute_value -> " + _SaCalData16.upper_absolute_value.ToString() 
            + ", lower_scaled_value-> " + _SaCalData16.lower_scaled_value.ToString()+
            ", upper_scaled_value-> "+ _SaCalData16.upper_scaled_value.ToString() +
            ", increment->"+_SaCalData16.increment.ToString()+
            ", Caltype[0-BAR,1-STAIR]-> " + _SaCalData16.cal_type.ToString(); 


            _lstring.Add(_lstr);

            return _lstring;

        }


        public Object ByteArrayToNormalStructureX(ref byte[] bytearray, Object _refobj)
        {
            object o = Activator.CreateInstance(_refobj.GetType());

            int _len = Marshal.SizeOf(o);

            IntPtr IPTR_databytes = Marshal.AllocHGlobal(_len);

            //SwapIt(_refobj.GetType(), bytearray, 0);

            Marshal.Copy(bytearray, 0, IPTR_databytes, _len);


            _refobj = Marshal.PtrToStructure(IPTR_databytes, _refobj.GetType());//typeof(_refobj));

            bytearray = bytearray.Skip(_len).ToArray();
            return _refobj;

        }



        public object ByteArrayToComplexStructureX(ref byte[] bytex,object obj)
        {
            if (obj == null) return null;


            object newCopy;
            if (obj.GetType().IsArray)
            {
                var t = obj.GetType();
                var e = t.GetElementType();
                var r = t.GetArrayRank();
                Array a = (Array)obj;
                newCopy = Array.CreateInstance(e, a.Length);
                Array n = (Array)newCopy;
                for (int i = 0; i < a.Length; i++)
                {
                    n.SetValue(ByteArrayToComplexStructureX(ref bytex,a.GetValue(i)), i);
                    //a = (Array)ByteArrayToNormalStructureX(ref bytex, a);
                }
                return newCopy;
        
            }
            else
            {
                newCopy = Activator.CreateInstance(obj.GetType(), true);
            
            }

//            UInt32 _leng = 0;
//            Object _obj = new Object();


            foreach (var field in newCopy.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (!field.FieldType.IsPrimitive && field.FieldType != typeof(string))
                {
                    var fieldCopy = ByteArrayToComplexStructureX(ref bytex, field.GetValue(obj));

                    field.SetValue(newCopy, fieldCopy);

                }
                else
                {
                    object o = (Object)Activator.CreateInstance(field.GetValue(obj).GetType());
                    
                    UInt16 _siz=(UInt16)Marshal.SizeOf(o.GetType());
                    Array.Reverse(bytex, 0, _siz);

                    o=ByteArrayToNormalStructureX(ref bytex, o);
                    

                    field.SetValue(newCopy, o);// field.GetValue(obj));
                    //field.SetValue(newCopy, field.GetValue(obj));

                }
            }
            return newCopy;
        }








        public List<string> SaFixedValSpec16_code(byte[] _value)
        {
            List<string> _lstring = new List<string>();
            string _lstr = "";
            string BigString = "";
            

            GeneralATTHeader _GeneralATTHeader = new GeneralATTHeader();

             _GeneralATTHeader = (GeneralATTHeader)ByteArrayToComplexStructureX(ref _value, _GeneralATTHeader);

            int count = (UInt16)((UInt16)(_value[0] << 8) + _value[1]);
            

            _SaFixedValSpec16._SaFixedValSpecEntry16 = new SaFixedValSpecEntry16[count];

            _SaFixedValSpec16 = (SaFixedValSpec16)ByteArrayToComplexStructureX(ref _value, _SaFixedValSpec16);


                BigString = "sa_fixed_val_id,sa_fixed_val ->";//ByteArrayToString(_value.Skip(_offset).ToArray());
                for (int _li = 0; _li < count; _li++)
                {
                    BigString = BigString + _SaFixedValSpec16._SaFixedValSpecEntry16[_li].sa_fixed_val_id.ToString() + "," + _SaFixedValSpec16._SaFixedValSpecEntry16[_li].sa_fixed_val.ToString()+ ";";
                }


                if (Attribute_ID.ContainsKey(_GeneralATTHeader.ATT_id))
                    _lstr = Attribute_ID[_GeneralATTHeader.ATT_id];
            else
                    _lstr = _GeneralATTHeader.ATT_id.ToString();

            _lstr = _lstr + ":" + BigString;
            _lstring.Add(_lstr);
            return _lstring;

        }




        public List<string> Unknown_ATT_code(byte[] _value)
        {
            List<string> _lstring = new List<string>();
            string _lstr = "";
            u_16 _ATT_id = 0;
            u_16 _leng = 0;
            UInt64 _datavalue = 0;
            string BigString = "";
            //SaSpec1.sample_type = new SampleType();


            //NuObsValue _NumericObsVal = new NuObsValue();
            _ATT_id = (UInt16)((UInt16)(_value[0] << 8) + _value[1]);   //dont forget the 2Bytes of itself
            _leng = (UInt16)((UInt16)(_value[2] << 8) + _value[3]);   //dont forget the 2Bytes of itself
            if (_value.Length == (_leng + 4))   // Measures the length
            {
                // _ATT_id = (UInt16)((UInt16)(_value[2] << 8) + _value[3]);
                int _offset = 4;
                BigString = ByteArrayToHEXString(_value.Skip(_offset).ToArray());//ByteArrayToString(_value.Skip(_offset).ToArray());
            }

            if (Attribute_ID.ContainsKey(_ATT_id))
                _lstr = Attribute_ID[_ATT_id];
            //else
             //   _lstr = _ATT_id.ToString();

            _lstr = _lstr + " [raw data] : " + BigString;

            _lstring.Add(_lstr);

            return _lstring;

        }
    






    }
}

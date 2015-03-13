using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using System.Web;


namespace capture
{
    public enum Protocol
    {
        TCP = 6,
        UDP = 17,
        Unknown = -1
    };

   

    public partial class capturerForm : Form
    {
        private Socket mainSocket;                          //The socket which captures all incoming packets
        private byte[] byteData = new byte[4096];
        private bool bContinueCapturing = false;            //A flag to check if packets are to be captured or not

        private delegate void AddTreeNode(TreeNode node);

        //ArrayList SOURCEIP= new ArrayList();
        Dictionary<string, string> SOURCEIP = new Dictionary<string, string>();
        Dictionary<string, string> SOURCEIP_COPY = new Dictionary<string, string>();
       // ArrayList SOURCEIP_COPY = new ArrayList();

        ArrayList DESTIP=new ArrayList();
        ArrayList OTHERIP = new ArrayList();
        
        ArrayList FILTERED_PORT = new ArrayList();
        ArrayList FILTERED_IP = new ArrayList();
        ArrayList FILTERED_PROTOCOL = new ArrayList();
        ArrayList FILTERED = new ArrayList();

        byte[] PACKET_DATA= new byte[1000];

        Int64 PORT_SRC_ADDRESS = 0;
        Int64 PORT_DEST_ADDRESS = 0;
        bool PARSE_PROCESS_STAT = false;
        string InterfaceIP;
        string CMB_box="TCP";
        string PROTOCOL = "";
        int selected_IND = 0;
        int selected_x = 0;
        long PCK_CTR1=0, PCK_CTR2 = 0;
        public capturerForm()
        {
            InitializeComponent();
            CMB_box = comboBox_Protocol.Text;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cmbInterfaces.Text == "")
            {
                MessageBox.Show("Select an Interface to capture the packets.", "MJsniffer", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (!bContinueCapturing)        
                {
                    //Start capturing the packets...

                    btnStart.Text = "&Stop";

                    bContinueCapturing = true;
                    InterfaceIP = cmbInterfaces.Text;

                    //For sniffing the socket to capture the packets has to be a raw socket, with the
                    //address family being of type internetwork, and protocol being IP
                    mainSocket = new Socket(AddressFamily.InterNetwork,
                        SocketType.Raw, ProtocolType.IP);
                    
                    //Bind the socket to the selected IP address
                    mainSocket.Bind(new IPEndPoint(IPAddress.Parse(cmbInterfaces.Text), 0));

                    //Set the socket  options
                    mainSocket.SetSocketOption(SocketOptionLevel.IP,            //Applies only to IP packets
                                               SocketOptionName.HeaderIncluded, //Set the include the header
                                               true);                           //option to true

                    byte[] byTrue = new byte[4] {1, 0, 0, 0};
                    byte[] byOut = new byte[4]{1, 0, 0, 0}; //Capture outgoing packets

                    //Socket.IOControl is analogous to the WSAIoctl method of Winsock 2
                    mainSocket.IOControl(IOControlCode.ReceiveAll,              //Equivalent to SIO_RCVALL constant
                                                                                //of Winsock 2
                                         byTrue,                                    
                                         byOut);

                    //Start receiving the packets asynchronously
                    mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
                        new AsyncCallback(OnReceive), null);
                    timer1.Start();
                }
                else
                {
                    btnStart.Text = "&Start";
                    bContinueCapturing = false;
                    //To stop capturing the packets close the socket
                    mainSocket.Close ();
                    timer1.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MJsniffer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                int nReceived = mainSocket.EndReceive(ar);

                //Analyze the bytes received...
                
                ParseData (byteData, nReceived);

                if (bContinueCapturing)     
                {
                    byteData = new byte[4096];
                    
                    //Another call to BeginReceive so that we continue to receive the incoming
                    //packets
                    mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
                        new AsyncCallback(OnReceive), null);
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MJsniffer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void ParseData(byte[] byteData, int nReceived)
        {
            TreeNode rootNode = new TreeNode();
            TreeNode tcpOrudpNode = new TreeNode();

            //Since all protocol packets are encapsulated in the IP datagram
            //so we start by parsing the IP header and see what protocol data
            //is being carried by it
            IPHeader ipHeader = new IPHeader(byteData, nReceived);

            //IPAddress IPsrc = NetCode.NetCode.ToIPAddress("10.226.38.232");
            IPAddress IPsrc = NetCode.NetCode.ToIPAddress("10.226.41.152");

            IPAddress IPsrcMy = NetCode.NetCode.ToIPAddress("10.226.42.35");
            IPAddress IPsrcSanjat = NetCode.NetCode.ToIPAddress("10.226.43.104");
            IPAddress IPsrcSanjatLaptop = NetCode.NetCode.ToIPAddress("10.226.38.232");
            IPAddress IPsrcPriyanka = NetCode.NetCode.ToIPAddress("10.226.41.152");
            IPAddress IPsrcList1 = NetCode.NetCode.ToIPAddress("192.168.0.10");

            TreeNode ipNode = MakeIPTreeNode(ipHeader);
            
            //string str1 = ipHeader.SourceAddress.ToString();
            //if (str1 == IPsrc.ToString())
            //{
            //    MessageBox.Show("Found");
            //}

            //if (ipHeader.SourceAddress.ToString() == IPsrc.ToString())
            
                rootNode.Nodes.Add(ipNode);

            //Now according to the protocol being carried by the IP datagram we parse 
            //the data field of the datagram
            //int intAddress = BitConverter.ToInt32(IPAddress.Parse(address).GetAddressBytes(), 0);
            //    string ipAddress = new IPAddress(BitConverter.GetBytes(intAddress)).ToString();

            PORT_SRC_ADDRESS=0;
            PORT_DEST_ADDRESS=0;

            //if (ipHeader.SourceAddress.ToString() == IPsrc.ToString())
            switch (ipHeader.ProtocolType)
            {
                    
                case Protocol.TCP:

                    TCPHeader tcpHeader = new TCPHeader(ipHeader.Data,              //IPHeader.Data stores the data being 
                                                                                    //carried by the IP datagram
                                                        ipHeader.MessageLength);//Length of the data field                    

                    TreeNode tcpNode = MakeTCPTreeNode(tcpHeader);
                  
                    rootNode.Nodes.Add(tcpNode);
                    tcpOrudpNode = (TreeNode)tcpNode.Clone();
                    Array.Resize(ref(PACKET_DATA),tcpHeader.MessageLength);
                    PACKET_DATA = tcpHeader.Data;
                        PORT_SRC_ADDRESS = Convert.ToInt64(tcpHeader.DestinationPort);
                        PORT_DEST_ADDRESS = Convert.ToInt64(tcpHeader.SourcePort);


                    //If the port is equal to 53 then the underlying protocol is DNS
                    //Note: DNS can use either TCP or UDP thats why the check is done twice
                    if (tcpHeader.DestinationPort == "53" || tcpHeader.SourcePort == "53")
                    {
                    
                        TreeNode dnsNode = MakeDNSTreeNode(tcpHeader.Data, (int)tcpHeader.MessageLength);
                        rootNode.Nodes.Add(dnsNode);
                      //  tcpOrudpNode = (TreeNode)dnsNode.Clone();
                    }

                    break;

                case Protocol.UDP:

                    UDPHeader udpHeader = new UDPHeader(ipHeader.Data,              //IPHeader.Data stores the data being 
                                                                                    //carried by the IP datagram
                                                       (int)ipHeader.MessageLength);//Length of the data field                    

                    TreeNode udpNode = MakeUDPTreeNode(udpHeader);
                    tcpOrudpNode = (TreeNode)udpNode.Clone();;
                    Array.Resize(ref(PACKET_DATA), udpHeader.Data.Length);
                    PACKET_DATA = udpHeader.Data;
                    
                    rootNode.Nodes.Add(udpNode);

                    PORT_SRC_ADDRESS = Convert.ToInt64(udpHeader.DestinationPort);
                    PORT_DEST_ADDRESS = Convert.ToInt64(udpHeader.SourcePort);

                    
                    

                    //If the port is equal to 53 then the underlying protocol is DNS
                    //Note: DNS can use either TCP or UDP thats why the check is done twice
                   if (udpHeader.DestinationPort == "53" || udpHeader.SourcePort == "53")
                    {
                        
                       
                        TreeNode dnsNode = MakeDNSTreeNode(udpHeader.Data,
                                                           //Length of UDP header is always eight bytes so we subtract that out of the total 
                                                           //length to find the length of the data
                                                           Convert.ToInt32(udpHeader.Length) - 8);  
                       
                       rootNode.Nodes.Add(dnsNode);
                    //   tcpOrudpNode = (TreeNode)dnsNode.Clone();
                    }

                    break;

                case Protocol.Unknown:
                    rootNode.Nodes.Add(ipHeader.ProtocolType.ToString());
                    break;
            }


            AddTreeNode addTreeNode = new AddTreeNode(OnAddTreeNode);

            rootNode.Text = ipHeader.SourceAddress.ToString() + "-" +
                ipHeader.DestinationAddress.ToString();

            //Thread safe adding of the nodes
            treeView.Invoke(addTreeNode, new object[] { rootNode });
           

            // Resolve various host names
            string _srcstr = ipHeader.SourceAddress.ToString();


          

            //if (!SOURCEIP.Contains(_srcstr))// || !SOURCEIP.Contains("->"))
            try
            {
                SOURCEIP.Add(_srcstr, "?");
            }
            catch (Exception ex)
            { }
            //SOURCEIP.Sort();


            
            if (!backgroundWorker1.IsBusy)
            {

                //listBox1.Refresh();
                SOURCEIP_COPY = SOURCEIP;

                //textBox6.Text = SOURCEIP_COPY.Count.ToString();
                backgroundWorker1.RunWorkerAsync();
            }
            




            // Filter by sorting individual condition
            string _str = ipHeader.SourceAddress.ToString() + ":" + ipHeader.DestinationAddress.ToString();
            
            bool _PortFound = false;
            bool _ProtocolFound = false;
            bool _IPFound = false;

            
            if (checkBox_Port.Checked == true && textBox_PORT.Text!=null)
                if (PORT_DEST_ADDRESS == Convert.ToInt32(textBox_PORT.Text) || PORT_SRC_ADDRESS == Convert.ToInt32(textBox_PORT.Text))
                { FILTERED_PORT.Add(_str); _PortFound = true; }


            if (checkBox_Protocol.Checked == true)
            {
                int _Protocol = -1;
                if ((CMB_box == "TCP") && (ipHeader.ProtocolType == Protocol.TCP))
                { _Protocol = Convert.ToInt16(Protocol.TCP); PROTOCOL = "TCP"; }
                else if ((CMB_box == "UDP") && (ipHeader.ProtocolType == Protocol.UDP))
                { _Protocol = Convert.ToInt16(Protocol.UDP); PROTOCOL = "UDP"; }
                else
                    PROTOCOL = "UNKNOWN";

                if (Convert.ToInt16(ipHeader.ProtocolType) == _Protocol)
                { FILTERED_PROTOCOL.Add(_str); _ProtocolFound = true; }

            }
            else
                PROTOCOL = "TCP/UDP";


            if (checkBox_IP.Checked == true && textBox_IP.Text!=null)
                //if (ipHeader.SourceAddress.ToString() == textBox_IP.Text || ipHeader.DestinationAddress.ToString() == textBox_IP.Text)
                if (_str.Contains(textBox_IP.Text) && _str.Contains(textBox_IPE.Text))
                   { FILTERED_IP.Add(_str); _IPFound = true; }
            

            /////////////////////////////////////// filter common part as per filter condition
            bool UPDATE_FLG = false;
            PARSE_PROCESS_STAT = true;
            if (checkBox_Port.Checked == false && checkBox_Protocol.Checked == false && checkBox_IP.Checked == false)
            {
                
                 FILTERED.Add(_str); UPDATE_FLG = true; 
            }
            else if (checkBox_Port.Checked == true && checkBox_Protocol.Checked == false && checkBox_IP.Checked == false)
            {
                if (_PortFound == true)
                { FILTERED.Add(_str); UPDATE_FLG = true; }
            }
            else if (checkBox_Port.Checked == false && checkBox_Protocol.Checked == true && checkBox_IP.Checked == false)
            {
                if (_ProtocolFound == true)
                { FILTERED.Add(_str); UPDATE_FLG = true; }
            }
            else if (checkBox_Port.Checked == false && checkBox_Protocol.Checked == false && checkBox_IP.Checked == true)
            {
                if (_IPFound == true)
                { FILTERED.Add(_str); UPDATE_FLG = true; }
            }
            else if (checkBox_Port.Checked == true && checkBox_Protocol.Checked == true && checkBox_IP.Checked == false)
            {
                if (_PortFound == true && _ProtocolFound == true && _IPFound == false)
                { FILTERED.Add(_str); UPDATE_FLG = true; }

            }
            else if (checkBox_Port.Checked == true && checkBox_Protocol.Checked == false && checkBox_IP.Checked == true)
            {
                if (_PortFound == true && _ProtocolFound == true && _IPFound == true)
                { FILTERED.Add(_str); UPDATE_FLG = true; }
            }
            else if (checkBox_Port.Checked == false && checkBox_Protocol.Checked == true && checkBox_IP.Checked == true)
            {
                //foreach (string element in FILTERED_IP)
                if (_PortFound == false && _ProtocolFound == true && _IPFound == true)
                { FILTERED.Add(_str); UPDATE_FLG = true; }

            }
            else if (checkBox_Port.Checked == true && checkBox_Protocol.Checked == true && checkBox_IP.Checked == true)
            {
                //foreach (string element in FILTERED_PORT)
                if (_PortFound == true && _ProtocolFound == true && _IPFound == true)
                { FILTERED.Add(_str); UPDATE_FLG = true; }
            }

            //Remove the entry which contains my own IP
            if (checkBox_MyIP.Checked == true && FILTERED.Count>0)
                if (FILTERED[FILTERED.Count - 1].ToString().Contains(InterfaceIP))
                { FILTERED.RemoveAt(FILTERED.Count - 1); UPDATE_FLG = false; }


            if (UPDATE_FLG == true && FILTERED.Count > 0)
            {
                UPDATE_FLG = false;

                FILTERED[FILTERED.Count - 1] = _str;// +"  [" + PORT_SRC_ADDRESS + ":" + PORT_DEST_ADDRESS + "]";

                //FILTERED.Sort();
                if (checkBox_rmvDuplicates.Checked == true)
                    FILTERED = RemoveDuplicates(FILTERED);




                TreeNode _FilteredRootNode = new TreeNode();

                IPHeader _ipHeader = new IPHeader(byteData, nReceived);
                TreeNode _ipNode = MakeIPTreeNode(_ipHeader);


                _FilteredRootNode.Nodes.Add(_ipNode);

                _FilteredRootNode.Nodes.Add(tcpOrudpNode);


                AddTreeNode _addTreeNode = new AddTreeNode(_OnAddTreeNode);

                string _DATA = ByteArrayToString(PACKET_DATA);
                _FilteredRootNode.Text = _ipHeader.SourceAddress.ToString() + "->" + _ipHeader.DestinationAddress.ToString() + "[" + _DATA + "]";

                string _SourceInfo = _ipHeader.SourceAddress.ToString()+"["+ PORT_SRC_ADDRESS +"]";
                string _DestInfo = _ipHeader.DestinationAddress.ToString() + "[" + PORT_DEST_ADDRESS + "]";
                string _strdummy = "";

                
                //Thread safe adding of the nodes
                treeView1.Invoke(_addTreeNode, new object[] { _FilteredRootNode });
                if(checkBoxSave.Checked)
                if(textBoxClient.Text!=null && textBoxServer.Text!=null)
                {
                    if (_SourceInfo.Contains(textBoxServer.Text) && _DestInfo.Contains(textBoxClient.Text))
                    {
                       PCK_CTR1++;
                       _strdummy = "Client<-Server,";
                        if(checkBoxHeader.Checked)
                       _strdummy = _strdummy + PROTOCOL + "," + _DestInfo + "<-" + _SourceInfo + ",";
                       _strdummy =  _strdummy+_DATA;

                        Log("log.txt", _strdummy);
                       
                    }
                    else if (textBoxClient.Text == textBoxServer.Text)
                     if(_SourceInfo.Contains(textBoxClient.Text) || _DestInfo.Contains(textBoxServer.Text))
                    {
                        PCK_CTR2++;

                        _strdummy = "**Client->Server,";
                        if (checkBoxHeader.Checked)
                        _strdummy = _strdummy + PROTOCOL + "," + _SourceInfo + "->" + _DestInfo + ",";
                        _strdummy = _strdummy + _DATA;


                        Log("log.txt",_strdummy);
                        
                    }
                    else if (_SourceInfo.Contains(textBoxClient.Text) && _DestInfo.Contains(textBoxServer.Text))
                    {
                        PCK_CTR2++;

                        _strdummy = "Client->Server,";
                        if (checkBoxHeader.Checked)
                            _strdummy = _strdummy + PROTOCOL + "," + _SourceInfo + "->" + _DestInfo + ",";
                        _strdummy = _strdummy + _DATA;


                        Log("log.txt", _strdummy);

                    }


                }


            }
            PARSE_PROCESS_STAT = false;


            
           
        }

        public void Log(string logname, string l_str_message)
        {
            string g_str_logs_path="";
            if(!logname.Contains(":"))
                g_str_logs_path = Application.StartupPath + logname;
            else
                g_str_logs_path = logname;
            label3.Text = "Saving at:" + g_str_logs_path;

            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(g_str_logs_path, true);

                file.WriteLine(l_str_message);
                //file.WriteLine("----------------------------------------------------------------------------------");
                //file.WriteLine();
                file.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Log: " + ex.Message);

            }
        }



        public ArrayList RemoveDuplicates(ArrayList inputArray)
        {
            System.Collections.ArrayList distinctArray = new System.Collections.ArrayList();

            foreach (string element in inputArray)
            {
                if (!distinctArray.Contains(element))
                    distinctArray.Add(element);
            }

            return distinctArray;

        }


        //Helper function which returns the information contained in the IP header as a
        //tree node
        private TreeNode MakeIPTreeNode(IPHeader ipHeader)
        {
            TreeNode ipNode = new TreeNode();

            ipNode.Text = "IP";            
            
            ipNode.Nodes.Add ("Ver: " + ipHeader.Version);
            ipNode.Nodes.Add ("Header Length: " + ipHeader.HeaderLength);
            ipNode.Nodes.Add ("Differntiated Services: " + ipHeader.DifferentiatedServices);
            ipNode.Nodes.Add("Total Length: " + ipHeader.TotalLength);
            ipNode.Nodes.Add("Identification: " + ipHeader.Identification);
            ipNode.Nodes.Add("Flags: " + ipHeader.Flags);
            ipNode.Nodes.Add("Fragmentation Offset: " + ipHeader.FragmentationOffset);
            ipNode.Nodes.Add("Time to live: " + ipHeader.TTL);
            switch (ipHeader.ProtocolType)
            {
                case Protocol.TCP:
                    ipNode.Nodes.Add ("Protocol: " + "TCP");
                    break;
                case Protocol.UDP:
                    ipNode.Nodes.Add ("Protocol: " + "UDP");
                    break;
                case Protocol.Unknown:
                    ipNode.Nodes.Add ("Protocol: " + "Unknown");
                    break;
            }
            ipNode.Nodes.Add("Checksum: " + ipHeader.Checksum);
            ipNode.Nodes.Add("Source: " + ipHeader.SourceAddress.ToString());
            ipNode.Nodes.Add("Destination: " + ipHeader.DestinationAddress.ToString());

            return ipNode;
        }

        //Helper function which returns the information contained in the TCP header as a
        //tree node
        private TreeNode MakeTCPTreeNode(TCPHeader tcpHeader)
        {
            TreeNode tcpNode = new TreeNode();

            tcpNode.Text = "TCP";

            tcpNode.Nodes.Add("Source Port: " + tcpHeader.SourcePort);
            tcpNode.Nodes.Add("Destination Port: " + tcpHeader.DestinationPort);
            tcpNode.Nodes.Add("Sequence Number: " + tcpHeader.SequenceNumber);

            if (tcpHeader.AcknowledgementNumber != "")
                tcpNode.Nodes.Add("Acknowledgement Number: " + tcpHeader.AcknowledgementNumber);

            tcpNode.Nodes.Add("Header Length: " + tcpHeader.HeaderLength);
            tcpNode.Nodes.Add("Flags: " + tcpHeader.Flags);
            tcpNode.Nodes.Add("Window Size: " + tcpHeader.WindowSize);
            tcpNode.Nodes.Add("Checksum: " + tcpHeader.Checksum);

            if (tcpHeader.UrgentPointer != "")
                tcpNode.Nodes.Add("Urgent Pointer: " + tcpHeader.UrgentPointer);
           
            //udpNode.Nodes.Add("Data: " + ByteArrayToString(udpHeader.Data));
            tcpNode.Nodes.Add("Data: " + ByteArrayToString(tcpHeader.Data));
            return tcpNode;
        }
        

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        //Helper function which returns the information contained in the UDP header as a
        //tree node
        private TreeNode MakeUDPTreeNode(UDPHeader udpHeader)
        {           
            TreeNode udpNode = new TreeNode();

            udpNode.Text = "UDP";
            udpNode.Nodes.Add("Source Port: " + udpHeader.SourcePort);
            udpNode.Nodes.Add("Destination Port: " + udpHeader.DestinationPort);
            udpNode.Nodes.Add("Length: " + udpHeader.Length);
            udpNode.Nodes.Add("Checksum: " + udpHeader.Checksum);
            udpNode.Nodes.Add("Data: " + ByteArrayToString(udpHeader.Data));
            return udpNode;
        }

       
        //Helper function which returns the information contained in the DNS header as a
        //tree node
        private TreeNode MakeDNSTreeNode(byte[] byteData, int nLength)
        {
            DNSHeader dnsHeader = new DNSHeader(byteData, nLength);

            TreeNode dnsNode = new TreeNode();

            dnsNode.Text = "DNS";
            dnsNode.Nodes.Add("Identification: " + dnsHeader.Identification);
            dnsNode.Nodes.Add("Flags: " + dnsHeader.Flags);
            dnsNode.Nodes.Add("Questions: " + dnsHeader.TotalQuestions);
            dnsNode.Nodes.Add("Answer RRs: " + dnsHeader.TotalAnswerRRs);
            dnsNode.Nodes.Add("Authority RRs: " + dnsHeader.TotalAuthorityRRs);
            dnsNode.Nodes.Add("Additional RRs: " + dnsHeader.TotalAdditionalRRs);

            return dnsNode;
        }

        private void OnAddTreeNode(TreeNode node)
        {
            treeView.Nodes.Add(node);
        }
        
        private void _OnAddTreeNode(TreeNode node1)
        {
            treeView1.Nodes.Add(node1);
        }

        private void SnifferForm_Load(object sender, EventArgs e)
        {
            string strIP = null;

            IPHostEntry HosyEntry = Dns.GetHostEntry((Dns.GetHostName()));
            if (HosyEntry.AddressList.Length > 0)
            {
                foreach (IPAddress ip in HosyEntry.AddressList)
                {
                    strIP = ip.ToString();
                    cmbInterfaces.Items.Add(strIP);
                }
            }            
        }

        private void SnifferForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bContinueCapturing)
            {
                mainSocket.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox3.Text = treeView.GetNodeCount(false).ToString();
            textBox2.Text = treeView1.GetNodeCount(false).ToString();

            label_MyIP.Text = "My IP : " + InterfaceIP;

            labelClient.Text = PCK_CTR1.ToString();
            labelServer.Text = PCK_CTR2.ToString();

            //listBox1.DataSource = null;
            //listBox2.DataSource = null;
            //listBox1.Items.Clear();
            //listBox2.Items.Clear();
            
            if (!backgroundWorker1.IsBusy)
                
            {
                
                listBox1.Refresh();
                if(listBox1.Items.Count>0)
                listBox1.SelectedIndex = selected_IND;//listBox1.SelectedIndex      
            //    SOURCEIP_COPY = SOURCEIP;
                
                textBox6.Text = SOURCEIP_COPY.Count.ToString();

              //  if (!backgroundWorker1.IsBusy)
              //  backgroundWorker1.RunWorkerAsync();
            }
            
            if (PARSE_PROCESS_STAT == false)
            {
                listBox_Filtered.DataSource = null;
                listBox_Filtered.Items.Clear();
                listBox_Filtered.DataSource = FILTERED;
                listBox_Filtered.Refresh();
                if(listBox_Filtered.Items.Count>selected_x)
                listBox_Filtered.SelectedIndex = selected_x;

                textBox1.Text = FILTERED.Count.ToString();

                listBox1.DataSource = null;
                listBox1.Items.Clear();
                try
                {
                    int _i = 0;
                    foreach (string items in SOURCEIP_COPY.Keys)
                    {
                        _i++;
                        //listBox1.DataSource = SOURCEIP_COPY.Values;
                        listBox1.Items.Add(_i+ " : " + items + " : " +SOURCEIP_COPY[items]);
                        

                        //if(SOURCEIP_COPY[items].Length>1)
                           // if(listBox1.SelectedIndex>0)
                          //  listBox1.SelectedIndex = selected_IND;//listBox1.SelectedIndex      
                        
                    }
                }
                catch (Exception ex)
                { }

                listBox1.Refresh();
                try
                {
                    listBox1.SelectedIndex = selected_IND;//listBox1.SelectedIndex      
                }
                catch (Exception ex)
                { }


            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            
        }

        private void comboBox_Protocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            CMB_box = comboBox_Protocol.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CMB_box = comboBox_Protocol.Text;
            FILTERED_IP.Clear();
            FILTERED_PORT.Clear();
            FILTERED_PROTOCOL.Clear();
            FILTERED.Clear();
            treeView1.Nodes.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            //string x = Dns.GetHostName();
            //try
            //{
            //    string PCName = Dns.GetHostEntry(IPAddress.Parse(textBox4.Text)).HostName;
            //    textBox5.Text = PCName;
            //}
            //catch (Exception ex)
            //{
            //    textBox5.Text = "Fail";

            //}
            ////Dns.GetHostEntry(System.Web.HttpContext.Current.Request.ServerVariables["192.168.0.10"]).HostName;

          //  IPAddress ip = IPAddress.Parse(textBox4.Text);
            try
            {

                System.Net.IPHostEntry ip = System.Net.Dns.GetHostByAddress(textBox4.Text);
                //Console.WriteLine("IP Address {0}: {1}", ++AddressCount, ip.HostName);
                textBox5.Text = ip.HostName.ToString();
            }
            catch (Exception ex)
            {
                textBox5.Text = "FAIL";
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            IPHostEntry entry;
            IPAddress  _ip;
            string PCName = "?";
            try
            {
                foreach (string ipaddres in SOURCEIP_COPY.Keys)
                {
                    _ip=IPAddress.Parse(ipaddres);
                    if (SOURCEIP_COPY[ipaddres] == "?")
                    {
                        try
                        {
                           
                           PCName = Dns.GetHostEntry(IPAddress.Parse(ipaddres)).HostName;
                            //SOURCEIP.Add(ipaddres, PCName);
                            if (PCName.Length > 0)
                            {



                                if (_ip.AddressFamily != AddressFamily.InterNetwork)
                                {
                                    //entry = Dns.GetHostEntry(ipaddres);
                                    SOURCEIP_COPY[ipaddres] = "External";// entry.HostName.ToString();
                                }
                                else
                                                                   
                                SOURCEIP_COPY[ipaddres] = PCName;


                            }
                            else
                                SOURCEIP_COPY[ipaddres] = "*";

                        }
                        catch (Exception ex)
                        {
                            SOURCEIP_COPY[ipaddres] = "*";
                        }
        

                    }

                }
            }
            catch (Exception ex)
            { }



            //for (int _i = 0; _i < SOURCEIP.Count; _i++)
            //{

            //    string ipaddres = SOURCEIP[_i].ToString();
            //    if (!ipaddres.Contains("->"))
            //    {
            //        string PCName = "->";
            //        try
            //        {
            //            PCName = PCName + Dns.GetHostEntry(IPAddress.Parse(ipaddres)).HostName;

            //        }
            //        catch (Exception ex)
            //        {
            //        }
            //        SOURCEIP_COPY[_i] = SOURCEIP_COPY[_i] + PCName;
            //        //textBox5.Text = PCName;

            //    }
            //}

        }


        public static bool IsLocalIpAddress(string host)
        {
            try
            { // get host IP addresses
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                // get local IP addresses
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // test if any host IP equals to any local IP or to localhost
                foreach (IPAddress hostIP in hostIPs)
                {
                    // is localhost
                    if (IPAddress.IsLoopback(hostIP)) return true;
                    // is local address
                    foreach (IPAddress localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP)) return true;
                    }
                }
            }
            catch { }
            return false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            selected_IND = listBox1.SelectedIndex;
        }

        private int updateIndex(int oldIndex)
        {
           // string val=SOURCEIP_COPY;\

           

            return 0;
        }

        private void listBox_Filtered_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] _dummy = new string[5];
        
            selected_x = listBox_Filtered.SelectedIndex;
            //_dummy[0] = listBox1.Items[selected_IND].ToString();
            if (selected_x > 0)
            {
                _dummy = listBox_Filtered.Items[selected_x].ToString().Split(':');
                textBoxServer.Text = _dummy[0];
                textBoxClient.Text = _dummy[1];
            }
        }

      



      


    }
}
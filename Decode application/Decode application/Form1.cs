using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;



namespace Decode_application
{
    public partial class Form1 : Form
    {
        DecoderAPI Dx = new DecoderAPI();
        //Decoder Dx1 = new Decoder();
        public byte[] G_DATA= new byte[1];
        public Form1()
        {
            InitializeComponent();
        }

       

        //Experiment


        public struct sinWaveStruct
{
    public double amp;      /*  sine wave amplitude */
    public double t;        /*  time                */
    public double period;   /*  oscillation period  */
    public double omega;    /*  phase angle         */
}

public struct sinWaveAdd
{
    public IntPtr waves;        /*  array of sinWaveStruct waves    */
    public int len;             /*  length of array */
}

        //Expe end



        public static byte[] _HEXStringToByteArray(string _hexstring)
        {
            _hexstring = System.Text.RegularExpressions.Regex.Replace(_hexstring, @"\s+", "");
            _hexstring = _hexstring.ToUpper();  // convert all to UPPER

            Regex r = new Regex(@"^[0-9A-F\r\n]+$");
            if (r.Match(_hexstring).Success == false)
                _hexstring = "";



            return Enumerable.Range(0, _hexstring.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(_hexstring.Substring(x, 2), 16))
                             .ToArray();
        }

        private void execute()
        {
           


            List<string> xstr=new List<string>();

            label2.Text = "Wait....";
            label2.Refresh();



            xstr= Dx.getdata(G_DATA, textBox1.Text);
            
            //xstr = Dx.getdata(G_DATA, textBox1.Text);
             




            listBox1.DataSource = xstr;
            listBox1.Refresh();
            listBox2.DataSource = Dx._GXUndecoded_Attributes;// _GUndecoded_Attributes;
             listBox2.Refresh();

             UInt32 _lctr = 0, _lc=0;

             string logname1 = "Decoded_data.txt";
             foreach (object item in xstr)
                         
                 Log(logname1, item.ToString());
                 //sw.WriteLine(item.ToString());


             string logname2 = "Un_Decoded_Attributes.txt";

             foreach (object item in Dx._GXUndecoded_Attributes)
                 Log(logname2, item.ToString());


             string logname3 = "Un_Decoded_data.txt";
             foreach (object item in Dx._GUndecoded_Chunk)
                 Log(logname3, item.ToString());


             string logname4 = "NewFrame_data.txt";
             foreach (object item in Dx._GSNewDataChunk)
                 Log(logname4, item.ToString());

             string logname5 = "NewATTributeList.txt";
             foreach ( KeyValuePair<UInt16, UInt16> entry  in Dx.NewAttribute)
                 Log(logname5, "ATT-ID : "+ entry.Key.ToString("X") + "  Count : " +entry.Value.ToString());


             label2.Text = "Status:Idle.";
             label2.Refresh();
            
            
            MessageBox.Show("Data Decoding Completed");

        }

        


        public void Log( string logname, string l_str_message)
        {
            string g_str_logs_path = Application.StartupPath + logname;
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

        private void button2_Click(object sender, EventArgs e)
        {
            
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            string Filename;// = "E:\\AIIMS\\AIIMS\\Data\\Hexdump.txt";
            Filename = openFileDialog1.FileName;
            if (Filename.Length < 1)
                return;
            byte[] fileBytes = File.ReadAllBytes(Filename);

            listBox1.DataSource = null;
            listBox2.DataSource = null;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox1.Refresh();
            listBox2.Refresh();
            
            G_DATA = fileBytes;
            //FileStream FS = File.OpenRead(Filename);
            //G_DATA = ReadFully[FS];
            execute();
        }



        public static byte[] ReadFully(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox3.Text = Dx.G_CTR.ToString();
            textBox3.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            G_DATA = _HEXStringToByteArray(textBox2.Text);
            execute();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Decoding starting...");
            sinWaveStruct[] arr = new sinWaveStruct[300];

            GCHandle arrHandle = GCHandle.Alloc(arr, GCHandleType.Pinned);
                try
                {
                    sinWaveAdd ixi;//= new sinWaveAdd();
                    ixi.waves = arrHandle.AddrOfPinnedObject();
                    ixi.len = 300;
                   // UInt16 leng = sizeof(typeof(ixi)); 
                    
                   // ixi.waves
                    
                    var x = Marshal.SizeOf(ixi);
                    
                }
                finally
                {
                    arrHandle.Free();
                }


        }

         


           

    }




}

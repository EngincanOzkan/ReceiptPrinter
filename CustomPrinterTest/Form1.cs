using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomPrinterTest
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
             * You can reach active printer names from this part
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                MessageBox.Show(printer);
            }*/
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            KioskUsbSerialPrinter ob = new KioskUsbSerialPrinter();
            ob.Print("BAŞLIK BAŞLIK BAŞLIK BAŞLIK BAŞLIK BAŞLIK BAŞLIK BAŞLIK BAŞLIK BAŞLIK BAŞLIK BAŞLIK BAŞLIK", "All in the Details Now that you have several different examples of descriptive text, you can try your hand at writing a detailed, descriptive sentence, paragraph or short story of your own.If you need help with powerful descriptions, try some figurative language to help to paint a picture and evoke emotions.If you need inspiration, explore the authors linked above, or check out our quotes from poets like \"H.D.\" Hilda Doolitle and Gerard Manley Hopkins, novelists like Angela Carter, or songwriters like Tori Amos and Tom Waits. All in the Details Now that you have several different examples of descriptive text, you can try your hand at writing a detailed, descriptive sentence, paragraph or short story of your own.If you need help with powerful descriptions, try some figurative language to help to paint a picture and evoke emotions.If you need inspiration, explore the authors linked above, or check out our quotes from poets like \"H.D.\" Hilda Doolitle and Gerard Manley Hopkins, novelists like Angela Carter, or songwriters like Tori Amos and Tom Waits. All in the Details Now that you have several different examples of descriptive text, you can try your hand at writing a detailed, descriptive sentence, paragraph or short story of your own.If you need help with powerful descriptions, try some figurative language to help to paint a picture and evoke emotions.If you need inspiration, explore the authors linked above, or check out our quotes from poets like \"H.D.\" Hilda Doolitle and Gerard Manley Hopkins, novelists like Angela Carter, or songwriters like Tori Amos and Tom Waits.");
        }

    }
}

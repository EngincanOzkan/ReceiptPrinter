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
        private int paperSizeWidth = 314; // 8 cm = 3.14 inch
        private int paperSizeHeight = 787; //  20 cm = 7.87 inch

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                //MessageBox.Show(printer);
            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();

            PrintDocument printDocument = new PrintDocument();

            printDocument.DefaultPageSettings.PaperSize = new PaperSize("CUSTOMSIZE", paperSizeWidth, paperSizeHeight);
            printDialog.Document = printDocument; //add the document to the dialog box...        

            printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt); //add an event handler that will do the printing

            //on a till you will not want to ask the user where to print but this is fine for the test envoironment.
            //DialogResult result = printDialog.ShowDialog();

           // if (result == DialogResult.OK)
           // {
                printDocument.DocumentName = "My Presentation";
               
                printDocument.Print();
           // }

        }



        public void CreateReceipt(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            //this prints the reciept

            Graphics graphic = e.Graphics;

            Font font = new Font("Times New Roman", 12); //must use a mono spaced font as the spaces need to line up
            Font fontTopDate = new Font("Times New Roman", 8);
            Font fontTitle = new Font("Times New Roman", 16, FontStyle.Bold);

            float fontHeight = font.GetHeight();

            int startX = 10;
            int startY = 10;
            int offset = 40;

            string currentDate = DateTime.Now.ToString("dd / MM / yyyy");

            float drawX1 = (paperSizeWidth / 2);
            float drawX2 = ((float)(currentDate.Length) * ((8f * 0.013f) * 20f));
            float drawX3 = drawX1 - drawX2;
            graphic.DrawString(currentDate, fontTopDate, new SolidBrush(Color.Black), drawX3, startY);

            string titleText = "BAŞLIKBAŞLIKBAŞLIKBAŞLIKBAŞLIKBAŞLIKBAŞLIKBAŞLIKBAŞLIK";
            string title = "";
            int indexOfTitle = 0;
            do
            {
                title += titleText[indexOfTitle];
                indexOfTitle++;
            } while (e.Graphics.MeasureString(title, fontTitle).Width < 280f && indexOfTitle != titleText.Length);
            if (e.Graphics.MeasureString(title, fontTitle).Width >= 280f)
            {
                title += "...";
            }

            graphic.DrawString(title, fontTitle, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight; //make the spacing consistent
            graphic.DrawString("-----------------------------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 5; //make the spacing consistent

            string descriptionText = "All in the Details Now that you have several different examples of descriptive text, you can try your hand at writing a detailed, descriptive sentence, paragraph or short story of your own.If you need help with powerful descriptions, try some figurative language to help to paint a picture and evoke emotions.If you need inspiration, explore the authors linked above, or check out our quotes from poets like \"H.D.\" Hilda Doolitle and Gerard Manley Hopkins, novelists like Angela Carter, or songwriters like Tori Amos and Tom Waits.";
            string description = "";
            int indexOfDescription = 0;
            do
            {
                do
                {
                    description += descriptionText[indexOfDescription];
                    indexOfDescription++;
                } while (e.Graphics.MeasureString(description, font).Width < 280f && indexOfDescription != descriptionText.Length);
                if (indexOfDescription != descriptionText.Length)
                {
                    int tempIndexOfDescription = indexOfDescription;
                    while(description[description.Length-1] != ' ' && description.Length != 1) { 
                        indexOfDescription--;
                        description = description.Remove(description.Length-1);
                    }
                    if(description.Length == 1) {
                        indexOfDescription--;
                        description = descriptionText.Substring(indexOfDescription, tempIndexOfDescription-indexOfDescription);
                        indexOfDescription = tempIndexOfDescription;
                    }
                }
                graphic.DrawString(description, font, new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight; //make the spacing consistent

                description = "";
            } while (indexOfDescription != descriptionText.Length);

            MessageBox.Show("PERFECTO");
        }
    }
}

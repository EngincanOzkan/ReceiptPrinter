using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomPrinterTest
{
    class KioskUsbSerialPrinter
    {
        private const int paperSizeWidth = 314; // 8 cm = 3.14 inch
        private const int paperSizeHeight = 787; //  20 cm = 7.87 inch

        private string currentDate { get; set; } //automatic producing
        private string titleText { get; set; } //printed title, it can be devide because the lenght of text
        private string descriptionText { get; set; } //printed text, it can be devide because the lenght of text

        public Font font { get; set; } 
        public Font fontTopDate { get; set; } 
        public Font fontTitle { get; set; }

        public KioskUsbSerialPrinter() {
            font = new Font("Times New Roman", 12);  //must use a mono spaced font as the spaces need to line up, and for text
            fontTopDate  = new Font("Times New Roman", 8); //for date printing
            fontTitle = new Font("Times New Roman", 16, FontStyle.Bold); //for title printing and that is BOLD text
            currentDate = CurrentDateString();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Print(string titleText, string descriptionText) {

            //titleText and descriptionText can modify when print call.
            this.titleText = titleText;
            this.descriptionText = descriptionText;

            PrintDialog printDialog = new PrintDialog();

            PrintDocument printDocument = new PrintDocument();

            printDocument.DefaultPageSettings.PaperSize = new PaperSize("CUSTOMSIZED", paperSizeWidth, paperSizeHeight); // we defined our file sizes for serial printer
            printDialog.Document = printDocument; //add the document to the dialog box...        

            printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt); //add an event handler that will do the printing

            //on a till you will not want to ask the user where to print but this is fine for the test envoironment.

            //if necessary you can active the dialog files commedns

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

            float fontHeight = font.GetHeight();

            int startX = 10;
            int startY = 10;
            int offset = 40;

            // draw date with its own font, 
            graphic.DrawString(currentDate, fontTopDate, new SolidBrush(Color.Black), (paperSizeWidth / 2) - (e.Graphics.MeasureString(currentDate, fontTopDate).Width / 2f), startY);

            //string titleText = "BAŞLIK BAŞLIK BAŞLIK BAŞLIK BAŞLIK BAŞLIK BAŞLIK BAŞLIK BAŞLIK";
           
            
            graphic.DrawString(PlaceTitle(e), fontTitle, new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + (int)fontHeight; //make the spacing consistent
            graphic.DrawString("-----------------------------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 5; //make the spacing consistent

            string description = "";
            int indexOfDescription = 0;
            do//This dowhile tries to reach the end of description or checks end of available size of page with offset.
            {
                do//this dowhile checks size of line with e.Graphics.MeasureString.
                {
                    if (descriptionText.Length != 0)
                    {
                        description += descriptionText[indexOfDescription];
                        indexOfDescription++;
                    }
                } while (e.Graphics.MeasureString(description, font).Width < 280f && indexOfDescription != descriptionText.Length);// this line also checks status of descriptionText agains to out of range.

                if (indexOfDescription != descriptionText.Length) // if we are not in the end we are searching for last word
                {
                    int tempIndexOfDescription = indexOfDescription;

                    //It checks the last letter, if the character is space loop will break
                    while (description[description.Length - 1] != ' ' && description.Length != 1)
                    {
                        indexOfDescription--; //we are counting to back
                        description = description.Remove(description.Length - 1); //putting back the last letter
                    }

                    if (description.Length == 1) // if there is not any word we are cutting text from end of the line.
                    {
                        indexOfDescription--;//it is required for take first letter
                        description = descriptionText.Substring(indexOfDescription, tempIndexOfDescription - indexOfDescription);//it puts back all letters which one we delete for search 'space'
                        indexOfDescription = tempIndexOfDescription;
                    }
                }
                graphic.DrawString(description, font, new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)fontHeight; //make the spacing consistent

                description = "";
            } while (indexOfDescription != descriptionText.Length && offset < 657);
            //when the ofset comes
            if (offset >= 657)
            {
                graphic.DrawString("...", font, new SolidBrush(Color.Black), startX, startY + offset);
            }

            // Create image. Thit image is static
            Image newImage = Image.FromFile("logo.png");

            // Draw original image to screen. 
            // Rectangle required for define position and height
            graphic.DrawImage(newImage, new Rectangle(10, paperSizeHeight - 100, paperSizeWidth - 20, paperSizeHeight - 667));
        }

        /// <summary>
        ///  //current date: / month (number) / year day(tr) - day(en)
        /// </summary>
        /// <returns> currentDate </returns>
        public string CurrentDateString() {
            string currentDate = DateTime.Now.ToString("dd / MM / yyyy");

            switch (DateTime.Now.DayOfWeek)
            {
                case (DayOfWeek.Monday): currentDate += " Pazartesi - Mon"; break;
                case (DayOfWeek.Tuesday): currentDate += " Salı - Tue"; break;
                case (DayOfWeek.Wednesday): currentDate += " Çarşamba - Wed"; break;
                case (DayOfWeek.Thursday): currentDate += " Perşembe - Thu"; break;
                case (DayOfWeek.Friday): currentDate += " Cuma - Fri"; break;
                case (DayOfWeek.Saturday): currentDate += " Cumartesi - Sat"; break;
                case (DayOfWeek.Sunday): currentDate += " Pazar - Sun"; break;
                default: break;
            }

            return currentDate;
        }

        /// <summary>
        /// it calculates and if necessery cutting the title
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private string PlaceTitle(PrintPageEventArgs e) {
            string title = "";
            int indexOfTitle = 0;

            do
            {
                if (titleText != "")
                {
                    title += titleText[indexOfTitle];
                    indexOfTitle++;
                }
                //e.Graphics.MeasureString function calculates size of string with current font
            } while (e.Graphics.MeasureString(title, fontTitle).Width < 280f && indexOfTitle != titleText.Length); // 2.80 inch enough for cut the text in our page size. 
            if (e.Graphics.MeasureString(title, fontTitle).Width >= 280f)//if while ends because of the size, it adds ".." to end
            {
                title += "..";
            }

            return title;
        }
}
}

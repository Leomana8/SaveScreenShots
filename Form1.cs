using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Utilities; // hook

namespace SaveScreenshots
{
    public partial class Form1 : Form
    {
        String pathToFolder;
        Int32 ImageIndex;
        String namefile;
        globalKeyboardHook gkh = new globalKeyboardHook();
        public Form1()
        {
            InitializeComponent();
            pathToFolder = "";
            ImageIndex = 0;
            KeyPreview = true;
            tbFolder.TabStop = false;
            notifyIcon1.Visible = false;
            
            // событие двойного щелчка на трей
            notifyIcon1.MouseDoubleClick += new MouseEventHandler(notifyIcon1_MouseDoubleClick);
            
        }


        private void button1_Click(object sender, EventArgs e)
        {
            namefile = "sss"; // имя по умолчанию
            // добавление заданного имени файла
            // проверка задали ли имя файлу
            if(!String.IsNullOrEmpty(tbNameFile.Text))
            {
                namefile = tbNameFile.Text;
            }

            // делаем нашу иконку в трее активной
            notifyIcon1.Visible = true;
			
            gkh.HookedKeys.Add(Keys.PrintScreen);
            gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
            
            this.Hide();
         
        }
        void gkh_KeyUp(object sender, KeyEventArgs e)
        {
            SaveScreen();
            
        }
        private void SaveScreen()
        {
            var image = Clipboard.GetImage();
            if (image != null)
            {
				// либо в имени дата, либо индекс
                if(cbDate.Checked)
                {
                    image.Save(pathToFolder + @"\" + namefile + "_" + 
                        DateTime.Now.ToString("dd.MM.yy_HH.mm.ss.f") + ".png", ImageFormat.Png);
                }
                else
                {
                    image.Save(pathToFolder + @"\" + namefile + "_" + ++ImageIndex + ".png", ImageFormat.Png);
                }

            }
        }

        private void tbFolder_Click(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // делаем нашу иконку скрытой
            notifyIcon1.Visible = false;
            this.Show();
        }

        private void tbFolder_TextChanged(object sender, EventArgs e)
        {
            // если поле не введено -  кнопку не показывать
            if (tbFolder.Text != "")
                bOk.Enabled = true;
            else
                bOk.Enabled = false;
			
			
            if (!System.IO.Directory.Exists(tbFolder.Text))
            {
				// ошибка в пути директории
                tbFolder.ForeColor = Color.Red;
                bOk.Enabled = false;
            }
            else
            {
                tbFolder.ForeColor = Color.Black;
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tbNameFile_TextChanged(object sender, EventArgs e)
        {
			// проверка на допустимость имени файла
            if (tbNameFile.Text.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
            {		
                tbNameFile.ForeColor = Color.Red;
                bOk.Enabled = false;
            }
            else
            {
                tbNameFile.ForeColor = Color.Black;
            }

        }


        private void bBrowse_Click(object sender, EventArgs e)
        {
            // показать диалог выбора пути папки
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbFolder.Text = folderBrowserDialog1.SelectedPath;
                pathToFolder = folderBrowserDialog1.SelectedPath;
            }
            // если поле пути не введено - кнопку не показывать
            if (tbFolder.Text != "")
                bOk.Enabled = true;
            else
                bOk.Enabled = false;
        }



    }
}
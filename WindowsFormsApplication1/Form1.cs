using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
       
       
        /// <summary>
        /// список найденных файлов
        /// </summary>
        private List<FileInfo> files = new List<FileInfo>();
        int time = 0;
        /// <summary>
        /// Структура данных
        /// </summary>
        Data data;
        TextFind tf;
        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 1000;
           
        }
        static List<FileInfo> statfiles = new List<FileInfo>();

        public List <FileInfo> FindedFiles
        {
            get { return files; }
            set { files = value; }
        }

         
        /// <summary>
        /// вывод диалога выбора рабочей папки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
                data.Workpath = folderBrowserDialog1.SelectedPath;
            }

        }
      
        /// <summary>
        /// Временная проверка ! переделать!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            time = 1;
            data.FileTemplate = textBox2.Text;
            data.TextToFind = textBox3.Text.ToLower();
            data.CheckDir = checkBox1.Checked;
            tf = new TextFind();
          
            if (data.Workpath == "")
            {
                MessageBox.Show("путь к файлу не задан",
                                    "Ошибка",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
            }
            else
            {
                tf.SetData = this.data;
                timer1.Enabled = true;
                timer1.Start();
                tf.BeginFind();
                tf.FileUnderWork += TrackingWorkFiles;
                tf.SearchFinished += Stop;
            }
        }

        /// <summary>
        /// Метод отслеживает и выводит обрабатываемый файл
        /// </summary>
        /// <param name="fi"></param>
       public delegate void trackingWorkFilesCallback(FileInfo fi);
        private void TrackingWorkFiles(FileInfo fi)
        {
            if (InvokeRequired)
            {
                trackingWorkFilesCallback d = new trackingWorkFilesCallback(TrackingWorkFiles);
                this.Invoke(d, new object[] { fi });
                
            }
            else
            {
                textBox5.Text = fi.Name;
                treeView1.Nodes.Add(fi.FullName);
            }
        }
        private void Stop(object sender, EventArgs e)
        {
            timer1.Stop();
        }
        /// <summary>
        /// Остановка поиска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (tf != null)
            {
                tf.IsStop = true;
                timer1.Stop();
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
         
                textBox4.Text = time++.ToString();
                  Update();
        }
       

        /// <summary>
        /// Проверка наличия файла критериев, если нет, то создать файл
        /// Заполнение полей критериями от последнего запуска программы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            data = new Data();
             
           
            if (File.Exists(@"res.txt"))
            {
                string str = File.ReadAllText(@"res.txt");
                string[] substrings = str.Split('~');

                for (int i = 0; i < substrings.Length; i++)
                {
                    switch (i)
                    {
                        case 0: data.Workpath = substrings[0];
                            break;
                        case 1: data.FileTemplate = substrings[1];
                            break;
                        case 2: data.TextToFind = substrings[3].ToLower();
                            break;
                        case 3: { if (substrings[2] == "checked") checkBox1.Checked = true; data.CheckDir = true; }
                            break;
                    }

                }
           
                if (data.Workpath != "")
                    textBox1.Text = data.Workpath;
                if (data.FileTemplate != "")
                    textBox2.Text = data.FileTemplate;
                if (data.TextToFind != "")
                    textBox3.Text = data.TextToFind;
            }
            else
            {
                try
                {
                    using (File.Create(@"res.txt"))
                    { }
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка при создании файла!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        /// <summary>
        /// Сохранение параметров поиска при закрытии программы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
             string dircheck;
            if (data.CheckDir)
                dircheck = "checked";
            else
            dircheck = "unchecked";
            data.FileTemplate = textBox2.Text;
            data.TextToFind = textBox3.Text.ToLower();
            string str = data.Workpath + '~' + data.FileTemplate + '~' + dircheck + '~' + data.TextToFind;
            File.WriteAllText(@"res.txt", str);

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            data.CheckDir = checkBox1.Checked; 
        }

    }
}

    

    
     
     




        


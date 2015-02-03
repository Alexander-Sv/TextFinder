using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class TextFind
    {
        /// <summary>
        /// Путь к директории поиска
        /// </summary>
        string workpath;
        /// <summary>
        /// Текс для поиска
        /// </summary>
        string texttofind;
        /// <summary>
        /// ШАблон для поиска файла
        /// </summary>
        string filetemplate;
        bool check;
        private  System.Threading.Thread searchtread;
        private List<FileInfo> findFiles = new List<FileInfo>();
        bool finding;
        Data data;
        public delegate void WorkFileTransfer(FileInfo fi);
        public event WorkFileTransfer FileUnderWork;
        public event EventHandler SearchFinished;
        public Data SetData
        {
            set
            {
                data = value;
                workpath = data.Workpath;
                texttofind = data.TextToFind;
                filetemplate = data.FileTemplate;
                check = data.CheckDir;
            }
        }
        public bool IsStop
        {
            get { return finding; }
            set { finding = value; }
        }

        private FileInfo workFIle;
        public FileInfo WorkFile
        {
            get { return workFIle; }
        }
        public void BeginFind()
        {
            if (finding)
                return;

            searchtread = new Thread(new ThreadStart(TextAnalysingFunc));
            IsStop = false;
            searchtread.Start();
        }
      

        private void TextAnalysingFunc()
        {
            List<DataStruct> matchdata = new List<DataStruct>();
           
                string[] textexample = texttofind.Split(' ');
                LookingForFiles(workpath, filetemplate, check);
                int workfilescount = findFiles.Count;
                //Проходим по списку найденных файлов и создаем массив строк из файла
                foreach (FileInfo fi in findFiles)
                {
                    if (IsStop)
                    {
                        MessageBox.Show("Поиск прерван", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    
                    workFIle = fi;
                    FileUnderWork(fi);
                    int matches = 0;
                    List<string> wordarray = new List<string>();
                    string textcontainer = File.ReadAllText(fi.FullName).ToLower();
                    string[] strings = textcontainer.Split('\n','\r');


                    for (int j = 0; j < strings.Length; j++)
                    {
                        string[] words = strings[j].Split(' ');


                        for (int i = 0; i < words.Length; i++)
                        {
                            if (words[i].Length >= 4)
                                wordarray.Add(words[i]);
                        }
                    }

                    for (int i = 0; i < textexample.Length; i++)
                    {
                        foreach (string word in wordarray)
                        {
                            
                            if (textexample[i] == word)
                                matches++;
                        }
                    }
                    if (matches != 0)
                        matchdata.Add(new DataStruct(fi, matches));
                    // Задержка для наглядности
                    System.Threading.Thread.Sleep(1500);

                }


                if (matchdata.Count != 0)
                {
                    string exitstr = string.Format("Совпадения найдены в: \n");
                    foreach (DataStruct d in matchdata)
                    {
                        exitstr += d.fileInfo.Name.ToString() + "  " + d.matches.ToString() + "\n";
                    }
                    MessageBox.Show(exitstr);
                    SearchFinished(this, null);
                }
                   
                else
                { MessageBox.Show(" Совпадений нет"); SearchFinished(this,null); }

               
           
        }


         private void LookingForFiles(string path, string filename, bool check)
        {
            DirectoryInfo di = null;
          
               di  = new DirectoryInfo(path);
            
            
            if (check)
            {
                
                foreach (FileInfo f in di.GetFiles(filename))
                {
                    findFiles.Add(f);
                }
            }
            else
            {
                RecursiveWalkDirs(di);
            }
            
        }
        /// <summary>
        /// Рекурсивный проход по всем подкаталогам 
        /// и поиск файлов по заданным критериям
        /// </summary>
        /// <param name="dir"></param>
        private void RecursiveWalkDirs(DirectoryInfo dir)
        {
            FileInfo[] file = null;
            DirectoryInfo[] dirs;

            try
            {
                file = dir.GetFiles(filetemplate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            if (file != null)
            {
                foreach (FileInfo f in file)
                {
                    findFiles.Add(f);
                }
                dirs = dir.GetDirectories();
                foreach (DirectoryInfo dirInfo in dirs)
                {
                    RecursiveWalkDirs(dirInfo);
                }

            }
        }

        /// <summary>
        /// структура данных для найденных файлов
        /// </summary>
        public struct DataStruct
        {
            FileInfo _fi;
            int _match;
            public DataStruct(FileInfo file, int matches)
            {
                _fi = file;
                _match = matches;
            }
            public FileInfo fileInfo { get { return _fi; } }
            public int matches { get { return _match; } }
        }

    }
    }

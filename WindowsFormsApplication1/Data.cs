using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{

    /// <summary>
    /// Класс контейнер для информации о пути для поиска,
    /// имени искомого файла и слове для поиска
    /// </summary>
    class Data
    {
        string _path;
        string _template;
        string _tofind;
        bool _check;
        /*public Data (string workpath, string filetemplate, string texttofind, bool check)
        {
            _path = workpath;
            _template = filetemplate;
            _tofind = texttofind;
            _check = check;
        }*/
        public string Workpath
        {
            get { return _path; }
            set { _path = value; }
        }
        public string FileTemplate
        {
            get { return _template; }
            set { _template = value; }
        }
        public string TextToFind
        {
            get { return _tofind; }
            set { _tofind = value; }
        }
        public bool CheckDir
        {
            get { return _check; }
            set { _check = value; }
        }

    }
}

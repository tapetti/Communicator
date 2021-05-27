using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Communicator
{
    public class DataGeneralSettings
    {
        private Color? _Terminal_Color_PC;
        public Color? Terminal_Color_PC
        {
            get { return _Terminal_Color_PC; }
            set { _Terminal_Color_PC = value; }
        }

        private Color? _Terminal_Color_Rec;
        public Color? Terminal_Color_Rec
        {
            get { return _Terminal_Color_Rec; }
            set { _Terminal_Color_Rec = value; }
        }

        private string _themeMode;
        public string themeMode
        {
            get { return _themeMode; }
            set { _themeMode = value; }
        }

        private string _themeColor;
        public string themeColor
        {
            get { return _themeColor; }
            set { _themeColor = value; }
        }

        private float _terminalFontSize;
        public float terminalFontSize
        {
            get { return _terminalFontSize; }
            set { _terminalFontSize = value; }
        }

        private bool _terminalFontBold;
        public bool terminalFontBold
        {
            get { return _terminalFontBold; }
            set { _terminalFontBold = value; }
        }
    }
}

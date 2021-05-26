using ControlzEx.Theming;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Communicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Settings : MetroWindow
    {
        public MainWindow mainWindow;

        public Settings(MainWindow _mainWindow)
        {
            InitializeComponent();
            //ThemeManager.Current.ChangeTheme(this, "Dark.Blue");
            this.mainWindow = _mainWindow;

            DataContext = new
            {
                themeModes = FetchThemeModes(),
                themeColors = FetchThemeColors()
            };
            
            themeMode.Text = mainWindow.themeMode;
            themeColor.Text = mainWindow.themeColor;

            terminalColorPC.SelectedColor = mainWindow.Terminal_Color_PC;
            terminalColorRec.SelectedColor = mainWindow.Terminal_Color_Rec;


            ThemeManager.Current.ChangeTheme(this, themeMode.Text + "." + themeColor.Text);
        }        

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private object FetchThemeModes()
        {
            return new string[] { "Light", "Dark" };
        }
        private object FetchThemeColors()
        {
            return new string[] { "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan", "Cobalt", "Indigo", "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna" };
        }

        private void themeMode_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            mainWindow.themeMode = themeMode.Text;
            mainWindow.themeColor = themeColor.Text;
            ThemeManager.Current.ChangeTheme(this, themeMode.Text + "." + themeColor.Text);
        }

        private void themeMode_DropDownClosed(object sender, EventArgs e)
        {
            mainWindow.themeMode = themeMode.Text;
            mainWindow.themeColor = themeColor.Text;
            ThemeManager.Current.ChangeTheme(this, themeMode.Text + "." + themeColor.Text);
            ThemeManager.Current.ChangeTheme(mainWindow, themeMode.Text + "." + themeColor.Text);
        }

        private void themeColor_DropDownClosed(object sender, EventArgs e)
        {
            mainWindow.themeMode = themeMode.Text;
            mainWindow.themeColor = themeColor.Text;
            ThemeManager.Current.ChangeTheme(this, themeMode.Text + "." + themeColor.Text);
            ThemeManager.Current.ChangeTheme(mainWindow, themeMode.Text + "." + themeColor.Text);
        }

        private void terminalColorPC_DropDownClosed(object sender, EventArgs e)
        {
            ColorPicker temp = (ColorPicker)sender;
            mainWindow.Terminal_Color_PC = temp.SelectedColor;
        }

        private void terminalColorRec_DropDownClosed(object sender, EventArgs e)
        {
            ColorPicker temp = (ColorPicker)sender;
            mainWindow.Terminal_Color_Rec = temp.SelectedColor;
        }

        private void btnFont_Click(object sender, RoutedEventArgs e)
        {
            var fontDialog = new FontDialog();

            var result = fontDialog.ShowDialog();

            if(System.Windows.Forms.DialogResult.OK == result)
            {
                System.Drawing.Font selectedFont = fontDialog.Font;
                
                mainWindow.Terminal.FontSize = selectedFont.Size;
                mainWindow.terminalFontSize = selectedFont.Size;
            }

        }
    }
}

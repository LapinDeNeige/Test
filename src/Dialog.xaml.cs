using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Text.RegularExpressions;
namespace Test_1
{
    /// <summary>
    /// Логика взаимодействия для Dialog.xaml
    /// </summary>
    public partial class Dialog : Window
    {
        DBClient dbClient;
        XMLReader xmlReader;

        string name;
        string surname;
        string fathersname;
        string phone;

        string selectedId;

        bool isDatabaseChecked = true;
        bool isAddFunction = true;
        public Dialog()
        {
            InitializeComponent();
            dbClient = new DBClient();
            xmlReader = new XMLReader();
        }
        public void DialogClose(object sender, EventArgs e)
        {
            if (this.IsActive)
                this.Visibility = Visibility.Hidden;
        }
        public void DialogClose()
        {
            if (this.IsActive)
                this.Visibility = Visibility.Hidden;
        }

        public void initVerification(object sender,EventArgs e)
        {
            bool allFieldsTrue=true;
            if (!verifyName())
            {
                nameFailed(dlg_name, "invalid name");
                allFieldsTrue = false;
            }
            name = dlg_name.Text;
            if (!verifySurname())
            {
                nameFailed(dlg_srnm, "Invalid surname");
                allFieldsTrue = false;
            }
            surname = dlg_srnm.Text;
            if (!verifyFathersName())
            {
                nameFailed(dlg_fthrnm, "Invalid Fathers Name");
                allFieldsTrue = false;
            }
            fathersname = dlg_fthrnm.Text;
            if (!verifyNumber())
            {
                nameFailed(dlg_number, "Invalid number");
                allFieldsTrue = false;
            }
            phone = dlg_number.Text;

            if(allFieldsTrue)
            {
                if (isDatabaseChecked)
                {
                    dbClient.updateClient(name, surname, fathersname, phone);
                    if (isAddFunction)
                    {
                        if (!addToDatabase())
                            MessageBox.Show("Added to Database failed");
                        else
                            DialogClose();
                    }
                    //else
                      //EditDatabase
                }

                else
                {
                    if (isAddFunction)
                    {
                        if (!addToXml())
                            MessageBox.Show("Added to XML  failed");
                        else
                            DialogClose();
                    }
                    else
                    {
                        xmlReader.editById( name, surname,fathersname, phone, selectedId);
                        DialogClose();
                    }
                }
            }
        }
       
        public void switchCheck(bool inState)
        {
            isDatabaseChecked = inState;
         }
        
        public void updateSelectedId(string id)
        {
            selectedId = id;
        }
        public void switchFunction(bool inState)
        {
            isAddFunction = inState;
        }
        private bool addToXml()
        {
            if (!xmlReader.addToXML(name, surname, fathersname, phone)) 
                return false;
            return true;
        }
        private bool addToDatabase()
        {
            if (!dbClient.sqlAdd())
                return false;
            return true;

        }
        private bool verifyStr(string nm)
        {

            if (nm.Length < 2 || nm.Length > 50 || nm==string.Empty)
                return false;
            return true;
        }
        private bool verifyName()
        {
            if (dlg_name.Foreground == Brushes.Red)
                return false;
            string str = dlg_name.Text;
            return verifyStr(str);
        }
        private bool verifySurname()
        {
            if (dlg_srnm.Foreground == Brushes.Red)
                return false;
            string srnm = dlg_srnm.Text;
            return verifyStr(srnm);
        }
        private bool verifyFathersName()
        {
            if (dlg_fthrnm.Foreground == Brushes.Red)
                return false;
            string fth = dlg_fthrnm.Text;
            return verifyStr(fth);
        }
        private void clearName(object sender,EventArgs e)
        { 
                clearBox(dlg_name);
        }

        private void clearSurame(object sender, EventArgs e)
        {
            clearBox(dlg_srnm);
        }
        private void clearFathersName(object sender, EventArgs e)
        {
            clearBox(dlg_fthrnm);
        }
        private void clearPhone(object sender,EventArgs e)
        {
            clearBox(dlg_number);
        }
        private void clearBox(TextBox boxInput)
        {
            if (boxInput.Foreground == Brushes.Red &&  boxInput.BorderBrush == Brushes.Red)
            {
                boxInput.Text = String.Empty;
                boxInput.Background = Brushes.White;
                boxInput.BorderBrush = Brushes.Black;
                boxInput.Foreground = Brushes.Black;
            }
        }
        private bool verifyNumber()
        {
            if (dlg_number.Foreground == Brushes.Red)
                return false;

            string nm = dlg_number.Text;
            if (nm.Length !=12)
                return false;
            if (nm[0] != '+' || nm[1] != '7')
                return false;
            return true;
        }
        private void numberHandle(object sender, TextCompositionEventArgs e)
        {
            Regex rg = new Regex("^[a-zA-Z]+");
            e.Handled = rg.IsMatch(e.Text);
        }
        private void nameFailed(TextBox boxName,string inputText)
        {
            boxName.BorderBrush = Brushes.Red;
            //boxName.Background = Brushes.Pink;
            boxName.Foreground = Brushes.Red;

            boxName.Text = inputText;
        }
        
        
        
    }
}

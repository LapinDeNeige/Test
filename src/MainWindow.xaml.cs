using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace Test_1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DBClient dbClient;  //Data Base Class
        private Dialog dd; //dialog window
        private List<dbContacts> dbContacts; //serialised class of contacts
        private XMLReader xmlReader;
        public bool isDatabaseChecked=false;
      
         
        public MainWindow()
        {
            dd = new Dialog();
            dbClient = new DBClient();
            dbContacts = new List<dbContacts>();
            xmlReader = new XMLReader();

            InitializeComponent();
            initProgramm();
        }

        private void initProgrammOnClick(object sender,EventArgs e)
        {
            initProgramm();
        }
        private void initProgramm()
        {
            if (radio_db.IsChecked.Value.Equals(true)) //if radio button database
            {

                dd.switchCheck(true); //radiobutton database
                getFromDataBase(); //begin loading to database
            }
            else //if xml radiobutton checked
            { //read from xml
                dd.switchCheck(false); //radiobutton xml
                getFromXml();
            }
            
                
        }
        private void showDialogWindow()
        {
            if (!dd.IsActive)
                dd.ShowDialog();
            //getFromDataBase();
            //uiClass.ShowDialog(sender,e);
        }

        private void getFromDataBase()
        {
            bool st = dbClient.readSql();
            //Thread.Sleep(5000);
            clearList();
            if (!st)
            {
                btn_one.IsEnabled = false;
                btn_two.IsEnabled = false;
                btn_three.IsEnabled = false;
                databaseError();

            }
            else
            {
                btn_one.IsEnabled = true;
                btn_two.IsEnabled = true;
                btn_three.IsEnabled = true;
                dbContacts = dbClient.dbContacts;
                displayOnWindow(); //display database content on window 
            }
        }
        private void getFromXml()
        {

            if (!xmlReader.readFromXml())
            {
                btn_one.IsEnabled = false;
                btn_two.IsEnabled = false;
                btn_three.IsEnabled = false;
                xmlError();
            }
            else
            {
                btn_one.IsEnabled = true;
                btn_two.IsEnabled = true;
                btn_three.IsEnabled = true;
                
                dbContacts = xmlReader.xmlContacts;
                displayOnWindow();
            }
        }

        private void displayOnWindow()
        {
           
            clearList();
            //List database content to Window
            for (int i = 0; i < dbContacts.Count; i++)
            {
                string nm = dbContacts[i].Name;
                string srnm = dbContacts[i].Surname;
                string ftrnm = dbContacts[i].Fathersname;
                string ph = dbContacts[i].Phone;
                string id = dbContacts[i].Id;
                addNamesToList(nm, srnm, ftrnm, ph, id);
            }
        }

        private void addContactOnClick(object sender,EventArgs e)
        {
            dd.switchFunction(true);
            showDialogWindow();
            initProgramm();
        }
        private void editContactOnClick(object sender,EventArgs e)
        {
            string selectedId;
            if ((selectedId = getSelectedId()) == null)
            {
                MessageBox.Show("Please select id");
                
            }
            else
            {
                dd.switchFunction(false);
                dd.updateSelectedId(selectedId);
                showDialogWindow();
            }
            initProgramm();
        }

        private void deleteContactOnClick(object sender,EventArgs e)
        {
            string selectedId;
            if ((selectedId = getSelectedId()) == null)
            {
                MessageBox.Show("Please select id");
            }
            else
                deleteFromXml(selectedId);

            initProgramm();
        }
        private void deleteFromDatabase(string id)
        {
            dbClient.sqlDelete(id);
        }
       private void deleteFromXml(string id)
        {
            xmlReader.deleteById(id);
        }
        private void clearList()
        {
            lst_name.Items.Clear();
            lst_surname.Items.Clear();
            lst_fathersname.Items.Clear();
            lst_number.Items.Clear();
            lst_id.Items.Clear();
        }
        
        private void addNamesToList(string name,string surname,string fathersname,string phone,string id)
        {
            addToList(lst_name, name);
            addToList(lst_surname, surname);
            addToList(lst_fathersname, fathersname);
            addToList(lst_number, phone);
            addToList(lst_id, id);
        }

        private void addToList(ListBox lstBox,string val)
        {
            lstBox.Items.Add(val);
        }

        private string getSelectedId() //delete selected contact by selected id column
        {
            string selectedID = null;
            if (lst_id.SelectedItems.Count == 0) //if no id listbox are selected 
                selectedID=null;
            else
            {
                for (int i = 0; i < lst_id.Items.Count; i++)
                {
                    ListBoxItem lb = (ListBoxItem)lst_id.ItemContainerGenerator.ContainerFromIndex(i);
                    if (lb.IsSelected)
                        selectedID = lb.Content.ToString();
                }
            }
            return selectedID;
        }

        private void xmlError()
        {
            dbContacts.Clear();
            MessageBox.Show("Не удалось открыть xml файл\n" +
                "Возможно файл не найден\n" +
                "Либо путь к нему некорректен"
                );
        }
        private void databaseError()
        {
            dbContacts.Clear();
            MessageBox.Show("Не удалось загрузить базу данных\n" +
                "Для этого нужно подключиться к локальной базе данных MS Sql Express\n" +
                "Строку подключения можно изменить по своему умолчанию\n" +
                "Она находится в классе DBClient");
        }

      
    }
}

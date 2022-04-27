using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Test_1
{
    class XMLReader
    {
        public List<dbContacts> xmlContacts;
        private XmlDocument dc;

        public XMLReader()
        {
            dc = new XmlDocument();
            xmlContacts = new List<dbContacts>();
        }
        private bool openDoc() //open file document
        {
            try
            {
                dc.Load("contacts.xml");
            }
            catch
            {
                return false;
            }
            return true;
        }

        private int getNodesCount() //get count of child nodes to get current id
        {
            int cnt = dc.GetElementsByTagName("user").Count;
            //int cnt=dc.SelectNodes("users").Count;
            return cnt+1;
        }
        public bool addToXML(string name, string surname, string fathersname, string phone) //add new node to xml file 
        {
            if (!openDoc())
                return false;

            XmlElement root = dc.DocumentElement;
            XmlElement userElement = dc.CreateElement("user");
            XmlElement nameElement = dc.CreateElement("name");
            XmlElement surnameElement = dc.CreateElement("surname");
            XmlElement fathersnameElement = dc.CreateElement("fathersname");
            XmlElement phoneElement = dc.CreateElement("phone");
            XmlElement idElement = dc.CreateElement("id");

            XmlText nmNode = dc.CreateTextNode(name);
            XmlText srnmNode = dc.CreateTextNode(surname);
            XmlText fthrnNode = dc.CreateTextNode(fathersname);
            XmlText phNode = dc.CreateTextNode(phone);
            XmlText idNode = dc.CreateTextNode(getNodesCount().ToString());

            nameElement.AppendChild(nmNode);
            surnameElement.AppendChild(srnmNode);
            fathersnameElement.AppendChild(fthrnNode);
            phoneElement.AppendChild(phNode);
            idElement.AppendChild(idNode);

            userElement.AppendChild(nameElement);
            userElement.AppendChild(surnameElement);
            userElement.AppendChild(fathersnameElement);
            userElement.AppendChild(phoneElement);
            userElement.AppendChild(idElement);

            root.AppendChild(userElement);

            saveDoc();
            return true;
        }
        public bool readFromXml() //read from file and save in serialisale list
        {
            if (!openDoc())
                return false;
            string name = "";
            string surname = "";
            string phone = "";
            string fathersname = "";
            string id = "";

            xmlContacts.Clear();
            XmlNodeList lst = dc.GetElementsByTagName("user");
            foreach (XmlNode nd in lst)
            {
                foreach (XmlNode node in nd.ChildNodes)
                {
                    string nn = node.Name;
                    if (nn.Equals("name"))
                    {
                        name = node.InnerText;
                    }
                    if (nn.Equals("surname"))
                    {
                        surname = node.InnerText;
                    }
                    if (nn.Equals("fathersname"))
                    {
                        fathersname = node.InnerText;
                    }
                    if (nn.Equals("phone"))
                    {
                        phone = node.InnerText;
                    }
                    if (nn.Equals("id"))
                    {
                        id = node.InnerText;
                        xmlContacts.Add(new dbContacts() { Name = name, Surname = surname, Fathersname = fathersname, Phone = phone, Id = id });
                    }
                }
            }
            return true;
        }

        private XmlNode getNodeById(string id) //get parent node of id
        {
            XmlNodeList nodeList = dc.GetElementsByTagName("id");
            XmlNode rtNode = null;
            foreach (XmlNode nd in nodeList)
            {
                if (nd.InnerText == id)
                {
                    rtNode = nd;
                   

                    break;
                }
            }
            return rtNode;
        }
        public bool deleteById(string id) //get parent node of id and delete all it's parents instances
        {
            XmlNode node;
            if (!openDoc())
                return false;
            if ((node = getNodeById(id)) == null)
                return false;
            XmlNode parent = node.ParentNode;
            parent.RemoveAll();
            dc.DocumentElement.RemoveChild(parent);

            saveDoc();
            return true;

        }

        public bool editById(string newName, string newSrnm, string newFthrn, string newPhone, string id)
        {                             //get parent node by id and edit all its children
            XmlNode node;
            if (!openDoc())
                return false;
            if ((node = getNodeById(id)) == null)
                return false;
            XmlNode parent = node.ParentNode;
            foreach (XmlNode nd in parent.ChildNodes)
            {
                if (nd.Name == "name")
                    nd.InnerText = newName;
                else if (nd.Name == "surname")
                    nd.InnerText = newSrnm;
                else if (nd.Name == "fathersname")
                    nd.InnerText = newFthrn;
                else if (nd.Name == "phone")
                    nd.InnerText = newPhone;

            }
            saveDoc();
            return true;
        }
        private void saveDoc() //save documents
        {
            dc.Save("contacts.xml");
        }
       
    }

}


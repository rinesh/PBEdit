using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PBEdit
{
    public partial class ArrayObjectEditor : Form
    {
        private string m_class;
        private string m_type;
        private string[] classList;
        private int windowNo;
        private static int windowLevel = 0;
        private static string currentArray = "";
        private static string currentindex = "";
        private static string[] index = new string[10];
        private static string[] xmlString = new string[10];

        public static string componentName;
        public static string fieldName;

        private string EntityString;
        private XElement arrayXML;

        public ArrayObjectEditor(string type)
        {
            InitializeComponent();

            m_class = type.Substring(0,type.IndexOf("("));
            m_type = type.Substring(type.IndexOf("(")+1, type.Length - type.IndexOf("(")-2);
            classList = new string[50];
            int n = ClassSchemaXML.GetClassesOfType(m_class, classList);
            for (int i = 0; i < n; i++)
            {
                cmbTypes.Items.Add(classList[i]);
            }
            EntityString = EntityXML.m_currentEntity.ToString();
            EntityString = EntityString.Replace("\\n", "");
            EntityString = EntityString.Replace("\n", "");
            EntityString = EntityString.Replace("\r", "");

            if (currentArray != "")
            {
                index[windowLevel] = currentindex;
                xmlString[windowLevel] = currentArray;
                windowNo = windowLevel++;
            }
            AddArrayElement();
        }

        public PropertyGridEx.PropertyGridEx GetPropertyGridEx()
        {
            return Properties;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbTypes.SelectedItem == null)
                return;

            string element = "";
            if (m_type == "Dictionary" && txtName.Text.Trim()!="")
            {
                element = txtName.Text.Trim();
                lstItems.Items.Add(element);
            }
            else  if(m_type == "Array")
            {
                element = "_" + lstItems.Items.Count.ToString();
                lstItems.Items.Add(element);
            }
            else
                return;

            XElement xml = new XElement("thing");
            if (windowLevel <= 0)
            {
                xml = new XElement("thing",
                                            new XElement("component", new XAttribute("name", componentName),
                                                new XElement(fieldName,
                                                             new XElement(element, new XAttribute("type", cmbTypes.SelectedItem.ToString()))
                                                            )
                                                        )
                                            );
            }
            else
            {
                string strXML = "<thing> <component name =\"" + componentName + "\"> <" + fieldName + ">";
                for (int i = windowLevel - 1; i >= 0; i--)
                {
                    if (i == windowLevel - 1)
                    {
                        if (index[i] != "")
                            strXML += "<" + index[i] + ">";

                        strXML += "<" + xmlString[i] + ">";
                        strXML += "<" + element + " type=\"" + cmbTypes.SelectedItem.ToString() + "\"></" + element + ">";
                        strXML += "</" + xmlString[i] + ">";

                        if (index[i] != "")
                            strXML += "</" + index[i] + ">";
                    }
                    else
                    {
                        strXML = strXML.Insert(0, "<" + xmlString[i] + ">");
                        strXML = strXML.Insert(strXML.Length - 1, "</" + xmlString[i] + ">");

                        if (index[i] != "")
                        {
                            strXML = strXML.Insert(0, "<" + index[i] + ">");
                            strXML = strXML.Insert(strXML.Length - 1, "</" + index[i] + ">");
                        }
                    }
                }
                strXML += "</" + fieldName + "></component></thing>";
                xml = XElement.Parse(strXML);
            }

            EntityString = (string)MainForm.proxy.Call("newComponent", xml.ToString());
            EntityString = EntityString.Replace("\\n", "");
            xml = ModifyComponentWithDefaultValues(element);

            if (windowLevel <= 0)
            {
                xml = new XElement("thing",
                                            new XElement("component", new XAttribute("name", componentName),
                                                new XElement(fieldName,
                                                             new XElement(element, xml.Elements())
                                                            )
                                                        )
                                            );
            }
            else
            {
                String objectstr = "";
                foreach (XElement Xnode in xml.Elements())
                {
                    objectstr += Xnode.ToString();
                }
                string strXML = "<thing> <component name =\"" + componentName + "\"> <" + fieldName + ">";
                for (int i = windowLevel - 1; i >= 0; i--)
                {
                    if (i == windowLevel - 1)
                    {
                        if (index[i] != "")
                            strXML += "<" + index[i] + ">";

                        strXML += "<" + xmlString[i] + ">";
                        strXML += "<" + element + ">" + objectstr + "</" + element + ">";
                        strXML += "</" + xmlString[i] + ">";

                        if (index[i] != "")
                            strXML += "</" + index[i] + ">";
                    }
                    else
                    {
                        strXML = strXML.Insert(0, "<" + xmlString[i] + ">");
                        strXML = strXML.Insert(strXML.Length - 1, "</" + xmlString[i] + ">");

                        if (index[i] != "")
                        {
                            strXML = strXML.Insert(0, "<" + index[i] + ">");
                            strXML = strXML.Insert(strXML.Length - 1, "</" + index[i] + ">");
                        }
                    }
                }
                strXML += "</" + fieldName + "></component></thing>";
                xml = XElement.Parse(strXML);
            }

            EntityString = (string)MainForm.proxy.Call("newComponent", xml.ToString());
            EntityString = EntityString.Replace("\\n", "");
            EntityXML.m_currentEntity = XElement.Parse(EntityString);
            lstItems.Items.Clear();
            AddArrayElement();
            //lstItems.Items.Add(cmbTypes.SelectedItem.ToString().Replace("::", "."));
        }

        private void lstItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstItems.SelectedItem != null)
            {
                AddVariables();
            }
        }

        private void AddVariables()
        {
            Properties.Item.Clear();
            string childType = null;

            if (arrayXML.HasAttributes)
                childType = arrayXML.Attribute("childType").Value;

            var elements = from element in arrayXML.Elements()//"_"
                           select new
                           {
                               childType = element.HasAttributes ? element.Attribute("type").Value : childType,
                               def = element,
                           };
            int count = -1;
            foreach (var element in elements)
            {
                count++;
                if (count != lstItems.SelectedIndex)
                    continue;
                if (element.def.HasAttributes)
                    childType = element.def.Attribute("type").Value;


                PBEdit.Property[] property = new Property[50];
                int propertyCount = ClassSchemaXML.GetProperties(childType, property);

                for (int i = 0; i < propertyCount; ++i)
                {
                    var propertyXMLs = element.def.Descendants(property[i].name);

                    foreach (var propertyXML in propertyXMLs)
                    {
                        if (propertyXML.HasAttributes)
                        {
                            property[i].TypeHint = propertyXML.Attribute("type").Value;
                        }

                        property[i].defaultValue = propertyXML.Value;

                        if (property[i].type == "Point")
                        {
                            string coord = propertyXML.Element("x").Value;
                            property[i].defaultValue = propertyXML.Element("x").Value + "|" + propertyXML.Element("y").Value;
                        }
                    }

                    if (property[i].type == "Number")
                    {
                        Properties.Item.Add(property[i].name, float.Parse(property[i].defaultValue), false, componentName, "", true);
                    }

                    if (property[i].type == "int")
                    {
                        Properties.Item.Add(property[i].name, int.Parse(property[i].defaultValue), false, componentName, "", true);
                    }

                    if (property[i].type == "Boolean")
                    {
                        Properties.Item.Add(property[i].name, Boolean.Parse(property[i].defaultValue), false, componentName, "", true);
                    }

                    if (property[i].type == "Point")
                    {
                        string[] point = property[i].defaultValue.Split(new char[] { '|' });
                        Point2D p = new Point2D(float.Parse(point[0]), float.Parse(point[1]));
                        Properties.Item.Add(property[i].name, p, false, componentName, "", true);
                        Properties.Item[Properties.Item.Count - 1].IsBrowsable = true;
                        Properties.Item[Properties.Item.Count - 1].BrowsableLabelStyle = PropertyGridEx.BrowsableTypeConverter.LabelStyle.lsEllipsis;
                    }
                    if (property[i].type == "ObjectType")
                    {
                        Properties.Item.Add(property[i].name, "", false, componentName, "", true);
                    }

                    if (property[i].type == "Array")
                    {
                        Properties.Item.Add(property[i].name, property[i].TypeHint, false, componentName, "", true);
                        Properties.Item[Properties.Item.Count - 1].CustomEditor = new ArrayEditorUI();

                    }
                    if (property[i].type == "PropertyReference")
                    {
                        Properties.Item.Add(property[i].name, property[i].defaultValue, false, componentName, "", true);
                        //Properties.Item[Properties.Item.Count - 1].Value = arg.value;
                        //Properties.Item[Properties.Item.Count - 1].CustomEditor = new MyEditor();// new MyEditor(arg.value);
                    }
                    if (property[i].type == "componentReference")
                    {
                        Properties.Item.Add(property[i].name, property[i].defaultValue, false, componentName, "", true);
                        //Properties.Item[Properties.Item.Count - 1].Value = arg.value;
                        //Properties.Item[Properties.Item.Count - 1].CustomEditor = new MyEditor();// new MyEditor(arg.value);
                    }
                    if (property[i].type == "Object")
                    {
                        Properties.Item.Add(property[i].name, property[i].TypeHint , false, componentName, "", true);
                        //Properties.Item[Properties.Item.Count - 1].Value = arg.value;
                        Properties.Item[Properties.Item.Count - 1].CustomEditor = new ObjectEditorUI();

                    }
                }
            }
            Properties.Refresh();
        }

        private void Properties_PropertyValueChanged(System.Object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
        {
            Console.WriteLine(e.ToString());
            XElement xml = new XElement("thing");
            if (windowLevel <= 0)
            {
                xml = new XElement("thing",
                                            new XElement("component", new XAttribute("name", componentName),
                                                new XElement(fieldName,
                                                             new XElement("" + lstItems.SelectedItem.ToString(), 
                                                                 new XElement(e.ChangedItem.Label,e.ChangedItem.Value) 
                                                              )
                                                          )
                                                       )
                                            );
            }
            else
            {
                string strXML = "<thing> <component name =\"" + componentName + "\"> <" + fieldName + ">";
                for (int i = windowLevel - 1; i >= 0; i--)
                {
                    if (i == windowLevel - 1)
                    {
                        if (index[i] != "")
                            strXML += "<" + index[i] + ">";

                        strXML += "<" + xmlString[i] + ">";
                        strXML += "<" + lstItems.SelectedItem.ToString() + "><" + e.ChangedItem.Label + ">" + e.ChangedItem.Value + "</" + e.ChangedItem.Label + ">" + "</" + lstItems.SelectedItem.ToString() + ">";
                        strXML += "</" + xmlString[i] + ">";

                        if (index[i] != "")
                            strXML += "</" + index[i] + ">";
                    }
                    else
                    {
                        strXML = strXML.Insert(0, "<" + xmlString[i] + ">");
                        strXML = strXML.Insert(strXML.Length - 1, "</" + xmlString[i] + ">");

                        if (index[i] != "")
                        {
                            strXML = strXML.Insert(0, "<" + index[i] + ">");
                            strXML = strXML.Insert(strXML.Length - 1, "</" + index[i] + ">");
                        }
                    }
                }
                strXML += "</" + fieldName + "></component></thing>";
                xml = XElement.Parse(strXML);
            }
            EntityString = (string)MainForm.proxy.Call("newComponent", xml.ToString());
            EntityString = EntityString.Replace("\\n", "");
            EntityXML.m_currentEntity = XElement.Parse(EntityString);
            //lstItems.Items.Clear();
            //AddArrayElement();
        }

        private void ArrayObjectEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (windowLevel > 0)
                windowLevel--;
            else
                currentArray = "";
        }

        private void ArrayObjectEditor_Load(object sender, EventArgs e)
        {

        }

        private XElement ModifyComponentWithDefaultValues(string element)
        {
            XElement objectXML = new XElement("_"); 

            //int localIndex = lstItems.Items.Count;
            string type = null;

            objectXML = GetArray();

            var lastfields = from field in objectXML.Elements()//"_"
                             select new
                             {
                                 name = field.Name,
                                 childType = field.HasAttributes ? field.Attribute("type").Value : type,
                                 def = field,
                             };

            int count = 0;
            foreach (var field in lastfields)
            {
                //if (count != localIndex)
                if (field.name != element)
                {
                    count++;
                    continue;
                }

                objectXML = field.def;
                type = objectXML.HasAttributes ? objectXML.Attribute("type").Value : type;
                break;
            }

            PBEdit.Property[] property = new Property[50];
            int propertyCount = 0;

            propertyCount = ClassSchemaXML.GetProperties(type, property);

            for (int i = 0; i < propertyCount; ++i)
            {
                if (property[i].hasDeafultValue)
                {
                    foreach (XElement Xnode in objectXML.Descendants())
                    {
                        if (Xnode.Name == property[i].name)
                        {
                            Xnode.Remove();
                            break;
                        }
                    }

                    if (property[i].type == "int" || property[i].type == "Number")
                    {
                        objectXML.Add(new XElement(property[i].name, property[i].defaultValue));
                    }

                    if (property[i].type == "Boolean")
                    {
                        objectXML.Add(new XElement(property[i].name, property[i].defaultValue));
                    }

                    if (property[i].type == "String")
                    {
                        objectXML.Add(new XElement(property[i].name, property[i].defaultValue));
                    }

                    if (property[i].type == "Point")
                    {
                        string[] point = property[i].defaultValue.Split(new char[] { '|' });
                        objectXML.Add(new XElement(property[i].name, new XElement("x", point[0]),
                                                      new XElement("y", point[1])
                                              ));
                    }

                    if (property[i].type == "ObjectType")
                    {
                        objectXML.Add(new XElement(property[i].name, property[i].defaultValue));
                    }

                    if (property[i].type == "Array")
                    {
                        objectXML.Add(new XElement(property[i].name, property[i].defaultValue));
                    }
                    if (property[i].type == "PropertyReference")
                    {
                        objectXML.Add(new XElement(property[i].name, property[i].defaultValue));
                    }
                    if (property[i].type == "componentReference")
                    {
                        objectXML.Add(new XElement(property[i].name, property[i].defaultValue));
                    }
                    if (property[i].type == "Object")
                    {
                        objectXML.Add(new XElement(property[i].name, property[i].defaultValue));
                    }
                }
            }
            return objectXML;
        }

        private void AddArrayElement()
        {
            arrayXML = GetArray();
            if (arrayXML == null)
                return;

            string childType = null;

            if (arrayXML.HasAttributes)
                childType = arrayXML.Attribute("childType").Value;

            var elements = from element in arrayXML.Elements()//"_"
                           select new
                           {
                               name = element.Name,
                               childType = element.HasAttributes ? element.Attribute("type").Value : childType,
                               def = element,
                           };
            int cnt = 0;
            foreach (var element in elements)
            {
                if(element.name == "_")
                    lstItems.Items.Add("_"+cnt++);
                else
                lstItems.Items.Add(element.name);//childType);
            }

        }

        private XElement GetArray()
        {
            XElement EntityXML = XElement.Parse(EntityString);
            XElement objectXML = new XElement("_");

            var components = from component in EntityXML.Descendants("component")
                             where component.Attribute("name").Value.ToString() == componentName
                             select new
                             {
                                 name = component.Attribute("name").Value,
                                 type = component.Attribute("type").Value,
                                 def = component,
                             };

            //int localIndex = lstItems.SelectedIndex;
            string type = null;

            foreach (var component in components)
            {
                var variables = from variable in component.def.Descendants(fieldName)
                                select new
                                {
                                    name = variable.Name,
                                    childType = variable.HasAttributes ? variable.Attribute("childType").Value : null,
                                    def = variable,
                                };

                foreach (var variable in variables)
                {
                    objectXML = variable.def;

                    for (int i = 0; i < windowLevel; i++)
                    {
                        if (index[i] != "")
                        {
                            var fields = from field in objectXML.Elements()//"_"
                                         select new
                                         {
                                             name = field.Name,
                                             childType = field.HasAttributes ? field.Attribute("type").Value : variable.childType,
                                             def = field,
                                         };
                            int cnt = 0;
                            foreach (var field in fields)
                            {
                                string name = field.name.ToString();
                                if (name =="_")
                                {
                                    name += cnt.ToString();
                                }
                                if (name != index[i])
                                {
                                    cnt++;
                                    continue;
                                }
                                
                                objectXML = field.def;
                                type = field.childType;
                                break;
                            }

                            objectXML = objectXML.Element(xmlString[i]);
                            if (objectXML!=null)
                            {
                                if (objectXML.Attribute("childType") != null)
                                    type = objectXML.HasAttributes ? objectXML.Attribute("childType").Value : type;
                                if (objectXML.Attribute("type") != null)
                                    type = objectXML.HasAttributes ? objectXML.Attribute("type").Value : type;
                            }
                        }
                        else
                        {
                            objectXML = objectXML.Element(xmlString[i]);
                        }
                    }
                }
            }
            return objectXML;
        }

        private void Properties_SelectedGridItemChanged(System.Object s, SelectedGridItemChangedEventArgs e)
        {
            Console.Write(e.ToString());
            //ArrayObjectEditor.componentName = e.NewSelection.Parent.Label.ToString();
            currentArray = e.NewSelection.Label.ToString();
            currentindex = lstItems.SelectedItem.ToString();//lstItems.SelectedIndex;
        }

    }
}

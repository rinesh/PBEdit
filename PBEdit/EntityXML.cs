using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PBEdit
{
    class EntityXML
    {
        #region Private Fields

        public static XElement m_currentEntity;
        public static XElement m_currentComponent;
        public static string m_currentProperty;

        #endregion

        #region Constructor
        public EntityXML()
        {
            m_currentEntity = new XElement("thing","");
            m_currentComponent = null;
        }
        #endregion

        public static void NewEntity(string name)
        {
            m_currentEntity = new XElement("entity",new XAttribute("name",name));
            m_currentComponent = null;
        }

        public static void SelectedEntity(string name)
        {
            m_currentEntity = XElement.Parse(name).Element("entity");// new XElement("entity", new XAttribute("name", name));
        }

        public static int NewComponent(string componenttype, string componentName)
        {
            if (!ClassSchemaXML.IsComponent(componenttype))
                return 0;

            XElement compXML = new XElement("component", new XAttribute("type", componenttype.Replace("::",".")), new XAttribute("name", componentName)
                                            );
            
            m_currentEntity.Add(compXML);
            m_currentComponent = compXML;
            return -1;
        }

        public static int ModifyComponentWithDefaultValues(string componentName, string EntityString)
        {
            m_currentEntity = XElement.Parse(EntityString);
            m_currentComponent = null;

            var components = from component in m_currentEntity.Descendants("component")
                             where component.Attribute("name").Value.ToString() == componentName
                        select new
                        {
                            name = component.Attribute("name").Value,
                            type = component.Attribute("type").Value,
                            def = component,
                        };

            PBEdit.Property[] property = new Property[50];
            int propertyCount=0;
            XElement componentXML;

            foreach (var component in components)
            {
                componentXML = component.def;

                propertyCount = ClassSchemaXML.GetProperties(component.type, property);
                m_currentComponent = new XElement("component", new XAttribute("type", component.type), new XAttribute("name", component.name));

                foreach (XElement node in componentXML.Descendants())
                {
                    if (node.Parent == componentXML)
                    {
                        if (node.HasElements)
                        {
                            m_currentComponent.Add(new XElement(node.Name, node.Descendants()));
                        }
                        else
                        {
                            m_currentComponent.Add(new XElement(node.Name, node.Value));
                        }
                    }
                }

                for (int i = 0; i < propertyCount; ++i)
                {
                    //var propertyXMLs = m_currentComponent.Descendants(property[i].name);
                    //bool found = false;
                    //foreach (var propertyXML in propertyXMLs)
                    //{
                    //    found = true;
                    //}

                    if (property[i].hasDeafultValue)
                    {
                        foreach (XElement Xnode in m_currentComponent.Descendants())
                        {
                            if (Xnode.Name == property[i].name)
                            {
                                Xnode.Remove();
                                break;
                            }
                        }

                        if (property[i].type == "int")
                        {
                            m_currentComponent.Add(new XElement(property[i].name, property[i].defaultValue));
                        }

                        if (property[i].type == "Boolean")
                        {
                            m_currentComponent.Add(new XElement(property[i].name, property[i].defaultValue));
                        }

                        if (property[i].type == "String")
                        {
                            m_currentComponent.Add(new XElement(property[i].name, property[i].defaultValue));
                        }

                        if (property[i].type == "Point")
                        {
                            string[] point = property[i].defaultValue.Split(new char[] { '|' });
                            m_currentComponent.Add(new XElement(property[i].name, new XElement("x", point[0]),
                                                          new XElement("y", point[1])
                                                  ));
                        }

                        if (property[i].type == "ObjectType")
                        {
                            m_currentComponent.Add(new XElement(property[i].name, property[i].defaultValue));
                        }

                        if (property[i].type == "Array")
                        {
                            m_currentComponent.Add(new XElement(property[i].name, property[i].defaultValue));
                        }
                        if (property[i].type == "PropertyReference")
                        {
                            m_currentComponent.Add(new XElement(property[i].name, property[i].defaultValue));
                        }
                        if (property[i].type == "componentReference")
                        {
                            m_currentComponent.Add(new XElement(property[i].name, property[i].defaultValue));
                        }
                        if (property[i].type == "Object")
                        {
                            m_currentComponent.Add(new XElement(property[i].name, property[i].defaultValue));
                        }

                    }
                }
            }

            return -1;
        }

    }
}

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Flash.External;
using System.Xml.Linq;


namespace PBEdit
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        #region Private Fields

        private bool appReady = false;
        private bool swfReady = false;
        public static ExternalInterfaceProxy proxy;
        //private static XElement classSchemaXML;

        #endregion

        #region Constructor

        public MainForm()
        {
                InitializeComponent();

                //cmbEntityList.Items.Add("Wall11");
                //cmbEntityList.Items.Add("Wall12");
                String swfPath = "D:\\PBEdit\\PBEdit\\Editor\\bin\\Editor.swf";
                //String swfPath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Editor.swf";
                this.MainFlash.LoadMovie(0, swfPath);

                // Create the proxy and register this app to receive notification when the proxy receives
                // a call from ActionScript
                proxy = new ExternalInterfaceProxy(MainFlash);
                proxy.ExternalInterfaceCall += new ExternalInterfaceCallEventHandler(proxy_ExternalInterfaceCall);

                appReady = true;

        }

        #endregion

        #region Event Handling
        /// <summary>
        /// Called by the proxy when an ActionScript ExternalInterface call
        /// is made by the SWF
        /// </summary>
        /// <param name="sender">The object raising the event</param>
        /// <param name="e">The event arguments associated with the event</param>
        /// <returns>The response to the function call.</returns>
        private object proxy_ExternalInterfaceCall(object sender, ExternalInterfaceCallEventArgs e)
        {
            switch (e.FunctionCall.FunctionName)
            {
                case "isReady":
                    return isReady();
                case "setSWFIsReady":
                    setSWFIsReady();
                    return null;
                case "SchemaRecieved":
                    SchemaRecieved((string)e.FunctionCall.Arguments[0]);
                    return null;
                case "SelectedEntityChanged":
                    SelectedEntityChanged((string)e.FunctionCall.Arguments[0]);
                    return null;
                case "statusChange":
                    //statusChange();
                    return null;
                default:
                    return null;
            }
        }
        #endregion

        #region Methods called by Flash Player

        /// <summary>
        /// Called to check if the page has initialized and JavaScript is available
        /// </summary>
        /// <returns></returns>
        private bool isReady()
        {
            return appReady;
        }

        /// <summary>
        /// Called to notify the page that the SWF has set it's callbacks
        /// </summary>
        private void setSWFIsReady()
        {
            // record that the SWF has registered it's functions (i.e. that C#
            // can safely call the ActionScript functions)
            swfReady = true;
            string schema = (string)proxy.Call("generateSchema", "Schema");
        }

        /// <summary>
        /// Called to check if the page has initialized and JavaScript is available
        /// </summary>
        /// <returns></returns>
        private bool SchemaRecieved(string schema)
        {
            //Console.Write("Schema Recieved \n"+schema);
            txtXML.Text = schema;
            ClassSchemaXML.SetclassSchemaXML(schema);
            AddComponentstoList();
            return true;
        }

        /// <summary>
        /// Called to check if the page has initialized and JavaScript is available
        /// </summary>
        /// <returns></returns>
        private bool SelectedEntityChanged(string entityString)
        {
            //Console.Write("Schema Recieved \n" + entityXML);
            txtXML.Text = entityString;
            entityString = entityString.Replace("\n", "");
            XElement entityXML = XElement.Parse(entityString);
            var componentXMLs = entityXML.Descendants("component");
            Properties.Item.Clear();
            Properties.Refresh();
            foreach (XElement componentXML in componentXMLs)
            {
                EntityXML.m_currentComponent = componentXML;
                AddComponet(componentXML.Attribute("name").Value, componentXML.Attribute("type").Value, entityString);
            }
            EntityXML.SelectedEntity(entityString);

            cmbEntityList.SelectedIndex = cmbEntityList.Items.IndexOf(entityXML.Element("entity").Attribute("name"));
            return true;
        }

        #endregion

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string schema = (string)proxy.Call("generateSchema", "Schema");
            string entitiesStr = (string)proxy.Call("getAllEntities");

            foreach (XElement entityXML in XElement.Parse(entitiesStr).Elements())
            {
                cmbEntityList.Items.Add(entityXML.Attribute("name").Value);
            }
        }

        private void btnPlayPause_Click(object sender, EventArgs e)
        {
            proxy.Call("playPause", "Play/Pause");
        }

        /// <summary>
        /// Parse Class Schema
        /// </summary>
        /// <returns></returns>
        private void AddComponentstoList()
        {
            //Console.Write("Schema Recieved \n" + entityXML);
            string[] componentList = new string[50];
            int totalComponents = ClassSchemaXML.GetComponents(componentList);
            cmbComponentList.Items.Clear();
            for (int i = 0; i < totalComponents;i++ )
            {
                cmbComponentList.Items.Add(componentList[i]);
            }
        }

        int entityNo = 0;
        private void btnNewEntity_Click(object sender, EventArgs e)
        {
            Properties.Item.Clear();
            Properties.Refresh();
            cmbEntityList.Items.Add("Entity" + entityNo.ToString());
            cmbEntityList.SelectedIndex = cmbEntityList.Items.Count -1 ;
            EntityXML.NewEntity("Entity" + entityNo.ToString());
            string done = (string)proxy.Call("newEntity", "Entity" + entityNo.ToString());
            entityNo++;
        }

        private void btnNewComponent_Click(object sender, EventArgs e)
        {
            if (cmbComponentList.SelectedItem == null)
                return;
            string componentType = cmbComponentList.SelectedItem.ToString();
            string componentName = componentType.Substring(componentType.IndexOf(".") + 1);
            while (componentName.IndexOf(".") != -1)
            {
                componentName = componentName.Substring(componentName.IndexOf(".") + 1);
            }
            int m = componentName.IndexOf("Component");
            if (componentName.IndexOf("Component") != -1)
                componentName = componentName.Remove(componentName.IndexOf("Component"));

            EntityXML.NewComponent(componentType, componentName);
            XElement xml = XElement.Parse("<thing />");
            xml.Add(EntityXML.m_currentComponent);
            string EntityString = (string)proxy.Call("newComponent", xml.ToString());
            if (EntityString != "false")
            {
                EntityXML.ModifyComponentWithDefaultValues(componentName, EntityString);
            }
            xml = XElement.Parse("<thing />");
            EntityXML.m_currentComponent.Attributes("type").Remove();
            //xml = XElement.Parse("<thing><component name=\"Box2DSpatial\"><size><x>500</x><y>500</y></size></component></thing>");
            xml.Add(EntityXML.m_currentComponent);
            EntityString = (string)proxy.Call("newComponent", xml.ToString());
            EntityString = EntityString.Replace("\\n", "");
            AddComponet(componentName,componentType, EntityString);
        }

        /// <summary>
        /// Add Components
        /// </summary>
        /// <returns></returns>
        private void AddComponet(string componentName,String componentType ,string EntityString)
        {
            PBEdit.Property[] property = new Property[50];
            int propertyCount = ClassSchemaXML.GetProperties(componentType, property);
            
            
            for (int i = 0; i < propertyCount; ++i)
            {
                var propertyXMLs = EntityXML.m_currentComponent.Descendants(property[i].name);

                foreach (var propertyXML in propertyXMLs)
                {
                    property[i].defaultValue = propertyXML.Value;

                    if (property[i].type == "Point")
                    {
                        string coord = propertyXML.Element("x").Value;
                        property[i].defaultValue = propertyXML.Element("x").Value + "|" + propertyXML.Element("y").Value;
                    }
                    if (property[i].type == "componentReference")
                    {
                        string reference;
                        if(propertyXML.HasAttributes)
                        {
                            if (propertyXML.FirstAttribute == propertyXML.LastAttribute)
                                reference = "entityName=\"" + propertyXML.Attribute("entityName").Value + "\"";
                            else
                                reference = "entityName=\"" + propertyXML.Attribute("entityName").Value + "\" componentName=" + propertyXML.Attribute("componentName").Value + "\"";

                            property[i].defaultValue = reference;
                        }
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
                if (property[i].type == "String")
                {
                    Properties.Item.Add(property[i].name, property[i].defaultValue, false, componentName, "", true);
                }
                if (property[i].type == "Boolean")
                {
                    Properties.Item.Add(property[i].name, Boolean.Parse(property[i].defaultValue), false, componentName, "", true);
                }
                if (property[i].type == "Point")
                {
                    string[] point = property[i].defaultValue.Split(new char[]{'|'});
                    Point2D p = new Point2D(float.Parse(point[0]), float.Parse(point[1]));
                    Properties.Item.Add(property[i].name, p, false, componentName, "", true);
                    Properties.Item[Properties.Item.Count - 1].IsBrowsable = true;
                    Properties.Item[Properties.Item.Count - 1].BrowsableLabelStyle = PropertyGridEx.BrowsableTypeConverter.LabelStyle.lsEllipsis;
                }
                //if (property[i].type == "ObjectType")
                //{
                //    Properties.Item.Add(property[i].name, "", false, componentName, "", true);
                //}
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
                    Properties.Item.Add(property[i].name, property[i].TypeHint, false, componentName, "", true);
                    //Properties.Item[Properties.Item.Count - 1].Value = arg.value;
                    Properties.Item[Properties.Item.Count - 1].CustomEditor = new ObjectEditorUI();
                }
            }
            
            Properties.Refresh();
        }
        
        private void Properties_PropertyValueChanged(System.Object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
        {
            Console.WriteLine(e.ToString());
            XElement changeProperty = new XElement("thing");

            string[] val = new string[10];
            int cnt = 0;
            GridItem gi= e.ChangedItem.Parent;
            while (gi.Value != null)
            {
                val[cnt++] = gi.Label;
                gi =gi.Parent;
            }

            XElement xmlProperty = new XElement("component", new XAttribute("name", gi.Label));
            while( cnt > 0 )
            {
                if (xmlProperty.LastNode == null)
                {
                    xmlProperty.Add(new XElement(val[--cnt]));
                }
                else
                {
                    xmlProperty.LastNode.AddAfterSelf(new XElement(val[--cnt]));
                }
            }
            if (xmlProperty.LastNode == null)
            {
                xmlProperty.Add(new XElement(e.ChangedItem.Label, e.ChangedItem.Value));
            }
            else
            {
                string s1 = xmlProperty.LastNode.ToString().Replace("<","").Replace(">","").Replace(" /","");
                xmlProperty.LastNode.ReplaceWith(new XElement(s1, new XElement(e.ChangedItem.Label, e.ChangedItem.Value)));// AddAfterSelf(new XElement(e.ChangedItem.Label, e.ChangedItem.Value));
            }
            changeProperty.Add(xmlProperty);
            string EntityString = (string)proxy.Call("newComponent", changeProperty.ToString());
        }

        private void cmbEntityList_TextChanged(object sender, EventArgs e)
        {
            //cmbEntityList.//SelectedValueChanged//SelectedIndexChanged
                
            string done = (string)proxy.Call("currentEntity", cmbEntityList.Text);

        }

        private void Properties_SelectedGridItemChanged(System.Object s, SelectedGridItemChangedEventArgs e)
        {
            Console.Write(e.ToString());
            ArrayObjectEditor.componentName = e.NewSelection.Parent.Label.ToString();
            ArrayObjectEditor.fieldName = e.NewSelection.Label.ToString();
        }
    }
    
}

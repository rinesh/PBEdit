using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PBEdit
{
    public partial class ObjectEditor : Form
    {
        private string EntityString;
        private string m_type;
        private string[] classList;

        public ObjectEditor(string type)
        {
            InitializeComponent();

            m_type = type;
            classList = new string[50];
            int n = ClassSchemaXML.GetClassesOfType(m_type, classList);
            for (int i = 0; i < n; i++)
            {
                cmbTypes.Items.Add(classList[i]);
            }
            EntityString = EntityXML.m_currentEntity.ToString();
            EntityString = EntityString.Replace("\\n", "");
            EntityString = EntityString.Replace("\n", "");
            EntityString = EntityString.Replace("\r", "");
        }

        public PropertyGridEx.PropertyGridEx GetPropertyGridEx()
        {
            return Properties;
        }
    }
}

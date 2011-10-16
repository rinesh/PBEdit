using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms.PropertyGridInternal;

namespace PBEdit
{
    public class ArrayEditorUI : System.Drawing.Design.UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                if (!context.PropertyDescriptor.IsReadOnly)
                {
                    return UITypeEditorEditStyle.Modal;
                }
            }
            return UITypeEditorEditStyle.None;
        }

        [RefreshProperties(RefreshProperties.All)]
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService wfes = provider.GetService(
                typeof(IWindowsFormsEditorService)) as
                IWindowsFormsEditorService;

            if (context == null || provider == null || context.Instance == null)
            {
                return base.EditValue(provider, value);
            }

            ArrayObjectEditor objEdit = new ArrayObjectEditor( value.ToString());

            if (wfes.ShowDialog(objEdit) == DialogResult.OK)
            {
                value = value.ToString();//objEdit.selectedFile;
            }
            value = value.ToString();
            return value;
        }

    }

    public class ObjectEditorUI : System.Drawing.Design.UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                if (!context.PropertyDescriptor.IsReadOnly)
                {
                    return UITypeEditorEditStyle.Modal;
                }
            }
            return UITypeEditorEditStyle.None;
        }

        [RefreshProperties(RefreshProperties.All)]
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService wfes = provider.GetService(
                typeof(IWindowsFormsEditorService)) as
                IWindowsFormsEditorService;

            if (context == null || provider == null || context.Instance == null)
            {
                return base.EditValue(provider, value);
            }
            ObjectEditor objEdit = new ObjectEditor(value.ToString());
            //PBEditUtils.AddComponetToPropertyGridEx(value.ToString(), objEdit.GetPropertyGridEx());
            if (wfes.ShowDialog(objEdit) == DialogResult.OK)
            {
                value = value.ToString();//objEdit.selectedFile;
            }
            value = value.ToString();
            return value;
        }
    }

    public class ComponentReferenceUI : System.Drawing.Design.UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                if (!context.PropertyDescriptor.IsReadOnly)
                {
                    return UITypeEditorEditStyle.Modal;
                }
            }
            return UITypeEditorEditStyle.None;
        }

        [RefreshProperties(RefreshProperties.All)]
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService wfes = provider.GetService(
                typeof(IWindowsFormsEditorService)) as
                IWindowsFormsEditorService;

            if (context == null || provider == null || context.Instance == null)
            {
                return base.EditValue(provider, value);
            }
            ComponentReference objEdit = new ComponentReference();

            if (wfes.ShowDialog(objEdit) == DialogResult.OK)
            {
                value = value.ToString();//objEdit.selectedFile;
            }
            value = value.ToString();
            return value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Popup;
using System.Data;

namespace XFramework
{
    [UserRepositoryItem("RegisterXTextEdit")]
    public class RepositoryItemXTextEdit : RepositoryItemTextEdit
    {
        static RepositoryItemXTextEdit()
        {
            RegisterXTextEdit();
        }

        public const string CustomEditName = "XTextEdit";

        public RepositoryItemXTextEdit()
        {
        }

        public override string EditorTypeName
        {
            get
            {
                return CustomEditName;
            }
        }

        public static void RegisterXTextEdit()
        {
            Image img = null;
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(CustomEditName, typeof(XTextEdit), typeof(RepositoryItemXTextEdit), typeof(XTextEditViewInfo), new XTextEditPainter(), true, img));
        }

        public override void Assign(RepositoryItem item)
        {
            BeginUpdate();
            try
            {
                base.Assign(item);
                RepositoryItemXTextEdit source = item as RepositoryItemXTextEdit;
                if (source == null) return;
                //
            }
            finally
            {
                EndUpdate();
            }
        }
    }

    [ToolboxItem(true)]
    public class XTextEdit : TextEdit
    {
        public string _fieldName = "";
        public string _methodName = "";
        public string _sqlDataType = "";

        static XTextEdit()
        {
            RepositoryItemXTextEdit.RegisterXTextEdit();
        }

        public XTextEdit()
        {
           
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new RepositoryItemXTextEdit Properties
        {
            get
            {
                return base.Properties as RepositoryItemXTextEdit;
            }
        }

        public override string EditorTypeName
        {
            get
            {
                return RepositoryItemXTextEdit.CustomEditName;
            }
        }

        [Category("XEditor")]
        [Description("Alan adını giriniz.")]
        public string XFieldName
        {
            get { return this._fieldName; }
            set
            {
                this._fieldName = value;
                this.Invalidate();
            }
        }

        [Category("XEditor")]
        [Description("Method adını giriniz.")]
        public string XMethodName
        {
            get { return this._methodName; }
            set
            {
                this._methodName = value;
                this.Invalidate();
            }
        }

        [Category("XEditor")]
        public string XSqlDataType
        {
            get { return this._sqlDataType; }
            set
            {
                this._sqlDataType = value;
                this.Invalidate();
            }
        }



    }

    public class XTextEditViewInfo : TextEditViewInfo
    {
        public XTextEditViewInfo(RepositoryItem item) : base(item)
        {
        }
    }

    public class XTextEditPainter : TextEditPainter
    {
        public XTextEditPainter()
        {
        }
    }
}

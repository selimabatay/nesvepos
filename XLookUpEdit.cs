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

namespace XFramework
{
    [UserRepositoryItem("RegisterXLookUpEdit")]
    public class RepositoryItemXLookUpEdit : RepositoryItemLookUpEdit
    {
        static RepositoryItemXLookUpEdit()
        {
            RegisterXLookUpEdit();
        }

        public const string CustomEditName = "XLookUpEdit";

        public RepositoryItemXLookUpEdit()
        {
        }

        public override string EditorTypeName
        {
            get
            {
                return CustomEditName;
            }
        }

        public static void RegisterXLookUpEdit()
        {
            Image img = null;
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(CustomEditName, typeof(XLookUpEdit), typeof(RepositoryItemXLookUpEdit), typeof(XLookUpEditViewInfo), new XLookUpEditPainter(), true, img));
        }

        public override void Assign(RepositoryItem item)
        {
            BeginUpdate();
            try
            {
                base.Assign(item);
                RepositoryItemXLookUpEdit source = item as RepositoryItemXLookUpEdit;
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
    public class XLookUpEdit : LookUpEdit
    {
        public string _fieldName = "";
        public string _methodName = "";
        public string _sqlDataType = "";
        public string _primaryField = "";
        public string _secondaryField = "";

        static XLookUpEdit()
        {
            RepositoryItemXLookUpEdit.RegisterXLookUpEdit();
        }

        public XLookUpEdit()
        {
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new RepositoryItemXLookUpEdit Properties
        {
            get
            {
                return base.Properties as RepositoryItemXLookUpEdit;
            }
        }

        public override string EditorTypeName
        {
            get
            {
                return RepositoryItemXLookUpEdit.CustomEditName;
            }
        }

        protected override PopupBaseForm CreatePopupForm()
        {
            return new XLookUpEditPopupForm(this);
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
        [Description("Sql veri tipini giriniz.")]
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

    public class XLookUpEditViewInfo : LookUpEditViewInfo
    {
        public XLookUpEditViewInfo(RepositoryItem item) : base(item)
        {
        }
    }

    public class XLookUpEditPainter : ButtonEditPainter
    {
        public XLookUpEditPainter()
        {
        }
    }

    public class XLookUpEditPopupForm : PopupLookUpEditForm
    {
        public XLookUpEditPopupForm(XLookUpEdit ownerEdit) : base(ownerEdit)
        {
        }
    }
}

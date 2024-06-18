using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CRM.Module.BusinessObjects
{
    [DefaultClassOptions]
    [FileTypeFilter("Excel files", 2, "*.xls", "*.xlsx")]

    public class FileAttachment : FileAttachmentBase
    { 
        public FileAttachment(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

      /*  private FileData _FileData;

        public FileData FileData
        {
            get { return _FileData; }
            set { SetPropertyValue<FileData>(nameof(FileData), ref _FileData, value); }
        }*/

        private string _FileName;

        public string FileName
        {
            get { return _FileName; }
            set { SetPropertyValue<string>(nameof(FileName), ref _FileName, value); }
        }
    }
}
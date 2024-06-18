using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;

namespace CRM.Win.Controllers
{
    public class GridCustomizationController : ViewController<ListView>
    {
        public GridCustomizationController() { }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            var editor = View.Editor as GridListEditor;
            if (editor != null) {
                var front = editor.GridView.Appearance.HeaderPanel.Font;
                editor.GridView.Appearance.HeaderPanel.Font = new System.Drawing.Font(front.FontFamily, front.Size, System.Drawing.FontStyle.Bold);
                editor.GridView.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.DarkBlue;
            }
        }
   
    }
}

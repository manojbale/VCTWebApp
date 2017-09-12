using System;

namespace CustomControls
{
    public class ClickablePanel : System.Web.UI.WebControls.Panel, System.Web.UI.IPostBackEventHandler
    {
        public ClickablePanel() { }

        public event EventHandler Click;

        protected virtual void OnClick(EventArgs e)
        {
            if (Click != null)
            {
                Click(this, e);
                
                
            }
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            OnClick(new EventArgs());
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            this.Attributes.Add("onclick", "__doPostBack('" + this.ClientID + "', '');");
            base.Render(writer);
        }
    }
}

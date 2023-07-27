namespace FreyaMobile.Features.Dialog;

public partial class YesNoDialog
{
    public string Title
    {
        get { return (string)TitleControl.Text; }
        set { TitleControl.Text = value; }
    }

    public string Body
    {
        get { return (string)BodyControl.Text; }
        set { BodyControl.Text = value; }
    }

    public YesNoDialog()
    {
        InitializeComponent();
    }

    private void CancelCliked(object sender, EventArgs e)
    {
        this.Close(false);
    }

    private void YesCliked(object sender, EventArgs e)
    {
        this.Close(true);
    }
}
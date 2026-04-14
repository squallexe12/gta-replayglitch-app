using System.Drawing;
using System.Windows.Forms;

internal sealed class WizardForm : Form
{
    private readonly CheckBox hideOnStartupCheckBox;

    public WizardForm(LocalizedText text)
    {
        Text = text.WizardTitle;
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(520, 320);
        Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

        TableLayoutPanel layout = new TableLayoutPanel();
        layout.Dock = DockStyle.Fill;
        layout.Padding = new Padding(18);
        layout.RowCount = 4;
        layout.ColumnCount = 1;
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));

        Label introLabel = new Label();
        introLabel.Dock = DockStyle.Fill;
        introLabel.Text = text.WizardIntro;
        introLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);

        TextBox stepsBox = new TextBox();
        stepsBox.Dock = DockStyle.Fill;
        stepsBox.Multiline = true;
        stepsBox.ReadOnly = true;
        stepsBox.BorderStyle = BorderStyle.None;
        stepsBox.BackColor = SystemColors.Control;
        stepsBox.Text = text.WizardSteps;

        hideOnStartupCheckBox = new CheckBox();
        hideOnStartupCheckBox.Text = text.WizardDontShowAgain;
        hideOnStartupCheckBox.Dock = DockStyle.Fill;

        FlowLayoutPanel actions = new FlowLayoutPanel();
        actions.Dock = DockStyle.Fill;
        actions.FlowDirection = FlowDirection.RightToLeft;

        Button createButton = new Button();
        createButton.Text = text.WizardCreateButton;
        createButton.AutoSize = true;
        createButton.Padding = new Padding(12, 6, 12, 6);
        createButton.Click += delegate
        {
            DialogResult = DialogResult.OK;
            Close();
        };

        Button laterButton = new Button();
        laterButton.Text = text.WizardLaterButton;
        laterButton.AutoSize = true;
        laterButton.Padding = new Padding(12, 6, 12, 6);
        laterButton.Click += delegate
        {
            DialogResult = DialogResult.Cancel;
            Close();
        };

        actions.Controls.Add(createButton);
        actions.Controls.Add(laterButton);

        layout.Controls.Add(introLabel, 0, 0);
        layout.Controls.Add(stepsBox, 0, 1);
        layout.Controls.Add(hideOnStartupCheckBox, 0, 2);
        layout.Controls.Add(actions, 0, 3);

        Controls.Add(layout);
        AcceptButton = createButton;
        CancelButton = laterButton;
    }

    public bool HideOnStartup
    {
        get { return hideOnStartupCheckBox.Checked; }
    }
}

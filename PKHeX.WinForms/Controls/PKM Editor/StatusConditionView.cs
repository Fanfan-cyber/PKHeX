using System;
using System.Windows.Forms;
using PKHeX.Core;
using PKHeX.Drawing.PokeSprite;

namespace PKHeX.WinForms.Controls;

public partial class StatusConditionView : UserControl
{
    private PKM? pk;
    private bool Loading;
    private readonly ToolTip Hover = new()
    {
        AutoPopDelay = 5000,
        InitialDelay = 200,
        ReshowDelay = 500,
        ShowAlways = true,
    };

    public StatusConditionView()
    {
        InitializeComponent();
        PB_Status.MouseHover += (s, e) => PB_Status.Cursor = Cursors.Hand;
    }

    public void LoadPKM(PKM entity)
    {
        pk = entity;
        LoadStoredValues();
    }

    public void LoadStoredValues()
    {
        if (pk is null || Loading)
            return;
        Loading = true;
        var status = pk.Status_Condition;
        SetStatus((StatusCondition)status);
        Loading = false;
    }

    private void SetStatus(StatusCondition status)
    {
        var color = status.GetStatusColor();
        PB_Status.BackColor = color;
        Hover.SetToolTip(PB_Status, $"Status Condition: {status}");
    }

    private void PB_Status_Click(object sender, EventArgs e)
    {
        ArgumentNullException.ThrowIfNull(pk);
        using var form = new StatusBrowser();
        form.LoadList(pk);
        form.ShowDialog();
        if (!form.WasChosen)
            return;
        var current = pk.Status_Condition;
        current &= ~0xFF;
        current |= (int)form.Choice;
        pk.Status_Condition = current;
        LoadStoredValues();
    }
}

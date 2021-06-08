using System.Windows.Forms;

class NumericUpDownEx : NumericUpDown
{
    protected override void UpdateEditText()
    {
        // Append the units to the end of the numeric value
        Text = Value + " d.p.";
    }
}

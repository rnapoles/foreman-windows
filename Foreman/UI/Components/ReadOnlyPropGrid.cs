using System.Windows.Forms;

namespace Foreman.UI.Components
{
    public class ReadOnlyPropGrid : PropertyGrid
    {
        public ReadOnlyPropGrid()
        {
            // this.ToolbarVisible = false; // categories need to be always visible
            this.PropertyValueChanged += ReadOnlyPropGrid_PropertyValueChanged;
            
        }

        private void ReadOnlyPropGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            e.ChangedItem.PropertyDescriptor.SetValue(this.SelectedObject, e.OldValue);
        }

    }
}

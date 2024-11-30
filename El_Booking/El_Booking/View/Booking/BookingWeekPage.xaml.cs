using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using El_Booking.Model.Repositories;
using El_Booking.ViewModel;
using El_Booking.ViewModel.BookingVM;

namespace El_Booking.View.Booking
{
    /// <summary>
    /// Interaction logic for BookingWeek.xaml
    /// </summary>
    public partial class BookingWeekPage : Page
    {
        public BookingWeekPage()
        {
            InitializeComponent();
        }

        private void MyDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            foreach (var cell in e.AddedCells)
            {
                // Get the row index of the selected cell
                var rowIndex = myDataGrid.Items.IndexOf(cell.Item);
                var columnIndex = cell.Column.DisplayIndex;

                MessageBox.Show($"Selected cell at row index: {rowIndex}, {columnIndex}");
            }
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            var viewModel = DataContext as BookingViewModel;

            if (dataGrid != null && viewModel != null)
            {
                if (dataGrid.SelectedCells.Count > 0)
                {
                    var cellInfo = dataGrid.SelectedCells[0];
                    // Get the row index of the selected cell
                    var rowIndex = myDataGrid.Items.IndexOf(cellInfo.Item);
                    var columnIndex = cellInfo.Column.DisplayIndex;

                    viewModel.SelectedCellContent = [columnIndex, rowIndex];
                }
            }
        }
    }
}

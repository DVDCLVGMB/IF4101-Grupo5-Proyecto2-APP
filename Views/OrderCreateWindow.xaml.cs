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

namespace Steady_Management_App.Views
{
    /// <summary>
    /// Lógica de interacción para OrderCreateWindow.xaml
    /// </summary>
    public partial class OrderCreateWindow : UserControl
    {
        private OrderDTO _order;
        private string PaymentMethod = "Efectivo";
        private string? CardNumber = null;

        public OrderCreateWindow(OrderDTO order)
        {
            InitializeComponent();
            _order = order;

            OrderGrid.ItemsSource = _order.Items;

            // Escuchar cambios en cantidades si deseas
            OrderGrid.CellEditEnding += OrderGrid_CellEditEnding;

            UpdateTotals();
        }

        private void OrderGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Puedes validar cantidades aquí también si lo necesitas
            UpdateTotals();
        }

        private void UpdateTotals()
        {
            TotalTaxText.Text = $"Impuesto: ₡{_order.TotalTaxes:N2}";
            TotalText.Text = $"Total: ₡{_order.Total:N2}";
        }

        private void PaymentMethod_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && rb.Tag is string method)
            {
                PaymentMethod = method;

                if (CardNumberTextBox != null)
                {
                    CardNumberTextBox.IsEnabled = method == "Tarjeta";
                }
            }
        }


        private void FinalizarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentMethod == "Tarjeta")
            {
                CardNumber = CardNumberTextBox.Text.Trim();
                if (string.IsNullOrEmpty(CardNumber) || CardNumber.Length < 12)
                {
                    MessageBox.Show("Debe ingresar un número de tarjeta válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Aquí podrías guardar la orden con método de pago
            MessageBox.Show($"Pedido finalizado con método: {PaymentMethod}\n{(CardNumber != null ? "Tarjeta: " + CardNumber : "")}");

            // Aquí puedes continuar con guardar, actualizar inventario, etc.
        }

    }


}

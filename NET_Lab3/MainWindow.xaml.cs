using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using NET_Lab3.Classes;

namespace NET_Lab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string SelectedPizza;
        private string SelectedSize;
        private double SelectedSauce;
        private double OrderCost;

        private string Street;
        private string BuildingNumber;
        private string ApartmentNumber;
        private string PhoneNumber;

        private string SelectedDeliveryTime;
        private string OrderComments;

        

        private List<string> SelectedToppings;


        public MainWindow()
        {
            InitializeComponent();
            SelectedToppings = new List<string>();
            OrderPizzaBtn.IsEnabled = false;
            AddToCartBtn.IsEnabled = false;
            OrderCost = 0;

            AddDeliveryTimes();

        }


        private void AddDeliveryTimes()
        {
            if (DeliveryHelper.IsPizzeriaOpen())
            {
                DeliveryTimeBox.Items.Add((new ComboBoxItem()).Content = "Tak szybko jak to możliwe");
                foreach (var item in DeliveryHelper.GetDeliveryTimeList())
                {
                    DeliveryTimeBox.Items.Add((new ComboBoxItem()).Content = item);
                }

                DeliveryTimeBox.SelectedItem = DeliveryTimeBox.Items[0];
                SelectedDeliveryTime = "Tak szybko jak to możliwe";
            }
            else
            {
                SelectedDeliveryTime = null;
                ErrorLabel.Content = "Restauracja jest aktualnie zamknięta. \nZapraszamy o 11.";
            }
        }

        private void TypeChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Type.SelectedItem != null)
            {
                SelectedPizza = ((ComboBoxItem)Type.SelectedItem).Content.ToString();
                Cost.Content = PizzaHelper.PizzaCosts[SelectedPizza].ToString("#.00") + " zł";
            }
               
            ToggleAddToCartBtnLocking();
        }

        private void ToppingAdded(object sender, RoutedEventArgs e)
        {
            if(sender is CheckBox)
                SelectedToppings.Add(((CheckBox)sender).Content.ToString());

            ToggleAddToCartBtnLocking();
        }

        private void ToppingRemoved(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
                SelectedToppings.Remove(((CheckBox)sender).Content.ToString());

            ToggleAddToCartBtnLocking();
        }

        private void SauceChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is Slider)
                SelectedSauce = ((Slider)sender).Value;

            ToggleAddToCartBtnLocking();
        }

        private void SizeChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton)
                SelectedSize = ((RadioButton)sender).Content.ToString();

            ToggleAddToCartBtnLocking();
        }
        private void UpdateCart()
        {
            var row = new RowDefinition();
            row.Height = GridLength.Auto;
            Cart.RowDefinitions.Add(row);

            TextBlock pizzaText = new TextBlock();
            TextBlock priceText = new TextBlock();

            pizzaText.Text = PizzaHelper.GetSummaryPizzaString(SelectedPizza, SelectedSize, SelectedSauce, SelectedToppings) + "\n";
            pizzaText.TextWrapping = TextWrapping.WrapWithOverflow;
            pizzaText.HorizontalAlignment = HorizontalAlignment.Left;
            pizzaText.FontWeight = FontWeights.Bold;

            priceText.Text = PizzaHelper.GetSummaryPizzaCost(SelectedPizza, SelectedSize, SelectedToppings).ToString("#.00") + " zł";
            priceText.HorizontalAlignment = HorizontalAlignment.Right;
            priceText.FontWeight = FontWeights.Bold;

            Cart.Children.Add(pizzaText);
            Cart.Children.Add(priceText);

            Grid.SetRow(pizzaText, Cart.RowDefinitions.Count - 1);
            Grid.SetColumn(pizzaText, 0);
            Grid.SetRow(priceText, Cart.RowDefinitions.Count - 1);
            Grid.SetColumn(priceText, 1);
        }

        private void DeliveryTimeChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedDeliveryTime = DeliveryTimeBox.SelectedItem.ToString();
            ToggleOrderPizzaBtnLocking();
        }

        private void AddToCart(object sender, RoutedEventArgs e)
        {
            UpdateCart();
            UpdateSummary();
            ToggleOrderPizzaBtnLocking();
            ClearControls();
            ClearPizzaFields();
        }

        private void UpdateSummary()
        {
            OrderCost += PizzaHelper.GetSummaryPizzaCost(SelectedPizza, SelectedSize, SelectedToppings);
            OrderCostLabel.Content = OrderCost.ToString("#.00") + " zł";

            if (OrderCost > 30)
            {
                DeliveryCostLabel.Content = "Gratis";
                FinalCostLabel.Content = OrderCostLabel.Content;
            }
            else
            {
                DeliveryCostLabel.Content = "5.00 zł";
                FinalCostLabel.Content = (OrderCost + 5).ToString("#.00") + " zł";
            }
        }

        private void ClearControls()
        {
            Type.SelectedItem = null;
            Sauce.Value = 0;
            Cost.Content = "0.00 zł";

            foreach (var item in Toppings.Children)
            {
                ((CheckBox)item).IsChecked = false;
            }
            
            foreach (var item in Size.Children)
            {
                ((RadioButton)item).IsChecked = false;
            }

            AddToCartBtn.IsEnabled = false;
        }

        private void ClearPizzaFields()
        {
            SelectedPizza = null;
            SelectedSauce = 0;
            SelectedSize = null;
            SelectedToppings = new List<string>();
        }

        private void OrderPizza(object sender, RoutedEventArgs e)
        {
            if(DeliveryHelper.IsPizzeriaOpen())
            {
                if(DeliveryHelper.IsDeliveryTimeValid(SelectedDeliveryTime))
                {
                    MessageBox.Show("Zamówienie zostało przyjęte. \nCzas dostawy: " + SelectedDeliveryTime + "\nAdres dostawy: " + DeliveryHelper.GetDeliveryAddressString(Street, BuildingNumber, ApartmentNumber) +
                        "\nNr. telefonu: " + PhoneNumber + "\nCałkowity koszt zamówienia: " + FinalCostLabel.Content.ToString() + "\nUwagi: " + (string.IsNullOrEmpty(OrderComments) ? "brak" : OrderComments));
                }
                else
                {
                    MessageBox.Show("Nie możemy przyjąć zamówienia, ponieważ wybrany czas dostawy jest już niewykonalny. \nProszę wybrać czas dostawy ponownie.");
                    AddDeliveryTimes();
                }
            }
            else
            {
                MessageBox.Show("Nie możemy przyjąć zamówienia, ponieważ restauracja jest już zamknięta. Zapraszamy o 11.");
                ErrorLabel.Content = "Restauracja jest aktualnie zamknięta. \nZapraszamy o 11.";
                AddDeliveryTimes();
            }
        }

        private void ToggleAddToCartBtnLocking()
        {
            if (string.IsNullOrEmpty(SelectedPizza) || string.IsNullOrEmpty(SelectedSize))
            {
                AddToCartBtn.IsEnabled = false;
            }
            else
            {
                AddToCartBtn.IsEnabled = true;
            }
        }

        private void ToggleOrderPizzaBtnLocking()
        {
            if(DeliveryHelper.IsAddressValid(Street, BuildingNumber, ApartmentNumber) && DeliveryHelper.IsPhoneNumberValid(PhoneNumber) && !string.IsNullOrEmpty(SelectedDeliveryTime))
            {
                if(Cart.RowDefinitions.Count > 1)
                {
                    OrderPizzaBtn.IsEnabled = true;
                }
            }
            else
            {
                OrderPizzaBtn.IsEnabled = false;
            }
        }

        private void StreetChanged(object sender, TextChangedEventArgs e)
        {
            Street = ((TextBox)sender).Text;
            ToggleOrderPizzaBtnLocking();
        }

        private void BuildingNumberChanged(object sender, TextChangedEventArgs e)
        {
            BuildingNumber = ((TextBox)sender).Text;
            ToggleOrderPizzaBtnLocking();
        }

        private void ApartmentChanged(object sender, TextChangedEventArgs e)
        {
            ApartmentNumber = ((TextBox)sender).Text;
            ToggleOrderPizzaBtnLocking();
        }

        private void PhoneNumberChanged(object sender, TextChangedEventArgs e)
        {
            PhoneNumber = ((TextBox)sender).Text;
            ToggleOrderPizzaBtnLocking();
        }

        private void CommentsChanged(object sender, TextChangedEventArgs e)
        {
            OrderComments = Comments.Text.ToString();
        }
    }
}

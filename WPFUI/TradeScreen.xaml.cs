using System.Windows;
using Engine.Models;
using Engine.ViewModels;
namespace WPFUI
{
    /// <summary>
    /// Interaction logic for TradeScreen.xaml
    /// </summary>
    public partial class TradeScreen : Window
    {
        public GameSession Session => DataContext as GameSession;

        public TradeScreen()
        {
            InitializeComponent();
        }

        private void OnClick_Sell(object sender, RoutedEventArgs e)
        {
            GroupedInventoryItem groupedInventoryItem = ((FrameworkElement)sender).DataContext as GroupedInventoryItem;

            if(groupedInventoryItem != null)
            {
                Session.CurrentPlayer.ReceiveGold(groupedInventoryItem.ITEM.PRICE);
                Session.CurrentTrader.AddItemToInventory(groupedInventoryItem.ITEM);
                Session.CurrentPlayer.RemoveItemFromInventory(groupedInventoryItem.ITEM);
            }
        }

        private void OnClick_Buy(object sender, RoutedEventArgs e)
        {
            GroupedInventoryItem groupedInventoryItem = ((FrameworkElement)sender).DataContext as GroupedInventoryItem;

            if(groupedInventoryItem != null)
            {
                if(Session.CurrentPlayer.GOLD >= groupedInventoryItem.ITEM.PRICE)
                {
                    Session.CurrentPlayer.SpendGold(groupedInventoryItem.ITEM.PRICE);
                    Session.CurrentTrader.RemoveItemFromInventory(groupedInventoryItem.ITEM);
                    Session.CurrentPlayer.AddItemToInventory(groupedInventoryItem.ITEM);
                }
                else
                {
                    MessageBox.Show("You do not have enough gold.");
                }
            }
        }

        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfTest
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // INPC Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // The list that contains Items that will be chosen in a Cart
        private ObservableCollection<Item> _ItemsList;
        public ObservableCollection<Item> ItemsList
        {
            get => _ItemsList;
            set
            {
                _ItemsList = value;
                OnPropertyChanged();
            }
        }

        // The list that contains Carts that will be shown in the ListView
        private ObservableCollection<Cart> _CartsList;
        public ObservableCollection<Cart> CartsList
        {
            get => _CartsList;
            set
            {
                _CartsList = value;
                OnPropertyChanged();
            }
        }

        // A signle Cart
        private Cart _Cart;
        public Cart Cart
        {
            get => _Cart;
            set
            {
                _Cart = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            // Populating ItemsList
            ItemsList = new ObservableCollection<Item>()
            {
                new Item("T-shirt"), new Item("Jeans"), new Item("Boots"),
            };

            // Populating CartsList
            CartsList = new ObservableCollection<Cart>()
            {
                new Cart(ItemsList[0]),
                new Cart(ItemsList[2]),
                new Cart(ItemsList[1]),
                new Cart(ItemsList[0]),
                new Cart(ItemsList[1]),
            };

            // Setting an Item to Cart
            Cart = new Cart(ItemsList[2]);

        }
    }

    // Cart Object
    public class Cart
    {
        public Item Item { get; set; }

        public Cart(Item item) => Item = item;
    }

    // Item Object
    public class Item
    {
        // Important to be private set so it cannot be changed
        public string Name { get; private set; }

        public Item(string name) => Name = name;
    }
}

using FadricaMobile.api.models;
using Xamarin.Forms;

namespace FadricaMobile
{
    class EntryRoscon
    {
        public Entry Entry { get; set; }

        public Roscon Roscon { get; set; }

        public Button AddButton { get; set; }

        public Button SubtractButton { get; set; }

        public Button EditButton { get; set; }

        // Amount to Add or Subtract from entry
        public int Amount { get; set; }
    }
}

using FadricaMobile.api.models;
using FadricaMobile.api.wrappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace FadricaMobile
{
    class MainPage : ContentPage
    {
        private readonly RosconWrapper rosconWrapper = new RosconWrapper();
        private readonly RosconTypeWrapper rosconTypeWrapper = new RosconTypeWrapper();

        private List<Roscon> roscones;
        private List<RosconType> rosconTypes;
        private Dictionary<Entry, Roscon> inputs = new Dictionary<Entry, Roscon>();
        private Roscon rosconFromAPI;

        private readonly Button saveButton = new Button()
        {
            Text = "Guardar"
        };
        private readonly Button updateButton = new Button()
        {
            Text = "Actualizar"
        };
        private readonly Label totalLabel = new Label() { Text = "0" };

        public MainPage()
        {
            StackLayout contentPanel = new StackLayout() { Spacing = 15 };
            StackLayout panel = new StackLayout() { Padding = new Thickness(17, 17, 17, 17) };
            Frame titleFrame = new Frame()
            {
                BackgroundColor = Color.FromHex("#2196F3"),
                Padding = new Thickness(17, 17, 17, 17),
                CornerRadius = 0,
                Content = new Label()
                {
                    Text = "Lista de Roscones",
                    HorizontalTextAlignment = TextAlignment.Center,
                    TextColor = Color.White,
                    FontSize = 36
                }
            };

            contentPanel.Children.Add(titleFrame);

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                InitializingElements(panel);
            }
            else
            {
                panel.Children.Add(new Label { 
                    Text = "No tienes conexión a internet"
                });
            }
            contentPanel.Children.Add(panel);

            this.Content = new ScrollView { Content = contentPanel };
        }

        private async void InitializingElements(StackLayout panel)
        {
            roscones = await rosconWrapper.GetAllRosconesAsync(null);
            rosconTypes = await rosconTypeWrapper.GetAllRosconTypeAsync();

            saveButton.Clicked += SaveEvent;
            updateButton.Clicked += UpdateEvent;

            SetRosconesAndTypes(roscones, rosconTypes, panel);
            panel.Children.Add(new Label { Text = "Total" });
            panel.Children.Add(totalLabel);
            panel.Children.Add(saveButton);
            panel.Children.Add(updateButton);
        }

        private void SetRosconesAndTypes(List<Roscon> roscones, List<RosconType> rosconTypes, StackLayout panel)
        {
            foreach (var rosconType in rosconTypes)
            {
                Label rosconTypeLabel = new Label()
                {
                    Text = rosconType.Tipo
                };
                Roscon roscon = roscones.Find(x => x.Tipo_Roscon == rosconType.Id);

                if (roscon == null)
                    // TODO -> Anno passing as argument
                    roscon = new Roscon { Id = null, Cantidad = 0, Tipo_Roscon = rosconType.Id, Anno = DateTime.Now.Year };

                Entry rosconInput = new Entry()
                {
                    Text = roscon.Cantidad.ToString(),
                    Keyboard = Keyboard.Numeric
                };

                inputs.Add(rosconInput, roscon);
                totalLabel.Text = (int.Parse(totalLabel.Text) + roscon.Cantidad).ToString();

                rosconInput.Completed += UpdateAmountsEvent;

                panel.Children.Add(rosconTypeLabel);
                panel.Children.Add(rosconInput);
            }
        }

        private async void CreateRoscon(Roscon roscon)
        {
            rosconFromAPI = await rosconWrapper.CreateRosconAsync(roscon);
            roscon.Id = rosconFromAPI.Id;
        }

        private async void UpdateRoscon(Roscon roscon)
        {
            rosconFromAPI = await rosconWrapper.UpdateRosconAsync(roscon);
        }

        private async void DisplayOkAlert(string title, string messageBody)
        {
            await DisplayAlert(title, messageBody, "OK");
        }

        private void SaveEvent (object sender, EventArgs e)
        {
            saveButton.IsEnabled = false;

            foreach (var roscon in inputs.Values)
            {
                if (roscon.Id == null)
                    CreateRoscon(roscon);
                else
                    UpdateRoscon(roscon);
            }

            DisplayOkAlert("Alerta", "Elementos creados con éxito");
            saveButton.IsEnabled = true;
        }

        private void UpdateEvent (object sender, EventArgs e)
        {

        }

        private void UpdateAmountsEvent(object sender, EventArgs e)
        {
            try
            {
                var totalAmount = int.Parse(totalLabel.Text);
                var entrySender = ((Entry)sender);
                var oldValue = inputs[entrySender].Cantidad;
                var newValue = int.Parse(entrySender.Text);

                totalAmount = totalAmount - oldValue + newValue;
                inputs[entrySender].Cantidad = newValue;

                totalLabel.Text = totalAmount.ToString();
            }
            catch(Exception error)
            {
                Log.Warning("Error in TotalAmountEvent", $"Error -> {error}");
            }
        }
    }
}

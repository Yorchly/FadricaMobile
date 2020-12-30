using FadricaMobile.api.models;
using FadricaMobile.api.wrappers;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FadricaMobile
{
    class MainPage : ContentPage
    {
        private readonly RosconWrapper rosconWrapper = new RosconWrapper();
        private readonly RosconTypeWrapper rosconTypeWrapper = new RosconTypeWrapper();
        private readonly List<Roscon> roscones;
        private readonly List<RosconType> rosconTypes;
        private readonly Label totalLabel = new Label() { Text = "0" };

        public MainPage()
        {
            StackLayout panel = new StackLayout() { Spacing = 15 };
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

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                roscones = rosconWrapper.GetAllRosconesAsync(null).GetAwaiter().GetResult();
                rosconTypes = rosconTypeWrapper.GetAllRosconTypeAsync().GetAwaiter().GetResult();

                Button saveButton = new Button()
                {
                    Text = "Guardar"
                };
                Button updateButton = new Button()
                {
                    Text = "Actualizar"
                };

                saveButton.Clicked += SaveEvent;
                updateButton.Clicked += UpdateEvent;

                panel.Children.Add(titleFrame);
                SetRosconesAndTypes(roscones, rosconTypes, panel);
                panel.Children.Add(totalLabel);
                panel.Children.Add(saveButton);
                panel.Children.Add(updateButton);
            }
            else
            {
                panel.Children.Add(new Label { 
                    Text = "No tienes conexión a internet"
                });
            }
            this.Content = panel;
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
                Entry rosconInput = new Entry()
                {
                    Text = roscon.Cantidad.ToString(),
                    Keyboard = Keyboard.Numeric
                };

                rosconInput.Completed += TotalAmountEvent;

                panel.Children.Add(rosconTypeLabel);
                panel.Children.Add(rosconInput);
            }
        }

        private void SaveEvent (object sender, EventArgs e)
        {

        }

        private void UpdateEvent (object sender, EventArgs e)
        {

        }

        private void TotalAmountEvent (object sender, EventArgs e)
        {

        }
    }
}

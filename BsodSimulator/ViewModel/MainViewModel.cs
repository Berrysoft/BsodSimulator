﻿using BsodSimulator.Model;
using BsodSimulator.Service;
using BsodSimulator.Util;
using BsodSimulator.View;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace BsodSimulator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MyColor SelectedColor
        {
            get => _selectedColor;
            set => SetProperty(ref _selectedColor, value,
              callback: async () => Bitmap = await QRCodeService.GenerateQRCodeAsync(Url, SelectedColor));
        }

        private MyColor _selectedColor;

        private string _emoji;

        public string Emoji
        {
            get => _emoji;
            set => SetProperty(ref _emoji, value);
        }


        private string _description;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private WriteableBitmap _bitmap;

        public WriteableBitmap Bitmap
        {
            get => _bitmap;
            set => SetProperty(ref _bitmap, value);
        }


        private int _percent;

        public int Percentage
        {
            get => _percent;
            set => SetProperty(ref _percent, value);
        }


        private bool _dynamicPercentage;

        public bool DynamicPercentage
        {
            get => _dynamicPercentage;
            set => SetProperty(ref _dynamicPercentage, value,
              callback: () => Percentage = 0);
        }


        private string _url;

        public string Url
        {
            get => _url;
            set => SetProperty(ref _url, value, callback: async () =>
             Bitmap = await QRCodeService.GenerateQRCodeAsync(Url, SelectedColor));
        }


        private string _stopCode;

        public string StopCode
        {
            get => _stopCode;
            set => SetProperty(ref _stopCode, value);
        }

        public List<MyColor> MyColors;

        private bool _classicBSOD;

        public bool ClassicBSOD
        {
            get => _classicBSOD;
            set => SetProperty(ref _classicBSOD, value, callback: () =>
                       {
                           if (_classicBSOD)
                           {
                               SelectedColor = MyColor.GetColorByName("DodgerBlue");
                           }
                       });
        }

        private bool _insiderGSOD;

        public bool InsiderGSOD
        {
            get => _insiderGSOD;
            set => SetProperty(ref _insiderGSOD, value, callback: () =>
                 {
                     if (InsiderGSOD)
                     {
                         SelectedColor = MyColor.GetColorByName("LimeGreen");
                     }
                 });
        }

        public bool RestartUponComplete { get; set; }


        public RelayCommand<Frame> GoToBSODPageCommand { get; set; }


        public MainViewModel()
        {
            MyColors = MyColor.GetColors();
            Emoji = ":(";
            Description = "Your PC ran into a problem and needs to restart. We're just collecting some error info, and then we'll restart for you";
            Percentage = 0;
            StopCode = "KERNEL_MODE_HEAP_CORRUPTION";
            ClassicBSOD = true;
            DynamicPercentage = true;
            RestartUponComplete = true;
            GoToBSODPageCommand = new RelayCommand<Frame>(f => f.Navigate(typeof(BsodPage), this));
            Url = "http://windows.com/stopcode";
        }
        public async Task UpdateProgress(CancellationToken t)
        {
            int progress = Percentage;
            try
            {
                await Task.Delay(1000, t);//wait until navigation finishes
                if (!DynamicPercentage)
                {
                    return;
                }
                Random r = new Random();
                while (_percent < 100)
                {
                    if (r.Next() % 2 != 0)
                    {
                        int interval = r.Next(1000, 2000);
                        await Task.Delay(interval, t);
                    }
                    else
                    {
                        int step = r.Next(5, 15);
                        int interval = r.Next(500, 1000);
                        await Task.Delay(interval, t);
                        Percentage += step;
                    }
                }
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            finally
            {
                Percentage = progress;
            }
        }
    }
}

using Luxoria.Modules.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LuxImport.Models
{
    public class Manifest : INotifyPropertyChanged
    {
        private string _name;
        private string _description;
        private string _version;

        private LuxoriaInfo _luxoria;

        private ICollection<LuxCfg.AssetInterface> _assets;

        private DateTime _createdAt;
        private DateTime _updatedAt;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }

        public LuxoriaInfo Luxoria
        {
            get => _luxoria;
            set => SetProperty(ref _luxoria, value);
        }

        public ICollection<LuxCfg.AssetInterface> Assets
        {
            get => _assets;
            set => SetProperty(ref _assets, value);
        }

        public DateTime CreatedAt { get; } = DateTime.Now;

        public DateTime UpdatedAt
        {
            get => _updatedAt;
            private set
            {
                if (_updatedAt != value)
                {
                    _updatedAt = value;
                    OnPropertyChanged(nameof(UpdatedAt));
                }
            }
        }

        public Manifest(string name, string description, string version, LuxoriaInfo luxoria)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _description = description ?? throw new ArgumentNullException(nameof(description));
            _version = version ?? throw new ArgumentNullException(nameof(version));
            _luxoria = luxoria ?? throw new ArgumentNullException(nameof(luxoria));

            _assets = new List<LuxCfg.AssetInterface>();

            _createdAt = DateTime.Now;
            _updatedAt = DateTime.Now;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                UpdatedAt = DateTime.Now; // Update `UpdatedAt` whenever a property changes
                OnPropertyChanged(propertyName);
            }
        }

        public class LuxoriaInfo : INotifyPropertyChanged
        {
            private string _version;

            public event PropertyChangedEventHandler? PropertyChanged;

            public string Version
            {
                get => _version;
                set
                {
                    if (_version != value)
                    {
                        _version = value;
                        OnPropertyChanged();
                    }
                }
            }

            protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public LuxoriaInfo(string version)
            {
                _version = version ?? throw new ArgumentNullException(nameof(version));
            }
        }
    }
}

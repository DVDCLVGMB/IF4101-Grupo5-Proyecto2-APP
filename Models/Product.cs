// Models/Product.cs
using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Steady_Management_App.Models
{
    public class Product : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private int _productId;
        private int _categoryId;
        private string _productName = string.Empty;
        private float _price;
        private string _categoryName = string.Empty;
        private string categoryName = string.Empty;

        #region Propiedades bindables

        public int ProductId
        {
            get => _productId;
            set => SetField(ref _productId, value);
        }

        public int CategoryId
        {
            get => _categoryId;
            set => SetField(ref _categoryId, value);
        }

        public string CategoryName
        {
            get => _categoryName;
            set => SetField(ref _categoryName, value);
        }
        public string ProductName
        {
            get => _productName;
            set
            {
                if (SetField(ref _productName, value))
                    ValidateProperty(value, nameof(ProductName));
            }
        }

        public float Price
        {
            get => _price;
            set
            {
                if (SetField(ref _price, value))
                    ValidateProperty(value, nameof(Price));
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            return true;
        }

        #endregion

        #region INotifyDataErrorInfo (validación sencilla)

        private readonly Dictionary<string, List<string>> errors = new();

        public bool HasErrors => errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return errors.SelectMany(kvp => kvp.Value);
            return errors.ContainsKey(propertyName)
                ? errors[propertyName]
                : Array.Empty<string>();
        }

        private void ValidateProperty(object? value, string propName)
        {
            errors.Remove(propName);

            switch (propName)
            {
                case nameof(ProductName):
                    if (string.IsNullOrWhiteSpace(ProductName))
                        AddError(propName, "El nombre no puede estar vacío.");
                    break;
                case nameof(Price):
                    if (Price < 0)
                        AddError(propName, "El precio debe ser ≥ 0.");
                    break;
                case nameof(CategoryId):
                    if (CategoryId <= 0)
                        AddError(propName, "Debe seleccionar una categoría.");
                    break;
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propName));
        }

        private void AddError(string propName, string error)
        {
            if (!errors.ContainsKey(propName))
                errors[propName] = new List<string>();
            errors[propName].Add(error);
        }

        #endregion
    }
}

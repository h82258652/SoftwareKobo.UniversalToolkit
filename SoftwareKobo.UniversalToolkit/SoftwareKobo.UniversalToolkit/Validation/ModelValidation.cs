using SoftwareKobo.UniversalToolkit.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Casting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

/* Example
public class Person : VerifiableBase
{
	private string _name;
	
	[MaxLength(5)]
	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
			RaisePropertyChanged();
		}
	}
}

<TextBox Text="{Binding Path=Name,UpdateSourceTrigger=PropertyChanged}"
	validation:DataValidation.ValidationObject="{Binding}"
	validation:DataValidation.ValidationProperty="Name"
	validation:DataValidation.ValidationPlaceholder="{Binding ElementName=txtErrorPlaceholder}">
	<validation:DataValidation.ErrorStyle>
		<Style></Style>
	</validation:DataValidation.ErrorStyle>
</TextBox>
<TextBlock x:Name="txtErrorPlaceholder"></TextBlock>
*/

namespace SoftwareKobo.UniversalToolkit.Validation
{


    public static class DataValidation
    {
        public static readonly DependencyProperty ValidationPropertyProperty = DependencyProperty.RegisterAttached("ValidationProperty", typeof(string), typeof(DataValidation), new PropertyMetadata
            (default(string), ValidationPropertyChanged));

        public static string GetValidationProperty(FrameworkElement obj)
        {
            return (string)obj.GetValue(ValidationPropertyProperty);
        }

        public static void SetValidationProperty(FrameworkElement obj,string value)
        {
            obj.SetValue(ValidationPropertyProperty, value);
        }


        public static readonly DependencyProperty ValidationObjectProperty = DependencyProperty.RegisterAttached("ValidationObject",typeof(VerifiableBase),typeof(DataValidation),new PropertyMetadata(default(VerifiableBase),ValidationObjectChanged));

        public static VerifiableBase GetValidationObject(FrameworkElement obj)
        {
            return (VerifiableBase)obj.GetValue(ValidationObjectProperty);
        }

        public static void SetValidationObject(FrameworkElement obj, VerifiableBase value)
        {
            obj.SetValue(ValidationObjectProperty, value);
        }

        public static readonly DependencyProperty ErrorStyleProperty = DependencyProperty.RegisterAttached("ErrorStyle",typeof(Style),typeof(DataValidation),new PropertyMetadata(default(Style),ErrorStyleChanged));


        public static Style GetErrorStyle(FrameworkElement obj)
        {
            return (Style)obj.GetValue(ErrorStyleProperty);
        }

        public static void SetErrorStyle(FrameworkElement obj,Style value)
        {
            obj.SetValue(ErrorStyleProperty, value);
        }

        public static readonly DependencyProperty ErrorMessagePlaceholderProperty = DependencyProperty.RegisterAttached("ErrorMessagePlaceholder", typeof(FrameworkElement), typeof(DataValidation), new PropertyMetadata(default(FrameworkElement), ErrorMessagePlaceholderChanged));

        public static void SetErrorMessagePlaceholder(FrameworkElement obj,FrameworkElement value)
        {
            obj.SetValue(ErrorMessagePlaceholderProperty, value);
        }

        public static FrameworkElement GetErrorMessagePlaceholder(FrameworkElement obj)
        {
            return (FrameworkElement)obj.GetValue(ErrorMessagePlaceholderProperty);
        }

        private static void ValidationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void ValidationObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           VerifiableBase value = (VerifiableBase)e.NewValue;
            
        }

        private static void ErrorStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void ErrorMessagePlaceholderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void ValidationChanged(FrameworkElement obj)
        {
            VerifiableBase validationObject = GetValidationObject(obj);
            if (validationObject == null)
            {
                // off
            }
            string validationProperty =  GetValidationProperty(obj);
            if (string.IsNullOrEmpty(validationProperty))
            {
                // off
            }

            FrameworkElement errorMessagePlaceholder = GetErrorMessagePlaceholder(obj);
            if (errorMessagePlaceholder!=null)
            {
                // set error message.
            }
        }
    }
}

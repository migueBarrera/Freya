using Freya.Desktop.Localization.Services;
using System.Windows.Data;

namespace Freya.Desktop.Core.Converters;

public class EmployeeRolToNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is EmployeeRol employeeRol)
        {
            switch (employeeRol)
            {
                case EmployeeRol.COMPANY_OWNER:
                    return TranslatorHelper.Translate("employee_rol_company_owner");
                case EmployeeRol.COMPANY_MANAGER:
                    return TranslatorHelper.Translate("employee_rol_company_manager");
                case EmployeeRol.CLINIC_MANAGER:
                    return TranslatorHelper.Translate("employee_rol_clinic_manager");
                case EmployeeRol.CLINIC_OFFICER:
                    return TranslatorHelper.Translate("employee_rol_clinic_office");
                default:
                    return "ERROR";
            }
        }
        else
        {
            throw new InvalidCastException();
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

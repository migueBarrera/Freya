using Microsoft.AppCenter.Utils.Synchronization;
using System.Xml.Linq;

namespace AppCenterServices;

public class AppCenterAnalitics
{
    public void PageView(string name)
    {
        if (Analytics.Instance.InstanceEnabled)
        {
            Analytics.TrackEvent(Events.PAGE_VIEW, new Dictionary<string, string>()
            {
                { "Page", name }
            });
        }
    }
    
    public void MenuClicked(string name)
    {
        if (Analytics.Instance.InstanceEnabled)
        {
            Analytics.TrackEvent(Events.MENU_CLICKED, new Dictionary<string, string>()
            {
                { "name", name }
            });
        }
    }
    
    public void Clicked(string name)
    {
        if (Analytics.Instance.InstanceEnabled)
        {
            Analytics.TrackEvent(Events.CLICKED, new Dictionary<string, string>()
            {
                { "name", name }
            });
        }
    }

    public void CheckOut(string name, string productId, Guid companyId, bool clinicHasASubcriptionActive, bool mensual)
    {
        if (Analytics.Instance.InstanceEnabled)
        {
            Analytics.TrackEvent(Events.CHECKOUT, new Dictionary<string, string>()
            {
                { "name", name },
                { "productId", productId },
                { "companyId", companyId.ToString() },
                { "clinicHasASubcriptionActive", clinicHasASubcriptionActive.ToString() },
                { "mensual", mensual.ToString() },
            });
        }
    }

    public void SignInData(Guid companyId, string rol)
    {
        if (Analytics.Instance.InstanceEnabled)
        {
            Analytics.TrackEvent(Events.SIGNIN_DATA, new Dictionary<string, string>()
            {
                { "rol", rol },
                { "companyId", companyId.ToString() },
            });
        }
    }

    public void AddEmployee(string state)
    {
        if (Analytics.Instance.InstanceEnabled)
        {
            Analytics.TrackEvent(Events.CHECK_EMPLOYEE, new Dictionary<string, string>()
            {
                { "state", state },
            });
        }
    }

    public void AddClient(string state)
    {
        if (Analytics.Instance.InstanceEnabled)
        {
            Analytics.TrackEvent(Events.CHECK_CLIENT, new Dictionary<string, string>()
            {
                { "state", state },
            });
        }
    }

    public void ClinicCreated(Guid companyId)
    {
        if (Analytics.Instance.InstanceEnabled)
        {
            Analytics.TrackEvent(Events.CLINIC_CREATED, new Dictionary<string, string>()
            {
                { "companyID", companyId.ToString() },
            });
        }
    }

    public void UploadMultimedia(Guid clinicId, string type)
    {
        if (Analytics.Instance.InstanceEnabled)
        {
            Analytics.TrackEvent(Events.UPLOAD_MULTIMEDIA, new Dictionary<string, string>()
            {
                { "clinicId", clinicId.ToString() },
                { "type", type },

            });
        }
    }
    
    public void UploadMultimediaState(string state, Guid clinicId)
    {
        if (Analytics.Instance.InstanceEnabled)
        {
            Analytics.TrackEvent(Events.UPLOAD_MULTIMEDIA_STATE, new Dictionary<string, string>()
            {
                { "state", state },
                { "clinicId", clinicId.ToString() },
            });
        }
    }
}

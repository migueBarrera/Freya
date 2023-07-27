using Models.Core.FAQ;

namespace FreyaManager.Features.Faqs.Models;

public class NewFaqModel : ObservableObject
{
    private string originalTitle = string.Empty;
    private string originalDesc = string.Empty;
    private string title_es = string.Empty;
    private string desc_es = string.Empty;
    private int order;

    public Guid Id { get; set; }

    public int Order { get => order; set => SetAndRaisePropertyChanged(ref order, value); }

    public string OriginalTitle { get => originalTitle; set => SetAndRaisePropertyChanged(ref originalTitle, value); }

    public string OriginalDesc { get => originalDesc; set => SetAndRaisePropertyChanged(ref originalDesc, value); }
    public string Title_es { get => title_es; set => SetAndRaisePropertyChanged(ref title_es, value); }
    public string Desc_es { get => desc_es; set => SetAndRaisePropertyChanged(ref desc_es, value); }

    public FAQ ConvertTo()
    {
        return new FAQ()
        {
            Id = Id,
            Desc_es = Desc_es,
            Title_es = Title_es,
            Order = Order,
            OriginalDesc = OriginalDesc,
            OriginalTitle = OriginalTitle,
        };
    }
    
    public static NewFaqModel ConvertTo(FAQ fAQ)
    {
        return new NewFaqModel()
        {
            Id = fAQ.Id,
            Desc_es = fAQ.Desc_es,
            Title_es = fAQ.Title_es,
            Order = fAQ.Order,
            OriginalDesc = fAQ.OriginalDesc,
            OriginalTitle = fAQ.OriginalTitle,
        };
    }
}

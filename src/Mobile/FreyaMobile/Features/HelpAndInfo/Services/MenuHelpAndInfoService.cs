using FreyaMobile.Features.FertileDay;
using FreyaMobile.Features.HelpAndInfo.Models;

namespace FreyaMobile.Features.HelpAndInfo.Services;

public class MenuHelpAndInfoService : IMenuHelpAndInfoService
{
    public IEnumerable<HAIMenuItem> GenerateMenu()
    {
        return new HAIMenuItem[]
        {
                //new HAIMenuItem()
                //{
                //    //Title = Res.AppStrings.Strings.Help_And_Info_Item_How_Your_Baby,
                //    Title = "Como esta tu bebe",
                //    //Image = ImageSource.FromResource("FreyaMobile.Res.Images.howyourbaby.png", assembly),
                //    Image = ImageSource.FromUri(new Uri("https://blogdelbebe.com/wp-content/uploads/2018/12/Semana-17_-min.jpg")),
                //    Type = TypeHAIMenu.COMO_ESTA_TU_FETO,
                //},
                //new HAIMenuItem()
                //{
                //    //Title = Res.AppStrings.Strings.Help_And_Info_Item_Kicks_Counter,
                //    Title = "Temporal",
                //    //Image = ImageSource.FromResource("FreyaMobile.Res.Images.contracciones.png", assembly),
                //    Image = ImageSource.FromUri(new Uri("https://blogdelbebe.com/wp-content/uploads/2018/12/Semana-17_-min.jpg")),
                //    Type = TypeHAIMenu.CONTADOR_PATADAS,
                //},
                new HAIMenuItem()
                {
                    //Title = Res.AppStrings.Strings.Help_And_Info_Item_Fertile_Days,
                    Title = "Días fértiles",
                    //Image = ImageSource.FromResource("FreyaMobile.Res.Images.diasfertiles.png", assembly),
                    Image = ImageSource.FromUri(new Uri("https://blogdelbebe.com/wp-content/uploads/2018/12/Semana-17_-min.jpg")),
                    Type = TypeHAIMenu.DIAS_FERTILES,
                },new HAIMenuItem()
                {
                    //Title = Res.AppStrings.Strings.Help_And_Info_Item_Fertile_Days,
                    Title = "Días fértiles",
                    //Image = ImageSource.FromResource("FreyaMobile.Res.Images.diasfertiles.png", assembly),
                    Image = ImageSource.FromUri(new Uri("https://blogdelbebe.com/wp-content/uploads/2018/12/Semana-17_-min.jpg")),
                    Type = TypeHAIMenu.DIAS_FERTILES,
                },
                //new HAIMenuItem()
                //{
                //    //Title = Res.AppStrings.Strings.Help_And_Info_Item_Streaming,
                //    Title = "Streaming",
                //    //Image = ImageSource.FromResource("FreyaMobile.Res.Images.streaming.png", assembly),
                //    Image = ImageSource.FromUri(new Uri("https://blogdelbebe.com/wp-content/uploads/2018/12/Semana-17_-min.jpg")),
                //    Type = TypeHAIMenu.STREAMING,
                //},
            //new HAIMenuItem(){ Title = "Contador de contracciones", Image = "counter.png", Type = TypeHAIMenu.CONTADOR_CONTRACCIONES },
            //new HAIMenuItem(){ Title = "Control de peso", Image = "counter.png", Type = TypeHAIMenu.CONTROL_PESO},
            //new HAIMenuItem(){ Title = "Nombres", Image = "counter.png", Type = TypeHAIMenu.NOMBRES },
            //new HAIMenuItem(){ Title = "Información", Image = "counter.png", Type = TypeHAIMenu.INFO },
        };
    }

    public string GetPageFromMenuItemType(TypeHAIMenu type)
    {
        string page = type switch
        {
            TypeHAIMenu.DIAS_FERTILES => $"{nameof(FertileDayPage)}",
            //TypeHAIMenu.STREAMING => new StreamingCodePage(),
            //TypeHAIMenu.CONTADOR_CONTRACCIONES => new ContractionCounterPage(),
            //TypeHAIMenu.COMO_ESTA_TU_FETO => new CalculatorPage(),
            //TypeHAIMenu.CONTADOR_PATADAS => new KicksCounterPage(),
            //TypeHAIMenu.NOMBRES => new NamesPage(),
            //TypeHAIMenu.CONTROL_PESO => new WeightControlPage(),
            //TypeHAIMenu.INFO => new InfoPage(),
            _ => throw new ArgumentException(),
        };
        return page;
    }
}

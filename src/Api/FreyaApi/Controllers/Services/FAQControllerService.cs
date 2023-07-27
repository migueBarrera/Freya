namespace FreyaApi.Controllers.Services;

public class FAQControllerService
{
    private readonly IFAQRepository fAQRepository;

    public FAQControllerService(
        IFAQRepository fAQRepository)
    {
        this.fAQRepository = fAQRepository;
    }

    public async Task<ActionResult<IEnumerable<FAQ>>> GetFAQs(ControllerBase controller)
    {
        var items = await fAQRepository.GetAllFAQ();

        return controller.Ok(items.Select(FAQMapper.ConvertTo));
    }

    public async Task<IActionResult> Create(FAQController controller, FAQ request)
    {
        var faq = FAQMapper.ConvertTo(request);
        var created = await fAQRepository.Create(faq);

        return controller.Created(string.Empty, faq);
    }

    internal async Task<IActionResult> DeleteFaq(FAQController controller, Guid faqId)
    {
        var deleted = await fAQRepository.Delete(faqId);

        return controller.NoContent();
    }

    internal async Task<IActionResult> UpdateFaq(FAQController controller, FAQ request)
    {
        var updated = await fAQRepository.Update(request);

        return controller.Ok(request);
    }
}

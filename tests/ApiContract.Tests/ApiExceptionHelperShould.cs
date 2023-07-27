using Models.Core.Errors;
using Refit;

namespace ApiContract.Tests;

[TestFixture]
public class ApiExceptionHelperShould
{
    [Test]
    public async Task DetectApiExceptions()
    {
        HttpContent content = new StringContent("{ \"errorCode\":\"CLINIC_NOT_HAS_A_VALID_SUBCRIPTION\"}");
        var e = await ApiException.Create(
            new HttpRequestMessage(), 
            HttpMethod.Post, 
            new HttpResponseMessage()
            {
                Content = content,
            }, 
            null!, 
            null);

        var result = ApiExceptionHelper.IsACustomException(e, out ErrorModel errorModel);

        Assert.That(result);
    }
    
    [Test]
    public async Task ParseErrorModelCorrectilyCLINIC_NOT_HAS_A_VALID_SUBCRIPTION()
    {
        HttpContent content = new StringContent("{ \"errorCode\":\"CLINIC_NOT_HAS_A_VALID_SUBCRIPTION\"}");
        var e = await ApiException.Create(
            new HttpRequestMessage(), 
            HttpMethod.Post, 
            new HttpResponseMessage()
            {
                Content = content,
            }, 
            null!, 
            null);

        var result = ApiExceptionHelper.IsACustomException(e, out ErrorModel errorModel);
        Assert.Multiple(() =>
        {
            Assert.That(result);
            Assert.That(errorModel.ErrorCode == ErrorType.CLINIC_NOT_HAS_A_VALID_SUBCRIPTION);
        });
    }
    
    [Test]
    public async Task ParseErrorModelCorrectilyINVALID_PARAMETERS()
    {
        HttpContent content = new StringContent("{ \"errorCode\":\"INVALID_PARAMETERS\"}");
        var e = await ApiException.Create(
            new HttpRequestMessage(), 
            HttpMethod.Post, 
            new HttpResponseMessage()
            {
                Content = content,
            }, 
            null!, 
            null);

        var result = ApiExceptionHelper.IsACustomException(e, out ErrorModel errorModel);
        Assert.Multiple(() =>
        {
            Assert.That(result);
            Assert.That(errorModel.ErrorCode == ErrorType.INVALID_PARAMETERS);
        });
    }
    
    [Test]
    public async Task ParseErrorModelCorrectilyREQUIRED_PARAMETERS()
    {
        HttpContent content = new StringContent("{ \"errorCode\":\"REQUIRED_PARAMETERS\"}");
        var e = await ApiException.Create(
            new HttpRequestMessage(), 
            HttpMethod.Post, 
            new HttpResponseMessage()
            {
                Content = content,
            }, 
            null!, 
            null);

        var result = ApiExceptionHelper.IsACustomException(e, out ErrorModel errorModel);
        Assert.Multiple(() =>
        {
            Assert.That(result);
            Assert.That(errorModel.ErrorCode == ErrorType.REQUIRED_PARAMETERS);
        });
    }
}

using System.Threading.Tasks;

namespace PicConvert.Contracts.Services;
public interface IActivationService
{
	Task ActivateAsync(object activationArgs);
}

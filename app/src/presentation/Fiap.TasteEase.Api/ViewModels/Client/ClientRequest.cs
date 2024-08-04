namespace Fiap.TasteEase.Api.ViewModels.Client;

public class CreateClientRequest
{
    public string Name { get; set; }
    public string TaxpayerNumber { get; set; }
    public string FullAddress { get; set; }
    public long? CellPhoneNumber { get; set; }
}
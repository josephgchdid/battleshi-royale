namespace game_service.Entities.DTO;

public class UpdateModel
{
    public string Id { get; set; }
    
    public string UpdateField { get; set; }
    
    public object NewValue { get; set; }  
}

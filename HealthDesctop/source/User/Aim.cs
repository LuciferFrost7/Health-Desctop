namespace HealthDesctop.source.User;

public class Aim
{
    public DateOnly StartDate { get; set; }  // Дата начала достижения цели
    public DateOnly FinishDate { get; set; } // Дата окончания достижения цели
    
    public Double StartWeight { get; set; }  // Начальный вес
    public Double FinishWeight { get; set; } // Окончательный вес
    
    public Boolean IsFinished { get; set; } // Окончена цель или нет
    
    // Пустой конструктор для работы с БД
    public Aim(){}
}
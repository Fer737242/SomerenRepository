using System.Collections.Generic;
using Someren.Models;

public interface IDrinkRepository
{
    List<Drink> GetAll();
    Drink? GetBytype(int type);
}

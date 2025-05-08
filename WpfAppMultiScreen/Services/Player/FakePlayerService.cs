using KioscoAutogestion.Baluma.Casino.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioscoAutogestion.Baluma.Casino.App.Services.Player
{
    public class FakePlayerService : IPlayerService
    {
        private readonly ISessionService _session;

        public FakePlayerService(ISessionService session)
        {
            _session = session;
        }

        public Task<PlayerDataResponse> GetPlayerDataAsync()
        {
            // Obtiene el número de usuario de la sesión (o 0 si no hay)
            var userNumber = _session.CurrentClient?.UserNumber ?? 0;

            var response = new PlayerDataResponse
            {
                Status = "OK",
                Data = new PlayerDataWrapper
                {
                    Message = "Datos de prueba cargados",
                    Response = new PlayerResponse
                    {
                        Acct = "ACCT" + userNumber,
                        FirstName = "Usuario" + userNumber,
                        DisplayName = "Usuario de Prueba",
                        Category = "Gold",
                        HasUncompletedActiveSurveys = false,
                        CategoryName = "Oro",
                        Balances = new Balances
                        {
                            PointsBal = 12345
                        },
                        CategoryPoints = new CategoryPoints
                        {
                            FirstDay = DateTime.Today.AddDays(-30),
                            LastDay = DateTime.Today,
                            CategoryPointsInt = 1500
                        },
                        CurrentNextCategory = new CurrentNextCategory
                        {
                            CurrentCategory = "Oro",
                            CurrentCategoryPointsFrom = 1000,
                            CurrentCategoryPointsTo = 2000,
                            CurrentCategoryPointsForChange = 2000,
                            NextCategory = "Platino",
                            NextCategoryPointsFrom = 2000,
                            NextCategoryPointsTo = 3000,
                            NextCategoryPointsForChange = 3000
                        }
                    }
                },
                Error = null
            };

            return Task.FromResult(response);
        }
    }


}

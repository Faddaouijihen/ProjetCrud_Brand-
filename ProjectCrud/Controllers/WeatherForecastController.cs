using Microsoft.AspNetCore.Mvc;

namespace ProjectCrud.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        // Supprimez la variable _logger si elle n'est pas utilis�e.
        // private readonly ILogger<WeatherForecastController> _logger;

        // Constructeur de la classe, qui injecte un logger en tant que d�pendance.
        public WeatherForecastController(/*ILogger<WeatherForecastController> logger*/)
        {
            // Aucune utilisation du logger dans le reste du code, donc il peut �tre omis.
            // _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            // G�n�re des pr�visions m�t�orologiques factices pour les 5 prochains jours.
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}

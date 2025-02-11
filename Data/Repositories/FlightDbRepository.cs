using Data.DataContext;
using Domain.Models;

namespace Data.Repositories
{
    public class FlightDbRepository
    {

        private AirlineDbContext _AirLineDBContext;

        //Constructor
        public FlightDbRepository(AirlineDbContext airlineDbContext) {
            //This is using the dependancy injection as to not have multiple instances of airlineDbContext class taking up memory
            _AirLineDBContext = airlineDbContext;
        }

        //Methods
        public IQueryable<Flight> GetFlights()
        {
            // Returns all flights in db [1]
            return _AirLineDBContext.Flights;
        }

        public Flight? GetFlight(Guid id)
        {
            // Get the flight info by id [1]
            return _AirLineDBContext.Flights.SingleOrDefault(x => x.Id == id);
        }
    }
}

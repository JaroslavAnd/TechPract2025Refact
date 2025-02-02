using InterfaceForDB;
using System;
using System.Collections.Generic;
using System.Linq;

public class RideService
{
    private readonly TaxiServiceContext _context;

    public RideService(TaxiServiceContext context)
    {
        _context = context;
    }

    public List<Rate> GetAllTariffs()
    {
        return _context.Rates.ToList();
    }

    public Driver GetAvailableDriver()
    {
        return _context.Drivers.FirstOrDefault(d => !_context.Rides.Any(r => r.Driver_id == d.Id));
    }

    public void CreateRide(int customerId, string pickupLocation, string dropoffLocation, Rate selectedTariff)
    {
        var driver = GetAvailableDriver();
        if (driver == null)
            throw new Exception("Немає доступних водіїв.");

        var ride = new Ride
        {
            Pickup_location = pickupLocation,
            Dropoff_location = dropoffLocation,
            Ride_date = DateTime.Now,
            Fare = selectedTariff.Base_fare,
            Customer_id = customerId,
            Driver_id = driver.Id,
            Rate_id = selectedTariff.Id
        };

        _context.Rides.Add(ride);
        _context.SaveChanges();
    }
}

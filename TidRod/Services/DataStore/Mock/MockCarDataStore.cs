using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TidRod.Models;
using TidRod.Services.Interface;

namespace TidRod.Services.DataStore.Mock
{
    public class MockCarDataStore : ICarDataStore<Car>
    {
        private readonly List<Car> cars;

        public MockCarDataStore()
        {
            cars =
                new List<Car>()
                {
                    new Car()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Mustang",
                        Images =
                            new List<string>()
                            {
                                "http://placehold.appspot.com/123123",
                                "http://placehold.appspot.com/123123"
                            },
                        Price = 150,
                        Obometer = 100000,
                        Gear = CarTransmission.Automatic,
                        PinLocation = "14.040314020853788, 100.61680784777563",
                        UserId = 1
                    },
                    new Car()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Mustang",
                        Images =
                            new List<string>()
                            {
                                "http://placehold.appspot.com/123123",
                                "http://placehold.appspot.com/123123"
                            },
                        Price = 150,
                        Obometer = 100000,
                        Gear = CarTransmission.Automatic,
                        PinLocation = "14.041818308915616, 100.61637795373697",
                        UserId = 1
                    },
                    new Car()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Mustang",
                        Images =
                            new List<string>()
                            {
                                "http://placehold.appspot.com/123123",
                                "http://placehold.appspot.com/123123"
                            },
                        Price = 150,
                        Obometer = 100000,
                        Gear = CarTransmission.Automatic,
                        PinLocation = "14.060302608449451, 100.61200058878445",
                        UserId = 1
                    },
                    new Car()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Mustang",
                        Images =
                            new List<string>()
                            {
                                "http://placehold.appspot.com/123123",
                                "http://placehold.appspot.com/123123"
                            },
                        Price = 150,
                        Obometer = 100000,
                        Gear = CarTransmission.Automatic,
                        PinLocation = "14.061634752546121, 100.62071240334684",
                        UserId = 1
                    },

                };
        }

        public async Task<bool> AddCarAsync(Car car)
        {
            cars.Add (car);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateCarAsync(Car car)
        {
            var oldCar =
                cars.Where((Car arg) => arg.Id == car.Id).FirstOrDefault();
            cars.Remove (oldCar);
            cars.Add (car);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteCarAsync(string id)
        {
            var oldCar = cars.Where((Car arg) => arg.Id == id).FirstOrDefault();
            cars.Remove (oldCar);

            return await Task.FromResult(true);
        }

        public async Task<Car> GetCarAsync(string id)
        {
            return await Task.FromResult(cars.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Car>>
        GetCarsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(cars);
        }
    }
}

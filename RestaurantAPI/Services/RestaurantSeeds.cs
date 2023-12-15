using RestaurantAPI.Entities;

namespace RestaurantAPI.Services
{
    public class RestaurantSeeds
    {
        private readonly RestaurantDbContext _context;
        public RestaurantSeeds(RestaurantDbContext context)
        {
            _context = context;
        }

        public void Seeds()
        {
            if (_context.Database.CanConnect())
            {
                if (!_context.Restaurants.Any())
                {
                    var restaurants = CreateRestaurant();
                    _context.Restaurants.AddRange(restaurants);
                    _context.SaveChanges();
                }
            }
        }

        private IEnumerable<Restaurant> CreateRestaurant()
        {
            List<Restaurant> restaurats = new List<Restaurant>()
            {
                new Restaurant
                {
                    Name = "Kebabownia",
                    Description = "tradycyjne tureckie danie, występuje w ponad 20 odmianach. Tym, co w Polsce określamy mianem kebabu, jest döner kebap, czyli pita wypełniona baraniną z rożna, z dodatkiem surówki i owczego sera.",
                    Category = "Street food",
                    HasDelivery = true,
                    ContactEmail = "Keb.ab@gmail.com",
                    ContactNumber = "999999999",
                    Address = new Address
                    {
                        City = "Wrocław",
                        Street = "Kebabkowa 50/50",
                        PostalCode = "50-500"
                    },
                    Dish = new List<Dish>
                    {
                        new Dish
                        {
                            Name = "Kebab w bułce",
                            Description = "mięso, sałata, sos, bułka",
                            Price = (decimal)25.99,
                        },
                        new Dish
                        {
                            Name = "Kebab w picie",
                            Description = "mięso, sałata, sos, pita",
                            Price = (decimal)30.99,
                        }
                    }
                },
                new Restaurant
                {
                    Name = "Suszarnia",
                    Description = "Sushi to japoński styl gotowania, w którym używa się warzyw, ryb (gotowanych lub surowych), następnie miesza się i formuje w pożądany kształt z ryżem doprawionym octem. Większość restauracji zazwyczaj serwuje wasabi, sos sojowy i marynowany imbir obok tego słynnego japońskiego przysmaku.\r\n",
                    Category = "Street food",
                    HasDelivery = true,
                    ContactEmail = "su.shi@gmail.com",
                    ContactNumber = "999999999",
                    Address = new Address
                    {
                        City = "Wrocław",
                        Street = "Suszarnia 50/50",
                        PostalCode = "50-500"
                    },
                    Dish = new List<Dish>
                    {
                        new Dish
                        {
                            Name = "Futomaki Kanon",
                            Description = "łosoś, paluszek krabowy, ogórek, kampyo, oshinko",
                            Price = (decimal)30.99,
                        },
                        new Dish
                        {
                            Name = "Chicken Panko",
                            Description = "kurczak teriyaki w panko, rukola, awokado, ogórek, szczypiorek",
                            Price = (decimal)50.99,
                        }
                    }
                },

            };

            return restaurats;
        }
    }
}

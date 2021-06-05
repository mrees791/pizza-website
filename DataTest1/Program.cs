using DataLibrary.Models;
using DataLibrary.Models.QuerySearches;
using DataLibrary.Models.Tables;
using DataLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTest1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Program program = new Program();
            await program.StartAsync();
        }

        public readonly string AdminUserName = "mrees791@gmail.com";

        public async Task StartAsync()
        {
            // This can only be ran after creating the databse and creating a user with username mrees791
            //await InitializeExampleDbAsync();

            //TestCustomerOrderJoin();
            //await CreateExampleDeliveryAddressesAsync();

            // Testing new async implementation
            //await TestAbtractClassAsync();
            //await TestAsyncBAD();

            //await TestGetList();
        }

        // Async task list test causes this exception:
        // There is already an open DataReader associated with this Command which must be closed first.
        /*private async Task TestAsyncBAD()
        {
            List<MenuPizzaToppingType> testToppingList = new List<MenuPizzaToppingType>();

            using (var pizzaDb = new PizzaDatabase())
            {
                var taskList = new List<Task>();

                for (int i = 0; i < 10; i++)
                {
                    taskList.Add(RunToppingTestQuery(pizzaDb, testToppingList));
                }

                foreach (var task in taskList)
                {
                    await task;
                }
            }
        }*/

        private async Task InitializeExampleDbAsync()
        {
            await CreateRolesAsync();
            await CreateExamplePizzaIngredientsAsync();
            await CreateExampleMenuPizzasAsync();
            await CreateExampleDeliveryAddressesAsync();
            await CreateExampleStoreLocationsAsync();
            //await CreateExampleOrdersAsync(124);

            Console.WriteLine("Example database created...");
        }

        public async Task CreateExampleDeliveryAddressesAsync()
        {
            DeliveryAddress address1 = new DeliveryAddress()
            {
                AddressType = "House",
                City = "Zanesville",
                Name = "My House",
                PhoneNumber = "7401234567",
                State = "OH",
                StreetAddress = "123 Happy Lane",
                UserId = AdminUserName,
                ZipCode = "43701"
            };

            DeliveryAddress address2 = new DeliveryAddress()
            {
                AddressType = "Business",
                City = "Zanesville",
                Name = "Work",
                PhoneNumber = "7401111111",
                State = "OH",
                StreetAddress = "111 Main Street",
                UserId = AdminUserName,
                ZipCode = "43701"
            };

            using (var pizzaDb = new PizzaDatabase())
            {
                await pizzaDb.InsertAsync(address1);
                await pizzaDb.InsertAsync(address2);
            }
        }

        public async Task CreateExampleOrdersAsync(int orderAmount)
        {
            if (orderAmount % 2 != 0)
            {
                throw new Exception($"orderAmount must be even.");
            }

            using (var pizzaDb = new PizzaDatabase())
            {
                int deliveryAddressId = 1002;
                SiteUser user = await pizzaDb.GetSiteUserByIdAsync(AdminUserName);
                DeliveryAddress address1 = await pizzaDb.GetAsync<DeliveryAddress>(deliveryAddressId);
                DeliveryInfo deliveryInfo = new DeliveryInfo()
                {
                    DateOfDelivery = new DateTime(9999, 12, 31),
                    DeliveryAddressName = address1.Name,
                    DeliveryAddressType = address1.AddressType,
                    DeliveryCity = address1.City,
                    DeliveryState = address1.State,
                    DeliveryStreetAddress = address1.StreetAddress,
                    DeliveryZipCode = address1.ZipCode,
                    DeliveryPhoneNumber = address1.PhoneNumber
                };

                CustomerOrder order1 = new CustomerOrder()
                {
                    CartId = 2,
                    DateOfOrder = DateTime.Now,
                    DeliveryInfoId = null,
                    IsCancelled = false,
                    IsDelivery = false,
                    OrderCompleted = false,
                    OrderPhase = OrderPhase.Order_Placed,
                    OrderSubtotal = 5.00m,
                    OrderTax = 1.00m,
                    OrderTotal = 6.00m,
                    StoreId = 1,
                    UserId = AdminUserName,
                };

                CustomerOrder order2 = new CustomerOrder()
                {
                    CartId = 2,
                    DateOfOrder = DateTime.Now,
                    DeliveryInfoId = deliveryInfo.Id,
                    IsCancelled = false,
                    IsDelivery = true,
                    OrderCompleted = false,
                    OrderPhase = OrderPhase.Order_Placed,
                    OrderSubtotal = 5.00m,
                    OrderTax = 1.00m,
                    OrderTotal = 6.00m,
                    StoreId = 1,
                    UserId = AdminUserName,
                };

                int orderPairs = orderAmount / 2;

                for (int i = 0; i < orderPairs; i++)
                {
                    await pizzaDb.Commands.AddCustomerOrderAsync(user, order1);
                    await pizzaDb.Commands.AddCustomerOrderAsync(user, order2, deliveryInfo);
                }
            }
        }

        public async Task CreateExampleStoreLocationsAsync()
        {
            StoreLocation store1 = new StoreLocation()
            {
                IsActiveLocation = true,
                City = "Zanesville",
                Name = "Maple Avenue",
                PhoneNumber = "7408888888",
                State = "OH",
                StreetAddress = "888 Maple Avenue",
                ZipCode = "43701"
            };

            StoreLocation store2 = new StoreLocation()
            {
                IsActiveLocation = true,
                City = "Zanesville",
                Name = "South Zanesville",
                PhoneNumber = "7409999999",
                State = "OH",
                StreetAddress = "999 Apple Street",
                ZipCode = "43702"
            };

            using (PizzaDatabase pizzaDb = new PizzaDatabase())
            {
                await pizzaDb.InsertAsync(store1);
                await pizzaDb.InsertAsync(store2);
            }
        }

        private async Task CreateExampleMenuPizzasAsync()
        {
            using (PizzaDatabase pizzaDb = new PizzaDatabase())
            {
                var categoryList = ListUtility.GetPizzaCategoryList();
                var cheeseAmountList = ListUtility.GetCheeseAmountList();
                var sauceAmountList = ListUtility.GetSauceAmountList();
                var toppingAmountList = ListUtility.GetToppingAmountList();

                var toppingTypes = await pizzaDb.GetListAsync<MenuPizzaToppingType>();

                var pepperoniToppingType = toppingTypes.Where(t => t.Name == "Pepperoni").FirstOrDefault();
                var mushroomToppingType = toppingTypes.Where(t => t.Name == "Mushrooms").FirstOrDefault();

                var pepperoniToppings = new List<MenuPizzaTopping>()
                {
                    new MenuPizzaTopping()
                    {
                        MenuPizzaToppingTypeId = pepperoniToppingType.Id,
                        ToppingAmount = "Regular",
                        ToppingHalf = "Whole"
                    }
                };

                var mushroomToppings = new List<MenuPizzaTopping>()
                {
                    new MenuPizzaTopping()
                    {
                        MenuPizzaToppingTypeId = mushroomToppingType.Id,
                        ToppingAmount = "Regular",
                        ToppingHalf = "Whole"
                    }
                };

                var supremeToppings = new List<MenuPizzaTopping>()
                {
                    new MenuPizzaTopping()
                    {
                        MenuPizzaToppingTypeId = pepperoniToppingType.Id,
                        ToppingAmount = "Regular",
                        ToppingHalf = "Whole"
                    },
                    new MenuPizzaTopping()
                    {
                        MenuPizzaToppingTypeId = mushroomToppingType.Id,
                        ToppingAmount = "Regular",
                        ToppingHalf = "Whole"
                    }
                };

                MenuPizza pepperoniPizza = new MenuPizza()
                {
                    AvailableForPurchase = true,
                    CategoryName = "Meats",
                    CheeseAmount = "Regular",
                    PizzaName = "Pepperoni",
                    Description = "Classic pepperoni pizza!",
                    SauceAmount = "Regular",
                    Toppings = pepperoniToppings,
                    SortOrder = 1,
                    MenuPizzaCheeseId = 1,
                    MenuPizzaCrustFlavorId = 1,
                    MenuPizzaSauceId = 1,
                };

                MenuPizza mushroomPizza = new MenuPizza()
                {
                    AvailableForPurchase = true,
                    CategoryName = "Veggie",
                    CheeseAmount = "Regular",
                    PizzaName = "Mushroom",
                    Description = "Classic mushroom pizza!",
                    SauceAmount = "Regular",
                    Toppings = pepperoniToppings,
                    SortOrder = 1,
                    MenuPizzaCheeseId = 1,
                    MenuPizzaCrustFlavorId = 1,
                    MenuPizzaSauceId = 1,
                };

                MenuPizza supremePizza = new MenuPizza()
                {
                    AvailableForPurchase = true,
                    CategoryName = "Popular",
                    CheeseAmount = "Regular",
                    PizzaName = "Supreme",
                    Description = "A delicious pizza with pepperoni and mushrooms.",
                    SauceAmount = "Regular",
                    Toppings = supremeToppings,
                    SortOrder = 1,
                    MenuPizzaCheeseId = 1,
                    MenuPizzaCrustFlavorId = 1,
                    MenuPizzaSauceId = 1,
                };

                await pizzaDb.InsertAsync(pepperoniPizza);
                await pizzaDb.InsertAsync(mushroomPizza);
                await pizzaDb.InsertAsync(supremePizza);
            }
        }

        private async Task CreateExamplePizzaIngredientsAsync()
        {
            using (PizzaDatabase pizzaDb = new PizzaDatabase())
            {
                MenuPizzaCheese mozarellaCheese = new MenuPizzaCheese()
                {
                    AvailableForPurchase = true,
                    Name = "Mozzarella Cheese",
                    Description = "Cheese!",
                    PriceLight = 0.1m,
                    PriceRegular = 0.2m,
                    PriceExtra = 0.3m
                };

                MenuPizzaCrustFlavor garlicCrustFlavor = new MenuPizzaCrustFlavor()
                {
                    AvailableForPurchase = true,
                    Name = "Garlic",
                    Description = "May ward off vampires."
                };

                MenuPizzaCrust regularCrust = new MenuPizzaCrust()
                {
                    AvailableForPurchase = true,
                    Name = "Regular Crust",
                    Description = "The typical crust thickness.",
                    PriceSmall = 0.5m,
                    PriceMedium = 0.8m,
                    PriceLarge = 1.25m
                };

                MenuPizzaSauce tomatoSauce = new MenuPizzaSauce()
                {
                    AvailableForPurchase = true,
                    Name = "Tomato Sauce",
                    Description = "The standard.",
                    PriceLight = 0.45m,
                    PriceRegular = 0.85m,
                    PriceExtra = 1.10m
                };

                MenuPizzaToppingType pepperoniTopping = new MenuPizzaToppingType()
                {
                    AvailableForPurchase = true,
                    Name = "Pepperoni",
                    Description = "Classic pepperoni.",
                    CategoryName = "Meats",
                    PriceLight = 0.75m,
                    PriceRegular = 1.0m,
                    PriceExtra = 1.3m
                };

                MenuPizzaToppingType italianSausageTopping = new MenuPizzaToppingType()
                {
                    AvailableForPurchase = true,
                    Name = "Italian Sausage",
                    Description = "Classic sausage.",
                    CategoryName = "Meats",
                    PriceLight = 0.85m,
                    PriceRegular = 1.1m,
                    PriceExtra = 1.4m
                };

                MenuPizzaToppingType baconTopping = new MenuPizzaToppingType()
                {
                    AvailableForPurchase = true,
                    Name = "Bacon",
                    Description = "Classic bacon.",
                    CategoryName = "Meats",
                    PriceLight = 0.45m,
                    PriceRegular = 0.9m,
                    PriceExtra = 1.2m
                };

                MenuPizzaToppingType meatballTopping = new MenuPizzaToppingType()
                {
                    AvailableForPurchase = true,
                    Name = "Meatball",
                    Description = "Classic meatballs.",
                    CategoryName = "Meats",
                    PriceLight = 0.35m,
                    PriceRegular = 0.8m,
                    PriceExtra = 1.1m
                };

                MenuPizzaToppingType hamTopping = new MenuPizzaToppingType()
                {
                    AvailableForPurchase = true,
                    Name = "Ham",
                    Description = "Classic ham.",
                    CategoryName = "Meats",
                    PriceLight = 0.5m,
                    PriceRegular = 0.9m,
                    PriceExtra = 1.25m
                };

                MenuPizzaToppingType grilledChickenTopping = new MenuPizzaToppingType()
                {
                    AvailableForPurchase = true,
                    Name = "Grilled Chicken",
                    Description = "Grilled chicken, high in protein.",
                    CategoryName = "Meats",
                    PriceLight = 0.7m,
                    PriceRegular = 1.25m,
                    PriceExtra = 1.75m
                };

                MenuPizzaToppingType beefTopping = new MenuPizzaToppingType()
                {
                    AvailableForPurchase = true,
                    Name = "Beef",
                    Description = "You're the beef.",
                    CategoryName = "Meats",
                    PriceLight = 0.8m,
                    PriceRegular = 1.35m,
                    PriceExtra = 1.95m
                };

                MenuPizzaToppingType porkTopping = new MenuPizzaToppingType()
                {
                    AvailableForPurchase = true,
                    Name = "Pork",
                    Description = "Delicious pork.",
                    CategoryName = "Meats",
                    PriceLight = 1.25m,
                    PriceRegular = 1.55m,
                    PriceExtra = 2.25m
                };

                MenuPizzaToppingType mushroomTopping = new MenuPizzaToppingType()
                {
                    AvailableForPurchase = true,
                    Name = "Mushrooms",
                    Description = "Freshly picked!",
                    CategoryName = "Veggie",
                    PriceLight = 0.25m,
                    PriceRegular = 0.5m,
                    PriceExtra = 0.75m
                };

                MenuPizzaToppingType redOnionsTopping = new MenuPizzaToppingType()
                {
                    AvailableForPurchase = true,
                    Name = "Red Onions",
                    Description = "Made of layers.",
                    CategoryName = "Veggie",
                    PriceLight = 0.45m,
                    PriceRegular = 0.75m,
                    PriceExtra = 0.95m
                };

                MenuPizzaToppingType blackOlivesTopping = new MenuPizzaToppingType()
                {
                    AvailableForPurchase = true,
                    Name = "Black Olives",
                    Description = "Delicious black olives.",
                    CategoryName = "Veggie",
                    PriceLight = 0.55m,
                    PriceRegular = 0.85m,
                    PriceExtra = 1.05m
                };

                MenuPizzaToppingType greenBellPeppersTopping = new MenuPizzaToppingType()
                {
                    AvailableForPurchase = true,
                    Name = "Green Bell Peppers",
                    Description = "Fresh bell peppers.",
                    CategoryName = "Veggie",
                    PriceLight = 0.70m,
                    PriceRegular = 1.0m,
                    PriceExtra = 1.30m
                };

                MenuPizzaToppingType bananaPeppersTopping = new MenuPizzaToppingType()
                {
                    AvailableForPurchase = true,
                    Name = "Banana Peppers",
                    Description = "Fresh banana peppers.",
                    CategoryName = "Veggie",
                    PriceLight = 0.75m,
                    PriceRegular = 1.05m,
                    PriceExtra = 1.35m
                };

                MenuPizzaToppingType tomatoTopping = new MenuPizzaToppingType()
                {
                    AvailableForPurchase = true,
                    Name = "Roma Tomatoes",
                    Description = "Freshly cut tomatoes.",
                    CategoryName = "Veggie",
                    PriceLight = 0.55m,
                    PriceRegular = 0.75m,
                    PriceExtra = 1.0m
                };

                mozarellaCheese.Id = await pizzaDb.InsertAsync(mozarellaCheese);
                garlicCrustFlavor.Id = await pizzaDb.InsertAsync(garlicCrustFlavor);
                regularCrust.Id = await pizzaDb.InsertAsync(regularCrust);
                tomatoSauce.Id = await pizzaDb.InsertAsync(tomatoSauce);

                // Meats
                await pizzaDb.InsertAsync(pepperoniTopping);
                await pizzaDb.InsertAsync(italianSausageTopping);
                await pizzaDb.InsertAsync(baconTopping);
                await pizzaDb.InsertAsync(meatballTopping);
                await pizzaDb.InsertAsync(hamTopping);
                await pizzaDb.InsertAsync(grilledChickenTopping);
                await pizzaDb.InsertAsync(beefTopping);
                await pizzaDb.InsertAsync(porkTopping);

                // Veggies
                await pizzaDb.InsertAsync(mushroomTopping);
                await pizzaDb.InsertAsync(redOnionsTopping);
                await pizzaDb.InsertAsync(blackOlivesTopping);
                await pizzaDb.InsertAsync(greenBellPeppersTopping);
                await pizzaDb.InsertAsync(bananaPeppersTopping);
                await pizzaDb.InsertAsync(tomatoTopping);
            }
        }

        private CartPizza CreateCartPizza2()
        {
            using (var pizzaDb = new PizzaDatabase())
            {
                CartPizzaTopping pepperoniTopping = new CartPizzaTopping()
                {
                    MenuPizzaToppingTypeId = 1,
                    ToppingAmount = "Regular",
                    ToppingHalf = "Whole"
                };

                CartPizza cartPizza = new CartPizza()
                {
                    MenuPizzaCheeseId = 1,
                    MenuPizzaCrustFlavorId = 1,
                    MenuPizzaCrustId = 1,
                    MenuPizzaSauceId = 1,
                    CheeseAmount = "Regular",
                    SauceAmount = "Regular",
                    Size = "Medium"
                };

                cartPizza.Toppings.Add(pepperoniTopping);

                return cartPizza;
            }
        }

        public async Task CreateEmployeeTestAsync()
        {
            using (var pizzaDb = new PizzaDatabase())
            {
                var users = await pizzaDb.GetListAsync<SiteUser>();

                for (int i = 0; i < 10; i++)
                {
                    SiteUser user = users.ElementAt(i);

                    string employeeId = $"A{i + 1}";

                    // Promote to employee
                    Employee employee = new Employee()
                    {
                        Id = employeeId,
                        UserId = user.Id
                    };

                    await pizzaDb.InsertAsync(employee);
                }
            }
        }

        public async Task CreateUsersTestAsync()
        {
            using (var pizzaDb = new PizzaDatabase())
            {
                for (int i = 0; i < 25; i++)
                {
                    string userName = $"user{i + 1}";
                    string email = $"{userName}@yahoo.com";

                    SiteUser user = new SiteUser()
                    {
                        Id = email,
                        Email = email,
                        PasswordHash = "HASHEDPASSWORD",
                        LockoutEndDateUtc = DateTime.UtcNow
                    };

                    user.Id = await pizzaDb.InsertAsync(user);
                }
            }
        }

        public async Task StartStoreLocationsTestAsync()
        {
            using (var pizzaDb = new PizzaDatabase())
            {
                for (int i = 0; i < 300; i++)
                {
                    StoreLocation storeLocation = new StoreLocation()
                    {
                        Name = $"My Store {i+1}",
                        City = "Hebron",
                        IsActiveLocation = true,
                        PhoneNumber = "5558959001",
                        State = "OH",
                        StreetAddress = "435 Main Street",
                        ZipCode = "43025"
                    };

                    storeLocation.Id = await pizzaDb.InsertAsync(storeLocation);
                }
            }
        }

        public async Task CreateRolesAsync()
        {
            using (var pizzaDb = new PizzaDatabase())
            {
                SiteRole employeeRole = await pizzaDb.GetSiteRoleByNameAsync("Employee");
                SiteRole managerRole = await pizzaDb.GetSiteRoleByNameAsync("Manager");
                SiteRole adminRole = await pizzaDb.GetSiteRoleByNameAsync("Admin");
                SiteRole executiveRole = await pizzaDb.GetSiteRoleByNameAsync("Executive");


                if (employeeRole == null)
                {
                    employeeRole = new SiteRole()
                    {
                        Name = "Employee"
                    };
                    await pizzaDb.InsertAsync(employeeRole);
                }

                if (managerRole == null)
                {
                    managerRole = new SiteRole()
                    {
                        Name = "Manager"
                    };
                    await pizzaDb.InsertAsync(managerRole);
                }

                if (executiveRole == null)
                {
                    executiveRole = new SiteRole()
                    {
                        Name = "Executive"
                    };
                    await pizzaDb.InsertAsync(executiveRole);
                }

                if (adminRole == null)
                {
                    adminRole = new SiteRole()
                    {
                        Name = "Admin"
                    };
                    await pizzaDb.InsertAsync(adminRole);
                }

                SiteUser acctUser = await pizzaDb.GetSiteUserByNameAsync(AdminUserName);
                bool isAdmin = await pizzaDb.UserIsInRole(acctUser, adminRole);

                if (!isAdmin)
                {
                    await pizzaDb.Commands.AddUserToRoleAsync(acctUser, adminRole);
                }
            }
        }
    }
}
using JampotCapstone.Models;
using Microsoft.AspNetCore.Identity;
using File = JampotCapstone.Models.File;

namespace JampotCapstone.Data;

public class SeedData
{
    public static void Seed(ApplicationDbContext ctx, IServiceProvider provider)
    {
        var userManager = provider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
        
        const string password = "Secret!123";
        const string role = "Admin";
        
        if (roleManager.FindByNameAsync("Admin").Result == null)
        {
            roleManager.CreateAsync(new IdentityRole(role)).Wait();
        }

        AppUser admin = new AppUser
        {
            UserName = "admin@example.com",
            Name = "admin",
            SignUpDate = DateTime.UtcNow,
            EmailConfirmed = true
        };
        bool isSuccess = userManager.CreateAsync(admin, password).Result.Succeeded;
        if (isSuccess)
        {
            userManager.AddToRoleAsync(admin, role).Wait();
        }

        if (!ctx.Products.Any())
        {
            #region files

            File landing1 = new File
            {
                FileID = 1,
                FileName = "/pics/landing/JampotDrink.png",
                ContentType = "image/png"
            };
            ctx.Files.Add(landing1);

            File landing2 = new File
            {
                FileID = 2,
                FileName = "/pics/landing/jackfruit-wrap.png",
                ContentType = "image/png"
            };
            ctx.Files.Add(landing2);

            File landing3 = new File
            {
                FileID = 3,
                FileName = "/pics/ReggaeChiaPudding.png",
                ContentType = "image/png"
            };
            ctx.Files.Add(landing3);

            File landing4 = new File
            {
                FileID = 4,
                FileName = "/pics/landing/map.png",
                ContentType = "image/png"
            };
            ctx.Files.Add(landing4);

            File landing5 = new File
            {
                FileID = 5,
                FileName = "/pics/landing/jackfruit-wrap-special.png",
                ContentType = "image/png"
            };
            ctx.Files.Add(landing5);

            File landing6 = new File
            {
                FileID = 6,
                FileName = "/pics/landing/People.png",
                ContentType = "image/png"
            };
            ctx.Files.Add(landing6);

            File placeholder = new File
            {
                FileID = 7,
                FileName = "/pics/landing/DesertBowl.png",
                ContentType = "image/png"
            };
            ctx.Files.Add(placeholder);

            File electrolyte = new File
            {
                FileID = 8,
                FileName = "/pics/ElectrolyteRefresh.png",
                ContentType = "image/png"
            };
            ctx.Files.Add(electrolyte);

            File garden = new File
            {
                FileID = 9,
                FileName = "/pics/GardenWrap.png",
                ContentType = "image/png"
            };
            ctx.Files.Add(garden);

            File menu = new File
            {
                FileID = 10,
                FileName = "/pics/Menu.png",
                ContentType = "image/png"
            };
            ctx.Files.Add(menu);
            ctx.SaveChanges();
            #endregion
            #region products

            ProductType food = new ProductType
            {
                TypeId = 1,
                Type = "food"
            };
            ctx.ProductTypes.Add(food);
            ProductType drink = new ProductType
            {
                TypeId = 2,
                Type = "drink"
            };
            ctx.ProductTypes.Add(drink);
            ProductType dessert = new ProductType
            {
                TypeId = 3,
                Type = "dessert"
            };
            ctx.ProductTypes.Add(dessert);

            ProductTag vegan = new ProductTag
            {
                TagID = 1,
                Tag = "vegan"
            };
            ctx.ProductTags.Add(vegan);
            ProductTag gFree = new ProductTag
            {
                TagID = 2,
                Tag = "gluten free"
            };
            ctx.ProductTags.Add(gFree);
            ProductTag spicy = new ProductTag
            {
                TagID = 3,
                Tag = "spicy"
            };
            ctx.ProductTags.Add(spicy);

            ProductTag special = new ProductTag
            {
                TagID = 4,
                Tag = "special"
            };
            ctx.ProductTags.Add(special);
            
            Product p1 = new Product
            {
                ProductId = 1,
                ProductName = "Garden Wrap",
                ProductIngredients = "Spinach Tortilla, Lettuce, Carrot, Purple Cabbage, Tomato, Cucumber, Bell Pepper, Alfalfa Sprouts, Roasted Red Pepper Hummus",
                ProductPrice = 14,
                ProductPhoto = new File
                {
                    FileName = "/pics/GardenWrap.png",
                    ContentType = "image/png"
                },
                ProductCategory = new List<ProductType>()
                {
                    food
                },
                Tags = new List<ProductTag>()
                {
                    vegan
                }
            };
            ctx.Products.Add(p1);
            
            Product p2 = new Product
            {
                ProductId = 2,
                ProductName = "Jerk Chicken Wrap",
                ProductIngredients = "Spinach Tortilla, Lettuce, Carrot, Purple Cabbage, Bell Pepper, Alfalfa Sprouts, Jerk Sauce",
                ProductPrice = 15,
                ProductPhoto = new File
                {
                    FileName = "/pics/JerkChickenWrap.png",
                    ContentType = "image/png"
                },
                ProductCategory = new List<ProductType>()
                {
                    food
                },
                Tags = new List<ProductTag>()
                {
                    spicy
                }
            };
            ctx.Products.Add(p2);
            
            Product p3 = new Product
            {
                ProductId = 3,
                ProductName = "Reggae Chia Pudding",
                ProductIngredients = "Coconut milk chia pudding with Hawaiian spirulina, topped with fresh strawberry, mango, and hemp hearts.",
                ProductPrice = 7.50M,
                ProductPhoto = landing3,
                ProductCategory = new List<ProductType>()
                {
                    dessert
                },
                Tags = new List<ProductTag>()
                {
                    vegan, gFree
                }
            };
            ctx.Products.Add(p3);
            
            Product p4 = new Product
            {
                ProductId = 4,
                ProductName = "Electrolyte Refresh",
                ProductIngredients = "Cucumber, Honeydew, Pineapple, Mint, Watermelon, Lime",
                ProductPrice = 10,
                ProductPhoto = new File
                {
                    FileName = "/pics/ElectrolyteRefresh.png",
                    ContentType = "image/png"
                },
                ProductCategory = new List<ProductType>()
                {
                    drink
                }
            };
            ctx.Products.Add(p4);
            
            Product p5 = new Product
            {
                ProductId = 5,
                ProductName = "Lycheehoo",
                ProductIngredients = "Lychee, Young Coconut Water",
                ProductPrice = 12,
                ProductPhoto = new File
                {
                    FileName = "/pics/Lycheehoi.png",
                    ContentType = "image/png"
                },
                ProductCategory = new List<ProductType>()
                {
                    drink
                }
            };
            ctx.Products.Add(p5);
            
            Product p6 = new Product
            {
                ProductId = 6,
                ProductName = "Soursop Juice",
                ProductIngredients = "Soursop, White Nutmeg, Cinnamon, Vanilla, Lime, Sugarcane",
                ProductPrice = 10,
                ProductPhoto = new File
                {
                    FileName = "/pics/soursop.png",
                    ContentType = "image/png"
                },
                ProductCategory = new List<ProductType>()
                {
                    drink
                }
            };
            ctx.Products.Add(p6);
            
            Product p7 = new Product
            {
                ProductId = 7,
                ProductName = "Lilikoi Lemonade",
                ProductIngredients = "Lilikoi, Lemon, Sugarcane",
                ProductPrice = 10,
                ProductPhoto = placeholder,
                ProductCategory = new List<ProductType>()
                {
                    drink
                }
            };
            ctx.Products.Add(p7);
            
            Product p8 = new Product
            {
                ProductId = 8,
                ProductName = "Ginger Lemonade",
                ProductIngredients = "Ginger, Lime, Sugarcane",
                ProductPrice = 10,
                ProductPhoto = placeholder,
                ProductCategory = new List<ProductType>()
                {
                    drink
                }
            };
            ctx.Products.Add(p8);
            
            Product p9 = new Product
            {
                ProductId = 9,
                ProductName = "Black Magic",
                ProductIngredients = "Young Coconut Water, Activated Charcoal, Edible Gold",
                ProductPrice = 10,
                ProductPhoto = placeholder,
                ProductCategory = new List<ProductType>()
                {
                    drink
                }
            };
            ctx.Products.Add(p9);
            
            Product p10 = new Product
            {
                ProductId = 10,
                ProductName = "Yummy Yummy",
                ProductIngredients = "Mangos, strawberries, and tasty things",
                ProductPrice = 9.99M,
                ProductPhoto = placeholder,
                ProductCategory = new List<ProductType>()
                {
                    dessert
                }
            };
            ctx.Products.Add(p10);
            
            Product p11 = new Product
            {
                ProductId = 11,
                ProductName = "Ital Jerk Jackfruit Wrap",
                ProductIngredients = "Spices, fruit, and tasty things",
                ProductPrice = 15M,
                ProductPhoto = landing2,
                ProductCategory = new List<ProductType>
                {
                    food
                },
                Tags = new List<ProductTag>
                {
                    spicy, vegan, special
                }
            };
            ctx.Products.Add(p11);
            ctx.SaveChanges();
            #endregion
            #region pages

            Page home = new Page
            {
                PageTitle = "Home",
                Files = [
                    landing5, landing1, landing2, landing6, landing3
                ]
            };
            ctx.Pages.Add(home);
            
            Page ask = new Page
            {
                PageTitle = "FAQ",
                Files = [
                    landing6
                ]
            };
            ctx.Pages.Add(ask);

            Page aboutUs = new Page
            {
                PageTitle = "About Us",
                Files = [landing1, landing6, landing2]
            };
            ctx.Pages.Add(aboutUs);
            
            Page contact = new Page
            {
                PageTitle = "Contact Us",
            };
            ctx.Pages.Add(contact);

            Page catering = new Page
            {
                PageTitle = "Catering",
                Files = [placeholder, electrolyte, garden]
            };
            ctx.Pages.Add(catering);
            
            Page careersPage = new Page
            {
                PageTitle = "Careers"
            };
            ctx.Pages.Add(careersPage);

            Page menuPage = new Page
            {
                PageTitle = "Menu",
                Files = [menu]
            };
            ctx.Pages.Add(menuPage);
            ctx.SaveChanges();
            #endregion
            #region text

            TextElement hours = new TextElement
            {
                Name = "Landing Hours",
                Content =
                    "7am-5pm Monday-Friday\n8am-5pm Saturday\n65-1158 Mamalahoa Hwy\nWaimea, HI 96743\n(808) 238-0671",
                Page = home
            };
            ctx.TextElements.Add(hours);

            TextElement catering1 = new TextElement
            {
                Name = "FLAVORS",
                Content =
                    "At Jam Pot, we bring the vibrant flavors and warm hospitality of Jamaica to the heart of Hawaii. Whether you're planning a family luau, corporate gathering, or an island celebration, our catering services are designed to add a touch of sunshine and Caribbean flair to any event. Our signature Jerk Chicken Wrap is sure to delight your taste buds, perfectly complemented by the refreshing tropical notes of our Lycheehoo! special. From savory dishes to invigorating beverages, every bite and sip is crafted with love and the freshest ingredients.",
                Page = catering
            };
            ctx.TextElements.Add(catering1);

            TextElement catering2 = new TextElement
            {
                Name = "OPTIONS",
                Content =
                    "No event is too big or small—our team will work closely with you to create a menu tailored to your needs. We specialize in offering a variety of wholesome and flavorful options, including wraps, bowls, patties, and salads. Every dish is made fresh, showcasing the authentic tastes of Jamaica while embracing the island spirit of Hawaii. With our commitment to quality and attention to detail, Jam Pot takes the stress out of catering so you can focus on making memories.",
                Page = catering
            };
            ctx.TextElements.Add(catering2);

            TextElement catering3 = new TextElement
            {
                Name = "WHY JAMPOT?",
                Content =
                    "Let us help you make your next event unforgettable. Contact Jam Pot today to learn more about our catering packages, pricing, and availability. Together, we'll bring the essence of Jamaica and the aloha spirit to your celebration, making it truly special.",
                Page = catering
            };
            ctx.TextElements.Add(catering3);

            TextElement about = new TextElement
            {
                Name = "About Us",
                Content =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                Page = aboutUs
            };
            ctx.TextElements.Add(about);

            TextElement careers = new TextElement
            {
                Name = "Hiring Intro",
                Content =
                    "Jampot is currently offering career options for:\n[Line Cook], [Dishwasher]\nYou are welcome to apply for non-listed positions, but we may not reach out until those roles are needed.",
                Page = careersPage
            };
            ctx.TextElements.Add(careers);

            TextElement contact1 = new TextElement
            {
                Name = "ADDRESS",
                Content = "65-1158 Mamalahoa Hwy\nWaimea, HI 96743",
                Page = contact
            };
            ctx.TextElements.Add(contact1);

            TextElement contact2 = new TextElement
            {
                Name = "PHONE",
                Content = "(808) 238-0671",
                Page = contact
            };
            ctx.TextElements.Add(contact2);

            TextElement contact3 = new TextElement
            {
                Name = "HOURS",
                Content = "Monday-Friday: 7:00am-5:00pm\nSaturday: 8:00am-5:00pm",
                Page = contact
            };
            ctx.TextElements.Add(contact3);

            TextElement faq1 = new TextElement
            {
                Name = "What are your holiday hours?",
                Content =
                    "We are closed on Thanksgiving Day and Christmas Day to allow our employees to spend time with their families.",
                Page = ask
            };
            ctx.TextElements.Add(faq1);

            TextElement faq2 = new TextElement
            {
                Name = "Do you offer delivery?",
                Content = "No.",
                Page = ask
            };
            ctx.TextElements.Add(faq2);

            TextElement faq3 = new TextElement
            {
                Name = "Do you have any ongoing promotions or special offers?",
                Content = "Check out our Specials page",
                Page = ask
            };
            ctx.TextElements.Add(faq3);

            TextElement faq4 = new TextElement
            {
                Name = "Do you offer online ordering?",
                Content = "Yes! Check out our Order Page",
                Page = ask
            };
            ctx.TextElements.Add(faq4);

            TextElement faq5 = new TextElement
            {
                Name = "How do I apply for a job at this restaurant?",
                Content = "You can fill out an application on our Careers page in the menu at the bottom of the page.",
                Page = ask
            };
            ctx.TextElements.Add(faq5);

            TextElement faq6 = new TextElement
            {
                Name = "Do you offer catering?",
                Content = "Yes! Check out our Catering page in the menu above.",
                Page = ask
            };
            ctx.TextElements.Add(faq6);
            
            ctx.SaveChanges();

            #endregion
            #region jobs

            JobTitle lineCook = new JobTitle
            {
                JobTitleID = 1,
                JobTitleName = "Line Cook",
            };
            ctx.JobTitles.Add(lineCook);

            JobTitle dishwasher = new JobTitle
            {
                JobTitleID = 2,
                JobTitleName = "Dishwasher",
            };
            ctx.JobTitles.Add(dishwasher);

            ctx.SaveChanges();
            #endregion

            ctx.SaveChanges();
        }
    }
}
using ETicket.Domain.DomainModels;
using ETicket.Domain.DTO;
using ETicket.Domain.Identity;
using ETicket.Services.Interface;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ETicketApplicationUser> userManager;

        public AdminController(IOrderService orderService, UserManager<ETicketApplicationUser> userManager) {

            this._orderService = orderService;
            this.userManager = userManager;
        }

        [HttpGet("[action]")] 
        public List<Order> GetAllActiveOrders()
        {
            return this._orderService.GetAllOrders();

        }


        [HttpPost("[action]")]
        public Order GetDetailsForOrder(BaseEntity model)
        {
            return this._orderService.GetOrderDetails(model);

        }


        [HttpPost("[action]")]
        public bool ImportAllUsers(List<UserRegistrationDto> model)
        {
            bool finalStatus = true;

            foreach (var item in model) 
            {

                var userCheck = userManager.FindByEmailAsync(item.Email).Result;

                if (userCheck == null)
                {
                    var user = new ETicketApplicationUser
                    {
                        UserName = item.Email,
                        NormalizedUserName = item.Email,
                        Email = item.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        UserCart = new ShoppingCart()
                    };
                    var result = userManager.CreateAsync(user, item.Password).Result;

                    finalStatus = finalStatus && result.Succeeded;

                }

                else 
                {
                    continue;

                }
            }

            return finalStatus;

        }


    }

}

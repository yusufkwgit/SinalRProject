using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR.BusinnesLayer.Abstract;
using SignalR.DtoLayer.MenuTableDto;
using SignalR.EntityLayer.Entities;
using SignalRApi.Hubs;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuTablesController : ControllerBase
    {
        private readonly IMenuTableService _menuTableService;
        private readonly IMapper _mapper;
        private readonly IHubContext<SignalRHub> _hubContext;

        public MenuTablesController(IMenuTableService menuTableService, IMapper mapper, IHubContext<SignalRHub> hubContext)
        {
            _menuTableService = menuTableService;
            _mapper = mapper;
            _hubContext = hubContext;
        }
        [HttpGet("MenuTableCount")]
        public IActionResult MenuTableCount()
        {
            return Ok(_menuTableService.TMenuTableCount());
        }
        [HttpGet]
        public IActionResult MenuTableList()
        {
            var values = _menuTableService.TGetListAll();
            return Ok(_mapper.Map<List<ResultMenuTableDto>>(values));
        }
        [HttpPost]
        public IActionResult CreateMenuTable(CreateMenuTableDto createMenuTableDto)
        {
            createMenuTableDto.Status = false;
            var value = _mapper.Map<MenuTable>(createMenuTableDto);
            _menuTableService.TAdd(value);
            return Ok("Masa Başarılı Bir Şekilde Eklendi");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteMenuTable(int id)
        {
            var value = _menuTableService.TGetByID(id);
            _menuTableService.TDelete(value);
            return Ok("Masa Silindi");
        }
        [HttpPut]
        public IActionResult UpdateMenuTable(UpdateMenuTableDto updateMenuTableDto)
        {
            var value = _mapper.Map<MenuTable>(updateMenuTableDto);
            _menuTableService.TUpdate(value);
            return Ok("Masa Bilgisi Güncellendi");
        }
        [HttpGet("{id}")]
        public IActionResult GetMenuTable(int id)
        {
            var value = _menuTableService.TGetByID(id);
            return Ok(_mapper.Map<GetMenuTableDto>(value));
        }

        [HttpGet("ChangeMenuTableStatusToTrue")]
        public IActionResult ChangeMenuTableStatusToTrue(int id)
        {
            _menuTableService.TChangeMenuTableStatusToTrue(id);
            return Ok("İşlem başarılı");
        }

        [HttpGet("ChangeMenuTableStatusToFalse")]
        public IActionResult ChangeMenuTableStatusToFalse(int id)
        {
            _menuTableService.TChangeMenuTableStatusToFalse(id);
            return Ok("İşlem başarılı");
        }

        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder([FromBody] int id)
        {
            using var context = new SignalR.DataAccessLayer.Concrete.SignalRContext();
            var table = context.MenuTables.FirstOrDefault(x => x.MenuTableID == id);
            if (table == null) return NotFound("Masa bulunamadı");

            var basketItems = context.Baskets.Where(x => x.MenuTableID == id).ToList();
            if (basketItems.Count == 0) return BadRequest("Sepet boş");

            var order = new Order
            {
                OrderDate = DateTime.Now,
                Description = "Onay Bekliyor",
                TableNumber = table.Name,
                TotalPrice = basketItems.Sum(x => x.TotalPrice)
            };
            context.Orders.Add(order);
            context.SaveChanges();

            foreach (var item in basketItems)
            {
                context.OrderDetails.Add(new OrderDetail
                {
                    Count = (int)item.Count,
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    TotalPrice = item.TotalPrice,
                    UnitPrice = item.Price
                });
            }

            context.Baskets.RemoveRange(basketItems);
            table.Status = true;
            context.SaveChanges();

            // Notify admins
            await _hubContext.Clients.All.SendAsync("ReceiveNewOrderNotification", $"{table.Name} yeni bir sipariş oluşturdu!");

            return Ok("Sipariş başarıyla tamamlandı");
        }

        [HttpPost("ApproveOrder/{id}")]
        public IActionResult ApproveOrder(int id)
        {
            _menuTableService.TApproveOrder(id);
            return Ok("Sipariş Onaylandı");
        }

        [HttpPost("RejectOrder/{id}")]
        public IActionResult RejectOrder(int id)
        {
            _menuTableService.TRejectOrder(id);
            return Ok("Sipariş Reddedildi");
        }

        [HttpPut("UpdateOrder")]
        public IActionResult UpdateOrder(int orderId, string description)
        {
            using var context = new SignalR.DataAccessLayer.Concrete.SignalRContext();
            var order = context.Orders.FirstOrDefault(x => x.OrderID == orderId);
            if (order != null)
            {
                order.Description = description;
                context.SaveChanges();
                return Ok("Sipariş notu güncellendi");
            }
            return NotFound("Sipariş bulunamadı");
        }

        [HttpGet("GetEmptyTables")]
        public IActionResult GetEmptyTables()
        {
            var values = _menuTableService.TGetListAll().Where(x => x.Status == false).ToList();
            return Ok(_mapper.Map<List<ResultMenuTableDto>>(values));
        }

        [HttpGet("GetMenuTableIdByName/{tableName}")]
        public IActionResult GetMenuTableIdByName(string tableName)
        {
            using var context = new SignalR.DataAccessLayer.Concrete.SignalRContext();
            var table = context.MenuTables.FirstOrDefault(x => x.Name == tableName);
            if (table != null)
            {
                return Ok(table.MenuTableID);
            }
            return NotFound("Masa bulunamadı");
        }

        [HttpGet("GetActiveOrderDetails/{tableId}")]
        public IActionResult GetActiveOrderDetails(int tableId)
        {
            using var context = new SignalR.DataAccessLayer.Concrete.SignalRContext();
            var table = context.MenuTables.FirstOrDefault(x => x.MenuTableID == tableId);
            if (table == null) return NotFound("Masa bulunamadı");

            var activeOrder = context.Orders.FirstOrDefault(x => x.TableNumber == table.Name && x.Description != "Ödendi" && x.Description != "Reddedildi");
            if (activeOrder == null) return NotFound("Aktif sipariş bulunamadı");

            var details = context.OrderDetails.Include(x => x.Product).Where(x => x.OrderID == activeOrder.OrderID).Select(x => new
            {
                x.OrderDetailID,
                x.ProductID,
                x.Product.ProductName,
                x.Count,
                x.UnitPrice,
                x.TotalPrice,
                x.OrderID
            }).ToList();

            return Ok(new { orderId = activeOrder.OrderID, details = details });
        }

        [HttpPost("TransferTable")]
        public IActionResult TransferTable(int oldTableId, int newTableId)
        {
            _menuTableService.TTransferTable(oldTableId, newTableId);
            return Ok("Masa başarıyla aktarıldı.");
        }

        [HttpPost("CloseTablePayment/{tableId}")]
        public IActionResult CloseTablePayment(int tableId)
        {
            _menuTableService.TCloseTablePayment(tableId);
            return Ok("Hesap kapatıldı ve tutar kasaya eklendi.");
        }

        [HttpPost("AddProductToOrder")]
        public IActionResult AddProductToOrder(int orderId, int productId)
        {
            _menuTableService.TAddProductToOrder(orderId, productId);
            return Ok("Ürün siparişe eklendi.");
        }

        [HttpDelete("RemoveProductFromOrder/{orderDetailId}")]
        public IActionResult RemoveProductFromOrder(int orderDetailId)
        {
            _menuTableService.TRemoveProductFromOrder(orderDetailId);
            return Ok("Ürün siparişten çıkarıldı.");
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderRepository
    {
        /// <summary>
        /// Yeni bir siparişi kaydeder
        /// </summary>
        /// <param name="order">Kaydedilecek sipariş</param>
        Task SaveAsync(Order order);

        /// <summary>
        /// Siparişi ID ile getirir
        /// </summary>
        /// <param name="orderId">Sipariş ID'si</param>
        /// <returns>Sipariş bulunursa Order nesnesi, bulunamazsa null</returns>
        Task<Order?> GetByIdAsync(string orderId);

        /// <summary>
        /// Tüm siparişleri getirir
        /// </summary>
        /// <returns>Sipariş listesi</returns>
        Task<List<Order>> GetAllAsync();

        /// <summary>
        /// Siparişin durumunu günceller
        /// </summary>
        /// <param name="orderId">Sipariş ID'si</param>
        /// <param name="newStatus">Yeni durum</param>
        Task UpdateStatusAsync(string orderId, string newStatus);
    }
} 
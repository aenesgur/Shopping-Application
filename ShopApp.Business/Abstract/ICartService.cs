using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Abstract
{
    public interface ICartService
    {
        void InitializeCart(string userId);
        Cart GetCartByUserId(string userId);

        void AddToCard(string userId, int productId, int quantity);
        void DeletFromCart(string userId, int productId);
        void ClearCart(string cartId);
    }
}
